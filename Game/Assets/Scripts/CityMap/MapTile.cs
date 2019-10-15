using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.CityMap
{
    public class MapTile : Tile
    {
        
        
        public GameObject Canvas { set; get; }

        public Vector3 ScreenPosition
        {
            set
            {
                screenPosition = value;
                if (Canvas != null)
                {
                    structure?.RenderOnto(Canvas, ScreenPosition);
                }
            }
            get => screenPosition;
        }

        private Vector3 screenPosition;

        /// <summary>
        /// Note that adding, changing or removing the structure will also update the
        /// corresponding game objects to display the structure onto the screen.
        /// </summary>
        public Structure Structure
        {
            get { return structure; }
            set
            {
                
                Assert.IsNotNull(Canvas,
                    "The ScreenPosition and Canvas to draw the structure on should " +
                    "be set before setting the structure");
                structure?.Unrender();
                structure = value;
                structure?.RenderOnto(Canvas, ScreenPosition);
            }
        }

        private Structure structure;

        /// <summary>
        /// Note that the MapTile tracks the terrain for sprite changes.
        /// </summary>
        public Terrain Terrain
        {
            get { return terrain; }
            set
            {
                Assert.IsNotNull(value, "The tile should always have a terrain.");
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

        /// <summary>
        /// Fires whenever the sprite for this tile has changed.
        /// Useful for refreshing the tile from the TileMap.
        /// </summary>
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
            return Structure?.GetStatsContribution();
        }

        public void HandleMouseEnter()
        {
            color = new Color(0.8f, 0.8f, 0.8f, 1);
            SpriteChange?.Invoke();
        }

        public void HandleMouseLeave()
        {
            color = Color.white;
            SpriteChange?.Invoke();
        }
    }
}
