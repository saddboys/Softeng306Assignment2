using System.Collections.Generic;
using Game.CityMap;
using Game.Story;
using UnityEngine;
using Random = System.Random;
using Terrain = Game.CityMap.Terrain;

namespace Story.Events.RandomEvent
{
    public class HeatwaveEvent : StoryEvent
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/heatwave"); }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "Heatwave";
        private const string DESCRIPTION = "A heatwave occurred. Some of the water has dried up. \n " +
                                           "Reputation decreased and temperature increased.";
        public override void OnYesClick()
        {
            // Dry up water
            FindWaterTerrain();
            StoryManager.city.Stats.Reputation -= 5;
            StoryManager.city.Stats.Temperature += 0.5;
            
        }

        /// <summary>
        /// Finds water terrains to remove 
        /// </summary>
        private void FindWaterTerrain()
        {
            Random random = new Random();
            MapTile[] tiles = StoryManager.city.Map.Tiles;
            foreach (var tile in tiles)
            {
                // Randomly dry up patches of water
                if (tile != null && tile.Terrain.TerrainType == Terrain.TerrainTypes.Ocean && random.Next(0,5) == 2)
                {
                    tile.Terrain = new Terrain(Terrain.TerrainTypes.Desert);
                }
            }
        }
    }
}