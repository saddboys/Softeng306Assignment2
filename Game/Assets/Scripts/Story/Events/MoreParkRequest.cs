using Game.CityMap;
using UnityEngine;

namespace Game.Story.Events
{
    public class MoreParkRequest : StoryRequest
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
            get { return Resources.LoadAll<Sprite>("EventSprites/park2")[0]; }
        }

        public override ToolBar ToolBar
        {
            get { return toolBar; }
            set
            {
                toolBar = value;
                toolBar.BuiltEvent += OnBuild;
            }
        }
        
        private ToolBar toolBar;

        private const string TITLE = "More Park Request";
        private const string DESCRIPTION = "You are growing in population. Build a public park.";

        public override void OnYesClick()
        {
            ToolBar.gameObject.SetActive(false);
            EndButton.interactable = false;
            ToolBar.CurrentFactory = new ParkFactory();
        }
        
        

        private void OnBuild()
        {
            ToolBar.gameObject.SetActive(true);
            EndButton.interactable = true;
            ToolBar.CurrentFactory = null;
            ToolBar.BuiltEvent -= OnBuild;
            
            
        }
        public override void OnNoClick()
        {
            City.Stats.Reputation -= 0.75;
        }
    }
}