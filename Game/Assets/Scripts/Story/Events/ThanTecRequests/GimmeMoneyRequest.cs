using System.Collections.Generic;
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
        
                
        private const string TITLE = "Invest in ThanTec";
        private const string DESCRIPTION = "Fund ThanTec's climate research?";
        public override Sprite EventImage { get; }
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "“ThanTec wants $8,000 for their climate research.”",
            "“Personally, I’m not so sure if it’s a good investment.”",
            "“Your thoughts?”"}); 

        public override void OnYesClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIANT_COOLER_REQUEST;
            
            // Change the money
            StoryManager.city.Stats.Wealth -= 8000;

            Destroy(StoryManager.storyManagerGameObject.GetComponent<GimmeMoneyRequest>());
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.CALLING_ON_LIFESTYLE_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<GimmeMoneyRequest>());
        }
    }
}