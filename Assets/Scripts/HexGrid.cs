using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class HexGrid : MonoBehaviour
{
    public int size = 4;        //육각형 한 변의 길이

    public HexCell cellPrefab;
    public Text cellLabelPrefab;

    Negative2DArray<HexCell> cellArray;
    List<HexCell> cells;
    Canvas gridCanvas;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.red;

    HexGridMesh hexGridMesh;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexGridMesh = GetComponentInChildren<HexGridMesh>();
        
        //grid에 들어갈 cell들과 배경이 되는 mesh 생성
        CreateCells();
        hexGridMesh.TriangulateAll(cells.Select(cell => cell.transform.position));

        StartCoroutine(reddish());
    }

    void Start()
    {
    }

    //-------------------------
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleMouseInput();
        }
    }

    //클릭한 cell의 색 변경
    void HandleMouseInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (hit.transform.CompareTag("HexCell"))
            {
                hit.transform.gameObject.GetComponent<HexCell>().SetCellColor(touchedColor);
            }
        }
    }

    //void TouchCell(Vector3 position)
    //{
    //    position = transform.InverseTransformPoint(position);
    //    Debug.Log("touched at " + position);
    //}
    //--------------

    void CreateCell(int x, int z, int index)
    {
        Vector3 position;
        position.x = x * HexMetrics.innerRadius * 2 + z * HexMetrics.innerRadius;
        position.y = 0f;
        position.z = z * HexMetrics.outerRadius * 1.5f;

        HexCell cell = Instantiate<HexCell>(cellPrefab);
        cells.Add(cell);
        cellArray[x, z] = cell;
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        cell.index = index;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    void CreateCells()
    {
        cells = new List<HexCell>();
        cellArray = new Negative2DArray<HexCell>(size - 1, -size + 1, size - 1, -size + 1);

        for (int z = -size + 1, index = 0; z <= size - 1; z++)
        {
            for (int x = -size + 1; x <= size - 1; x++)
            {
                if (x + z <= -size || x + z >= size)
                    continue;
                CreateCell(x, z, index++);
            }
        }
    }

    //public HexCell getCell(HexCoordinates coord)
    //{

    //}

    //임시 기능
    IEnumerator reddish()
    {
        float f = 0f;
        while (true)
        {
            cells[20].SetCellColor(new Color(1, 1-f, 1-f));
            f += 0.02f;
            if (f > 0.8f) f = 0.2f;
            yield return true;
        }
    }
}
