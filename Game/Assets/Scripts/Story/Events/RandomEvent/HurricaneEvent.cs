using System.Collections.Generic;
using Game.CityMap;
using Game.Story;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Story.Events.RandomEvent
{
    public class HurricaneEvent : StoryEvent
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
            get { return Resources.LoadAll<Sprite>("EventSprites/circus2")[0]; }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "Hurricane";
        private const string DESCRIPTION = "Oh no! A hurricane happened :(";
        public override void OnYesClick()
        {
            // Destroy random buildings
            DestroyBuildings();
            // Happiness goes down
            StoryManager.city.Stats.Reputation -= 10;
        }

        private void DestroyBuildings()
        {
            Random random = new Random();
            MapTile[] tiles = StoryManager.city.Map.Tiles;
            foreach (var tile in tiles)
            {
                if (tile.Structure != null && random.Next(0,5) == 1)
                {
                    tile.Structure.Unrender();
                    StoryManager.city.Stats.UpdateContribution(tile.Structure.GetStatsChangeOnDemolish());
                    tile.Structure = null;
                }
            }
        }
    }
}