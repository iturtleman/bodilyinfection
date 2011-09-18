using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    public class Camera
    {
        protected float _zoom;
        protected Matrix _transform;

        public Camera()
        {
            _zoom = 1.0f;
        }

        public float Rotation { get; set; }
        public Vector2 Pos { get; set; }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
            }
        }

        public Matrix GetTransformation(GraphicsDevice device)
        {
            return Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
        }
    }
}
