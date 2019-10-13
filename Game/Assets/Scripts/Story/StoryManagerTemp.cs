//using System;
//using System.Collections.Generic;
//using Game.Story;
//using Game.Story.Events;
//using UnityEngine;
//using UnityEngine.UI;
//using Random = System.Random;
//
//namespace Game.Story
//{
//    public class StoryManagerTemp : MonoBehaviour
//    {
////        public enum RandomEvents {CONDITIONAL_REQUEST_HOUSE}
////        public enum StoryEvents {INITIAL_THANTEC, RESEARCH_FACILITY_REQUEST, PUSHING_HARDER_REQUEST}
//        [SerializeField] 
//        protected City city;
//        [SerializeField] 
//        private ToolBar toolbar;
//        [SerializeField] 
//        private Button endTurnButton;
//        [SerializeField]
//        private GameObject canvas;
//        [SerializeField]
//        private GameObject storyManagerGameObject;
//
//        private EventFactory factory;
//        private Queue<int> storyQueue;
//        private StoryManager.StoryEvents nextStoryEvent;
//        private StoryEvent storyEvent;
//        private Random random;
//        private List<StoryManager.RandomEvents> eventPool;
//        void Start()
//        {
//            factory = new EventFactory();
//            random = new Random();
//            // Create a queue for turn number of the story events
//            storyQueue = new Queue<int>(new[] {4,8,12 });
//            nextStoryEvent = StoryManager.StoryEvents.INITIAL_THANTEC;
//            city.NextTurnEvent += HandleTurnEvent;
//            
//            // Generate the event pool
//            GeneratePool();
//
//            city.Stats.CO2ChangeEvent += HandleCO2ChangeEvent;
//            city.Stats.PopulationChangeEvent += HandlePopulationChangeEvent;
//            city.Stats.ReputationChangeEvent +=HandleReputationChangeEvent;
//            city.Stats.TemperatureChangeEvent += HandleTemperatureChangeEvent;
//            city.Stats.WealthChangeEvent += HandleWealthChangeEvent;
//            city.Stats.ElectricCapacityChangeEvent += HandleElectricCapacityChangeEvent;
//        }
//
//        private void HandleCO2ChangeEvent()
//        {
//            
//        }
//
//        private void HandlePopulationChangeEvent()
//        {
//            if (eventPool.Contains(StoryManager.RandomEvents.CONDITIONAL_REQUEST_HOUSE))
//            {
//                if (city.Stats.Population <= 10000) eventPool.Remove(StoryManager.RandomEvents.CONDITIONAL_REQUEST_HOUSE);
//            }
//            else
//            {
//                if (city.Stats.Population > 10000) eventPool.Add(StoryManager.RandomEvents.CONDITIONAL_REQUEST_HOUSE);
//            }
//            
//        }
//
//        private void HandleReputationChangeEvent()
//        {
//            
//        }
//
//        private void HandleTemperatureChangeEvent()
//        {
//            
//        }
//
//        private void HandleWealthChangeEvent()
//        {
//            
//        }
//
//        private void HandleElectricCapacityChangeEvent()
//        {
//            
//        }
//        /// <summary>
//        /// Generates the pool of events.
//        /// </summary>
//        private void GeneratePool()
//        {
//            eventPool = new List<StoryManager.RandomEvents>();
//            StoryManager.RandomEvents[] events = (StoryManager.RandomEvents[])Enum.GetValues(typeof(StoryManager.RandomEvents));
//            foreach(var eventObj in events)
//            {
//                string eventString = eventObj.ToString();
//                if (!eventString.Contains("Conditional"))
//                {
//                    // Set all cooldowns to be 0 initially.
//                    eventPool.Add(eventObj);
//                }
//            }
//        }
//
//        private void HandleTurnEvent()
//        {
//            if (city.Turn == storyQueue.Peek())
//            {
//                // Create new story event here
//                storyEvent = factory.CreateStoryEvent(nextStoryEvent);
//                // Get rid of the first thing in the queue
//                storyQueue.Dequeue();
//                CreatePopUp();
//            }
//            else
//            {
//                // Events have a 10% chance of popping up
//                if (random.Next(0, 10) == 2)
//                {
//                    StoryManager.RandomEvents randomEvent = eventPool[random.Next(0,eventPool.Count)];
//                    // Randomly spawn events from the event pool
//                    storyEvent = factory.CreateRandomEvent(randomEvent);
//                    CreatePopUp();
//                }
//            }
//            
//            // Change whats in the pool 
//        }
//        
//        
//
//        private void CreatePopUp()
//        {
//            EventPopUp popUp;
//            if (storyEvent != null && !city.HasEnded)
//            {
//                popUp = storyManagerGameObject.AddComponent<EventPopUp>();
//                popUp.name = "event-pop-up";
//                popUp.Canvas = canvas;
//                popUp.CityMap = city.Map;
//                canvas.SetActive(true);
//                storyEvent.City = city;
//                storyEvent.ToolBar = toolbar;
//                storyEvent.EndButton = endTurnButton;
//                storyEvent.NextEvent = nextStoryEvent;
//                popUp.StoryEvent = storyEvent;
//                popUp.Create();
//            }
//        }
//    }
//    
////    class EventFactory
////    {
////        private StoryEvent storyEvent;
////        public StoryEvent CreateStoryEvent(StoryManagerTemp.StoryEvents storyEvents)
////        {
////            switch (storyEvents)
////            {
////                case StoryManagerTemp.StoryEvents.INITIAL_THANTEC:
////                    return new ThanTecRequest();
////            }
////
////            return null;
////        }
////
////        public StoryEvent CreateRandomEvent(StoryManagerTemp.RandomEvents randomEvents)
////        {
////            switch (randomEvents)
////            {
////                case StoryManagerTemp.RandomEvents.CONDITIONAL_REQUEST_HOUSE:
////                    return new MoreHouseRequest();
////            }
////            return null;
////        }
////    }
//}
