using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CityMap
{
    public class CityMap : MonoBehaviour
    {
        /// <summary>
        /// Subscribe to this event (map.StructureBuildRequestEvent += YourHandler) to
        /// add logic to test whether structures can be built on certain tiles.
        /// </summary>
        public event EventHandler<StructureBuildRequestArgs> StructureBuildRequestEvent;

        /// <summary>
        /// Subscribe to this event (map.StructureBuildEvent += YourListener) to
        /// get notified when a new structure is built.
        /// </summary>
        public event EventHandler<StructureBuildRequestArgs> StructureBuiltEvent;

        /// <summary>
        /// Subscribe to this event (map.TileClickedEvent += YourListener) to
        /// get notified when the user clicks on any tile. Useful for implementing
        /// things like the toolbar when adding structures.
        /// </summary>
        public event EventHandler<TileClickArgs> TileClickedEvent;

        public List<MapTile> Tiles { get; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Accumulate all the stats contributions (e.g. the CO2, the profits, etc.)
        /// of al the tiles in the map at the current state.
        /// </summary>
        /// <returns>The overall stats contribution.</returns>
        public Stats GetStatsContribution()
        {
            // Get stats from its tiles.
            throw new System.NotImplementedException();
        }
    }
}
