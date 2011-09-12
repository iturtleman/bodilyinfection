using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace AnimationEditor
{
    public class CollisionRect : Collision
    {
        public Point TL;
        public double Width;
        public double Height;

        public CollisionRect(Point p, double w, double h)
        {
            // TODO: Complete member initialization
            TL = p;
            Width = w;
            Height = h;
        }

        public override XElement GetLine()
        {
            XElement e = new XElement("Collision");
            e.SetAttributeValue("Type", "Rectangle");
            e.SetAttributeValue("TL", TL);
            e.SetAttributeValue("Width", Width);
            e.SetAttributeValue("Height", Height);
            return e;
        }
    }
}
