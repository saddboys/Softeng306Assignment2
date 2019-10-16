using System.Collections;
using System.Collections.Generic;
using Game;
using Game.CityMap;
using Game.Story;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

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
            get { return Resources.LoadAll<Sprite>("EventSprites/hurricane")[0]; }
        }

        private Coroutine coroutine;

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "Hurricane";
        private const string DESCRIPTION = "Oh no! A hurricane happened :(";
        public override void OnYesClick()
        {
            // Destroy random buildings
            coroutine = StartCoroutine(DestroyBuildings());
            // Happiness goes down
            StoryManager.city.Stats.Reputation -= 10;
        }

        IEnumerator DestroyBuildings()
        {
            StoryManager.city.NextTurnEvent += StopHurricane;
            MapTile[] tiles = StoryManager.city.Map.Tiles;
            foreach (var tile in tiles)
            {

                if (tile.Structure != null)
                {
                   
                    new DemolishFactory(StoryManager.city).BuildOnto(tile);
                    yield return new WaitForSeconds(3);
                    
                }
            }
        }

        private void StopHurricane()
        {
            StoryManager.city.NextTurnEvent -= StopHurricane;
            StopCoroutine(coroutine);
            ParticleSystem particles = StoryManager.city.Map.parent.transform.Find("CopyStructures").Find("CustomDemolishParticle").gameObject
                .GetComponent<ParticleSystem>();
             particles.Stop();
             Destroy(particles);
        }


        public override void GenerateScene(GameObject canvas)
        {
            StoryManager.city.NextTurnEvent += StopWind;
            GameObject customParticleSystem = new GameObject("HurricaneParticle");
            customParticleSystem.transform.SetParent(StoryManager.city.Map.gameObject.transform,false);
            customParticleSystem.transform.position = new Vector3(10,14,32);

            Quaternion quaternion = Quaternion.Euler(0, 0, -20);
            customParticleSystem.transform.rotation = quaternion;
            ParticleSystem particles = customParticleSystem.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);

            ParticleSystem.MainModule mainParticle = particles.main;
            mainParticle.startLifetime = 2f;
            mainParticle.startSpeed = 20;

            ParticleSystem.EmissionModule emissionModule = particles.emission;
            emissionModule.rateOverTime = 50;
            
            Renderer renderer =  particles.GetComponent<Renderer>();
            renderer.sortingLayerName = "Terrain";
            ParticleSystem.TextureSheetAnimationModule textureSheet =
                particles.textureSheetAnimation;
            textureSheet.enabled = true;
            textureSheet.mode = ParticleSystemAnimationMode.Sprites;
            textureSheet.AddSprite(Resources.Load<Sprite>("EventSprites/park2"));

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.SingleSidedEdge;
            shapeModule.radius = 25;
            shapeModule.rotation = new Vector3(0,0,180);
        }

        private void StopWind()
        {
            ParticleSystem particles = StoryManager.city.Map.gameObject.transform.Find("HurricaneParticle")
                .GetComponent<ParticleSystem>();
            particles.Stop();
            StoryManager.city.NextTurnEvent -= StopWind;
            StartCoroutine(StoppingWind());
        }

        IEnumerator StoppingWind()
        {
            yield return new WaitForSeconds(2);
            Destroy(StoryManager.city.Map.gameObject.transform.Find("HurricaneParticle").gameObject);
        }
    }
}