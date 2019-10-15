using System.Collections.Generic;
using UnityEngine;

namespace Game.Story.Events
{
    public class GiantCoolerRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Hi-Tech Cooler!";
        private const string DESCRIPTION = "Build ThanTec's new cooling technology?";
        public override Sprite EventImage { get; }
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "“Excellent news! ThanTec has informed me that all our investments has yielded " +
            "a new technology to bring temperatures down in an area. Kind of like an " +
            "‘outdoors air conditioner’, they said. We just need to spend money to install them in the area.”"}); 

        public override void OnYesClick()
        {
            // will go to ending
        }

        public override void OnNoClick()
        {
            // no ending? idk
        }
    }
}