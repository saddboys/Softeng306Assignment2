using System.Collections.Generic;
using UnityEngine;
using Game.Story;
using Game.CityMap;

namespace Game.Story.Events.VerdantLandRequests
{
    public class PushingHarderRequest: StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }

        public override Sprite EventImage { get; }
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "Mayor, I have an idea.",
            "Most of the carbon emissions in the area come from factories.",
            "If we shut down all the factories, we can dramatically reduce emissions.", 
            "What do you think?"});

        private const string TITLE = "Pushing Harder!";
        private const string DESCRIPTION = "Shut down all factories to reduce emissions?";
        public override void OnYesClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.BAN_THE_CARS_REQUEST;
            
            DestroyFactories();
            
            Destroy(StoryManager.storyManagerGameObject.GetComponent<PushingHarderRequest>());
            
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.BAN_THE_CARS_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<PushingHarderRequest>());
        }

        /// <summary>
        /// Removes all factories from the map
        /// </summary>
        private void DestroyFactories()
        {
            MapTile[] tiles = StoryManager.city.Map.Tiles;
            foreach (var tile in tiles)
            {
                if (tile != null && tile.Structure != null && tile.Structure.GetType() == typeof(Factory))
                {
                    // tile.Structure.Unrender();
                    new DemolishFactory(StoryManager.city).BuildOnto(tile);
                    // StoryManager.city.Stats.UpdateContribution(tile.Structure.GetStatsChangeOnDemolish());
                    // tile.Structure = null;
                }
            }
        }
    }
}