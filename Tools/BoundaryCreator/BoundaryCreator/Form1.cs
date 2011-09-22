using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace BoundaryCreator
{
    public partial class Form1 : Form
    {
        List<Tuple<float, float>> polygonPoints = new List<Tuple<float, float>>();
        float thickness;
        List<Tuple<float, float, float, float, float>> separatedPoints = new List<Tuple<float, float, float, float, float>>();
        IEnumerable<string> animPre = new List<string>();

        public Form1()
        {
            InitializeComponent();
            animPreFile.Enabled = false;
            generate.Enabled = false;
            save.Enabled = false;
        }

        private void pointsFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Collision Points Files|*.colpoints";
            openFileDialog1.Title = "Select a Collision Points File";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();

            string file = openFileDialog1.FileName;
            IEnumerable<string> lines = new List<string>();
            thickness = -1.0f;
            //try
            //{
                lines = File.ReadLines(file);

                bool endPolygon = false;

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine.Length == 0 || (trimmedLine[0] == '/' && trimmedLine[1] == '/'))
                        continue;
                    if (trimmedLine[0] == '#')
                    {
                        endPolygon = true;
                        continue;
                    }
                    if (thickness == -1.0f)
                    {
                        thickness = float.Parse(trimmedLine.ToString());
                        continue;
                    }
                    if (endPolygon)
                    {
                        string[] points = trimmedLine.Split(';');
                        string[] point1 = points[0].Split(',');
                        string[] point2 = points[1].Split(',');
                        separatedPoints.Add(new Tuple<float, float, float, float, float>(float.Parse(point1[0]), float.Parse(point1[1]), float.Parse(point2[0]), float.Parse(point2[1]), float.Parse(points[2])));
                    }
                    else
                    {
                        string[] point = trimmedLine.Split(',');
                        polygonPoints.Add(new Tuple<float, float>(float.Parse(point[0]), float.Parse(point[1])));
                    }
                }

                textBox1.Clear();
                animPreFile.Enabled = true;
                generate.Enabled = false;
                save.Enabled = false;
            //}
            //catch {}
        }


        private void animPreFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Anim Files|*.anim";
            openFileDialog1.Title = "Select an Anim File";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();

            string file = openFileDialog1.FileName;

            try
            {
                animPre = File.ReadLines(file);
                generate.Enabled = true;
            }
            catch {}
        }

        private void generate_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            //Create collision text
            string collisionText = "";
            int count = polygonPoints.Count;
            for (int i = 0; i < count; i++)
            {
                collisionText += "    <Collision Type=\"OBB\" Corner1=";
                collisionText += "\"" + polygonPoints[i].Item1.ToString("0.0####") + "," + polygonPoints[i].Item2.ToString("0.0####") + "\"";
                collisionText += " Corner2=";
                collisionText += "\"" + polygonPoints[(i + 1) % count].Item1.ToString("0.0####") + "," + polygonPoints[(i + 1) % count].Item2.ToString("0.0####") + "\"";
                collisionText += " Thickness=\"" + thickness.ToString("0.0####") + "\"/>" + Environment.NewLine;
            }

            count = separatedPoints.Count;
            for (int i = 0; i < count; i++)
            {
                collisionText += "    <Collision Type=\"OBB\" Corner1=";
                collisionText += "\"" + separatedPoints[i].Item1.ToString("0.0####") + "," + separatedPoints[i].Item2.ToString("0.0####") + "\"";
                collisionText += " Corner2=";
                collisionText += "\"" + separatedPoints[i].Item3.ToString("0.0####") + "," + separatedPoints[i].Item4.ToString("0.0####") + "\"";
                collisionText += " Thickness=\"" + separatedPoints[i].Item5.ToString("0.0####") + "\"/>" + Environment.NewLine;
            }

            int index = 10000000;
            textBox1.Text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Animation>" + Environment.NewLine;
            foreach (string frame in animPre)
            {
                if (!frame.Contains("<Frame"))
                    continue;

                int index1 = frame.Trim().IndexOf("/>");
                int index2 = frame.Trim().IndexOf(">");
                if (index1 != -1)
                    index = index1;
                if (index2 != -1)
                    index = Math.Min(index, index2);
                if (index == 10000000)
                    textBox1.Text = "Bad Formatting";
                else
                {
                    string formattedFrame = frame.Trim().Remove(index);
                    textBox1.Text += "  " + formattedFrame + ">" + Environment.NewLine;
                    textBox1.Text += collisionText;
                    textBox1.Text += "  </Frame>" + Environment.NewLine;
                }
            }

            if (index != 10000000)
            {
                textBox1.Text += "</Animation>";
                save.Enabled = true;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Anim File | .anim";
            saveFileDialog1.Title = "Save an Anim File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.

                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(textBox1.Text);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
