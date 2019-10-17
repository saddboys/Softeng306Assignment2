using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story
{
    
    /// <summary>
    /// A skeleton for an event.
    /// Events are occurrences in the story that cannot be declined.
    /// </summary>
    public abstract class StoryEvent : MonoBehaviour
    {
        public enum EventTypes  { Request, Event}

        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract Sprite EventImage { get; }
        
        public abstract Queue<string> Dialogues { get; }

        public virtual StoryManager StoryManager { get; set; }
        public virtual EventTypes EventType
        {
            get { return EventTypes.Event; }
        }

        public virtual void GenerateScene(GameObject canvas)
        {
            return;
        }
        
        
        
        /// <summary>
        /// Both events and requests will have an option to accept the event.
        /// </summary>
        public abstract void OnYesClick();

    }
}
