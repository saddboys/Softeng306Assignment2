using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story.Events
{
    /// <summary>
    /// Requests the user to build a research facility for thantec
    /// </summary>
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
            if (StoryManager.city.Stats.Wealth > 3000 && StoryManager.city.Stats.ElectricCapacity > 1) return true;
            else
            {
                StoryManager.NextStoryEvent = EventFactory.StoryEvents.PUSHING_HARDER_REQUEST;
                return false;
            }
        }

        private void OnBuild()
        {
            StoryManager.toolbar.gameObject.SetActive(true);
            StoryManager.endTurnButton.interactable = true;
            StoryManager.toolbar.CurrentFactory = null;
            StoryManager.city.Stats.transform.Find("Menu Button").gameObject.SetActive(true);
            Destroy(StoryManager.canvas.transform.Find("help").gameObject);
            StoryManager.toolbar.BuiltEvent -= OnBuild;
        }
        
        public override void OnYesClick()
        {
            storyManager.toolbar.BuiltEvent += OnBuild;
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            StoryManager.city.Stats.transform.Find("Menu Button").gameObject.SetActive(false);
            StoryManager.toolbar.CurrentFactory = new ResearchFacilityFactory(StoryManager.city);
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
            i.sprite = Resources.Load<Sprite>("EventSprites/Thaleah_DemoBackground");
            helpPanel.transform.SetParent(StoryManager.canvas.transform, false);

            GameObject helpDescription = new GameObject("Title");
            Text titleText = helpDescription.AddComponent<Text>();
            titleText.text = "Place the new research facility!";
            titleText.font = Resources.Load<Font>("Fonts/visitor1");
            titleText.color = new Color32(219, 219, 219,255);
            titleText.fontSize = 15;
            titleText.alignment = TextAnchor.MiddleCenter;
            helpDescription.transform.SetParent(helpPanel.transform,false);
            helpDescription.GetComponent<RectTransform>().sizeDelta = new Vector2(200,100);
            helpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(250,50);
            helpPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,80);
        }
    }
}