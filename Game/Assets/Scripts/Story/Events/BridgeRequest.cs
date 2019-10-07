using UnityEngine;

namespace Game.Story.Events
{
    public class BridgeRequest : StoryRequest
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

        private const string TITLE = "Bridge Request";
        private const string DESCRIPTION = " Bridge description here";
        private const Sprite SPRITE = null;
        public override void OnNoClick()
        {
            Debug.Log("BRIDGE REQUEST NO");
        }

        public override void OnYesClick()
        {
            Debug.Log("BRIDGE REQUEST YES");
        }
    }
}