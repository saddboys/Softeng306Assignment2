using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.UI;

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
            get { return Resources.LoadAll<Sprite>("EventSprites/morehouse")[0]; }
        }

        public override Queue<string> Dialogues { get; }

        private StoryManager storyManager;
        public override StoryManager StoryManager 
        {
            get { return storyManager; }
            set
            {
                storyManager = value;
            }
        }


        private const string TITLE = "More House Request";
        private const string DESCRIPTION = "You are rich. Please build more houses.";
        public override void OnYesClick()
        {
            storyManager.toolbar.BuiltEvent += OnBuild;
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            StoryManager.city.Stats.transform.Find("Menu Button").gameObject.SetActive(false);
            
            CreateHelpPopup();
            StoryManager.toolbar.CurrentFactory = new HouseFactory(StoryManager.city);
            Destroy(StoryManager.storyManagerGameObject.GetComponent<MoreHouseRequest>());
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
        public override void OnNoClick()
        {
            StoryManager.city.Stats.Reputation -= 0.5;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<MoreHouseRequest>());
        }
        
        private void CreateHelpPopup()
        {
            GameObject helpPanel = new GameObject("help");
            helpPanel.AddComponent<CanvasRenderer>();
            Image i = helpPanel.AddComponent<Image>();
            i.sprite = Resources.Load<Sprite>("EventSprites/Thaleah_DemoBackground");
            helpPanel.transform.SetParent(StoryManager.canvas.transform, false);
            helpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(150,50);

            GameObject helpDescription = new GameObject("Title");
            Text titleText = helpDescription.AddComponent<Text>();
            titleText.text = "Place the building";
            titleText.font = Resources.Load<Font>("Fonts/visitor1");
            titleText.color = new Color32(219, 219, 219,255);
            titleText.fontSize = 15;
            titleText.alignment = TextAnchor.MiddleCenter;
            helpDescription.transform.SetParent(helpPanel.transform,false);
            helpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(150,50);
            helpPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,80);
        }
    }
}