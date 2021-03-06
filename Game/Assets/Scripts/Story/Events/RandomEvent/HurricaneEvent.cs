using System.Collections;
using System.Collections.Generic;
using Game;
using Game.CityMap;
using Game.Story;
using UnityEngine;

namespace Story.Events.RandomEvent
{
    public class HurricaneEvent : StoryEvent
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }

        public override Sprite EventImage
        {
            get { return Resources.Load<Sprite>("EventSprites/hurricane"); }
        }

        private Coroutine coroutine;

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "Hurricane";
        private const string DESCRIPTION = "There is currently a hurricane!\n Quickly make your choices before all of" +
                                           " your buildings get destroyed!";
        public override void OnYesClick()
        {
            // Destroy random buildings
            coroutine = StartCoroutine(DestroyBuildings());
            // Happiness goes down
            StoryManager.city.Stats.Reputation -= 10;
        }

        /// <summary>
        /// Destroy the buildings slowly when the hurricane occurs
        /// </summary>
        /// <returns></returns>
        IEnumerator DestroyBuildings()
        {
            StoryManager.city.NextTurnEvent += StopHurricane;
            StoryManager.city.EndGameEvent += StopOtherEvents;
            StoryManager.city.RestartGameEvent += StopOtherEvents;
            MapTile[] tiles = StoryManager.city.Map.Tiles;
            foreach (var tile in tiles)
            {

                if (tile != null && tile.Structure != null)
                {
                    if (tile.Structure.GetType() != typeof(Mountain))
                    {
                        DemolishFactory demolishFactory = new DemolishFactory(StoryManager.city);
                        if (demolishFactory.CanBuildOnto(tile, out _))
                        {
                            demolishFactory.BuildOnto(tile);
                        }
                        yield return new WaitForSeconds(3);
                    }
                }
            }
        }

        /// <summary>
        /// Stop the destruction and wind effects after a next turn event
        /// </summary>
        private void StopHurricane()
        {
            StoryManager.city.NextTurnEvent -= StopHurricane;
            StopCoroutine(coroutine);
        }
        
        /// <summary>
        /// Removes all events when restarting the game
        /// </summary>
        private void StopOtherEvents()
        {
            StoryManager.city.NextTurnEvent -= StopHurricane;
            StoryManager.city.NextTurnEvent -= StopWind;
            StoryManager.city.EndGameEvent -= StopOtherEvents;
            StoryManager.city.RestartGameEvent -= StopOtherEvents;
        }


        /// <summary>
        /// Creates the wind particles 
        /// </summary>
        /// <param name="canvas"></param>
        public override void GenerateScene(GameObject canvas)
        {
            StoryManager.city.NextTurnEvent += StopWind;
            GameObject customParticleSystem = new GameObject("HurricaneParticle");
            customParticleSystem.transform.SetParent(StoryManager.city.Map.gameObject.transform,false);
            customParticleSystem.transform.position = new Vector3(0,0,-2);

            Quaternion quaternion = Quaternion.Euler(0, 0, 0);
            customParticleSystem.transform.rotation = quaternion;
            ParticleSystem particles = customParticleSystem.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);
            
            ParticleSystem.MainModule mainParticle = particles.main;
            mainParticle.startLifetime = 2f;
            mainParticle.startSpeed = 0;
            mainParticle.startSize =  0.5f;
            mainParticle.maxParticles = 20;

            ParticleSystem.TrailModule trailMode = particles.trails;
            trailMode.enabled = true;
            trailMode.lifetime = new ParticleSystem.MinMaxCurve(0.6f);
            AnimationCurve trailCurve = new AnimationCurve();
            trailCurve.AddKey(0, 0);
            trailCurve.AddKey(0.5f, 1);
            trailCurve.AddKey(1, 0);
            trailMode.widthOverTrail = new ParticleSystem.MinMaxCurve(1,trailCurve);
            

            ParticleSystem.EmissionModule emissionModule = particles.emission;
            emissionModule.rateOverTime = 3;
            ParticleSystemRenderer particleRenderer =  particles.GetComponent<ParticleSystemRenderer>();
            particleRenderer.sortingLayerName = "Terrain";
            particleRenderer.trailMaterial =  Resources.Load<Material>("Wind");

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Cone;
            shapeModule.radius = 20;
            shapeModule.rotation = new Vector3(0,0,180);

            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = particles.velocityOverLifetime;
            velocityOverLifetimeModule.enabled = true;

            // The below animation curves cause the loop effect
            AnimationCurve curveX = new AnimationCurve();
            curveX.AddKey(0, 10);
            curveX.AddKey(0.3f, 10);
            curveX.AddKey(0.5f, -10);
            curveX.AddKey(0.7f, 10);
            curveX.AddKey(1, 10);
            
            AnimationCurve curveY = new AnimationCurve();
            curveY.AddKey(0, 0);
            curveY.AddKey(0.3f, 0);
            curveY.AddKey(0.35f, 10);
            curveY.AddKey(0.65f, -10);
            curveY.AddKey(0.7f, 0);
            curveY.AddKey(1, 0);
            
            AnimationCurve curveZ = new AnimationCurve();
            curveZ.AddKey(0, 0);
            curveZ.AddKey(1.0f, 0);
            
            velocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(1, curveX);
            velocityOverLifetimeModule.y = new ParticleSystem.MinMaxCurve(1, curveY);
            velocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(0, curveZ);

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = particles.colorOverLifetime;
            colorOverLifetimeModule.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(new [] { new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f) },
                new [] { new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(0.0f, 1.0f) });
            colorOverLifetimeModule.color = gradient;

            ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = particles.sizeOverLifetime;
            sizeOverLifetimeModule.enabled = true;
            AnimationCurve velocityCurveX = new AnimationCurve();
            velocityCurveX.AddKey(0, 0);
            velocityCurveX.AddKey(0.4f, 0.3f);
            velocityCurveX.AddKey(0.55f, 0.85f);
            velocityCurveX.AddKey(0.75f, 0.85f);
            velocityCurveX.AddKey(0.85f, 0.6f);
            velocityCurveX.AddKey(1, 0);
            AnimationCurve velocityCurveY = new AnimationCurve();
            velocityCurveY.AddKey(0, 0);
            velocityCurveY.AddKey(1, 0);
            AnimationCurve velocityCurveZ = new AnimationCurve();
            velocityCurveZ.AddKey(0, 0);
            velocityCurveZ.AddKey(1, 0);
            
            sizeOverLifetimeModule.x =  new ParticleSystem.MinMaxCurve(1, velocityCurveX);
            sizeOverLifetimeModule.y =  new ParticleSystem.MinMaxCurve(0, velocityCurveY);
            sizeOverLifetimeModule.z =  new ParticleSystem.MinMaxCurve(0, velocityCurveZ);

            StoryManager.city.Weather.Windy();
        }

        /// <summary>
        /// Stops the wind particles 
        /// </summary>
        private void StopWind()
        {
            if (StoryManager.city.Map.Tiles.Length != 0)
            {
                ParticleSystem particles = StoryManager.city.Map.gameObject.transform.Find("HurricaneParticle")
                    .GetComponent<ParticleSystem>();
                particles.Stop();
            }

            StoryManager.city.NextTurnEvent -= StopWind;
            StartCoroutine(StoppingWind());
        }

        /// <summary>
        /// Stops the wind slowly to create a more realistic effect
        /// </summary>
        /// <returns></returns>
        IEnumerator StoppingWind()
        {
            yield return new WaitForSeconds(2);
            Destroy(StoryManager.city.Map.gameObject.transform.Find("HurricaneParticle").gameObject);
            StoryManager.city.Weather.Normal();
        }
    }
}