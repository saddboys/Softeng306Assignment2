using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story.Events
{
    [System.Serializable]
    public class CreateThanTecRequest : StoryRequest
    {
        /// <summary>
        /// The ThanTec request will need a thanTec object
        /// </summary>
        /// <param name="thanTec"></param>
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
            get { return Resources.Load<Sprite>("EventSprites/thantec"); }
        }

        public override Queue<string> Dialogues
        {
            get
            {
                return dialogMessages;
            }
        } 
        private const string TITLE = "ThanTec";
        private const string DESCRIPTION = "ThanTec wishes you to build their office. \nDo you accept?";
        [TextArea(3,10)]
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "You have a request from the company ThanTec for a new office.",
            "ThanTec! Imagine how much better off we would be if such a company was here.",
            "What should we do?"}); 
        
        private StoryManager storyManager;
        public override StoryManager StoryManager 
        {
            get { return storyManager; }
            set
            {
                storyManager = value;
            }
        }

        public override bool ConditionMet() {
            if (storyManager.city.Stats.Wealth > 250 && storyManager.city.Stats.ElectricCapacity > 0) return true;
            else return false;
        }
        
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
            storyManager.toolbar.BuiltEvent += OnBuild;
            // Mark result of event in route tracker
            StoryManager.StoryChoices.Add((int) EventFactory.StoryEvents.INITIAL_THANTEC, true);
            
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            
            StoryManager.toolbar.CurrentFactory = new ThantecFactory(StoryManager.city);
            CreateHelpPopup();
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.RESEARCH_FACILITY_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CreateThanTecRequest>());
        }

        public override void OnNoClick()
        {
            StoryManager.NextStoryEvent = EventFactory.StoryEvents.PUSHING_HARDER_REQUEST;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<CreateThanTecRequest>());
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
            titleText.text = "Place the ThanTec building!";
            titleText.font = Resources.Load<Font>("Fonts/Electronic");
            titleText.color = new Color32(219, 219, 219,255);
            titleText.fontSize = 15;
            titleText.alignment = TextAnchor.MiddleCenter;
            helpDescription.transform.SetParent(helpPanel.transform,false);
            helpDescription.GetComponent<RectTransform>().sizeDelta = new Vector2(200,100);
            helpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(200,50);
            helpPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,80);
        }
    }
}