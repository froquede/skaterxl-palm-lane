using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DrawSplines : MonoBehaviour
{
    public Color color = Color.red;
    public bool enabled = true;

    private void OnDrawGizmos()
    {
        if(enabled == true) {
            for(int i = 0; i < this.transform.childCount; i++)
            {
                Transform Children = this.transform.GetChild(i);
                Vector3 lastSibling = new Vector3();
                for(int n = 0; n < Children.transform.childCount; n++)
                {
                    Transform Sibling = Children.transform.GetChild(n);
                    if (lastSibling.x != 0) {
                        DrawLine(lastSibling, Sibling.transform.position);
                    }
                    lastSibling = Sibling.transform.position;
                }
            }
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }
}