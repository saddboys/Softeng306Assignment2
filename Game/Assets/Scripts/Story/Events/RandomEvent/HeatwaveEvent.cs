using System.Collections.Generic;
using Game.Story;
using UnityEngine;

namespace Story.Events.RandomEvent
{
    public class HeatwaveEvent : StoryEvent
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
            get { return Resources.LoadAll<Sprite>("EventSprites/circus2")[0]; }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "Heatwave";
        private const string DESCRIPTION = "There is a heatwave";
        public override void OnYesClick()
        {
            // Dry up water?
            // Decrease happiness maybe
            
        }
    }
}