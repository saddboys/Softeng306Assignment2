using System.Collections.Generic;
using UnityEngine;

namespace Game.Story.Events.RandomEvent
{
    public class FloodEvent : StoryEvent
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
            get { return SPRITE; }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "FLOOD";
        private const string DESCRIPTION = "Flooding happened bad luck :(";
        private const Sprite SPRITE = null;
        public override void OnYesClick()
        {
            Debug.Log("FLOOD EVENT ACTIVATED");
        }
    }
}