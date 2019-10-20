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
            if (tile.Structure.GetType() == typeof(House) && City.Stats.Population < 5)
            {
                reason = "Cannot demolish a house when you have less than 5k people";
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
            customParticleSystem.transform.SetParent(copyOfGameObject.transform,false);
            customParticleSystem.transform.position = copyOfGameObject.transform.position;
            ParticleSystem particles = customParticleSystem.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);
            particles.transform.localPosition  = new Vector3(0,0,0);
            

            ParticleSystem.MainModule mainModule = particles.main;
            //mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color32(194,194,194,255));
            mainModule.startColor =  new ParticleSystem.MinMaxGradient(new Color32(194,194,194,255)
                , new Color32(120,120,120,255));
            mainModule.startLifetime = 0.2f;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(0.7f,0.8f);
            mainModule.maxParticles = 20;
            mainModule.startSpeed = 0;

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Cone;
            shapeModule.angle = 25;
            shapeModule.radius = 0.8f;
            shapeModule.scale = new Vector3(1,0.5f,1);

            ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = particles.textureSheetAnimation;
            textureSheetAnimationModule.enabled = true;
            textureSheetAnimationModule.mode = ParticleSystemAnimationMode.Sprites;
            textureSheetAnimationModule.SetSprite(0,Resources.Load<Sprite>("Textures/CloudParticle"));

            Renderer renderer = particles.GetComponent<Renderer>();
            renderer.sortingLayerName = "Structure";
            renderer.sortingOrder = 100;
            yield return new WaitForSeconds(3);
            
            GameObject.Destroy(copyOfGameObject);
        }

        private void Test(MapTile tile)
        {
            GameObject copyOfGameObject = GameObject.Instantiate(tile.Structure.GameObject);
            copyOfGameObject.name = "CopyStructures";
            copyOfGameObject.transform.SetParent(tile.Structure.GameObject.transform.parent.gameObject.transform);
            GameObject customParticleSystem = new GameObject("CustomDemolishParticle");
            customParticleSystem.transform.SetParent(copyOfGameObject.transform,false);
            customParticleSystem.transform.position = copyOfGameObject.transform.position;
            ParticleSystem particles = customParticleSystem.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);
            particles.transform.localPosition  = new Vector3(0,0,0);
            

            ParticleSystem.MainModule mainModule = particles.main;
            //mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color32(194,194,194,255));
            mainModule.startColor =  new ParticleSystem.MinMaxGradient(new Color32(194,194,194,255)
                , new Color32(120,120,120,255));
            mainModule.startLifetime = 0.2f;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(0.7f,0.8f);
            mainModule.maxParticles = 20;
            mainModule.startSpeed = 0;

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Cone;
            shapeModule.angle = 25;
            shapeModule.radius = 0.8f;
            shapeModule.scale = new Vector3(1,0.5f,1);

            ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = particles.textureSheetAnimation;
            textureSheetAnimationModule.enabled = true;
            textureSheetAnimationModule.mode = ParticleSystemAnimationMode.Sprites;
            textureSheetAnimationModule.SetSprite(0,Resources.Load<Sprite>("Textures/CloudParticle"));

            Renderer renderer = particles.GetComponent<Renderer>();
            renderer.sortingLayerName = "Structure";
            renderer.sortingOrder = 100;
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
}