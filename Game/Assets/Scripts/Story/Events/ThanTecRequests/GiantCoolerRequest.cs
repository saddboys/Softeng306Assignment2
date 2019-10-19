using System;
using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story.Events
{
    public class GiantCoolerRequest : StoryRequest
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }
        
                
        private const string TITLE = "Hi-Tech Cooler!";
        private const string DESCRIPTION = "Build ThanTec's new cooling technology?";

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/giantcooler"); }
        }

        public override Queue<string> Dialogues
        {
            get { return dialogMessages; }
        }
        private Queue<string> dialogMessages = new Queue<string>(new[] { 
            "Excellent news!", 
            "ThanTec has informed me that all our investments has yielded a new technology to bring temperatures down in the area.",
            "Kind of like an ‘outdoors air conditioner’, they said.",
            "We just need to spend money to install them in the area."}); 
        
        public override StoryManager StoryManager 
        {
            get { return storyManager; }
            set
            {
                storyManager = value;
            }
        }
        private StoryManager storyManager;

        public override void OnYesClick()
        {
            if (StoryManager.city.Stats.Temperature >= 0.5)
            {
                StoryManager.city.Stats.Temperature -= 0.5;
            } 
            else 
            {
                StoryManager.city.Stats.Temperature = 0;
            }
            storyManager.toolbar.BuiltEvent += OnBuild;

            // Go to tech ending
            StoryManager.StoryEnding = (int) StoryManager.StoryEndings.TECH_ENDING;
            
            StoryManager.toolbar.gameObject.SetActive(false);
            StoryManager.endTurnButton.interactable = false;
            
            StoryManager.toolbar.CurrentFactory = new GiantCoolerFactory(StoryManager.city);
            CreateHelpPopup();
            Destroy(StoryManager.storyManagerGameObject.GetComponent<GiantCoolerRequest>());
        }

        public override void OnNoClick()
        {
            // Go to non-tech ending
            StoryManager.StoryEnding = (int) StoryManager.StoryEndings.REVISIONIST_ENDING;
            Destroy(StoryManager.storyManagerGameObject.GetComponent<GiantCoolerRequest>());
        }

        private void OnBuild()
        {
            StoryManager.toolbar.gameObject.SetActive(true);
            StoryManager.endTurnButton.interactable = true;
            StoryManager.toolbar.CurrentFactory = null;
            Destroy(StoryManager.canvas.transform.Find("help").gameObject);
            StoryManager.toolbar.BuiltEvent -= OnBuild;
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
            titleText.text = "Place the giant cooler!";
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