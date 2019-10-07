using System;
using System.Collections;
using System.Collections.Generic;
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
        public enum Events { Event_Flood, Request_Tower, Request_Bridge}

        [SerializeField] 
        private City city;

        [SerializeField] 
        private ToolBar toolbar;

        [SerializeField] 
        private Button endTurnButton;
        [SerializeField]
        private GameObject canvas;
        private List<Events> eventPool;
        private int turnsLeft = 2;
        private EventPopUp popUp;
        [SerializeField]
        private GameObject storyManagerGameObject;

        void Start()
        {
            city.NextTurnEvent += HandleTurnEvent;
            GenerateEventPool();
        }

        private void HandleTurnEvent()
        {
            if (turnsLeft == 0)
            {
                popUp = storyManagerGameObject.AddComponent<EventPopUp>();
                canvas.SetActive(true);
                popUp.Canvas = this.canvas;
                popUp.CityMap = city.Map;
                StoryEvent storyEvent = CreateEvent();
                storyEvent.City = city;
                storyEvent.ToolBar = toolbar;
                storyEvent.EndButton = endTurnButton;
                popUp.StoryEvent = storyEvent;
                popUp.Create();
                turnsLeft = 2;
            }

            turnsLeft--;
        }

        private void CheckStats()
        {
            StatsBar statsBar = city.Stats;
            if (statsBar.Wealth > 10)
            {
                
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
            Debug.Log("GOES HERE");
            Random random = new Random();
            if (eventPool.Count == 0)
            {
                GenerateEventPool();
            }
            int nextValue = random.Next(0, eventPool.Count);
            Debug.Log(nextValue);
            Events type = eventPool[nextValue];
            switch (type)
            {
                case Events.Event_Flood:
                    eventPool.Remove(Events.Event_Flood);
                    return new FloodEvent();
                case Events.Request_Bridge:
                    eventPool.Remove(Events.Request_Bridge);
                    return new BridgeRequest();
                case Events.Request_Tower:
                    eventPool.Remove(Events.Request_Tower);
                    return new TowerRequest();     
                default:
                    return null;
            }
        }

        /// <summary>
        /// Generates the pool of events.
        /// </summary>
        private void GenerateEventPool()
        {
            Events[] events = (Events[])Enum.GetValues(typeof(Events));
            eventPool = new List<Events>(events);
        }
    }
}
