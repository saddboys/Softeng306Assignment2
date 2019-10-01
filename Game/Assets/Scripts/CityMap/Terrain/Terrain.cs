using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.CityMap
{
    public abstract class Terrain
    {
        private const string TERRAIN_PATH = "Textures/terrain";

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

        /// <summary>
        /// Get all the sprites in the terrain resources
        /// </summary>
        /// <returns></returns>
        public Sprite[] GetSprites()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(TERRAIN_PATH);
            return sprites;
        }
    }
}
