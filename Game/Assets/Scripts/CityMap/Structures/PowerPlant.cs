using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class PowerPlant : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = 1,
                Wealth = 10,
                ElectricCapacity = 0,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = -20,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/powerPlant", new Vector2(1, 1.5f));

            // Render smoke.
            // Now the fun begins...

            GameObject smoke = new GameObject();
            smoke.transform.SetParent(GameObject.transform);
            smoke.transform.localPosition = new Vector3(-0.23f, 0.55f);
            smoke.transform.localScale = new Vector3(1, 1, 1);

            ParticleSystem particles = smoke.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);

            var main = particles.main;
            main.startLifetime = 10;
            main.startSize = 0.1f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
            main.maxParticles = 4000;

            var emission = particles.emission;
            emission.rateOverTime = 200;
            emission.enabled = true;

            var shape = particles.shape;
            shape.angle = 10;
            shape.radius = 3.0f;
            shape.rotation = new Vector3(-90, 90, 0);
            shape.scale = new Vector3(0.05f, 0.05f, 0.05f);
            shape.enabled = true;

            var limit = particles.limitVelocityOverLifetime;
            limit.limit = 1;
            limit.dampen = 1;
            limit.multiplyDragByParticleSize = true;
            limit.multiplyDragByParticleVelocity = true;
            limit.drag = 400;
            limit.enabled = true;

            var force = particles.forceOverLifetime;
            force.x = new ParticleSystem.MinMaxCurve(0.05f, 0.01f);
            force.y = new ParticleSystem.MinMaxCurve(-0.4f, 0.4f);
            force.randomized = true;
            force.enabled = true;

            var colorOverLifetime = particles.colorOverLifetime;
            Gradient gradientLifetime = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = Color.white;
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = Color.white;
            colorKeys[1].time = 1.0f;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 82.0f / 255.0f;
            alphaKeys[0].time = 0.0f;
            alphaKeys[1].alpha = 0;
            alphaKeys[1].time = 1.0f;
            gradientLifetime.SetKeys(colorKeys, alphaKeys);
            colorOverLifetime.color = gradientLifetime;
            colorOverLifetime.enabled = true;

            var colorBySpeed = particles.colorBySpeed;
            Gradient gradientSpeed = new Gradient();
            GradientColorKey[] colorKeysSpeed = new GradientColorKey[2];
            colorKeysSpeed[0].color = Color.white;
            colorKeysSpeed[0].time = 0.0f;
            colorKeysSpeed[1].color = Color.black;
            colorKeysSpeed[1].time = 1.0f;
            GradientAlphaKey[] alphaKeysSpeed = new GradientAlphaKey[2];
            alphaKeysSpeed[0].alpha = 1;
            alphaKeysSpeed[0].time = 0;
            alphaKeysSpeed[1].alpha = 1;
            alphaKeysSpeed[1].time = 1;
            gradientSpeed.SetKeys(colorKeysSpeed, alphaKeysSpeed);
            colorBySpeed.color = gradientSpeed;
            colorBySpeed.range = new Vector2(0, 0.3f);
            colorBySpeed.enabled = true;

            var renderer = smoke.GetComponent<ParticleSystemRenderer>();
            renderer.sortingLayerName = "Structure";
            renderer.sortingOrder = 1000;
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        public PowerPlantFactory(City city) : base(city) { }
        public PowerPlantFactory() : base() { }
        public override int Cost
        {
            get { return 4000; }
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
                City.Stats.ElectricCapacity += 20;
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
    }
}