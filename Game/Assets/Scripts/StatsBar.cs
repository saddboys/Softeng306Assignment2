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
        private GameObject tooltipCanvas;
        public int RestartCount { get; private set; }

        ForecastableStat co2Stat;
        ForecastableStat tempStat;
        ForecastableStat popStat;
        ForecastableStat elecStat;
        ForecastableStat repStat;
        AnimatedStat scoreStat;
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
            // Monkeypatch fix. Don't want to touch game scene.
            var moneyLabel = moneyValueText.rectTransform.parent.gameObject.GetComponent<RectTransform>();
            moneyLabel.position = new Vector3
            {
                x = moneyLabel.position.x - 30,
                y = moneyLabel.position.y,
                z = moneyLabel.position.z,
            };
            var moneyTransform = moneyValueText.rectTransform;
            moneyTransform.position = new Vector3
            {
                x = moneyTransform.position.x + 20,
                y = moneyTransform.position.y,
                z = moneyTransform.position.z,
            };
            var scoreLabel = scoreValueText.rectTransform.parent.gameObject.GetComponent<Text>();
            scoreLabel.font = Resources.Load<Font>("Fonts/visitor1");
            scoreLabel.material = Resources.Load<Material>("Fonts/visitor1");
            Shadow shadow = scoreLabel.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);

            co2Stat = new ForecastableStat(tooltipQueue, co2ValueText, "F0", " MT", " CO2", 2, 0.5f);
            tempStat = new ForecastableStat(tooltipQueue, temperatureValueText, "F2", "°C", " Change", 0.01f, 0.001f, false);
            popStat = new ForecastableStat(tooltipQueue, populationValueText, "F1", "k", " Population", 1, 0.1f);
            elecStat = new ForecastableStat(tooltipQueue, electricCapacityValueText, "F0", "", " Electricity", 1, 0.1f);
            repStat = new ForecastableStat(tooltipQueue, reputationValueText, "F0", "%", " Reputation", 2, 0.5f);
            scoreStat = new AnimatedStat(tooltipQueue, scoreValueText, "F0", "", " Score", 200, false, false);
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
                var behaviour = tooltip.AddComponent<StatsChangeTooltip>();
                behaviour.Init(String.Join("\n", tooltipQueue), this, tooltipCanvas.transform);
                tooltipQueue.Clear();
            }

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

            // This will magically destroy all active tooltips.
            RestartCount++;
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

    public class StatsChangeTooltip : MonoBehaviour
    {
        private StatsBar parentBar;
        private int restartId;

        public void Init(string textContent, StatsBar bar, Transform parent)
        {
            parentBar = bar;
            restartId = bar.RestartCount;
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.position = Input.mousePosition;
            rectTransform.pivot = new Vector2(0, 0);
            rectTransform.SetParent(parent);
            ContentSizeFitter fitter = gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            HorizontalLayoutGroup group = gameObject.AddComponent<HorizontalLayoutGroup>();
            group.childControlHeight = true;
            group.childControlWidth = true;
            Text text = gameObject.AddComponent<Text>();
            text.font = Resources.Load<Font>("Fonts/visitor1");
            text.material = Resources.Load<Material>("Fonts/visitor1");
            text.text = textContent;
            text.fontSize = 12;
            Shadow shadow = gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);
        }

        private void Update()
        {
            // Make change tooltips float upwards and fade out.
            Text text = GetComponent<Text>();
            Color color = text.color;
            Vector3 pos = transform.position;
            pos.y += color.a / 2.0f;
            transform.position = pos;
            color.a -= 0.005f;
            text.color = color;

            // Remove tooltips that are no longer visible,
            // or if stats bar has restarted.
            if (GetComponent<Text>().color.a < 0.0f || restartId != parentBar.RestartCount)
            {
                Destroy(gameObject);
            }
        }
    }

    public class ForecastableStat : AnimatedStat
    {
        public AnimatedStat Forecast { get; private set; }

        public ForecastableStat(List<String> tooltipQueue, Text text, string format, string suffix, string explainer, float step, float forecastStep, bool fixAnchor = true)
            : base(tooltipQueue, text, format, suffix, explainer, step, false, fixAnchor)
        {
            text.alignment = TextAnchor.UpperCenter;
            GameObject forecastObject = new GameObject();
            Text forecastText = forecastObject.AddComponent<Text>();
            forecastText.font = Resources.Load<Font>("Fonts/visitor1");
            forecastText.material = Resources.Load<Material>("Fonts/visitor1");
            forecastText.color = new Color(1, 1, 1);
            forecastText.fontSize = 10;
            forecastText.rectTransform.anchorMin = new Vector2(0.5f, 0);
            forecastText.rectTransform.anchorMax = new Vector2(0.5f, 0);
            forecastText.rectTransform.pivot = new Vector2(0.5f, 0);
            forecastText.rectTransform.SetParent(text.transform);
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
                    tooltipQueue?.Add("+" + change.ToString(format) + suffix + explainer);
                }
                else if (change < 0)
                {
                    change *= -1;
                    tooltipQueue?.Add("-" + change.ToString(format) + suffix + explainer);
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
            bool isRelative = false,
            bool fixAnchor = true)
        {
            this.tooltipQueue = tooltipQueue;
            this.text = text;
            this.format = format;
            this.suffix = suffix;
            this.explainer = explainer;
            this.step = step;
            this.isRelative = isRelative;

            text.font = Resources.Load<Font>("Fonts/visitor1");
            text.material = Resources.Load<Material>("Fonts/visitor1");
            if (fixAnchor)
            {
                text.rectTransform.anchorMin = new Vector2(1, 0);
                text.rectTransform.anchorMax = new Vector2(1, 1);
                text.rectTransform.offsetMin = new Vector2(text.rectTransform.offsetMin.x, 0);
                text.rectTransform.offsetMax = new Vector2(text.rectTransform.offsetMax.x, 0);
            }
            Shadow shadow = text.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);
            ContentSizeFitter fitter = text.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            HorizontalLayoutGroup group =
                text.gameObject.GetComponent<HorizontalLayoutGroup>() ?? text.gameObject.AddComponent<HorizontalLayoutGroup>();
            group.childControlWidth = true;

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
