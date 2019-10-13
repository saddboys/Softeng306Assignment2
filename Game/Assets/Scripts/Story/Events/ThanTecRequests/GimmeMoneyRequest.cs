using UnityEngine;

namespace Game.Story.Events
{
    public class GimmeMoneyRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Give me money";
        private const string DESCRIPTION = "Fund tech project";
        public override Sprite EventImage { get; }
        public override void OnYesClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIANT_COOLER_REQUEST;
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
        }
    }
}