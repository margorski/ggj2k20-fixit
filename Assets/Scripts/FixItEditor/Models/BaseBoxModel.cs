using Assets.Scripts.FixItEditor.Enums;
using System.Collections;
using System.Collections.Generic;
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

        private BoxPoint _position;
        private uint _id = GetId();
        private BoxColor _boxColor = BoxColor.White;
        private BoxShape _boxShape = BoxShape.Circle;
        private List<IBoxModel> _targetBoxes = new List<IBoxModel>();
        private List<IBoxModel> _sourceBoxes = new List<IBoxModel>();

        public uint Id { get => _id; }
        public BoxColor BoxColor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public BoxShape BoxShape { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public BoxPoint BoxPosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        /// <summary>
        /// mocked - need to be derived by class!
        /// </summary>
        public BoxType BoxType { get => BoxType.Action; }
        public List<IBoxModel> TargetBoxes { get => _targetBoxes; }
        public List<IBoxModel> SourceBoxes { get => _sourceBoxes; }

        public uint MaxOutConnections => 1;

        public uint MaxInConnections => 1;
    }

}