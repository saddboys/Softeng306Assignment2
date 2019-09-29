
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition
{
    public float x;
    public float y;

    public TilePosition(float x, float y)
    {
        this.x = x;
        this.y = y * (float)0.75;
    }

    public Vector2 pos()
    {
        if (y % 1.5 == 0)
        {
            return new Vector2(x, y);
        }
        else
        {
            return new Vector2(x + (float)0.5, y);
        }
    }
}
