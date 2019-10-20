using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story.Events
{
    public class ResearchFacilityRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Research Facility";
        private const string DESCRIPTION = "Build ThanTec's research facility?\n$3000k, 10 Electricity";

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/researchfacility"); }
        }

        private StoryManager storyManager;
        public override StoryManager StoryManager 
        {
            get { return storyManager; }
            set
            {
                storyManager = value;
            }
        }
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "Mayor, we have another request from ThanTec.", 
            "They want us to build them a new research facility.",
            "They say it’s for their climate solution project.",
            "What do you think we should do?"}); 

        public override bool ConditionMet() {
            if (StoryManager.city.Stats.Wealth > 3000 && storyManager.city.Stats.ElectricCapacity >= 10) return true;
            else return false;
        }

        private void OnBuild()
        {
            storyManager.city.Stats.Wealth -= 3000;
            storyManager.city.Stats.ElectricCapacity -=10;
            StoryManager.toolbar.gameObject.SetActive(true);
            StoryManager.endTurnButton.interactable = true;
            StoryManager.toolbar.CurrentFactory = null;
            Destroy(StoryManager.canvas.transform.Find("help").gameObject);
            StoryManager.toolbar.BuiltEvent -= OnBuild;
        }
        
        public override void OnYesClick()
        {
            storyManager.toolbar.BuiltEvent += OnBuild;
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;

            StoryManager.toolbar.CurrentFactory = new ResearchFacilityFactory();
            CreateHelpPopup();
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIMME_MONEY_REQUEST;
            
            Destroy(StoryManager.storyManagerGameObject.GetComponent<ResearchFacilityRequest>());
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.BAN_THE_CARS_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<ResearchFacilityRequest>());
        }
        
        private void CreateHelpPopup()
        {
            GameObject helpPanel = new GameObject("help");
            helpPanel.AddComponent<CanvasRenderer>();
            Image i = helpPanel.AddComponent<Image>();
            i.color = Color.white;
            helpPanel.transform.SetParent(StoryManager.canvas.transform, false);
            helpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(100,50);
            helpPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            
            GameObject helpDescription = new GameObject("Title");
            Text titleText = helpDescription.AddComponent<Text>();
            titleText.text = "Place the building";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.color = Color.black;
            titleText.fontSize = 10;
            titleText.alignment = TextAnchor.MiddleCenter;
            helpDescription.transform.SetParent(helpPanel.transform,false);
            helpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(100,50);
            helpPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,80);
        }
    }
}