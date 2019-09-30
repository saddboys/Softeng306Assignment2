using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class City : MonoBehaviour
    {
        public Stats Stats { get; }
        public CityMap.CityMap Map { get; }
        public int Turn { get; private set; }

        // Start is called before the first frame update
        void Start()
        {

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
        }
    }
}
