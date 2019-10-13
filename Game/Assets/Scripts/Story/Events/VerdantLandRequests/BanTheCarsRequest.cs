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
        
                
        private const string TITLE = "Ban the cars!";
        private const string DESCRIPTION = "Secretary requests that you get rid of cars, to reduce pollution from them.";
        public override Sprite EventImage { get; }
        public override Queue<string> Dialogues { get; }

        public override void OnYesClick()
        {

            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CallingOnLifestyleRequest>());
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CallingOnLifestyleRequest>());
        }
    }
}