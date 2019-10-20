using System.Collections.Generic;
using UnityEngine;
using Game.CityMap;

namespace Game.Story.Events.VerdantLandRequests
{
    /// <summary>
    /// A story request which asks the users to limit the energy usage for the residents
    /// </summary>
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
        private const string DESCRIPTION = "Limit resident's energy usage? This will make happiness go down.";

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
            // ending goes here

            // Decrease happiness, decrease population
            StoryManager.city.Stats.Reputation -= 20;
            StoryManager.city.Stats.Population -= 10;

            // Decrease energy usage and reduce carbon emissions of houses
            House.StructElectricity += 1;
            House.StructCO2 -= 1;

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