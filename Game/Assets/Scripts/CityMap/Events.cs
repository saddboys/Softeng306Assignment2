using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CityMap
{
    public class TileClickArgs : EventArgs
    {
        public TileClickArgs(MapTile tile)
        {
            this.Tile = tile;
        }
        public MapTile Tile { get; }
    }
}
