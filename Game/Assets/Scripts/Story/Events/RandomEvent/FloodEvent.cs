using System;
using System.Collections;
using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;
using Terrain = Game.CityMap.Terrain;

namespace Game.Story.Events.RandomEvent
{
    public class FloodEvent : StoryEvent
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
            get { return Resources.Load<Sprite>("EventSprites/flood"); }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "FLOOD";
        private const string DESCRIPTION = "Flooding happened bad luck :(";
        private List<Stack<Vector3Int>> tempFloodTiles;
       // private Stack<Vector3Int> tempFloodTilePositions;
        private Random random;

        public override void OnYesClick()
        {
            StoryManager.city.NextTurnEvent += DecreaseWater;
            StoryManager.city.EndGameEvent += StopAtEnd;
            random = new Random();
            GenerateFloodPositions();
        }
        private void GenerateFloodPositions()
        {
            
            int height = StoryManager.city.Map.HEIGHT;
            int width = StoryManager.city.Map.WIDTH;
            Tilemap map = StoryManager.city.Map.map;

            if (tempFloodTiles == null)
            {
                tempFloodTiles = new List<Stack<Vector3Int>>();
            }
            // Choose between 1-6 flood patches
            int numberOfPatches = random.Next(5, 20);

            for (int i = 0; i < numberOfPatches; i++)
            {
                int x = random.Next(-width/2+1, width/2 + 1);
                int y = random.Next(-height/2+1, height/2+ 1);
                Vector3Int position = new Vector3Int(x, y, 0);
                Vector3Int rotateCellPosition = StoryManager.city.Map.RotateCellPosition(position, true);
              //  Vector3Int position = new Vector3Int(-18, -14, 0);
                //PrintStuff(position);
                var tile = map.GetTile<MapTile>(rotateCellPosition);
                tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean);
                tile.Structure = null;
                Stack<Vector3Int> newStack = new Stack<Vector3Int>();
                newStack.Push(rotateCellPosition);
                tempFloodTiles.Add(newStack);
                
                GenerateSurroundingWater(100,i);
            }
        }

        private void GenerateSurroundingWater(int probabilityToIncrease, int listPosition)
        {
            
            int generatedValue = random.Next(0, 101);
            if (generatedValue < probabilityToIncrease)
            {
                Tilemap map = StoryManager.city.Map.map;

                // Generate next position
                probabilityToIncrease -= 1;
                Stack<Vector3Int> current = tempFloodTiles[listPosition];
                Vector3Int topPosition = current.Peek();
                // Get the next position for the surroundings
                int addX = random.Next(-1,1);
                int addY = random.Next(-1,1);
                int nextX = topPosition.x + addX;
                int nextY = topPosition.y + addY;

                int height = StoryManager.city.Map.HEIGHT;
                int width = StoryManager.city.Map.WIDTH;
                
                Vector3Int position = new Vector3Int(nextX,nextY , 0);
                
                var tile = map.GetTile<MapTile>(position);
                if (tile != null)
                {
                    tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean);
                    tile.Structure = null;
                    current.Push(position);
                }
                GenerateSurroundingWater(probabilityToIncrease, listPosition);
            }
        }

        private void DecreaseWater()
        {
            if (tempFloodTiles != null && tempFloodTiles.Count == 0)
            {
                StoryManager.city.NextTurnEvent -= DecreaseWater;
                return;
            }
            Tilemap map = StoryManager.city.Map.map;
            for (int i = 0; i < tempFloodTiles.Count; i++)
            {
                Stack<Vector3Int> tempFloodTilePositions = tempFloodTiles[i];
                // Decrease 2 to 4 tiles at a time
                int toDecrease = random.Next(2, 5);
                for (int j = 0; j < toDecrease; j++)
                {
                    if (tempFloodTilePositions.Count != 0)
                    {
                        var tile = map.GetTile<MapTile>(tempFloodTilePositions.Pop());
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass);
                    }
                    
                }
            }
        }
        
        private void StopAtEnd()
        {
            StoryManager.city.NextTurnEvent -= StopRain;
            StoryManager.city.EndGameEvent -= StopAtEnd;
        }

        public override void GenerateScene(GameObject canvas)
        {
            StoryManager.city.NextTurnEvent += StopRain;
            GameObject customParticleSystem = new GameObject("CustomParticleSystem");
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
            textureSheet.AddSprite(Resources.Load<Sprite>("EventSprites/rainparticle"));

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.SingleSidedEdge;
            shapeModule.radius = 25;
            shapeModule.rotation = new Vector3(0,0,180);
        }

        private void StopRain()
        {
            StoryManager.city.NextTurnEvent -= StopRain;
            ParticleSystem particles = StoryManager.city.Map.gameObject.transform.Find("CustomParticleSystem")
                .GetComponent<ParticleSystem>();
            particles.Stop();
            StartCoroutine(StoppingRain());
        }

        IEnumerator StoppingRain()
        {
            yield return new WaitForSeconds(2);
            Destroy(StoryManager.city.Map.gameObject.transform.Find("CustomParticleSystem").gameObject);
            Destroy(StoryManager.storyManagerGameObject.GetComponent<FloodEvent>());
            
        }
    }
}