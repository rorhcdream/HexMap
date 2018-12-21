using UnityEngine;
using System.Collections.Generic;

//hex cell들 아래에 존재하는 파란색 배경
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexGridMesh : MonoBehaviour
{
    Mesh hexGridMesh;

    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;

    Color gridColor = new Color(0.4f, 0.4f, 1f);

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexGridMesh = new Mesh();
        hexGridMesh.name = "Hex Grid Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    //주어진 position들을 중심으로 육각 mesh들을 생성
    public void TriangulateAll(IEnumerable<Vector3> cellPositions)
    {
        hexGridMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        foreach (Vector3 pos in cellPositions)
        {
            Triangulate(pos);
        }

        hexGridMesh.vertices = vertices.ToArray();
        hexGridMesh.triangles = triangles.ToArray();
        hexGridMesh.colors = colors.ToArray();
        hexGridMesh.RecalculateNormals();
    }

    //cellPos를 중심으로 육각 mesh를 생성
    void Triangulate(Vector3 cellPos)
    { 
        for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
        {
            AddTriangle(
                cellPos,
                cellPos + HexMetrics.GetFirstCorner(direction),
                cellPos + HexMetrics.GetSecondCorner(direction)
            );
            AddTriangleColor(gridColor);
        }  
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

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
}