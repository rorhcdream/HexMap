using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color;
    public int index;

    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;
    MeshCollider meshCollider;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();

        meshCollider = gameObject.AddComponent<MeshCollider>();
        Triangulate();
    }

    public void Triangulate()
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();

        Vector3 center = transform.localPosition;
        for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
        {
            AddTriangle(
                center,
                center + HexMetrics.GetFirstInnerCorner(direction),
                center + HexMetrics.GetSecondInnerCorner(direction)
            );
        }
        //for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
        //{
        //    AddQuad(
        //        HexMetrics.GetFirstInnerCorner(direction),
        //        HexMetrics.GetSecondInnerCorner(direction),
        //        HexMetrics.GetFirstCorner(direction),
        //        HexMetrics.GetSecondCorner(direction)
        //        );
        //}

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        SetCellColor(color);
        hexMesh.RecalculateNormals();

        meshCollider.sharedMesh = hexMesh;
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }

    void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }


    public void SetCellColor(Color color)
    {
        this.color = color;
        colors.Clear();
        for (int i = 0; i < 18; i++)
        {
            colors.Add(color);
        }
        //for (int i = 0; i < 24; i++)
        //{
        //    colors.Add(Color.white);
        //}
        hexMesh.colors = colors.ToArray();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
        {
            Gizmos.DrawLine(transform.TransformPoint(HexMetrics.GetFirstCorner(direction))
                            ,transform.TransformPoint(HexMetrics.GetSecondCorner(direction)));
        }
    }
}
