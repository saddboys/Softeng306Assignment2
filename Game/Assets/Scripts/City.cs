using System;
﻿using System.Collections;
using System.Collections.Generic;
using Game.Story;
using UnityEngine;
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

        private readonly int maxTurns = 20;
        public int MaxTurns => maxTurns;

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

        /// <summary>
        /// Fires at the beginning of each new turn.
        /// Useful for spawning events and for updating structures.
        /// E.g. Some structures take 3 turns to build, etc.
        /// </summary>
        public event Action NextTurnEvent;
        
        public event Action EndGameEvent;
        private Weather weather;
        // Start is called before the first frame update
        void Start()
        {
            weather = new Weather(Map.map.gameObject);

            endTurnButton.onClick.AddListener(EndTurn);
            
            //Restart();
            Turn = 1;
            Stats.Restart();

            InvokeRepeating("UpdateForecast", 0, 0.1f);
        }

        // Update is called once per frame
        void Update()
        {
            weather.Update();
        }

        private void UpdateForecast()
        {
            Stats.UpdateForecast(Map.GetStatsContribution());
        }

        /// <summary>
        /// Function called at the end of the turn to do stat calculations.
        /// </summary>
        public void EndTurn() {
            Stats.UpdateContribution(Map.GetStatsContribution());
            Turn++;
            CheckEndGame();
//            NextTurnEvent?.Invoke();
        }

        /// <summary>
        /// Function called to check if the conditions to end game have been met
        /// </summary>
        public void CheckEndGame()
        {
            Debug.Log("Checking end game " + turn);
            if (Turn == MaxTurns || Stats.Wealth <= 0 || Stats.Temperature > 2)
            {
                Debug.Log("Trying to end game");
                EndGameEvent?.Invoke();
            }
            else
            {
                NextTurnEvent?.Invoke();
            }
            
        }

        /// <summary>
        /// Function to restart the game either when game ends or when user selects restart from the menu
        /// </summary>
        public void Restart()
        {
            hasEnded = false;
            EndTurnButton.interactable = true;
            Turn = 1;
            
            Stats.Restart();
            Map.Regenerate();
        }
    }
}
