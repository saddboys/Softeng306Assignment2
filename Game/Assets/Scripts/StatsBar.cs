using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StatsBar : MonoBehaviour
    {
        private readonly List<String> tooltipQueue = new List<string>();
        private readonly List<GameObject> tooltips = new List<GameObject>();
        private GameObject tooltipCanvas;

        ForecastableStat co2Stat;
        ForecastableStat tempStat;
        ForecastableStat popStat;
        ForecastableStat elecStat;
        ForecastableStat repStat;
        ForecastableStat scoreStat;
        ForecastableStat wealthStat;
        public double CO2 { get => co2Stat.Value; set => co2Stat.Value = value; }
        public double Temperature { get => tempStat.Value; set => tempStat.Value = value; }
        public double Population { get => popStat.Value; set => popStat.Value = value; }
        public double ElectricCapacity { get => elecStat.Value; set => elecStat.Value = value; }
        public double Reputation { get => repStat.Value; set => repStat.Value = Mathf.Clamp((float)value, 0, 100); }
        public double Score { get => scoreStat.Value; set => scoreStat.Value = value; }
        public double Wealth { get => wealthStat.Value; set => wealthStat.Value = value; }

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
        public Action CO2ChangeEvent;
        public Action TemperatureChangeEvent;
        public Action PopulationChangeEvent;
        public Action ElectricCapacityChangeEvent;
        public Action ReputationChangeEvent;
        public Action WealthChangeEvent;

        private void Start()
        {
            co2Stat = new ForecastableStat(tooltipQueue, co2ValueText, "F0", " MT", " CO2", 2, 0.5f);
            tempStat = new ForecastableStat(tooltipQueue, temperatureValueText, "F2", "°C", " Change", 0.01f, 0.001f);
            popStat = new ForecastableStat(tooltipQueue, populationValueText, "F1", "k", " Population", 1, 0.1f);
            elecStat = new ForecastableStat(tooltipQueue, electricCapacityValueText, "F0", "", " Electricity", 1, 0.1f);
            repStat = new ForecastableStat(tooltipQueue, reputationValueText, "F0", "%", " Reputation", 2, 0.5f);
            scoreStat = new ForecastableStat(tooltipQueue, scoreValueText, "F0", "", " Score", 200, 50);
            wealthStat = new ForecastableStat(tooltipQueue, moneyValueText, "C", "k", "", 50, 5);
            co2Stat.ChangeEvent += OnChange;
            co2Stat.ChangeEvent += CO2ChangeEvent.Invoke;
            tempStat.ChangeEvent += OnChange;
            tempStat.ChangeEvent += TemperatureChangeEvent.Invoke;
            popStat.ChangeEvent += OnChange;
            popStat.ChangeEvent += PopulationChangeEvent.Invoke;
            elecStat.ChangeEvent += OnChange;
            elecStat.ChangeEvent += ElectricCapacityChangeEvent.Invoke;
            repStat.ChangeEvent += OnChange;
            repStat.ChangeEvent += ReputationChangeEvent.Invoke;
            scoreStat.ChangeEvent += OnChange;
            wealthStat.ChangeEvent += OnChange;
            wealthStat.ChangeEvent += WealthChangeEvent.Invoke;
            

            tooltipCanvas = new GameObject();
            Canvas canvas = tooltipCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 2;
        }

        private void Update()
        {
            // Gradually increment stats changes.
            co2Stat.Update();
            tempStat.Update();
            popStat.Update();
            popStat.Update();
            elecStat.Update();
            repStat.Update();
            scoreStat.Update();
            wealthStat.Update();

            // Create a new tooltip.
            if (tooltipQueue.Count > 0)
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
                text.text = String.Join("\n", tooltipQueue);
                text.fontSize = 10;
                Shadow shadow = tooltip.AddComponent<Shadow>();
                shadow.effectColor = new Color(0, 0, 0);
                tooltipQueue.Clear();
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
        /// Display the changes that would take place once the turn ends.
        /// </summary>
        /// <param name="stats"></param>
        public void UpdateForecast(Stats stats)
        {
            co2Stat.Forecast.Value = stats.CO2 - CO2;
            tempStat.Forecast.Value = stats.Temperature + CalculateTemperatureChange(stats.CO2);
            popStat.Forecast.Value = stats.Population;
            elecStat.Forecast.Value = stats.ElectricCapacity;
            repStat.Forecast.Value = stats.Reputation;
            scoreStat.Forecast.Value = stats.Score;
            wealthStat.Forecast.Value = stats.Wealth;
        }

        /// <summary>
        /// Updates the stats for one game turn.
        /// </summary>
        public void UpdateContribution(Stats stats)
        {
            co2Stat.Reset(0, co2Stat.Shown);
            AddContribution(stats);
            Temperature += CalculateTemperatureChange(CO2);
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
            Score += stats.Score;
            Wealth += stats.Wealth;
        }

        public void Restart()
        {
            co2Stat.Reset(0);
            tempStat.Reset(0);
            popStat.Reset(30); // TODO: Random - based on houses
            elecStat.Reset(4); // TODO: Random
            repStat.Reset(50);
            scoreStat.Reset(0);
            wealthStat.Reset(10000);

            foreach (var tooltip in tooltips)
            {
                Destroy(tooltip);
            }
            tooltips.Clear();
        }

        private void OnChange()
        {
            ChangeEvent?.Invoke();
        }

        private static double CalculateTemperatureChange(double co2)
        {
            return co2 / 1000;
        }
    }

    public class ForecastableStat : AnimatedStat
    {
        public AnimatedStat Forecast { get; private set; }

        public ForecastableStat(List<String> tooltipQueue, Text text, string format, string suffix, string explainer, float step, float forecastStep)
            : base(tooltipQueue, text, format, suffix, explainer, step)
        {
            GameObject forecastObject = new GameObject();
            Shadow shadow = forecastObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);
            Text forecastText = forecastObject.AddComponent<Text>();
            forecastText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            forecastText.color = new Color(1, 1, 1);
            forecastText.fontSize = 10;
            forecastText.rectTransform.anchorMin = new Vector2(0.5f, 0);
            forecastText.rectTransform.anchorMax = new Vector2(0.5f, 0);
            forecastText.rectTransform.pivot = new Vector2(0.5f, 0);
            forecastText.rectTransform.SetParent(text.transform);
            forecastText.rectTransform.localPosition = new Vector3(0, -50);
            ContentSizeFitter fitter = forecastObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            HorizontalLayoutGroup group = forecastObject.AddComponent<HorizontalLayoutGroup>();
            group.childControlHeight = true;
            group.childControlWidth = true;
            Forecast = new AnimatedStat(tooltipQueue: null, forecastText, format, suffix, explainer, forecastStep, true);
        }

        public override void Update()
        {
            base.Update();
            Forecast.Update();
        }
    }

    public class AnimatedStat
    {
        public double Value
        {
            get { return value; }
            set
            {
                double change = value - this.value;
                this.value = value;
                if (change > 0)
                {
                    tooltipQueue?.Add("+" + change + suffix + explainer);
                }
                else if (change < 0)
                {
                    change *= -1;
                    tooltipQueue?.Add("-" + change + suffix + explainer);
                }
                ChangeEvent?.Invoke();
            }
        }
        private double value;

        public double Shown
        {
            get { return shown; }
            set
            {
                shown = value;
                if (isRelative)
                {
                    if (shown < 0)
                    {
                        text.text = "-" + (-shown).ToString(format) + suffix;
                    }
                    else
                    {
                        text.text = "+" + shown.ToString(format) + suffix;
                    }
                }
                else
                {
                    text.text = shown.ToString(format) + suffix;
                }
            }
        }
        private double shown;

        private readonly List<String> tooltipQueue;
        private readonly Text text;
        private readonly string format;
        private readonly string suffix;
        private readonly string explainer;
        private readonly float step;
        private readonly bool isRelative;

        public event Action ChangeEvent;

        public AnimatedStat(
            List<String> tooltipQueue,
            Text text,
            string format,
            string suffix,
            string explainer,
            float step,
            bool isRelative = false)
        {
            this.tooltipQueue = tooltipQueue;
            this.text = text;
            this.format = format;
            this.suffix = suffix;
            this.explainer = explainer;
            this.step = step;
            this.isRelative = isRelative;
        }

        public virtual void Update()
        {
            Shown += Mathf.Clamp((float)(Value - Shown), -step, step);
        }

        public void Reset(double value, double shown = 0)
        {
            this.value = value;
            Shown = shown;
            ChangeEvent?.Invoke();
        }
    }
}
