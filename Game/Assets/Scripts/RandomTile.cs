using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

    public class RandomTile : ParentTile
    {
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            this.name = "testing";
            base.GetTileData(position, tilemap, ref tileData);
            Debug.Log("here first");
            //Sprite[] sprites = GetSprite("Textures/structures/test");
            Sprite[] sprites = GetSprite();
            tileData.sprite = sprites[20];
            this.sprite = sprites[20];
            //Debug.Log("Name is" + sprites[20].name);
            
        }
        
        
        
    }