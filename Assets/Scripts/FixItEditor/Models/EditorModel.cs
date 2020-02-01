﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FixItEditor.Models
{
    public class EditorModel
    {
        public void AddBox(IBoxModel box)
        {
            Message.Add(box.Id, box);
        }

        public bool AddConnection(IBoxModel source, IBoxModel target)
        {
            if(source.MaxOutConnections > source.TargetBoxes.Count() &&
               target.MaxInConnections > target.SourceBoxes.Count())
            {
                if(GetEndpointSources(source).Any(x => x.Id == target.Id))
                {
                    return false; // uuuuu spierdalaj
                }
                source.TargetBoxes.Add(target);
                target.SourceBoxes.Add(source);
                return true;
            }
            return false; //spierdalaj
        }

        public Dictionary<uint, IBoxModel> Message { get; } = new Dictionary<uint, IBoxModel>();

        private List<IBoxModel> GetEndpointSources(IBoxModel box)
        {
            List<IBoxModel> sourceBoxes = new List<IBoxModel>();
            if (!box.SourceBoxes.Any())
            {
                sourceBoxes.Add(box);
            }

            foreach(var sourceBox in box.SourceBoxes)
            {
                sourceBoxes.AddRange(GetEndpointSources(sourceBox));
            }

            return sourceBoxes;
        }
    }
}