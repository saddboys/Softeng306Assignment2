using System.Collections.Generic;
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
            get { return Resources.LoadAll<Sprite>("EventSprites/house")[0]; }
        }

        public override Queue<string> Dialogues { get; }

        private StoryManager storyManager;
        public override StoryManager StoryManager 
        {
            get { return storyManager; }
            set
            {
                storyManager = value;
                storyManager.toolbar.BuiltEvent += OnBuild;
            }
        }


        private const string TITLE = "More House Request";
        private const string DESCRIPTION = "You are rich. Please build more houses.";
        public override void OnYesClick()
        {
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            StoryManager.toolbar.CurrentFactory = new HouseFactory();
            Destroy(StoryManager.storyManagerGameObject.GetComponent<MoreHouseRequest>());
        }
        
        

        private void OnBuild()
        {
            StoryManager.toolbar.gameObject.SetActive(true);
            StoryManager.endTurnButton.interactable = true;
            StoryManager.toolbar.CurrentFactory = null;
            StoryManager.toolbar.BuiltEvent -= OnBuild;
        }
        public override void OnNoClick()
        {
            StoryManager.city.Stats.Reputation -= 0.5;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<MoreHouseRequest>());
        }
    }
}