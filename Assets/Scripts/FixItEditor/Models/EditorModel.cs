using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FixItEditor.Models
{
    public class EditorModel : MonoBehaviour
    {
        public GameObject SquarePrefab;

        private NetworkManager networkManagerDupaCwel;

        private readonly Dictionary<uint, IBoxModel> _boxes = new Dictionary<uint, IBoxModel>();

        public void Awake()
        {
            networkManagerDupaCwel = FindObjectOfType<NetworkManager>();
            InvokeRepeating("OnPeriodicalUpdate", 0, 0.1f);
        }

        public void OnPeriodicalUpdate()
        {
            networkManagerDupaCwel?.SendMessage(this);
        }

        public void Start()
        {
            AddBox(new BaseBoxModel(), new BoxPoint(0,0));
            AddBox(new BaseBoxModel(), new BoxPoint(3,3));
            AddBox(new BaseBoxModel(), new BoxPoint(-3,-3));
            AddBox(new BaseBoxModel(), new BoxPoint(3,-3));
            AddBox(new BaseBoxModel(), new BoxPoint(-3,3));
        }

        public void AddBox(IBoxModel box, BoxPoint point)
        {
            var square = Instantiate(SquarePrefab, new Vector3(point.X, point.Y), new Quaternion());
            var action = square.GetComponent<MouseAction>();
            action.MainEditorModel = this;
            action.BoxModel = box;
            _boxes.Add(box.Id, box);
        }

        public bool AddConnection(IBoxModel source, IBoxModel target)
        {
            if (source == null || target == null) return false; //ech
            if (!_boxes.ContainsKey(source.Id) || !_boxes.ContainsKey(target.Id))
            {
                throw new Exception($"AddConnection: Boxes not in list _boxes s[{source.Id}]t[{target.Id}]");
            }
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

        public void RemoveConnection(IBoxModel source, IBoxModel target)
        {
            if (source == null || target == null) return;
            if (!_boxes.ContainsKey(source.Id) || !_boxes.ContainsKey(target.Id))
            {
                throw new Exception($"RemoveConnection: Boxes not in list _boxes s[{source.Id}]t[{target.Id}]");
            }
            if(!source.TargetBoxes.Remove(target) || !target.SourceBoxes.Remove(source))
            {
                throw new Exception("Boxes were not connected s[{source.Id}]t[{target.Id}]");
            }
        }
        
        public List<BoxModelSerializable> Message
        { 
            get
            {
                var message = new List<BoxModelSerializable>();
                foreach(var box in _boxes)
                {
                    var model = new BoxModelSerializable();
                    model.Populate(box.Value);
                    message.Add(model);
                }
                return message;
            }
        }


        private List<IBoxModel> GetEndpointSources(IBoxModel box)
        {
            List<IBoxModel> sourceBoxes = new List<IBoxModel>();
            if (!box.HasSources)
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
