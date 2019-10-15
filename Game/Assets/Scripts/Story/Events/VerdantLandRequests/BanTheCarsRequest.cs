using System.Collections.Generic;
using UnityEngine;

namespace Game.Story.Events.VerdantLandRequests
{
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
        public override Sprite EventImage { get; }
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "“Cars are big emitters of carbon dioxide, right? " +
            "What if we get rid of cars? The town is fairly small. People can walk or bike.”"}); 

        public override void OnYesClick()
        {

            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            
            // Decrease happiness, reduce carbon emissions and also affects money 
            Destroy(StoryManager.storyManagerGameObject.GetComponent<BanTheCarsRequest>());
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<BanTheCarsRequest>());
        }
    }
}