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
                if (co2ValueText == null)
                {
                    return;
                }
                co2ValueText.text = co2.ToString();
            }
        }
        // C anomaly (infinite - 3)
        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                if (temperatureValueText == null)
                {
                    return;
                }
                temperatureValueText.text = temperature.ToString();
            }
        }
        // Thousands (0 - infinite)
        public int Population
        {
            get { return population; }
            set
            {
                population = value;
                if (populationValueText == null)
                {
                    return;
                }
                populationValueText.text = population.ToString();
            }
        }
        // "electricity tokens" (-20 to 20)
        public double ElectricCapacity
        {
            get { return electricCapacity; }
            set
            {
                electricCapacity = value;
                if (electricCapacityValueText == null)
                {
                    return;
                }
                electricCapacityValueText.text = electricCapacity.ToString();
            }
        }
        // "reputation rate" (0% to 100%) 
        public double Reputation
        {
            get { return reputation; }
            set
            {
                reputation = value;
                if (reputationValueText == null)
                {
                    return;
                }
                reputationValueText.text = reputation.ToString();
            }
        }
        // "points" (0 - infinite)
        public double Score
        {
            get { return score; }
            set
            {
                score = value;
                if (scoreValueText == null)
                {
                    return;
                }
                scoreValueText.text = score.ToString();
            }
        }
        // k dollars ($0 - $infinite)
        public double Wealth
        {
            get { return wealth; }
            set
            {
                wealth = value;
                if (moneyValueText == null)
                {
                    return;
                }
                moneyValueText.text = wealth.ToString();
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
