using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class PowerPlant : Structure
    {
        
        public static int StructCO2 = 30;
        public static int StructReputation = -1;
        public static int StructCost = 1500;
        public static int StructUpkeep = -200;
        public static int StructScore = 600;
        public static int StructPopulation = -5;
        public static int StructElectricity = 30;
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = StructCO2,
                Wealth = StructUpkeep,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                Population = -StructPopulation,
                Reputation = -StructReputation,
                ElectricCapacity = -StructElectricity,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/powerPlant", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/powerPlant", new Vector2(1, 1.5f));
            }

            // Render smoke.
            GameObject smoke = new GameObject();
            smoke.transform.SetParent(GameObject.transform);
            smoke.transform.localPosition = new Vector3(-0.23f, 0.55f);
            smoke.transform.localScale = new Vector3(1, 1, 1);

            ParticleSystem particles = smoke.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);

            var main = particles.main;
            main.startLifetime = 5;
            main.startSize = 1;
            main.startSpeed = 1;
            main.startColor = new Color(221, 221, 221);
            main.maxParticles = 40;

            var emission = particles.emission;
            emission.rateOverTime = 3;
            emission.enabled = true;

            var shape = particles.shape;
            shape.angle = 10;
            shape.radius = 0.0001f;
            shape.rotation = new Vector3(-90, 90, 0);
            shape.scale = new Vector3(0.05f, 0.05f, 0.05f);
            shape.enabled = true;

            var limit = particles.limitVelocityOverLifetime;
            limit.limit = 1;
            limit.dampen = 1;
            limit.multiplyDragByParticleSize = false;
            limit.multiplyDragByParticleVelocity = true;
            limit.drag = 4;
            limit.enabled = true;

            var force = particles.forceOverLifetime;
            force.x = new ParticleSystem.MinMaxCurve(0.15f, 0.2f);
            force.y = new ParticleSystem.MinMaxCurve(-0.45f, 0.5f);
            force.randomized = true;
            force.enabled = true;

            var colorOverLifetime = particles.colorOverLifetime;
            Gradient gradientLifetime = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = new Color(178, 178, 178);
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = Color.white;
            colorKeys[1].time = 0.1f;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];
            alphaKeys[0].alpha = 0;
            alphaKeys[0].time = 0;
            alphaKeys[1].alpha = 1;
            alphaKeys[1].time = 0.15f;
            alphaKeys[2].alpha = 1;
            alphaKeys[2].time = 0.5f;
            alphaKeys[3].alpha = 0;
            alphaKeys[3].time = 1.0f;
            gradientLifetime.SetKeys(colorKeys, alphaKeys);
            colorOverLifetime.color = gradientLifetime;
            colorOverLifetime.enabled = true;

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1,
                new AnimationCurve(new Keyframe(0.0f, 0.12f), new Keyframe(0.025f, 0.45f, 5, 5), new Keyframe(0.2f, 0.7f)));
            sizeOverLifetime.enabled = true;

            var renderer = smoke.GetComponent<ParticleSystemRenderer>();
            renderer.sortingLayerName = "Structure";
            renderer.sortingOrder = 1000;
            Material material = new Material(renderer.material);
            material.mainTexture = Resources.Load<Texture>("Textures/CloudParticle");
            renderer.material = material;
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Power plant";
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        public PowerPlantFactory(City city) : base(city) { }
        public PowerPlantFactory() : base() { }
        public override int Cost
        {
            get { return PowerPlant.StructCost; }
        }
        public override int Population
        {
            get { return -PowerPlant.StructPopulation; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/powerPlant");

        protected override Structure Create()
        {
            return new PowerPlant();
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += PowerPlant.StructElectricity;
                City.Stats.Score += PowerPlant.StructScore;
            }
        }

        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!base.CanBuildOnto(tile, out reason))
            {
                return false;
            }

            if (tile.Terrain.TerrainType == Terrain.TerrainTypes.Ocean)
            {
                reason = "Cannot build onto water";
                return false;
            }

            return true;
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a power plant";
            meta = "Cost: $" + PowerPlant.StructCost + "k" + "\t\t" +
                   "CO2: " + PowerPlant.StructCO2 + "MT" + "\n" +
                   "Electricity: " + PowerPlant.StructElectricity + "\t\t" +
                   "Upkeep: $" + -PowerPlant.StructUpkeep + "k";
            details = "Requires 5k workers. Everything needs power to function. Be careful with it's pollution. " +
                      "Click on a tile to build a power plant. ";
        }
    }
}