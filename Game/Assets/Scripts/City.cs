using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class City : MonoBehaviour
    { 
        public int Turn
        {
            get
            {
                return turn;
            }
            private set
            {
                turn = value;
                turnText.text = turn.ToString();
            }
        }
        private int turn;

        [SerializeField]
        private StatsBar stats;

        public StatsBar Stats
        {
            get { return stats; }
        }

        [SerializeField]
        private CityMap.CityMap map;
        public CityMap.CityMap Map
        {
            get
            {
                return map;
            }
        }
        [SerializeField]
        private Button endTurnButton;
        public Button EndTurnButton
        {
            get => endTurnButton;
            set => endTurnButton = value;
        }
        public bool HasEnded
        {
            get
            {
                return hasEnded;
            }
        }
        private bool hasEnded = false; 

        [SerializeField]
        private Text turnText;

        [SerializeField]
        private GameSceneController controller;

        public GameSceneController Controller
        {
            get { return controller; }
        }
        
        /// <summary>
        /// Fires at the beginning of each new turn.
        /// Useful for spawning events and for updating structures.
        /// E.g. Some structures take 3 turns to build, etc.
        /// </summary>
        public event Action NextTurnEvent;

        // Start is called before the first frame update
        void Start()
        {
            endTurnButton.onClick.AddListener(EndTurn);
            
            //Restart();
            Turn = 1;
            Stats.Restart();
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Function called at the end of the turn to do stat calculations.
        /// </summary>
        public void EndTurn() {
            Stats.UpdateContribution(Map.GetStatsContribution());
            Turn++;
            CheckEndGame();
            NextTurnEvent?.Invoke();
        }

        /// <summary>
        /// Function called to check if the conditions to end game have been met
        /// </summary>
        public void CheckEndGame()
        {
            double temp = stats.Temperature;
            double wealth = stats.Wealth;
                
            if (Turn == 20)
            {
                string reason = "Congratulations! You have sustainably developed your city!";
                EndGame(true, reason);
                hasEnded = true;
            } else if (wealth <= 0)
            {
                string reason = "You've run out of assets to support your city!";
                EndGame(false, reason);
                hasEnded = true;
            } else if (temp > 2)
            {
                string reason = "Your actions have resulted in the earth overheating... our planet is now inhabitable";
                EndGame(false, reason);
                hasEnded = true;
            } 
        }

        /// <summary>
        /// The function triggers the game over overlay, specifying the reason for failure
        /// </summary>
        /// <param name="reason">The reason the player has lost the game</param>
        public void EndGame(bool isWon, string reason)
        {
            EndTurnButton.interactable = false;
            if (isWon)
            {
                Controller.GameWon(reason);   
            }
            else 
            {
                Controller.GameOver(reason);    
            }
        }

        /// <summary>
        /// Function to restart the game either when game ends or when user selects restart from the menu
        /// </summary>
        public void Restart()
        {
            hasEnded = false;
            EndTurnButton.interactable = true;
            Turn = 0;
            Stats.Restart();
            Map.Regenerate();
        }
    }
}
