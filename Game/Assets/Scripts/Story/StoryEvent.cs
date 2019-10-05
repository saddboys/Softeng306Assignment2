using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Story
{
    
    /// <summary>
    /// A storage class which defines an event
    /// </summary>
    public abstract class StoryEvent 
    {
        public enum EventType  { Request, Event}
        public enum Events { Event_Flood, Request_Tower}
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Events Event { get; set; }
        public Sprite EventImage { get; set; }

    }
}
