using Assets.Scripts.FixItEditor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FixItEditor.Models
{
    [Serializable]
    public class BoxModelSerializable
    {
        public uint Id;
        public BoxColor BoxColor;
        public BoxShape BoxShape;
        public BoxType BoxType;
        public List<uint> TargetBoxes;
        public List<uint> SourceBoxes;
        public bool HasTargets;
        public bool HasSources;
        public uint MaxOutConnections;
        public uint MaxInConnections;
    }
}