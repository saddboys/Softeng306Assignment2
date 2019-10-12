using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story
{
    public class StoryManagerTemp : MonoBehaviour
    {
        private enum RandomEvents {}
        private enum StoryEvents {INITIAL_THANTEC}
        [SerializeField] 
        private City city;
        

        [SerializeField] 
        private ToolBar toolbar;

        [SerializeField] 
        private Button endTurnButton;
        [SerializeField]
        private GameObject canvas;

        private Queue<int> storyQueue;
        private StoryEvents nextEvent;

        private void Start()
        {
            // Create a queue for the story events
            storyQueue = new Queue<int>(new[] {4,8,12 });
            nextEvent = StoryEvents.INITIAL_THANTEC;
            city.NextTurnEvent += HandleTurnEvent;
        }

        private void HandleTurnEvent()
        {
            
        }
    }
}