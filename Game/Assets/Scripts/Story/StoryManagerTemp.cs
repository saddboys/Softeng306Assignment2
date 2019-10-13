using System;
using System.Collections.Generic;
using Game.Story;
using Game.Story.Events;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.Story
{
    public class StoryManagerTemp : MonoBehaviour
    {
        public enum RandomEvents {ELECTRIC_VEHICLES_REQUEST, CONDITIONAL_REQUEST_HOUSE}
        public enum StoryEvents {INITIAL_THANTEC, RESEARCH_FACILITY_REQUEST, PUSHING_HARDER_REQUEST}
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
        private StoryEvents nextStoryEvent;
        private StoryEvent storyEvent;
        private Random random;
        private List<RandomEvents> eventPool;
        void Start()
        {
            factory = new EventFactory();
            random = new Random();
            // Create a queue for turn number of the story events
            storyQueue = new Queue<int>(new[] {4,8,12 });
            nextStoryEvent = StoryEvents.INITIAL_THANTEC;
            city.NextTurnEvent += HandleTurnEvent;
            
            // Generate the event pool
            GeneratePool();

            city.Stats.ChangeEvent += ChangePoolEvents;
        }

        private void ChangePoolEvents()
        {
            
        }
        /// <summary>
        /// Generates the pool of events.
        /// </summary>
        private void GeneratePool()
        {
            eventPool = new List<RandomEvents>();
            RandomEvents[] events = (RandomEvents[])Enum.GetValues(typeof(RandomEvents));
            foreach(var eventObj in events)
            {
                string eventString = eventObj.ToString();
                if (!eventString.Contains("Conditional"))
                {
                    // Set all cooldowns to be 0 initially.
                    eventPool.Add(eventObj);
                }
            }
        }

        private void HandleTurnEvent()
        {
            if (city.Turn == storyQueue.Peek())
            {
                // Create new story event here
                storyEvent = factory.CreateStoryEvent(nextStoryEvent);
                // Get rid of the first thing in the queue
                storyQueue.Dequeue();
                CreatePopUp();
            }
            else
            {
                // Events have a 10% chance of popping up
                if (random.Next(0, 10) == 2)
                {
                    RandomEvents randomEvent = eventPool[random.Next(0,eventPool.Count)];
                    // Randomly spawn events from the event pool
                    storyEvent = factory.CreateRandomEvent(randomEvent);
                    CreatePopUp();
                }
            }
            
            // Change whats in the pool 
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
                storyEvent.NextEvent = nextStoryEvent;
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

        public StoryEvent CreateRandomEvent(StoryManagerTemp.RandomEvents randomEvents)
        {
            switch (randomEvents)
            {
                case StoryManagerTemp.RandomEvents.CONDITIONAL_REQUEST_HOUSE:
                    return new MoreHouseRequest();
            }
            return null;
        }
    }
}
