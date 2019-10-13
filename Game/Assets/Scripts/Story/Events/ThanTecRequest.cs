using UnityEngine;

namespace Game.Story.Events
{
    public class ThanTecRequest : StoryRequest
    {
        private ThanTec.ThanTec thanTec;
        
        /// <summary>
        /// The ThanTec request will need a thanTec object
        /// </summary>
        /// <param name="thanTec"></param>
        public ThanTecRequest()
        {
            this.thanTec = thanTec;
        }
        public override string Title { get; }
        public override string Description { get; }
        public override Sprite EventImage { get; }
        
        private const string TITLE = "Here comes the science!";
        private const string DESCRIPTION = "A new research facility wishes to build their office in XXX. Do you accept?";
        public override void OnYesClick()
        {
            //TODO: Allow the user to build the thanTec building
            
            
            // Set the next story event here
        }

        public override void OnNoClick()
        {
          
            // Set the no next story event here
        }
    }
}