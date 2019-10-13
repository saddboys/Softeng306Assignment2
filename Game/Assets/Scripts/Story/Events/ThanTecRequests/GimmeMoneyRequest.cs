using UnityEngine;

namespace Game.Story.Events
{
    public class GimmeMoneyRequest : StoryRequest
    {
        public override string Title { get; }
        public override string Description { get; }
        public override Sprite EventImage { get; }
        public override void OnYesClick()
        {
            throw new System.NotImplementedException();
        }

        public override void OnNoClick()
        {
            throw new System.NotImplementedException();
        }
    }
}