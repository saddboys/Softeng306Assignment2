using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StatsBar : MonoBehaviour
    {
        // MegaTonnes (0 - infinite)
        public double CO2
        {
            get { return co2; }
            set
            {
                co2 = value;
                co2ValueText.text = co2.ToString() + " MT";
                ChangeEvent?.Invoke();
            }
        }
        // C anomaly (infinite - 3)
        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                temperatureValueText.text = temperature.ToString() + "°C Change";
                ChangeEvent?.Invoke();
            }
        }
        // Thousands (0 - infinite)
        public int Population
        {
            get { return population; }
            set
            {
                population = value;
                populationValueText.text = population.ToString() + "k";
                ChangeEvent?.Invoke();
            }
        }
        // "electricity tokens" (-20 to 20)
        public double ElectricCapacity
        {
            get { return electricCapacity; }
            set
            {
                electricCapacity = value;
                electricCapacityValueText.text = electricCapacity.ToString();
                ChangeEvent?.Invoke();
            }
        }
        // "reputation rate" (0% to 100%) 
        public double Reputation
        {
            get { return reputation; }
            set
            {
                reputation = value;
                reputationValueText.text = reputation.ToString() + "%";
                ChangeEvent?.Invoke();
            }
        }
        // "points" (0 - infinite)
        public double Score
        {
            get { return score; }
            set
            {
                score = value;
                scoreValueText.text = score.ToString();
                ChangeEvent?.Invoke();
            }
        }
        // k dollars ($0 - $infinite)
        public double Wealth
        {
            get { return wealth; }
            set
            {
                wealth = value;
                moneyValueText.text = "$" + wealth.ToString() + "k";
                ChangeEvent?.Invoke();
            }
        }

        private double co2;
        private double temperature;
        private int population;
        private double electricCapacity;
        private double reputation;
        private double score;
        private double wealth;

        // UI properties
        [SerializeField]
        private Text moneyValueText;
        [SerializeField]
        private Text electricCapacityValueText;
        [SerializeField]
        private Text reputationValueText;
        [SerializeField]
        private Text populationValueText;
        [SerializeField]
        private Text temperatureValueText;
        [SerializeField]
        private Text co2ValueText;
        [SerializeField]
        private Text scoreValueText;

        public Action ChangeEvent;

        /// <summary>
        /// Adds individual fields of one Stats object onto itself.
        /// </summary>
        public void UpdateContribution(Stats stats)
        {
            if (stats == null)
            {
                stats = new Stats();
            }
            CO2 = stats.CO2;
            Temperature += stats.Temperature + CO2 / 10;
            Population += stats.Population;
            ElectricCapacity += stats.ElectricCapacity;
            Reputation += stats.Reputation;
            if (Reputation > 100) Reputation = 100;
            if (Reputation < 0) Reputation = 0;
            Score += stats.Reputation;
            Wealth += stats.Wealth;
        }

        public void Restart()
        {
            CO2 = 0;
            Temperature = 0;
            Population = 30; // TODO: Random - based on houses
            ElectricCapacity = 4; // TODO: Random
            Reputation = 50;
            Score = 0;
            Wealth = 10000;
        }
    }
}
