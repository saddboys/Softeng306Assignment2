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
        private const string DESCRIPTION = "Build a research facility for ThanTec to investigate climate solutions.";
        public override Sprite EventImage { get; }
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
        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { "please build","o pls"}); 

        private void OnBuild()
        {
            StoryManager.toolbar.gameObject.SetActive(true);
            StoryManager.endTurnButton.interactable = true;
            StoryManager.toolbar.CurrentFactory = null;
            Destroy(StoryManager.canvas.transform.Find("help").gameObject);
            StoryManager.toolbar.BuiltEvent -= OnBuild;
        }
        
        public override void OnYesClick()
        {
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            
            // Build power plant
            StoryManager.toolbar.CurrentFactory = new PowerPlantFactory();
            CreateHelpPopup();
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIMME_MONEY_REQUEST;
            
            // Maybe change stats somewhere (stats, CO2 etc.)
            
            
            Destroy(StoryManager.storyManagerGameObject.GetComponent<ResearchFacilityRequest>());
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.GIMME_MONEY_REQUEST;
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