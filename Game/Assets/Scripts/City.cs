using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class City : MonoBehaviour
    { 
        public int TurnsLeft
        {
            get
            {
                return turnsLeft;
            }
            private set
            {
                turnsLeft = value;
                turnLeftText.text = turnsLeft.ToString();
            }
        }
        private int turnsLeft;

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
        private Text turnLeftText;

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
            TurnsLeft = 50;
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
            TurnsLeft--;
            NextTurnEvent?.Invoke();
        }

        public void Restart()
        {
            Stats.Restart();
            Map.Regenerate();
        }
    }
}
