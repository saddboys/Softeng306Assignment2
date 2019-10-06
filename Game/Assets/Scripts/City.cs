using System;
﻿using System.Collections;
using System.Collections.Generic;
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
        [SerializeField]
        private Text turnText;

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
            Restart();
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
            Stats.AddContribution(Map.GetStatsContribution());
            Turn++;
            NextTurnEvent?.Invoke();
        }

        public void Restart()
        {
            Turn = 1;
            Stats.Restart();
            Map.Regenerate();
        }
    }
}
