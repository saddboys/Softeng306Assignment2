using Game.CityMap;
using UnityEngine;

namespace Game.Story.Events
{
    public class MoreHouseRequest : StoryRequest
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

        private const string TITLE = "More House Request";
        private const string DESCRIPTION = "You are rich. Please build more houses.";
        private const Sprite SPRITE = null;
        public override void OnYesClick()
        {
            ToolBar.gameObject.SetActive(false);
            EndButton.interactable = false;
            ToolBar.currentFactory = new HouseFactory();
        }

        public override void OnNoClick()
        {
            throw new System.NotImplementedException();
        }
    }
}