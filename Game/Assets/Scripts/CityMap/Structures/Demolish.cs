using System;
using System.Collections;
using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;
using Terrain = Game.CityMap.Terrain;

namespace Game.CityMap
{
    public class DemolishFactory : StructureFactory
    {
        public override int Cost
        {
            get
            {
                return 100;
            }
        }
        public DemolishFactory(City city) : base(city)
        {
            buildSound = Resources.Load<AudioClip>("SoundEffects/Demolish");
        }
        public DemolishFactory() : base() { }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/Demolish");

        protected override Structure Create()
        {
            return null;
        }
        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!CanBuild(out reason))
            {
                return false;
            }
            if (tile.Structure == null)
            {
                reason = "Nothing to demolish here";
                return false;
            } 
            if (tile.Structure.GetType() == typeof(Thantec) || 
                tile.Structure.GetType() == typeof(ResearchFacility) || 
                tile.Structure.GetType() == typeof(GiantCooler))
            {
                reason = "Cannot demolish a Thantec building";
                return false;
            } 
            if (tile.Structure.GetType() == typeof(House) && City.Stats.Population < 5)
            {
                reason = "Cannot demolish a house when you have less than 5k people";
                return false;
            }

            if (tile.Structure.StructElectricity > City.Stats.ElectricCapacity)
            {
                reason = "Demolishing this will create an electricity shortage";
                return false;
            } 
            
            if (tile.Structure.GetType() == typeof(Mountain))
            {
                reason = "Cannot demolish mountains";
                return false;
            }

            reason = "";
            return true;
        }

        public override void BuildOnto(MapTile tile)
        {
           // City.RestartGameEvent += StopDemolish;
            //City.NextTurnEvent += StopDemolish;
            City?.StartCoroutine(GenerateDestructionParticles(tile));
             //Test(tile);
            // Note: Get structure before it is demolished.
            if (City != null)
            {
                City.Stats.AddContribution(tile.Structure.GetStatsChangeOnDemolish());
            } 

            base.BuildOnto(tile);
        }
        
        IEnumerator GenerateDestructionParticles(MapTile tile)
        {
            GameObject copyOfGameObject = GameObject.Instantiate(tile.Structure.GameObject);

            copyOfGameObject.name = "CopyStructures";
            copyOfGameObject.transform.SetParent(tile.Structure.GameObject.transform.parent.gameObject.transform);
            GameObject customParticleSystem = new GameObject("CustomDemolishParticle");
            customParticleSystem.transform.SetParent(copyOfGameObject.transform.parent,false);
            customParticleSystem.transform.position = copyOfGameObject.transform.position;
            ParticleSystem particles = customParticleSystem.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);
            particles.Stop();

            copyOfGameObject.AddComponent<ShakerBehaviour>();

            ParticleSystem.MainModule mainModule = particles.main;
            mainModule.startColor = Color.white;
            mainModule.startLifetime = 1;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(0.3f,0.5f);
            mainModule.startSpeed = 0.6f;
            mainModule.maxParticles = 100;
            mainModule.loop = false;
            mainModule.duration = 1;

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Sphere;
            shapeModule.angle = 25;
            shapeModule.radius = 0.8f;
            shapeModule.scale = new Vector3(0.3f, 0.1f, 1);
            shapeModule.position = new Vector3(0, -0.4f, 0);

            var emission = particles.emission;
            emission.rateOverTime = 30;
            emission.enabled = true;

            var colorOverLifetime = particles.colorOverLifetime;
            Gradient gradientLifetime = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = Color.white;
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = Color.white;
            colorKeys[1].time = 1.0f;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 1;
            alphaKeys[0].time = 0.5f;
            alphaKeys[1].alpha = 0;
            alphaKeys[1].time = 1.0f;
            gradientLifetime.SetKeys(colorKeys, alphaKeys);
            colorOverLifetime.color = gradientLifetime;
            colorOverLifetime.enabled = true;

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1,
                new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f, 0, 0)));
            sizeOverLifetime.enabled = true;

            var limit = particles.limitVelocityOverLifetime;
            limit.separateAxes = true;
            limit.limitX = 1;
            limit.limitY = 0;
            limit.limitZ = 0;
            limit.dampen = 1;
            limit.enabled = true;

            ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = particles.textureSheetAnimation;
            textureSheetAnimationModule.enabled = true;
            textureSheetAnimationModule.mode = ParticleSystemAnimationMode.Sprites;
            textureSheetAnimationModule.SetSprite(0,Resources.Load<Sprite>("Textures/CloudParticle"));

            Renderer renderer = particles.GetComponent<Renderer>();
            renderer.sortingLayerName = "Structure";
            renderer.sortingOrder = 100;

            particles.Play();

            var existingSmoker = tile.Structure.GameObject.GetComponentInChildren<ParticleSystem>();
            if (existingSmoker != null)
            {
                var position = existingSmoker.transform.localPosition;
                var scale = existingSmoker.transform.localScale;

                // Wait until structure does its unrendering.
                yield return new WaitForEndOfFrame();

                // Copy over existing smoker and make them stay at their place relative to the world even when the building is falling.
                existingSmoker.transform.SetParent(copyOfGameObject.transform);
                existingSmoker.transform.localScale = scale;
                existingSmoker.transform.localPosition = position;

                // Stop any existing smokers. Let their trail stay though.
                foreach (var smoker in copyOfGameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    smoker.Stop();
                }
            }

            yield return new WaitForSeconds(10);

            GameObject.Destroy(copyOfGameObject);
            GameObject.Destroy(customParticleSystem);
        }

        private void StopDemolish()
        {
            if (City.Map.parent.transform.Find("CopyStructures") != null)
            {
                ParticleSystem particles = City.Map.parent.transform.Find("CopyStructures").Find("CustomDemolishParticle")
                    .gameObject
                    .GetComponent<ParticleSystem>();
                particles.Stop();
            }
            City.NextTurnEvent -= StopDemolish;
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Demolish a structure";
            details = "Be careful with what you destroy, as your actions cannot be undone. Click on a tile if you are sure.";
        }
    }

    public class ShakerBehaviour : MonoBehaviour
    {
        private float startingY;
        private float startingTime;
        private SpriteRenderer sprite;

        private void Start()
        {
            startingY = transform.position.y;
            startingTime = Time.time;
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // Shake and make it fall.
            var newPosition = new Vector3
            {
                x = transform.position.x,
                y = startingY + Mathf.Sin(Time.time * Mathf.PI * 2 * 15) * 0.03f - (Time.time - startingTime) * 0.3f,
                z = 0,
            };
            var translation = newPosition - transform.position;
            transform.position = newPosition;

            foreach (var smoke in GetComponentsInChildren<ParticleSystem>())
            {
                smoke.transform.position -= translation;
            }
            if (sprite != null)
            {
                // Fade away.
                sprite.color = new Color(1, 1, 1, 1 - (Time.time - startingTime));
            }
        }
    }
}