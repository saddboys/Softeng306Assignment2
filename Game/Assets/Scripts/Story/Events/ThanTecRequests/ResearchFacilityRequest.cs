using UnityEngine;

namespace Game.Story.Events
{
    public class ResearchFacilityRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Research Facility";
        private const string DESCRIPTION = "Build a research facility for ThanTec to investigate climate solutions.";
        public override Sprite EventImage { get; }
        public override void OnYesClick()
        {
            // Prevent the tech ending somehow.
            
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIMME_MONEY_REQUEST;
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIMME_MONEY_REQUEST;
        }
    }
}