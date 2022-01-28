using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class DrawSplines : MonoBehaviour {
    public Color color = Color.red;
    public bool enabled = true;
    public bool draw_points = true;
    public bool draw_lines = true;
    public bool draw_direction = true;
    public float size = .05f;
    [Range(0f, 1f)]
    public float direction_opacity = .4f;

    private void OnDrawGizmos() {
        if(enabled == true) {
            for(int i = 0; i < this.transform.childCount; i++)
            {
                Transform Children = this.transform.GetChild(i);
                Vector3 lastSibling_position = new Vector3();
                Quaternion lastSibling_rotation = new Quaternion();
                for(int n = 0; n < Children.transform.childCount; n++)
                {
                    Transform Sibling = Children.transform.GetChild(n);
                    if (lastSibling_position.x != 0) {
                        if(draw_lines) DrawLine(lastSibling_position, Sibling.transform.position);
                    }

                    if(n + 1 < Children.transform.childCount) {
                        Transform Next = Children.transform.GetChild(n + 1);
                        Sibling.transform.LookAt(Next.position);
                    }
                    else {
                        if(n + 1 == Children.transform.childCount) {
                            Sibling.transform.rotation = lastSibling_rotation;
                        }
                    }

                    if(draw_points) DrawBox(Sibling.transform.position, n);
                    if(draw_direction) DrawLine(Sibling.transform.position, Sibling.transform.position + .3f * Sibling.transform.forward, direction_opacity);
                    lastSibling_position = Sibling.transform.position;
                    lastSibling_rotation = Sibling.transform.rotation;
                }
            }
        }
    }

    private void DrawBox(Vector3 pos, int i, float alpha = 1f) {
        color.a = alpha;
        Handles.color = color;
        Gizmos.color = color;
        Handles.DrawWireCube(pos, new Vector3(size, size, size));
        Gizmos.DrawCube(pos, new Vector3(size + .001f, size + .001f, size + .001f));
    }

    private void DrawLine(Vector3 start, Vector3 end, float alpha = 1f) {
        color.a = alpha;
        Handles.color = color;
        Handles.DrawLine(start, end);
    }
}
#endif