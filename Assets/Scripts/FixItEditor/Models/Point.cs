using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FixItEditor.Models
{
    public class BoxPoint
    {
        public float X;
        public float Y;
        public BoxPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y);
        }
    }
}
