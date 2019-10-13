using System;
using System.Collections.Generic;
using Game.Story;
using Game.Story.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story
{
    public class StoryManagerTemp : MonoBehaviour
    {
        private enum RandomEvents {}
        public enum StoryEvents {INITIAL_THANTEC}
        [SerializeField] 
        protected City city;
        [SerializeField] 
        private ToolBar toolbar;
        [SerializeField] 
        private Button endTurnButton;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private GameObject storyManagerGameObject;

        private EventFactory factory;
        private Queue<int> storyQueue;
        private StoryEvents nextEvent;
        private StoryEvent storyEvent;
        
        void Start()
        {
            factory = new EventFactory();
            // Create a queue for the story events
            storyQueue = new Queue<int>(new[] {4,8,12 });
            nextEvent = StoryEvents.INITIAL_THANTEC;
            city.NextTurnEvent += HandleTurnEvent;
        }

        private void HandleTurnEvent()
        {
            if (city.Turn == storyQueue.Peek())
            {
                // Create new story event here
                storyEvent = factory.CreateStoryEvent(nextEvent);
                // Get rid of the first thing in the queue
                storyQueue.Dequeue();
                CreatePopUp();
            }
        }

        private void CreatePopUp()
        {
            EventPopUp popUp;
            if (storyEvent != null && !city.HasEnded)
            {
                popUp = storyManagerGameObject.AddComponent<EventPopUp>();
                popUp.name = "event-pop-up";
                popUp.Canvas = canvas;
                popUp.CityMap = city.Map;
                canvas.SetActive(true);
                storyEvent.City = city;
                storyEvent.ToolBar = toolbar;
                storyEvent.EndButton = endTurnButton;
                popUp.StoryEvent = storyEvent;
                popUp.Create();
            }
        }
    }
    
    class EventFactory
    {

        public EventFactory()
        {
            
        }
        private StoryEvent storyEvent;
        public StoryEvent CreateStoryEvent(StoryManagerTemp.StoryEvents storyEvents)
        {
            switch (storyEvents)
            {
                case StoryManagerTemp.StoryEvents.INITIAL_THANTEC:
                    return new ThanTecRequest();
            }

            return null;
        }

        public StoryEvent CreateRandomEvent()
        {
            return null;
        }
    }
}
