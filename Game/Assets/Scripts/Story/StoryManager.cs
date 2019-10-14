using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Story.Events;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.Story
{
    /// <summary>
    /// A class for the handling of events.
    /// </summary>
    public class StoryManager : MonoBehaviour
    {
        [SerializeField] 
        public City city;
        [SerializeField] 
        public ToolBar toolbar;
        [SerializeField] 
        public Button endTurnButton;
        [SerializeField]
        public GameObject canvas;
        [SerializeField]
        public GameObject storyManagerGameObject;
        public EventFactory.StoryEvents NextStoryEvent { get; set; }
        private EventFactory factory;
        private Queue<int> storyQueue;
        
        private StoryEvent storyEvent;
        private Random random;
        private List<EventFactory.RandomEvents> eventPool;
        void Start()
        {
            factory = new EventFactory();
            factory.ManagerObject = storyManagerGameObject;
            random = new Random();
            // Create a queue for turn number of the story events
            storyQueue = new Queue<int>(new[] {4,8,12,16,20 });
            NextStoryEvent = EventFactory.StoryEvents.INITIAL_THANTEC;
            city.NextTurnEvent += HandleTurnEvent;
            
            // Generate the event pool
            GeneratePool();

            city.Stats.CO2ChangeEvent += HandleCO2ChangeEvent;
            city.Stats.PopulationChangeEvent += HandlePopulationChangeEvent;
            city.Stats.ReputationChangeEvent +=HandleReputationChangeEvent;
            city.Stats.TemperatureChangeEvent += HandleTemperatureChangeEvent;
            city.Stats.WealthChangeEvent += HandleWealthChangeEvent;
            city.Stats.ElectricCapacityChangeEvent += HandleElectricCapacityChangeEvent;

            city.EndGameEvent += ResetStory;
        }

        private void ResetStory()
        {
            storyQueue = new Queue<int>(new[] {4,8,12,16,20 });
            NextStoryEvent = EventFactory.StoryEvents.INITIAL_THANTEC;
        }
        private void HandleCO2ChangeEvent()
        {
            
        }

        private void HandlePopulationChangeEvent()
        {
            if (eventPool.Contains(EventFactory.RandomEvents.CONDITIONAL_REQUEST_HOUSE))
            {
                if (city.Stats.Population <= 10) eventPool.Remove(EventFactory.RandomEvents.CONDITIONAL_REQUEST_HOUSE);
            }
            else
            {
                if (city.Stats.Population > 10) eventPool.Add(EventFactory.RandomEvents.CONDITIONAL_REQUEST_HOUSE);
            }
            
        }

        private void HandleReputationChangeEvent()
        {
            
        }

        private void HandleTemperatureChangeEvent()
        {
            
        }

        private void HandleWealthChangeEvent()
        {
            
        }

        private void HandleElectricCapacityChangeEvent()
        {
            
        }
        /// <summary>
        /// Generates the pool of events.
        /// </summary>
        private void GeneratePool()
        {
            eventPool = new List<EventFactory.RandomEvents>();
            EventFactory.RandomEvents[] events = (EventFactory.RandomEvents[])Enum.GetValues(typeof(EventFactory.RandomEvents));
            foreach(var eventObj in events)
            {
                string eventString = eventObj.ToString();
                if (!eventString.Contains("CONDITIONAL"))
                {
                    eventPool.Add(eventObj);
                }
            }
        }

        private void HandleTurnEvent()
        {
            // For testing an event
            if (city.Turn == 2)
            {
                storyEvent = factory.CreateRandomEvent(EventFactory.RandomEvents.FLOOD_EVENT);
                CreatePopUp();   
            }

//            if (city.Turn == storyQueue.Peek())
//            {
//                // Create new story event here
//                storyEvent = factory.CreateStoryEvent(NextStoryEvent);
//                // Get rid of the first thing in the queue
//                storyQueue.Dequeue();
//                CreateDialog();
//                //CreatePopUp();
//            }
//            else
//            {
//                // Events have a 10% chance of popping up
//                if (random.Next(0, 10) == 1)
//                {
//                    EventFactory.RandomEvents randomEvent = eventPool[random.Next(0,eventPool.Count)];
//                    // Randomly spawn events from the event pool
//                    storyEvent = factory.CreateRandomEvent(randomEvent);
//                    CreatePopUp();
//                }
//            }
        }

        private void CreatePopUp()
        {
            Debug.Log("goes here");
            EventPopUp popUp;
            if (storyEvent != null && !city.HasEnded)
            {
                popUp = storyManagerGameObject.AddComponent<EventPopUp>();
                popUp.name = "event-pop-up";
                popUp.Canvas = canvas;
                popUp.CityMap = city.Map;
                canvas.transform.Find("Panel").gameObject.SetActive(true);
                storyEvent.StoryManager = this;
                popUp.StoryEvent = storyEvent;
                popUp.Create();
            }
        }

        private void CreateDialog()
        {
            DialogPopUp dialogPopUp;
            if (storyEvent != null && !city.HasEnded)
            {
                dialogPopUp = storyManagerGameObject.AddComponent<DialogPopUp>();
                dialogPopUp.Canvas = canvas;
                dialogPopUp.StoryEvent = storyEvent;
                canvas.transform.Find("Panel").gameObject.SetActive(true);
                dialogPopUp.Finished += CreatePopUp;
                dialogPopUp.Create();
            }
        }
        
    }
}

