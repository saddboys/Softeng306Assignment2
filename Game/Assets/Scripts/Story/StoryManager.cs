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
        [SerializeField]
        public GameObject IntroStory;

        public GameSceneController Controller;
        
        public EventFactory.StoryEvents NextStoryEvent { get; set; }
        private EventFactory factory;
        private Queue<int> storyQueue;
        
        private StoryEvent storyEvent;
        private Random random;
        private List<EventFactory.RandomEvents> eventPool;
        
        public enum StoryEndings {TECH_ENDING, REVISIONIST_ENDING, NEUTRAL_ENDING}

        private int storyEnding = (int) StoryEndings.NEUTRAL_ENDING;
        public int StoryEnding
        {
            get => storyEnding;
            set => storyEnding = value;
        } 
//        private Dictionary<int, bool> storyChoices;


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
//            city.EndGameEvent += ResetStory;
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
//               // storyEvent = factory.CreateRandomEvent(EventFactory.RandomEvents.FLOOD_EVENT);
//              storyEvent = factory.CreateStoryEvent(EventFactory.StoryEvents.GIANT_COOLER_REQUEST);
//                CreatePopUp();   
//            }

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
                if (random.Next(0, 10) == 1 && city.Turn != city.MaxTurns - 1)
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
        /// Determines the ending to show
        /// </summary>
        private void HandleEndGame()
        { 
            
            // Check reason for ending game
            if (city.Turn == city.MaxTurns)
            {
                
                CheckNonFinalStoryEventEffect();
                Debug.Log("End Story reached " + StoryEnding);
                string reason = "";
                
                // Check which storyline we are on
                switch (StoryEnding)
                {
                    case (int) StoryEndings.TECH_ENDING:
                        reason = "You keep the town’s temperature under the threshold! People are happy and can keep living like they do, but outside the town, the world continues to heat and go chaotic. However, with technology, we can survive through it. If only everyone in the world had access to the technology…";
                        break;
                    case (int) StoryEndings.REVISIONIST_ENDING:
                        reason =
                            "You did it! The town’s temperature stayed below the threshold. Perhaps climate change can be managed after all, though it required sacrificing some modern comforts. The people aren’t the happiest about that, but they’re not too unhappy. At least their planet is still there.";
                        break;
                    case (int) StoryEndings.NEUTRAL_ENDING:
                        reason = "Undecided ending";
                        break;
                        
                }
                Controller.GameWon(reason);

            } else if (city.Stats.Wealth <= 0)
            {
                string reason = "You've run out of assets to support your city!";
                Controller.GameOver(reason);
                
            } else if (city.Stats.Temperature > 2)
            {
                string reason = "Your actions have resulted in the earth overheating... our planet is now inhabitable";
                Controller.GameOver(reason);
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
                storyEnding = (int) StoryEndings.REVISIONIST_ENDING;
            }
        }

    }
}

