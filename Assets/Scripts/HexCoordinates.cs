using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X { get { return x; } }
    public int Y { get { return -x - z; } }
    public int Z { get { return z; } }

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x, z);
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        int z = Mathf.RoundToInt(position.z / (HexMetrics.outerRadius * 1.5f));
        int x = Mathf.RoundToInt(position.x / (HexMetrics.innerRadius * 2f) - z / 2f);
        

        return new HexCoordinates(x, z);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}