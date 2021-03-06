using System.Collections;
using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
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

        private const string TITLE = "Flood";
        private const string DESCRIPTION = "A flood occurred! \nSome tiles have been destroyed by the water!";
        private List<Stack<MapTile>> tempFloodTiles;
        private List<Stack<Vector3Int>> tempFloodPosition;
       // private Stack<Vector3Int> tempFloodTilePositions;
        private Random random;

        public override void OnYesClick()
        {
            StoryManager.city.NextTurnEvent += DecreaseWater;
            StoryManager.city.EndGameEvent += StopAtEnd;
            StoryManager.city.RestartGameEvent += StopAtEnd;
            random = new Random();
            GenerateFloodPositions();
        }
        
        
        /// <summary>
        /// Creates the anchor points for where the flooding begins
        /// </summary>
        private void GenerateFloodPositions()
        {
            Tilemap map = StoryManager.city.Map.map;
            BoundsInt bounds = map.cellBounds;
            int height = bounds.size.y;
            int width = bounds.size.x;
            int lowestX = bounds.position.x;
            int lowestY = bounds.position.y;
            if (tempFloodTiles == null)
            {
                tempFloodTiles = new List<Stack<MapTile>>();
            }

            if (tempFloodPosition == null)
            {
                tempFloodPosition = new List<Stack<Vector3Int>>();
            }
            // Choose between 5-20 flood patches
            int numberOfPatches = random.Next(5, 20);
            for (int i = 0; i < numberOfPatches; i++)
            {
                // Choose a random location at the bounds of the cellmap
                int x = random.Next(lowestX, lowestX+width);
                int y = random.Next(lowestY, lowestY+height);

                Vector3Int position = new Vector3Int(x, y, 0);
                var tile = map.GetTile<MapTile>(position);
                if (tile != null)
                {
                    tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean);
                    tile.Structure = null;
                    Stack<MapTile> newStack = new Stack<MapTile>();
                    newStack.Push(tile);
                    Stack<Vector3Int> tilePosition = new Stack<Vector3Int>();
                    tilePosition.Push(position);
                    tempFloodTiles.Add(newStack);
                    tempFloodPosition.Add(tilePosition);
                    GenerateSurroundingWater(100);
                }
            }
        }

        /// <summary>
        ///  Recursively generate the surrounding water tiles at those anchor points
        /// </summary>
        /// <param name="probabilityToIncrease"></param>
        private void GenerateSurroundingWater(int probabilityToIncrease)
        {
            
            int generatedValue = random.Next(0, 101);
            if (generatedValue < probabilityToIncrease)
            {
                Tilemap map = StoryManager.city.Map.map;

                // Generate next position
                probabilityToIncrease -= 1;
                Stack<Vector3Int> current = tempFloodPosition[tempFloodTiles.Count-1];
                Stack<MapTile> currentTile = tempFloodTiles[tempFloodTiles.Count-1];
                Vector3Int topPosition = current.Peek();
                // Get the next position for the surroundings
                int addX = random.Next(-1,1);
                int addY = random.Next(-1,1);
                int nextX = topPosition.x + addX;
                int nextY = topPosition.y + addY;

                Vector3Int position = new Vector3Int(nextX,nextY , 0);
                
                var tile = map.GetTile<MapTile>(position);
                if (tile != null)
                {
                    tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean);
                    tile.Structure = null;
                    current.Push(position);
                    currentTile.Push(tile);
                    
                }
                GenerateSurroundingWater(probabilityToIncrease);
            }
        }
        

        /// <summary>
        /// Decreases the flooded water amount after each turn
        /// </summary>
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
                Stack<MapTile> tempTiles = tempFloodTiles[i];
                // Decrease 2 to 4 tiles at a time
                int toDecrease = random.Next(2, 5);
                for (int j = 0; j < toDecrease; j++)
                {
                    if (tempTiles.Count != 0)
                    {
                       // var tile = map.GetTile<MapTile>(tempFloodTilePositions.Pop());
                       var tile = tempTiles.Pop();
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass);
                    }
                }
            }
        }
        
        /// <summary>
        /// Stop the rain effects when the game ends
        /// </summary>
        private void StopAtEnd()
        {
            StoryManager.city.NextTurnEvent -= StopRain;
            StoryManager.city.EndGameEvent -= StopAtEnd;
            StoryManager.city.RestartGameEvent -= StopAtEnd;
        }

        /// <summary>
        /// Generates the rain particles when a flood occurs. This stops at the next turn
        /// </summary>
        /// <param name="canvas"></param>
        public override void GenerateScene(GameObject canvas)
        {
            StoryManager.city.NextTurnEvent += StopRain;
            GameObject customParticleSystem = new GameObject("CustomParticleSystem");
            customParticleSystem.transform.SetParent(StoryManager.city.Map.gameObject.transform,false);
            customParticleSystem.transform.position = new Vector3(-10,14,32);

            Quaternion quaternion = Quaternion.Euler(0, 0, 20);
            customParticleSystem.transform.rotation = quaternion;
            ParticleSystem particles = customParticleSystem.AddComponent<ParticleSystem>();
            Particles.InitParticleSystem(particles);

            ParticleSystem.MainModule mainParticle = particles.main;
            mainParticle.startLifetime = 2f;
            mainParticle.startSpeed = 80;
            mainParticle.startSize = 0.1f;
            mainParticle.maxParticles = 2000;

            ParticleSystem.EmissionModule emissionModule = particles.emission;
            emissionModule.rateOverTime = 1000;

            var trails = particles.trails;
            trails.lifetime = 0.01f;
            trails.enabled = true;
            
            var renderer =  particles.GetComponent<ParticleSystemRenderer>();
            renderer.sortingLayerName = "Terrain";
            renderer.trailMaterial = new Material(renderer.material);

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.SingleSidedEdge;
            shapeModule.radius = 25;
            shapeModule.rotation = new Vector3(0,0,180);

            StoryManager.city.Weather.Stormy();
        }

        /// <summary>
        /// Stops the rain particles
        /// </summary>
        private void StopRain()
        {
            StoryManager.city.NextTurnEvent -= StopRain;
            ParticleSystem particles = StoryManager.city.Map.gameObject.transform.Find("CustomParticleSystem")
                .GetComponent<ParticleSystem>();
            particles.Stop();
            StartCoroutine(StoppingRain());
        }

        /// <summary>
        /// Stop the rain iteratively to create a more realistic effect
        /// </summary>
        /// <returns></returns>
        IEnumerator StoppingRain()
        {
            yield return new WaitForSeconds(2);
            Destroy(StoryManager.city.Map.gameObject.transform.Find("CustomParticleSystem").gameObject);
            Destroy(StoryManager.storyManagerGameObject.GetComponent<FloodEvent>());
            StoryManager.city.Weather.Normal();
        }
    }
}