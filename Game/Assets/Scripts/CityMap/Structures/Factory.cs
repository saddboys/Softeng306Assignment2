using Game;
using Game.CityMap;
using UnityEngine;
using UnityEditor;

namespace Game.CityMap
{
    public class Factory : Structure
    {
        public const int StructCO2 = 10;
        public const int StructReputation = 0;
        public const int StructCost = 3000;
        public const int StructUpkeep = 500;
        public const int StructScore = 750;
        public const int StructPopulation = -5;
        public const int StructElectricity = -10;
        
        public override Stats GetStatsContribution()
        {
            return new Stats()
            {
                CO2 = StructCO2,
                Wealth = StructUpkeep
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                Population = -StructPopulation,
                ElectricCapacity = -StructElectricity,
                Reputation = -StructReputation
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/FactoryNew", new Vector2(5f, 5f));
            }
            else
            {           
                Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/FactoryNew", new Vector2(5f, 5f));
            }

            // Render smoke.
            GameObject smoke = new GameObject();
            smoke.transform.SetParent(GameObject.transform);
            smoke.transform.localPosition = new Vector3(-2.7f, 6.6f);
            smoke.transform.localScale = new Vector3(1, 1, 1);

            ParticleSystem particles = smoke.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);

            var main = particles.main;
            main.startLifetime = 4;
            main.startSize = 1;
            main.startSpeed = 1;
            main.startColor = new Color(221, 221, 221);
            main.maxParticles = 40;

            var emission = particles.emission;
            emission.rateOverTime = 4;
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
            force.y = new ParticleSystem.MinMaxCurve(-0.37f, 0.4f);
            force.randomized = true;
            force.enabled = true;

            var colorOverLifetime = particles.colorOverLifetime;
            Gradient gradientLifetime = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = new Color(178, 178, 178);
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = Color.white;
            colorKeys[1].time = 0.1f;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0.6f;
            alphaKeys[1].alpha = 0;
            alphaKeys[1].time = 1.0f;
            gradientLifetime.SetKeys(colorKeys, alphaKeys);
            colorOverLifetime.color = gradientLifetime;
            colorOverLifetime.enabled = true;

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1,
                new AnimationCurve(new Keyframe(0.0f, 0.1f), new Keyframe(0.23f, 0.3f)));
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
            title = "Factory";
        }
    }

    public class FactoryFactory : StructureFactory
    {
        public override int Cost
        {
            get
            {
                return Factory.StructCost;
            }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/FactoryNew");
        public FactoryFactory(City city) : base(city) { }
        public FactoryFactory() : base() { }

        protected override Structure Create()
        {
            return new Factory();
        }
        
        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < -Factory.StructElectricity)
            {
                reason = "Not enough electric capacity";
                return false;
            }
            return true;
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

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += Factory.StructElectricity;
                City.Stats.Reputation += Factory.StructReputation;
                City.Stats.Score += Factory.StructScore;
                City.Stats.Population += Factory.StructPopulation;
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a factory";
            details = "Citizens of your town need a place to work. Click on a tile to build a factory.";
        }
    }
}