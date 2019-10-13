using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StatsBar : MonoBehaviour
    {
        public double CO2 { get; set; }
        public double Temperature { get; set; }
        public int Population { get; set; }
        public double ElectricCapacity { get; set; }
        public double Reputation { get; set; }
        public double Score { get; set; }
        public double Wealth { get; set; }

        // MegaTonnes (0 - infinite)
        public double CO2Shown
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
        public double TemperatureShown
        {
            get { return temperature; }
            set
            {
                temperature = value;
                temperatureValueText.text = temperature.ToString("F2") + "°C Change";
                ChangeEvent?.Invoke();
            }
        }
        // Thousands (0 - infinite)
        public int PopulationShown
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
        public double ElectricCapacityShown
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
        public double ReputationShown
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
        public double ScoreShown
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
        public double WealthShown
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

        private void Start()
        {
        }

        private void Update()
        {
            CO2Shown += Mathf.Clamp((float)(CO2 - CO2Shown), -1, 1);
            TemperatureShown += Mathf.Clamp((float)(Temperature - TemperatureShown), -0.01f, 0.01f);
            PopulationShown += (int)Mathf.Clamp((float)(Population - PopulationShown), -1, 1);
            ElectricCapacityShown += Mathf.Clamp((float)(ElectricCapacity - ElectricCapacityShown), -1, 1);
            ReputationShown += Mathf.Clamp((float)(Reputation - ReputationShown), -1, 1);
            ScoreShown += Mathf.Clamp((float)(Score - ScoreShown), -100, 100);
            WealthShown += Mathf.Clamp((float)(Wealth - WealthShown), -100, 100);
        }

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
            Temperature += stats.Temperature + CO2 / 1000;
            Population += stats.Population;
            ElectricCapacity += stats.ElectricCapacity;
            Reputation += stats.Reputation;
            if (Reputation > 100) Reputation = 100;
            if (Reputation < 0) Reputation = 0;
            Score += stats.Score;
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
