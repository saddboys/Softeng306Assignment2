using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEditor;
    public class ParentTile : Tile
    {
        protected TileData data;
        protected Vector3Int offsetCoords;
        private Structure structure;

        [SerializeField] private Sprite preview;
        public Sprite[] GetSprite()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
            Debug.Log("Length is: " + sprites.Length);
            return sprites;
        }

        public void SetStructure(Structure structure)
        {
            this.structure = structure;
        }

        public Structure GetStructure()
        {
            return structure;
        }

        public void SetSprite(Sprite sprite)
        {
            data.sprite = sprite;
            Debug.Log("Sprite is now: "+ data.sprite);
            this.sprite = sprite;
        }

    }