using UnityEngine;

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
        
        private const string TITLE = "Pushing Harder!";
        private const string DESCRIPTION = "Secretary requests that you shut down factories, since they’re big contributors to emissions.";
        public override void OnYesClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.BAN_THE_CARS_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<BanTheCarsRequest>());
            
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.BAN_THE_CARS_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<BanTheCarsRequest>());
        }
    }
}