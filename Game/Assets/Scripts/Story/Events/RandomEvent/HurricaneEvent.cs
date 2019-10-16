using System.Collections;
using System.Collections.Generic;
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
                    yield return new WaitForSeconds(3);
                    new DemolishFactory(StoryManager.city).BuildOnto(tile);
                    
                }
            }
        }

        private void StopHurricane()
        {
            StopCoroutine(coroutine);
            ParticleSystem particles = StoryManager.city.Map.parent.transform.Find("CopyStructures").Find("CustomDemolishParticle").gameObject
                .GetComponent<ParticleSystem>();
             particles.Stop();
             Destroy(particles);
        }
        
        
    }
}