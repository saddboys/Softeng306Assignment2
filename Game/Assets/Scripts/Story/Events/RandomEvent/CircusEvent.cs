using System.Collections.Generic;
using UnityEngine;

namespace Game.Story.Events
{
    public class CircusEvent : StoryEvent
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

        private const string TITLE = "Circus event";
        private const string DESCRIPTION = "The circus is in town. :D";
        public override void OnYesClick()
        {
            StoryManager.city.Stats.Reputation += 1;
            StoryManager.city.Stats.ElectricCapacity -= 2; // maybe only temporarily decrease until they leave?
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CircusEvent>());
        }
    }
}