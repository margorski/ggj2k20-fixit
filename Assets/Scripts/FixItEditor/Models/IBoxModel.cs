using Assets.Scripts.FixItEditor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FixItEditor.Models
{
    public interface IBoxModel
    {
        uint Id { get; }
        BoxColor BoxColor { set;  get; }
        BoxShape BoxShape { get; set; }
        BoxPoint BoxPosition { set; get; }
        BoxType BoxType { get; }
        List<IBoxModel> TargetBoxes { get; }
        List<IBoxModel> SourceBoxes { get; }
        bool HasTargets { get; }
        bool HasSources { get; }
        uint MaxOutConnections { get; }
        uint MaxInConnections { get; }
        bool CanShootConnection { get; }
        bool CanAcceptConnection { get; }

    }
}
