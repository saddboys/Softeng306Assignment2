using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Game.CityMap
{
    public class MapTile : Tile
    {
        public Structure Structure { get; set; }

        public Terrain Terrain
        {
            get
            {
                return terrain;
            }
            set
            {
                if (terrain != null)
                {
                    terrain.SpriteChange -= UpdateSprite;
                }
                terrain = value;
                terrain.SpriteChange += UpdateSprite;
                UpdateSprite();
            }
        }

        private Terrain terrain;

        public event Action SpriteChange;

        /// <summary>
        /// Note: Gets called whenever the sprite has been changed.
        /// </summary>
        private void UpdateSprite()
        {
            sprite = terrain.Sprite;
            SpriteChange?.Invoke();
        }

        /// <summary>
        /// Calculate how much this tile will contribute to the stats, such
        /// as CO2 generated, profits/losses, etc.
        /// </summary>
        /// <returns>The tile's stats contribution.</returns>
        public Stats GetStatsContribution()
        {
            // Get stats from its terrain and structure.GetStatsContribution
            throw new System.NotImplementedException();
        }
    }
}
