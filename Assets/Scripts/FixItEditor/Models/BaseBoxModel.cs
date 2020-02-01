using Assets.Scripts.FixItEditor.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.FixItEditor.Models
{

    public class BaseBoxModel : IBoxModel
    {
        private static readonly object syncLock = new object();
        private static uint globalId = 0;
        static uint GetId() 
        {
            lock(syncLock)
            {
                return globalId++;
            }
        }

        private uint _id = GetId();
        private BoxColor _boxColor = BoxColor.White;
        private BoxShape _boxShape = BoxShape.Circle;
        private List<IBoxModel> _targetBoxes = new List<IBoxModel>();
        private List<IBoxModel> _sourceBoxes = new List<IBoxModel>();

        public uint Id { get => _id; }
        public BoxColor BoxColor { get; set; } = BoxColor.White;
        public BoxShape BoxShape { get; set; } = BoxShape.Circle;
        public BoxPoint BoxPosition { get; set; } = new BoxPoint(0,0);
        /// <summary>
        /// mocked - need to be derived by class!
        /// </summary>
        public BoxType BoxType { get => BoxType.INPUT_ONE; }
        public List<IBoxModel> TargetBoxes { get => _targetBoxes; }
        public List<IBoxModel> SourceBoxes { get => _sourceBoxes; }

        public uint MaxOutConnections => 1;

        public uint MaxInConnections => 1;

        public bool HasTargets => TargetBoxes.Any();

        public bool HasSources => SourceBoxes.Any();

        public bool CanShootConnection => MaxOutConnections > TargetBoxes.Count;

        public bool CanAcceptConnection => MaxInConnections > SourceBoxes.Count;
    }

}