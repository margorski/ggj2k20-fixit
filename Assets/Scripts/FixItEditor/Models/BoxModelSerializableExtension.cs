using Assets.Scripts.FixItEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FixItEditor.Models
{
    static class BoxModelSerializableExtension
    {
        public static void Populate(this BoxModelSerializable modelS, IBoxModel model)
        {
            modelS.Id                = model.Id               ;
            modelS.BoxColor          = model.BoxColor         ;
            modelS.BoxShape          = model.BoxShape         ;
            modelS.BoxType           = model.BoxType          ;
            modelS.TargetBoxes       = model.TargetBoxes.Select(x => x.Id).ToList();
            modelS.SourceBoxes       = model.SourceBoxes.Select(x => x.Id).ToList();
            modelS.HasTargets        = model.HasTargets       ;
            modelS.HasSources        = model.HasSources       ;
            modelS.MaxOutConnections = model.MaxOutConnections;
            modelS.MaxInConnections  = model.MaxInConnections ;
        }
            
    }
}