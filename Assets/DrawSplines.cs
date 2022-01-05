using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class DrawSplines : MonoBehaviour {
    public Color color = Color.red;
    public bool enabled = true;
    public float size = .05f;

    private void OnDrawGizmos() {
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
                    DrawBox(Sibling.transform.position, n);
                    lastSibling = Sibling.transform.position;
                }
            }
        }
    }

    private void DrawBox(Vector3 pos, int i) {
        Handles.color = color;
        Gizmos.color = color;
        Handles.DrawWireCube(pos, new Vector3(size, size, size));
        Gizmos.DrawCube(pos, new Vector3(size + .01f, size + .01f, size + .01f));
    }

    private void DrawLine(Vector3 start, Vector3 end) {
        Handles.color = color;
        Handles.DrawLine(start, end);
    }
}
#endif