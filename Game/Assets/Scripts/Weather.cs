using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Game
{
    public class Weather
    {
        private Random random = new Random();
        private ParticleSystem particles;
        private float mapWidth;
        private float mapHeight;
        private bool isGenerating;
        private double triggerSecondsLeft;
        private bool windy = false;
        private GameObject sunFilter;
        private GameObject darkFilter;

        public Weather(GameObject gameObject, GameObject sunFilter, GameObject darkFilter)
        {
            this.sunFilter = sunFilter;
            this.darkFilter = darkFilter;

            Tilemap map = gameObject.GetComponent<Tilemap>();
            mapWidth = map.size.x * map.cellSize.x;
            mapHeight = map.size.y * map.cellSize.y;

            particles = gameObject.AddComponent<ParticleSystem>();
            particles.Stop();
            Particles.InitParticleSystem(particles);

            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(1, 1.2f);
            main.startLifetime = mapWidth / 1.1f;
            main.duration = mapWidth / 1.1f;
            main.prewarm = true;
            main.startSize = 3;
            main.startColor = Color.white;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = 2000;

            var emission = particles.emission;
            emission.rateOverTime = 0;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.position = gameObject.transform.position * -1 - new Vector3(mapWidth / 2, 0);
            shape.rotation = new Vector3(0, 90, 0);
            shape.scale = new Vector3(2, 1, 1);
            shape.randomPositionAmount = 1;

            var renderer = particles.GetComponent<ParticleSystemRenderer>();
            renderer.sortingLayerName = "Structure";
            renderer.sortingOrder = 2000;
            Material material = new Material(renderer.material);
            material.mainTexture = Resources.Load<Texture>("Textures/CloudParticle");
            renderer.material = material;

            particles.Play();
        }

        public void Update()
        {
            var emission = particles.emission;
            triggerSecondsLeft -= Time.deltaTime;
            if (triggerSecondsLeft < 0) {
                if (isGenerating)
                {
                    emission.rateOverTime = 0;
                    triggerSecondsLeft = random.Next(1, 6);
                    isGenerating = false;
                }
                else if (!isGenerating)
                {
                    var shape = particles.shape;
                    shape.position = new Vector3(shape.position.x, (float)random.NextDouble() * mapHeight - mapHeight / 2.0f);
                    emission.rateOverTime = windy ? 7 : 3;
                    triggerSecondsLeft = random.Next(2, 5);
                    isGenerating = true;
                }
            }
        }

        public void Normal()
        {
            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(1, 1.2f);
            windy = false;
            sunFilter.SetActive(false);
            darkFilter.SetActive(false);
        }

        public void Windy()
        {
            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(4, 4.2f);
            windy = true;
            sunFilter.SetActive(false);
            darkFilter.SetActive(true);
        }

        public void Stormy()
        {
            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(4, 4.2f);
            windy = true;
            sunFilter.SetActive(false);
            darkFilter.SetActive(true);
        }


        public void Sunny()
        {
            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 0.6f);
            windy = true;
            sunFilter.SetActive(true);
            darkFilter.SetActive(false);
        }
    }
}
