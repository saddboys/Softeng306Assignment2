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
        /// Subscribe to this event (tile.StructureBuildRequestEvent += YourHandler) to
        /// add logic to test whether structures can be built on certain tiles.
        /// </summary>
        public event EventHandler<StructureBuildRequestArgs> StructureBuildRequestEvent;

        /// <summary>
        /// Subscribe to this event (tile.StructureBuildEvent += YourListener) to
        /// get notified when a new structure is built.
        /// </summary>
        public event EventHandler<StructureBuildRequestArgs> StructureBuiltEvent;

        /// <summary>
        /// Subscribe to this event (tile.TileClickedEvent += YourListener) to
        /// get notified when the user clicks on this tile. Useful for implementing
        /// things like the toolbar when adding structures.
        /// </summary>
        public event EventHandler<TileClickArgs> TileClickedEvent;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("TEST");
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Test to see if a certain structure can be built on this tile.
        /// Useful for changing the UI feedback when hovering over invalid tiles, etc.
        /// and preventing invalid actions.
        /// </summary>
        /// <param name="structure">The structure to test if possible to build.</param>
        /// <returns>An object containing IsCancelled and CancelledReason if invalid.</returns>
        public StructureBuildRequestArgs CanBuildStructure(Structure structure)
        {
            // E.g.
            var args = new StructureBuildRequestArgs(structure, this);
            StructureBuildRequestEvent(this, args);
            return args;
        }

        /// <summary>
        /// Attempt to build a structure onto this tile, doing nothing if fails.
        /// </summary>
        /// <param name="structure">The structure to build.</param>
        public void RequestBuildStructure(Structure structure)
        {
            // E.g.
            StructureBuildRequestArgs args = CanBuildStructure(structure);
            // And check if it is cancelled.
            Debug.Log(args.IsCancelled);
            // Add it in if it is not cancelled,
            // TODO
            // Finally notify.
            StructureBuiltEvent(this, args);
            throw new System.NotImplementedException();
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
