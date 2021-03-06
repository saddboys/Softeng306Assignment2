﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField]
        public GameObject IntroStory;

        public GameSceneController Controller;
        
        public EventFactory.StoryEvents NextStoryEvent { get; set; }
        private EventFactory factory;
        private Queue<int> storyQueue;
        private StoryEvent storyEvent;
        private Random random;
        private List<EventFactory.RandomEvents> eventPool;
        private int chanceOfRandomEvent;
        private bool shownTutorial = false;
        
        public enum StoryEndings {TECH_ENDING, REVISIONIST_ENDING, NEUTRAL_ENDING}

        private int storyEnding = (int) StoryEndings.NEUTRAL_ENDING;
        public int StoryEnding
        {
            get => storyEnding;
            set => storyEnding = value;
        }
        
        public Dictionary<int, bool> StoryChoices = new Dictionary<int, bool>();

        void Start()
        {
            city.RestartGameEvent += ResetStory;
            factory = new EventFactory();
            factory.ManagerObject = storyManagerGameObject;
            random = new Random();
            ResetStory();
           
            city.NextTurnEvent += HandleTurnEvent;
            
            // Generate the event pool
            GeneratePool();

            city.Stats.CO2ChangeEvent += HandleCO2ChangeEvent;
            city.Stats.PopulationChangeEvent += HandlePopulationChangeEvent;
            city.Stats.ReputationChangeEvent +=HandleReputationChangeEvent;
            city.Stats.TemperatureChangeEvent += HandleTemperatureChangeEvent;
            city.Stats.WealthChangeEvent += HandleWealthChangeEvent;
            city.Stats.ElectricCapacityChangeEvent += HandleElectricCapacityChangeEvent;

            city.EndGameEvent += HandleEndGame;
        }

        /// <summary>
        /// When the game restarts, reset the necessary parameters
        /// </summary>
        private void ResetStory()
        {
            // Create a queue for turn number of the story events
            storyQueue = new Queue<int>(new[] {4,8,12,16,20 });
            StoryChoices  = new Dictionary<int, bool>();
            NextStoryEvent = EventFactory.StoryEvents.INITIAL_THANTEC;
        }
        private void HandleCO2ChangeEvent()
        {
            
        }

        private void HandlePopulationChangeEvent()
        {
            if (eventPool.Contains(EventFactory.RandomEvents.CONDITIONAL_REQUEST_HOUSE))
            {
                if (city.Stats.Population <= 30) eventPool.Remove(EventFactory.RandomEvents.CONDITIONAL_REQUEST_HOUSE);
            }
            else
            {
                if (city.Stats.Population > 30) eventPool.Add(EventFactory.RandomEvents.CONDITIONAL_REQUEST_HOUSE);
            }
            
        }

        private void HandleReputationChangeEvent()
        {
            
        }

        private void HandleTemperatureChangeEvent()
        {
            chanceOfRandomEvent = (int)(city.Stats.Temperature * 20);
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

        public void GeneratePopupThroughEvent(EventFactory.RandomEvents randomEvent)
        {
            storyEvent = factory.CreateRandomEvent(randomEvent);
            CreatePopUp();
        }

        /// <summary>
        /// Listener for each turn count.
        /// This will check if its time to show a story event
        /// or it will generate a random event
        /// </summary>
        private void HandleTurnEvent()
        {
//            For testing an event
//            if (city.Turn == 2)
//            {
//                storyEvent = factory.CreateRandomEvent(EventFactory.RandomEvents.FLOOD_EVENT);
//             // storyEvent = factory.CreateStoryEvent(EventFactory.StoryEvents.GIANT_COOLER_REQUEST);
//                CreatePopUp();   
//            }
//
            // For tutorial event and further explaination
             Debug.Log("enter here city turn is : " + city.Turn);
            if (city.Turn == 2 && !shownTutorial)
            {
                CreateTutorial();

                // Only show the turn-2 tutorial dialog once. Don't reshow upon restart or at other levels.
                shownTutorial = true;
            }
            if (city.Turn == storyQueue.Peek())
            {
                // Create new story event here
                storyEvent = factory.CreateStoryEvent(NextStoryEvent);
                storyEvent.StoryManager = this;
                
                if (!storyEvent.ConditionMet())
                {
                    storyEvent = factory.CreateStoryEvent(NextStoryEvent);
                }

                // Get rid of the first thing in the queue
                storyQueue.Dequeue();
                CreateDialog();
            }
            else
            {
                // Events have a 10% chance of popping up
                // Check for penultimate turn to prevent buggy behaviour
                if (random.Next(0, 100) <= chanceOfRandomEvent && city.Turn != city.MaxTurns - 1 && city.Turn != 2)
                {
                    EventFactory.RandomEvents randomEvent = eventPool[random.Next(0,eventPool.Count)];
                    // Randomly spawn events from the event pool
                    storyEvent = factory.CreateRandomEvent(randomEvent);
                    CreatePopUp();
                }
            }
        }

        /// <summary>
        /// Creates the popup for when an event occurs
        /// </summary>
        private void CreatePopUp()
        {
            EventPopUp popUp;
            if (storyEvent != null && !city.HasEnded)
            {
                popUp = storyManagerGameObject.AddComponent<EventPopUp>();
                popUp.name = "event-pop-up";
                popUp.Canvas = canvas;
                popUp.CityMap = city.Map;
                canvas.transform.Find("Panel").gameObject.SetActive(true);
                popUp.StoryEvent = storyEvent;
                storyEvent.StoryManager = this;
                popUp.Create();
                FindObjectOfType<DialogueManager>().Finished -= CreatePopUp;
            }
        }

        /// <summary>
        /// This function creates the dialog that will be displayed 
        /// </summary>
        private void CreateDialog()
        {
            Dialogue dialog = new Dialogue(); 
            if (storyEvent != null && !city.HasEnded)
            {
                dialog.name = "Secretary";
                dialog.sentences = storyEvent.Dialogues.ToArray();
                IntroStory.SetActive(true);
                FindObjectOfType<DialogueManager>().StartDialogue(dialog);
                FindObjectOfType<DialogueManager>().Finished += CreatePopUp;
            }
           
        }

        /// <summary>
        /// Creates the dialog for the tutorial continuation after the second turn
        /// </summary>
        public void CreateTutorial(){
             Dialogue dialog = new Dialogue(); 
          
            if ( !city.HasEnded)
            {  
                dialog.name = "Secretary";
                dialog.sentences =   new String[] {"Congratulations! You made it to your second turn.", 
                "As you just saw, your city has changed a bit. ",
                "Once per turn, each building in your city earns or loses money, produces or reduces CO2 "+
                "emissions and makes your people happier or sadder.",
                "It’s truly a beautiful sight; every little thing in this city counts. Click on a tile to learn more about what it brings to the city.",
                "Well go now! You have 18 turns to reach the highest score possible. But be careful, don't let your resources become too low or else it's game over! "
                };
                IntroStory.SetActive(true);
                FindObjectOfType<DialogueManager>().StartDialogue(dialog);
            }
        }
        
        /// <summary>
        /// Determines the ending to show
        /// </summary>
        private void HandleEndGame()
        {
            // Check reason for game end
            if (city.Turn == city.MaxTurns)                    // CASE 1: Game won
            {
                // Determine if non-final story event has affected story route 
                CheckNonFinalStoryEventEffect();
                string reason = "";
                
                // Check which storyline we are on
                switch (StoryEnding)
                {
                    case (int) StoryEndings.TECH_ENDING:
                        reason = "You kept the town\'s temperature under the threshold!\n" +
                                 " People are happy and can keep living like they do, but outside the town, the world " +
                                 "continues to heat and go chaotic.\n However, with technology, we can survive " +
                                 "through it.\n If only everyone in the world had access to the technology...";
                        break;
                    case (int) StoryEndings.REVISIONIST_ENDING:
                        reason =
                            "You did it!\n The town\'s temperature stayed below the threshold.\n" +
                            " Perhaps climate change can be managed after all, though it required sacrificing some " +
                            "modern comforts.\n The people aren\'t the happiest about that, but they\'re not too " +
                            "unhappy.\n At least their planet is still there.";
                        break;
                    case (int) StoryEndings.NEUTRAL_ENDING:
                        reason = "Congratulations!\n You managed to keep the temperature under the threshold!\n" +
                                 " And you didn\'t make too many drastic changes! By doing what this one city did, " +
                                 "perhaps climate change has been averted... right?\nAre these actions enough?" +
                                 " Or are they just postponing the inevitable?\n Without drastic action, is it possible" +
                                 " to stop temperatures from rising?";
                        break;
                        
                }
                Controller.GameWon(reason, city.Stats.Score);

            } else if (city.Stats.Wealth <= 0)                // CASE 2: Game Lost due to lack to assets
            {
                string reason = "You've run out of assets to support your city!";
                Controller.GameOver(reason, city.Stats.Score);
            } else if (city.Stats.Temperature > 2)            // CASE 3: Game Lost due to exceeding temp limit
            {    
                string reason = "Your actions have resulted in the our town overheating... " +
                                "our planet is one step closer to becoming inhabitable";
                Controller.GameOver(reason, city.Stats.Score);
            }
           
            ResetStory();
        }

        /// <summary>
        /// This method is used check the result of events that determine the ending, but are not the final story events
        /// in a route. 
        /// </summary>
        private void CheckNonFinalStoryEventEffect()
        {
            List<int> keys = StoryChoices.Keys.ToList();
            bool thanTechExists = false;
            
            if (keys.Contains((int) EventFactory.StoryEvents.INITIAL_THANTEC))
            {
                StoryChoices.TryGetValue((int) EventFactory.StoryEvents.INITIAL_THANTEC, out thanTechExists);
            }

            if (thanTechExists && StoryEnding != (int) StoryEndings.TECH_ENDING)
            {
                // storyEnding = (int) StoryEndings.REVISIONIST_ENDING;
            }
        }

    }
}

