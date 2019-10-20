using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Story.Events.VerdantLandRequests
{
    /// <summary>
    /// A story request which asks the user to ban the cars
    /// </summary>
    public class BanTheCarsRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Ban the Cars!";
        private const string DESCRIPTION = "Get rid of cars, to reduce pollution from them?";

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/banthecars"); }
        }

        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "Cars are big emitters of carbon dioxide, right?",
            "What if we get rid of the cars?",
            "The town is fairly small. People can walk or bike."}); 

        public override void OnYesClick()
        {

            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            
            // Decrease happiness, reduce carbon emissions and also affects money 
            StoryManager.city.Stats.Reputation -= 10;
            StoryManager.city.Stats.CO2 -= 25;
            if (StoryManager.city.Stats.Wealth >= 2000) 
            {
                StoryManager.city.Stats.Wealth -= 2000;
            } 
            else
            {
                StoryManager.city.Stats.Wealth = 0;
            }

            Destroy(StoryManager.storyManagerGameObject.GetComponent<BanTheCarsRequest>());
            
            // Set ending route
            StoryManager.StoryEnding = (int) StoryManager.StoryEndings.REVISIONIST_ENDING;
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<BanTheCarsRequest>());
            
            // Set ending route
            StoryManager.StoryEnding = (int) StoryManager.StoryEndings.NEUTRAL_ENDING;
        }
    }
}