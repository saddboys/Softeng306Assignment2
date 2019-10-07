using UnityEngine;

namespace Game.Story.Events
{
    public class TowerRequest : StoryRequest
    {
        public override string Title
        {
            get
            {
                return TITLE;
            }
        }
        public override string Description
        {
            get { return DESCRIPTION; }
        }
        public override Sprite EventImage
        {
            get { return SPRITE; }
        }

        private const string TITLE = "Tower Request";
        private const string DESCRIPTION = " Tower request description";
        private const Sprite SPRITE = null;
        
        // need to somehow change stats through here.
        public override void OnNoClick()
        {
            Debug.Log("TOWER REQUEST NO");
        }

        public override void OnYesClick()
        {
            Debug.Log("TOWER REQUEST YES");
        }
        
    }
}