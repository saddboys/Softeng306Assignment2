using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.CityMap
{
    
    public class Terrain
    {
        public enum TerrainTypes
        {
            Grass,
            Desert,
            Ocean,
            Beach,
            GrassHill,
            DesertHill,
        };
        public TerrainTypes TerrainType { get; set; }

        public Terrain(TerrainTypes terrainType, Sprite[] sprites)
        {
            TerrainType = terrainType;
            if (terrainType.Equals(TerrainTypes.Grass))
            {
                
                sprite = sprites[12];
            }
            else
            {
                sprite = sprites[23];
            }
        }
        
        public Sprite Sprite
        {
            get { return sprite; }
            set
            {
                sprite = value;
                // Check if the spriteChange is null first before invoking the event
                SpriteChange?.Invoke();
            }
        }

        private Sprite sprite;

        /// <summary>
        /// Fires whenever the sprite for this terrain has changed.
        /// Useful for updating a Tile's sprite based on its Terrain.
        /// </summary>
        public event Action SpriteChange;
    }
}
