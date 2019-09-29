
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition
{
    private float x;
    private float y;
    private float z;

    public TilePosition(float x, float y, float z)
    {
        this.x = x;
        this.y = y * (float)0.75;
        this.z = z;
    }

    public Vector3 Pos()
    {
        if (y % 1.5f == 0f)
        {
            return new Vector3(x, y, z);
        }
        else
        {
            return new Vector3(x + (float)0.5, y, z);
        }
    }
}
