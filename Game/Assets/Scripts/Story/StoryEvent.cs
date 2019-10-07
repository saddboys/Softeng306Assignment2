using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Story
{
    
    /// <summary>
    /// A skeleton for an event.
    /// Events are occurrences in the story that cannot be declined.
    /// </summary>
    public abstract class StoryEvent 
    {
        public enum EventTypes  { Request, Event}

        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract Sprite EventImage { get; }
        
        public City City { get; set; }
        public virtual EventTypes EventType
        {
            get { return EventTypes.Event; }
        }
        
        /// <summary>
        /// Both events and requests will have an option to accept the event.
        /// </summary>
        public abstract void OnYesClick();

    }
}
