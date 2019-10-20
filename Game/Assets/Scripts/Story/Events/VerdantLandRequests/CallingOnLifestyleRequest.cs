using System.Collections.Generic;
using UnityEngine;
using Game.CityMap;

namespace Game.Story.Events.VerdantLandRequests
{
    public class CallingOnLifestyleRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Calling on Lifestyle";
        private const string DESCRIPTION = "Limit energy usage?\nHouses use 1 less energy and produce 1 less CO2.";

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/lifestyle"); }
        }

        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "If we limit the energy consumption of our residents, that should help keep temperatures down.",
            "They might not be very happy about this, but the environment comes first, right?"});

        public override void OnYesClick()
        {
            // Decrease happiness, decrease population
            StoryManager.city.Stats.Reputation -= 20;
            StoryManager.city.Stats.Population -= 10;

            // Decrease energy usage and reduce carbon emissions of houses
            House.StructElectricity += 1;
            House.StructCO2 -= 1;

            // Go to non-tech ending
            StoryManager.StoryEnding = (int) StoryManager.StoryEndings.REVISIONIST_ENDING;

            Destroy(StoryManager.storyManagerGameObject.GetComponent<CallingOnLifestyleRequest>());
        }

        public override void OnNoClick()
        {
            // Increase CO2 emissions from houses
            House.StructCO2 += 1;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CallingOnLifestyleRequest>());
        }
    }
}