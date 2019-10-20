using UnityEngine;

namespace Game.CityMap
{
    public class GiantCooler : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                
                Wealth = 50,
                CO2 = 1,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = 1,
                Population = -4,
                Reputation = 1
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {

            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "EventSprites/cooler", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "EventSprites/cooler", new Vector2(1, 1.5f));
            }
            
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "A giant cooler";
            details = "Thantec has developed a new machine to cool down the town!";
        }
        
    }
    
    public class GiantCoolerFactory : StructureFactory
    {
        public GiantCoolerFactory(City city) : base(city) { }
        public GiantCoolerFactory() : base() { }

        public override int Cost
        {
            get { return 250; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("EventSprites/cooler");

        protected override Structure Create()
        {
            return new GiantCooler();
        }

        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < 1)
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

            if (tile.Terrain.TerrainType==(Terrain.TerrainTypes.Ocean))
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
                City.Stats.ElectricCapacity -= 1;
                City.Stats.Population += 4;
                City.Stats.Reputation -= 1;
            }
            
            GenerateWindEffect(tile);
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build the cooler!";
            details = "Thantec has developed a new machine to cool down the town!";
        }
        
        private void GenerateWindEffect(MapTile tile)
        {
            GameObject air = new GameObject();
            air.transform.SetParent(tile.Structure.GameObject.transform);
            air.transform.localPosition = new Vector3(1f, -0.5f,-4.5f);
            air.transform.localScale = new Vector3(0.5f, 0.5f, 1);

            ParticleSystem particles = air.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);

            ParticleSystem.MainModule mainParticle = particles.main;
            mainParticle.startLifetime = 1.5f;
            mainParticle.startSpeed = 0;
            mainParticle.startSize =  0.1f;
            mainParticle.maxParticles = 5;
            
            
            ParticleSystem.TrailModule trailMode = particles.trails;
            trailMode.enabled = true;
            trailMode.lifetime = new ParticleSystem.MinMaxCurve(0.8f);
            trailMode.dieWithParticles = false;
            AnimationCurve trailCurve = new AnimationCurve();
            trailCurve.AddKey(0, 0);
            trailCurve.AddKey(0.5f, 1);
            trailCurve.AddKey(1, 0);
            trailMode.widthOverTrail = new ParticleSystem.MinMaxCurve(1,trailCurve);
            

            ParticleSystem.EmissionModule emissionModule = particles.emission;
            emissionModule.rateOverTime = 3;
            ParticleSystemRenderer particleRenderer =  particles.GetComponent<ParticleSystemRenderer>();
            particleRenderer.trailMaterial =  Resources.Load<Material>("Wind");
            

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Box;
            shapeModule.rotation = new Vector3(0,0,180);
            shapeModule.scale = new Vector3(0.1f,1,1);

            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = particles.velocityOverLifetime;
            velocityOverLifetimeModule.enabled = true;

            AnimationCurve curveX = new AnimationCurve();
            curveX.AddKey(0, 1);
            curveX.AddKey(1, 1);
            
            AnimationCurve curveY = new AnimationCurve();
            curveY.AddKey(0, 0);
            curveY.AddKey(0.33f, 1);
            curveY.AddKey(0.66f, -1);
            curveY.AddKey(1, 0);
            
            AnimationCurve curveZ = new AnimationCurve();
            curveZ.AddKey(0, 0);
            curveZ.AddKey(1.0f, 0);

            //velocityOverLifetimeModule.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(1, curveX);
            velocityOverLifetimeModule.y = new ParticleSystem.MinMaxCurve(1, curveY);
            velocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(0, curveZ);

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = particles.colorOverLifetime;
            colorOverLifetimeModule.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(new [] { new GradientColorKey(new Color32(167,232,242,255), 0.0f),
                new GradientColorKey(Color.white, 1.0f) },
                new [] { new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(0.0f, 1.0f) });
            colorOverLifetimeModule.color = gradient;
        }
    }
}