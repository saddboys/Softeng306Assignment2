using System.Collections.Generic;
using UnityEngine;

namespace Game.Story.Events
{
    /// <summary>
    /// This event occurs when the user has not met the necessary requirements for
    /// the initial thantec event.
    /// </summary>
    public class MissedOpportunityEvent : StoryEvent
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/missed"); }
        }

        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] {
            "ThanTec has arrived! However, they saw our low income.",
            "They thought that it would be a poor investment to come here"}); 

        private const string TITLE = "Missed Opportunity";
        private const string DESCRIPTION = "You have missed the Thantec build as you haven't met the requirements";
        public override void OnYesClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.PUSHING_HARDER_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CreateThanTecRequest>());
        }
    }
}