using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GizmoLine
{
    public Vector3 a;
    public Vector3 b;
    public Color color;

    public GizmoLine(Vector3 a, Vector3 b, Color color)
    {
        this.a = a;
        this.b = b;
        this.color = color;
    }
}

public class Gizmos
{
    public static void DrawLine(Vector3 a, Vector3 b, Color color)
    {
        UnityEngine.Gizmos.color = color;
        UnityEngine.Gizmos.DrawLine(a, b);

        GizmoRenderer.gizmoLines.Add(new GizmoLine(a, b, color));
    }
}

public class GizmoRenderer : MonoBehaviour
{
    public static List<GizmoLine> gizmoLines = new List<GizmoLine>();

    [Header("Components")]
    public Material material;

    private void OnPostRender()
    {
        material.SetPass(0);
        GL.Begin(GL.LINES);

        for (int i = 0; i < gizmoLines.Count; i++) {
            GL.Color(gizmoLines[i].color);
            GL.Vertex(gizmoLines[i].a);
            GL.Vertex(gizmoLines[i].b);
        }

        GL.End();
        gizmoLines.Clear();
    }
}
