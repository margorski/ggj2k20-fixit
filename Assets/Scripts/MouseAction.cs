using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MouseAction : MonoBehaviour
{

    private GameObject target = null;
    public bool AreConnected = false;
    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.yellow, Color.red);
        lineRenderer.startWidth = .5f;
        lineRenderer.endWidth = .1f;
    }

    private void Update()
    {
        DrawConnectionLine();
        RightClick();
        OnMouseDown();
    }

    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void RightClick()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(1))
        {
            if (AreConnected && target)
            {
                AreConnected = false;
                target = null;

            }
            if (!AreConnected)
            {
                lineRenderer.SetPosition(0, this.transform.localPosition);
                lineRenderer.SetPosition(1, this.transform.localPosition);
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.collider.gameObject;
                this.AreConnected = true;
            }
        }
    }

    void DrawConnectionLine()
    {
        if (target)
        {
            lineRenderer.SetPosition(0, transform.localPosition);
            lineRenderer.SetPosition(1, target.transform.position);
            
        }
    }

}