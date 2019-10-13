using UnityEngine;

namespace Game.Story.Events
{
    public class CreateThanTecRequest : StoryRequest
    {
        /// <summary>
        /// The ThanTec request will need a thanTec object
        /// </summary>
        /// <param name="thanTec"></param>
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }

        public override Sprite EventImage { get; }
        
        private const string TITLE = "Here comes the science!";
        private const string DESCRIPTION = "A new research facility wishes to build their office in XXX. \nDo you accept?";
        public override void OnYesClick()
        {
            //TODO: Allow the user to build the thanTec building
            
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.RESEARCH_FACILITY_REQUEST;
            // Set the next story event here
        }

        public override void OnNoClick()
        {
          
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.PUSHING_HARDER_REQUEST;
            // Set the no next story event here
        }
    }
}