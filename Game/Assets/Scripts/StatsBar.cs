using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StatsBar : MonoBehaviour
    {
        private List<String> changeTooltipQueue = new List<string>();
        private List<GameObject> tooltips = new List<GameObject>();
        private GameObject tooltipCanvas;

        public double CO2
        {
            get { return co2; }
            set
            {
                double change = value - co2;
                co2 = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+" + change + " MT CO2");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-" + change + " MT CO2");
                }
            }
        }
        public double Temperature
        {
            get { return temperature; }
            set
            {
                double change = value - temperature;
                temperature = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+" + change + "°C");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-" + change + "°C");
                }
            }
        }
        public int Population
        {
            get { return population; }
            set
            {
                double change = value - population;
                population = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+" + change + "k Population");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-" + change + "k Population");
                }
            }
        }
        public double ElectricCapacity
        {
            get { return electricCapacity; }
            set
            {
                double change = value - electricCapacity;
                electricCapacity = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+" + change + " Electricity");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-" + change + " Electricity");
                }
            }
        }
        public double Reputation
        {
            get { return reputation; }
            set
            {
                double change = value - reputation;
                reputation = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+" + change + "% Reputation");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-" + change + "% Reputation");
                }
            }
        }
        public double Score
        {
            get { return score; }
            set
            {
                double change = value - score;
                score = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+" + change + " Score");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-" + change + " Score");
                }
            }
        }
        public double Wealth
        {
            get { return wealth; }
            set
            {
                double change = value - wealth;
                wealth = value;
                if (change > 0)
                {
                    changeTooltipQueue.Add("+$" + change + "k");
                }
                else if (change < 0)
                {
                    change *= -1;
                    changeTooltipQueue.Add("-$" + change + "k");
                }
            }
        }

        // MegaTonnes (0 - infinite)
        public double CO2Shown
        {
            get { return co2Shown; }
            set
            {
                co2Shown = value;
                co2ValueText.text = co2Shown.ToString() + " MT";
                ChangeEvent?.Invoke();
            }
        }
        // C anomaly (infinite - 3)
        public double TemperatureShown
        {
            get { return temperatureShown; }
            set
            {
                temperatureShown = value;
                temperatureValueText.text = temperatureShown.ToString("F2") + "°C Change";
                ChangeEvent?.Invoke();
            }
        }
        // Thousands (0 - infinite)
        public int PopulationShown
        {
            get { return populationShown; }
            set
            {
                populationShown = value;
                populationValueText.text = populationShown.ToString() + "k";
                ChangeEvent?.Invoke();
            }
        }
        // "electricity tokens" (-20 to 20)
        public double ElectricCapacityShown
        {
            get { return electricCapacityShown; }
            set
            {
                electricCapacityShown = value;
                electricCapacityValueText.text = electricCapacityShown.ToString();
                ChangeEvent?.Invoke();
            }
        }
        // "reputation rate" (0% to 100%) 
        public double ReputationShown
        {
            get { return reputationShown; }
            set
            {
                reputationShown = value;
                reputationValueText.text = reputationShown.ToString() + "%";
                ChangeEvent?.Invoke();
            }
        }
        // "points" (0 - infinite)
        public double ScoreShown
        {
            get { return scoreShown; }
            set
            {
                scoreShown = value;
                scoreValueText.text = scoreShown.ToString();
                ChangeEvent?.Invoke();
            }
        }
        // k dollars ($0 - $infinite)
        public double WealthShown
        {
            get { return wealthShown; }
            set
            {
                wealthShown = value;
                moneyValueText.text = "$" + wealthShown.ToString() + "k";
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

        private double co2Shown;
        private double temperatureShown;
        private int populationShown;
        private double electricCapacityShown;
        private double reputationShown;
        private double scoreShown;
        private double wealthShown;

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
            tooltipCanvas = new GameObject();
            Canvas canvas = tooltipCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        private void Update()
        {
            // Gradually increment stats changes.
            CO2Shown += Mathf.Clamp((float)(CO2 - CO2Shown), -1, 1);
            TemperatureShown += Mathf.Clamp((float)(Temperature - TemperatureShown), -0.01f, 0.01f);
            PopulationShown += (int)Mathf.Clamp((float)(Population - PopulationShown), -1, 1);
            ElectricCapacityShown += Mathf.Clamp((float)(ElectricCapacity - ElectricCapacityShown), -1, 1);
            ReputationShown += Mathf.Clamp((float)(Reputation - ReputationShown), -1, 1);
            ScoreShown += Mathf.Clamp((float)(Score - ScoreShown), -100, 100);
            WealthShown += Mathf.Clamp((float)(Wealth - WealthShown), -50, 50);

            // Create a new tooltip.
            if (changeTooltipQueue.Count > 0)
            {
                GameObject tooltip = new GameObject();
                RectTransform rectTransform = tooltip.AddComponent<RectTransform>();
                rectTransform.position = Input.mousePosition;
                rectTransform.pivot = new Vector2(0, 0);
                rectTransform.SetParent(tooltipCanvas.transform);
                ContentSizeFitter fitter = tooltip.AddComponent<ContentSizeFitter>();
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                HorizontalLayoutGroup group = tooltip.AddComponent<HorizontalLayoutGroup>();
                group.childControlHeight = true;
                group.childControlWidth = true;
                Text text = tooltip.AddComponent<Text>();
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.text = String.Join("\n", changeTooltipQueue);
                text.fontSize = 10;
                Shadow shadow = tooltip.AddComponent<Shadow>();
                shadow.effectColor = new Color(0, 0, 0);
                changeTooltipQueue.Clear();
                tooltips.Add(tooltip);
            }

            // Make change tooltips float upwards and fade out.
            foreach (var tooltip in tooltips)
            {
                Text text = tooltip.GetComponent<Text>();
                Color color = text.color;
                Vector3 pos = tooltip.transform.position;
                pos.y += color.a / 2.0f;
                tooltip.transform.position = pos;
                color.a -= 0.005f;
                text.color = color;
            }

            // Remove tooltips that are no longer visible.
            tooltips.RemoveAll(tooltip =>
            {
                if (tooltip.GetComponent<Text>().color.a < 0.0f)
                {
                    Destroy(tooltip);
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// Updates the stats for one game turn.
        /// </summary>
        public void UpdateContribution(Stats stats)
        {
            co2 = 0;
            AddContribution(stats);
            Temperature += CO2 / 1000;
        }

        /// <summary>
        /// Adds individual fields of one Stats object onto itself.
        /// </summary>
        public void AddContribution(Stats stats)
        {
            if (stats == null)
            {
                stats = new Stats();
            }
            CO2 += stats.CO2;
            Temperature += stats.Temperature;
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
            co2 = 0;
            temperature = 0;
            population = 30; // TODO: Random - based on houses
            electricCapacity = 4; // TODO: Random
            reputation = 50;
            score = 0;
            wealth = 10000;
            foreach (var tooltip in tooltips)
            {
                Destroy(tooltip);
            }
            tooltips.Clear();
        }
    }
}
