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
            get { return terrain;}
            set
            {
                // Gets called whenever the sprite has been changed
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

        private void UpdateSprite()
        {
            
            sprite = terrain.Sprite;
        }

        /// <summary>
        /// Subscribe to this event (tile.TileClickedEvent += YourListener) to
        /// get notified when the user clicks on this tile. Useful for implementing
        /// things like the toolbar when adding structures.
        /// </summary>
        public event EventHandler<TileClickArgs> TileClickedEvent;

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
