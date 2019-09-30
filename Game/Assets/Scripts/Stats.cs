﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Stats : MonoBehaviour
    {
        public const double TEMPERATURE_UPPER_BOUND = 3;

        // MegaTonnes
        public double CO2 { get; private set; }
        // C anomaly 
        public double Temperature { get; private set; }
        // Thousands
        public int Population { get; private set; }
        // -20 to 20 "electricity tokens"
        public double ElectricCapacity { get; private set; }
        // 0% (none) to 100% (max)
        public double Reputation { get; private set; }
        // 1000 - infinite
        public double Score { get; private set; }
        // $1,000
        public double Wealth { get; private set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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
            CO2 += stats.CO2;
            Temperature += stats.Temperature;
            Population += stats.Population;
            ElectricCapacity += stats.ElectricCapacity;
            Reputation += stats.Reputation;
            Score += stats.Reputation;
            Wealth += stats.Wealth;
        }

        /// <summary>
        /// Checks if the current temperature has exceeded the temperature upper bound.
        /// </summary>
        /// <returns> True if the current temperature has exceeded the temperature upper bound. </returns>
        public boolean isTempTooHigh(Stats stats) {
            if (stats.Temperature >= TEMPERATURE_UPPER_BOUND) {
                return true;
            }
            return false;
        }
    }
}