using Assets.Scripts.FixItEditor.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MouseAction : MonoBehaviour
{

    LineRenderer lineRenderer;
    public EditorModel MainEditorModel;
    public GameObject ConnectionCollider;
    public IBoxModel BoxModel;
    private bool _connectionInProgress = false;

private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.yellow, Color.red);
        lineRenderer.startWidth = .5f;
        lineRenderer.endWidth = .1f;
        updateBoxPosition();
    }

    private void updateBoxPosition()
    {
        BoxModel.BoxPosition.X = gameObject.transform.position.x;
        BoxModel.BoxPosition.Y = gameObject.transform.position.y;
    }

    private void Update()
    {
        MouseClick();
        OnMouseDown();
        updateBoxPosition();
        connectinLineRender();
    }

    private void connectinLineRender()
    {
        if (BoxModel.HasTargets)
        {
            lineRenderer.SetPosition(0, transform.localPosition);
            lineRenderer.SetPosition(1, BoxModel.TargetBoxes.First().BoxPosition.ToVector3());
            var vector = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
            ConnectionCollider.transform.localPosition = vector / 2;
            var angle = Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
            ConnectionCollider.transform.rotation = new Quaternion();
            ConnectionCollider.transform.Rotate(0, 0, -angle);
            ConnectionCollider.transform.localScale = new Vector3(ConnectionCollider.transform.localScale.x, vector.magnitude * 0.8f /*avoid overlap with shape*/);
        }
        else if (!_connectionInProgress)
        {
            lineRenderer.SetPosition(0, transform.localPosition);
            lineRenderer.SetPosition(1, transform.localPosition);
        }
    }

    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }


    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    void MouseClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject target = null;
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.collider.gameObject;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (target &&
                target == gameObject &&
                BoxModel.CanShootConnection)
            {
                _connectionInProgress = true;
            }

            if (BoxModel.HasTargets &&
               target &&
               target == ConnectionCollider)
            {
                MainEditorModel.RemoveConnection(BoxModel, BoxModel.TargetBoxes.First() /*CHANGE THIS IF MORE THAN 1 CONNECTION*/);
            }
        }

        if (Input.GetMouseButton(1) &&
            _connectionInProgress)
        {
            lineRenderer.SetPosition(0, transform.localPosition);
            lineRenderer.SetPosition(1, Camera.main.ScreenPointToRay(Input.mousePosition).origin);
        }

        if(Input.GetMouseButtonUp(1))
        {
            if(_connectionInProgress)
            {
                if (target &&
                target != gameObject)
                {
                    var mAction = target.GetComponent<MouseAction>();
                    if (mAction)
                    {
                        if (!MainEditorModel.AddConnection(BoxModel, mAction.BoxModel))
                        {
                            lineRenderer.SetPosition(0, transform.localPosition);
                            lineRenderer.SetPosition(1, transform.localPosition);
                        }
                    }
                }
                else
                {
                    lineRenderer.SetPosition(0, transform.localPosition);
                    lineRenderer.SetPosition(1, transform.localPosition);
                }
                _connectionInProgress = false;
            }
        }

    }
}