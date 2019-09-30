using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CityMap
{
    public abstract class Structure : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public abstract bool CanBuildOn(Terrain terrain);

        /// <summary>
        /// Calculate how much this structure will contribute to the stats, such
        /// as CO2 generated, profits/losses, etc.
        /// </summary>
        /// <returns>The structure's stats contribution.</returns>
        public abstract Stats GetStatsContribution();
    }
}