using System;
using System.Collections;
using System.Collections.Generic;
using Game.Story.Events;
using UnityEngine;
using Random = System.Random;

namespace Game.Story
{
    /// <summary>
    /// A class for the handling of events.
    /// </summary>
    public class StoryManager
    {
        public enum Events { Event_Flood, Request_Tower, Request_Bridge}
        private List<Events> eventPool;

        public StoryManager()
        {
            GenerateEventPool();
        }
        
        /// <summary>
        /// Generates an event at random.
        /// Once an event has occurred, remove it from the pool.
        /// This is done so that other events are able to occur.
        /// </summary>
        /// <returns>StoryEvent</returns>
        public StoryEvent CreateEvent()
        {
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
