using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.CityMap
{
    
    public class Terrain
    {
        static System.Random random = new System.Random();
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

        public Terrain(TerrainTypes terrainType)
        {
            TerrainType = terrainType;
            if (terrainType.Equals(TerrainTypes.Grass))
            {
                
                int value = random.Next(0, 10);
                if (value < 5)
                {
                    sprite = Resources.Load<Sprite>("Textures/Terrain/hexGrassLine");
                }
                else 
                {
                    sprite = Resources.Load<Sprite>("Textures/Terrain/hexGrassLineMedium");
                }
                
            }
            else if (terrainType.Equals(TerrainTypes.GrassHill))
            {
                sprite = Resources.Load<Sprite>("Textures/Terrain/hexGrassHill");
            }
            else if (terrainType.Equals(TerrainTypes.Desert))
            {
                int value = random.Next(0, 9);
                if (value < 5)
                {
                    sprite = Resources.Load<Sprite>("Textures/Terrain/hexSand");
                }
                else
                {
                    sprite = Resources.Load<Sprite>("Textures/Terrain/hexSandMeedium");
                }
            }
            else if (terrainType.Equals(TerrainTypes.Desert))
            {
                sprite = Resources.Load<Sprite>("Textures/Terrain/hexSand");
            }
            else if (terrainType.Equals(TerrainTypes.DesertHill))
            {
                sprite = Resources.Load<Sprite>("Textures/Terrain/hexSandHill");
            }
            else
            {
                sprite = Resources.Load<Sprite>("Textures/Terrain/hexWater");
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
