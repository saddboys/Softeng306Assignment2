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
        
                
        private const string TITLE = "Giant Cooler!";
        private const string DESCRIPTION = "Choose whether to use the machine tech thing to localize coolness and solve all your climate woes in this area only.";
        public override Sprite EventImage { get; }
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { "please build","o pls"}); 

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