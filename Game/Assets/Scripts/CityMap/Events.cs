using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CityMap
{
    public class StructureBuildArgs : EventArgs
    {
        public StructureBuildArgs(Structure structure, MapTile tile)
        {
            this.Structure = structure;
            this.Tile = tile;
        }

        public Structure Structure { get; }
        public MapTile Tile { get; }
    }

    public class StructureBuildRequestArgs : StructureBuildArgs
    {
        public StructureBuildRequestArgs(Structure structure, MapTile tile) : base(structure, tile)
        {
        }

        public bool IsCancelled { get; private set; }
        public string CancelledReason { get; private set; }

        public void Cancel(String reason)
        {
            IsCancelled = true;
            CancelledReason = reason;
        }
    }

    public class TileClickArgs : EventArgs
    {
        public TileClickArgs(MapTile tile)
        {
            this.Tile = tile;
        }
        public MapTile Tile { get; }
    }
}
