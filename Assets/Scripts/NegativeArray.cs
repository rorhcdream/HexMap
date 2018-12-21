using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//음수 인덱스 지원 배열
public class Negative2DArray<T>
{
    private T[,] array;
    private int xOffset, yOffset;

    public Negative2DArray(int xMax, int xMin, int yMax, int yMin)
    {
        xOffset = -xMin;
        yOffset = -yMin;

        array = new T[xMax + xOffset + 1, yMax + yOffset + 1];
    }

    public T this[int x, int y]
    {
        get { return array[x + xOffset, y + yOffset]; }
        set { array[x + xOffset, y + yOffset] = value; }
    }
}
