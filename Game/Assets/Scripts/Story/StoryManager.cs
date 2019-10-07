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
        public enum Events { Request_Bridge, Conditional_Request_House, Request_Tower, Event_Flood}
        [SerializeField] 
        private City city;

        [SerializeField] 
        private ToolBar toolbar;

        [SerializeField] 
        private Button endTurnButton;
        [SerializeField]
        private GameObject canvas;
        //private List<Events> eventPool;
        private Dictionary<Events,int> eventPool; 
        // So on the 5th turn the popup will appear
        private int turnsLeft = 4;
        private EventPopUp popUp;
        [SerializeField]
        private GameObject storyManagerGameObject;
        private Random random;
        void Start()
        {
            random = new Random();
            city.NextTurnEvent += HandleTurnEvent;
            GenerateEventPool();
        }

        /// <summary>
        ///  This fires whenever a new turn is called.
        ///  This will handle anything with the pop up events.
        /// </summary>
        private void HandleTurnEvent()
        {
            turnsLeft--;
            DecrementCooldown();
            if (turnsLeft == 0)
            {
               
                CheckStats();
                StoryEvent storyEvent = CreateEvent();
                if (storyEvent != null && !city.HasEnded)
                {
                    popUp = storyManagerGameObject.AddComponent<EventPopUp>();
                    popUp.name = "event-pop-up";
                    popUp.Canvas = this.canvas;
                    popUp.CityMap = city.Map;
                    canvas.SetActive(true);
                    storyEvent.City = city;
                    storyEvent.ToolBar = toolbar;
                    storyEvent.EndButton = endTurnButton;
                    popUp.StoryEvent = storyEvent;
                    popUp.Create();
                }
                turnsLeft = 4;
            }
        }

        /// <summary>
        /// Method which will check the stats and add additional events to pool
        /// </summary>
        private void CheckStats()
        {
            StatsBar statsBar = city.Stats;
            Events[] keys = eventPool.Keys.ToArray();
            if (statsBar.Wealth > 10)
            {
                if (!keys.Contains(Events.Conditional_Request_House))
                {
                    eventPool.Add(Events.Conditional_Request_House,0);
                }
            }
        }
        /// <summary>
        /// Generates an event at random.
        /// Once an event has occurred, remove it from the pool.
        /// This is done so that other events are able to occur.
        /// </summary>
        /// <returns>StoryEvent</returns>
        public StoryEvent CreateEvent()
        {
            // When all of the cooldowns are finished, we regenerate the pool
            if (CheckAll())
            {
                GenerateEventPool();
            }
            // Guarantees that the next event will be offcooldown
            Events[] keys = GetOffCooldownEvents();
            int nextValue = random.Next(0, keys.Length);
            Events currentEvent = keys[nextValue];
            eventPool[currentEvent]--;
            switch (currentEvent)
            {
                case Events.Event_Flood:
                    return new FloodEvent();
                case Events.Request_Bridge:
                    return new BridgeRequest();
                case Events.Request_Tower:
                    return new TowerRequest();
                case Events.Conditional_Request_House:
                    return new MoreHouseRequest();
                default:
                    return null;
            }
        }

        
        /// <summary>
        /// Retrieve all of the events that have no cooldown
        /// </summary>
        /// <returns></returns>
        private Events[] GetOffCooldownEvents()
        {
            List<Events> eventsList = new List<Events>();
            foreach (var events in eventPool)
            {
                if (events.Value == 0)
                {
                    eventsList.Add(events.Key);
                }
            }

            return eventsList.ToArray();
        }

        private void DecrementCooldown()
        {
            Events[] keys = eventPool.Keys.ToArray();
            foreach (Events events in keys)
            {
                if (eventPool[events] > 0)
                {
                    eventPool[events]--;
                }
            }
        }

        /// <summary>
        /// Checks if all of the cooldowns have finished.
        /// </summary>
        /// <returns></returns>
        private bool CheckAll()
        {
            
            int[] values = eventPool.Values.ToArray();
            return values.All(v => v < 0);
        }

        /// <summary>
        /// Generates the pool of events.
        /// </summary>
        private void GenerateEventPool()
        {
            eventPool = new Dictionary<Events, int>();
            Events[] events = (Events[])Enum.GetValues(typeof(Events));
            foreach(var eventObj in events)
            {
                string eventString = eventObj.ToString();
                if (!eventString.Contains("Conditional"))
                {
                    // Set all cooldowns to be 0 initially.
                    eventPool.Add(eventObj,0);
                }
            }
            
        }
    }
}
