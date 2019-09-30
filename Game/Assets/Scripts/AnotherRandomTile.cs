using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnotherRandomTile : ParentTile
{

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        this.name = "anotherrandomtile";
        
        base.GetTileData(position, tilemap, ref tileData);
        Sprite[] sprites = GetSprite();
        if (this.sprite == null)
        {
            tileData.sprite = sprites[10];
            this.sprite = sprites[10];
        }

        data = tileData;
        Debug.Log("Name is" + sprites[10].name);
    }
        
        
        
}