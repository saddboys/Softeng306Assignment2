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

        public Terrain(TerrainTypes terrainType, Sprite[] sprites)
        {
            TerrainType = terrainType;
            if (terrainType.Equals(TerrainTypes.Grass))
            {
                
                int value = random.Next(0, 2);
                if (value == 0)
                {
                    sprite = Resources.Load<Sprite>("Textures/terrain/hexGrass");
                }
                else if (value == 1)
                {
                    sprite = Resources.Load<Sprite>("Textures/terrain/hexGrass1");
                }
                else
                {
                    sprite = Resources.Load<Sprite>("Textures/terrain/hexGrass2");
                }
            }
            else if (terrainType.Equals(TerrainTypes.Desert))
            {
                int value = random.Next(0, 2);
                if (value == 0)
                {
                    sprite = Resources.Load<Sprite>("Textures/terrain/hexSand");
                }
                else if (value == 1)
                {
                    sprite = Resources.Load<Sprite>("Textures/terrain/hexSand1");
                }
                else
                {
                    sprite = Resources.Load<Sprite>("Textures/terrain/hexSand2");
                }
            }
            else
            {
                sprite = Resources.Load<Sprite>("Textures/terrain/hexwater 1");
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
