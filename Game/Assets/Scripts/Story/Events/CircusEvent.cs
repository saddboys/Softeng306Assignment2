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
            get { return SPRITE; }
        }

        private const string TITLE = "Circust event";
        private const string DESCRIPTION = "The circus is in town. :D";
        private const Sprite SPRITE = null;
        public override void OnYesClick()
        {
            City.Stats.Reputation += 1;
            City.Stats.ElectricCapacity -= 2; // maybe only temporarily decrease until they leave?
            Debug.Log("CIRCUS EVENT ACTIVATED");
        }
    }
}