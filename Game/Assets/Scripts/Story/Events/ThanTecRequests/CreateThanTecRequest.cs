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

        public override Sprite EventImage { get; }

        public override Queue<string> Dialogues
        {
            get
            {
                return dialogMessages;
            }
        } 
        private const string TITLE = "Here comes the science!";
        private const string DESCRIPTION = "A new research facility wishes to build their office in XXX. \nDo you accept?";
        [TextArea(3,10)]
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "FUIBGIBGUbfusfbeiubreugbrieugbiuergbeiurbgierbguierbgiuerbgiuergiureiguebruigeriugbeiurgbiuergbiuerbgiuerbgiuerbgerugiberiguberg" +
            "eifberiugbiuergbiuergbuierbguiebguiergb",
            "abeifberiugbiuergbiuergbuierbguiebguierg" +
            "eifberiugbiuergbiuergbuierbguiebguiergb" +
            "eifberiugbiuergbiuergbuierbguiebguiergb" +
            "eifberiugbiuergbiuergbuierbguiebguiergbb",
            "111111111111111111111111111111111111111111" +
            "1111111111111111111111111111111111111111111"}); 
        
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
            
            //TODO: Allow the user to build the thanTec building
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            
            // Placeholder building for now
            StoryManager.toolbar.CurrentFactory = new HouseFactory();
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