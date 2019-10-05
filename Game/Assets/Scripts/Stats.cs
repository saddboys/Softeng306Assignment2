using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Stats
    {
        // MegaTonnes (0 - infinite)
        public double CO2 { get; set; }
        // C anomaly (infinite - 3)
        public double Temperature { get; set; }
        // Thousands (0 - infinite)
        public int Population { get; set; }
        // "electricity tokens" (-20 to 20)
        public double ElectricCapacity { get; set; }
        // "reputation rate" (0% to 100%) 
        public double Reputation { get; set; }
        // "points" (0 - infinite)
        public double Score { get; set; }
        // k dollars ($0 - $infinite)
        public double Wealth { get; set; }

        /// <summary>
        /// An addition operator that will add all the fields of 2 Stats objects together.
        /// </summary>
        /// <returns> The overall summed stats.
        public static Stats operator +(Stats a, Stats b)
        {
            Stats sum = new Stats();
            sum.AddContribution(a);
            sum.AddContribution(b);
            return sum;
        }

        /// <summary>
        /// Adds individual fields of one Stats object onto itself.
        /// </summary>
        public void AddContribution(Stats stats)
        {
            if (stats == null)
            {
                return;
            }
            CO2 += stats.CO2;
            Temperature += stats.Temperature;
            Population += stats.Population;
            ElectricCapacity += stats.ElectricCapacity;
            Reputation += stats.Reputation;
            Score += stats.Reputation;
            Wealth += stats.Wealth;
        }
    }
}
