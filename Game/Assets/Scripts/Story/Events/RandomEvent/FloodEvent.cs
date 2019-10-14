using System;
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
            get { return SPRITE; }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "FLOOD";
        private const string DESCRIPTION = "Flooding happened bad luck :(";
        private const Sprite SPRITE = null;
        private List<Stack<Vector3Int>> tempFloodTiles;
       // private Stack<Vector3Int> tempFloodTilePositions;
        private Random random;
        public override void OnYesClick()
        {
            StoryManager.city.NextTurnEvent += DecreaseWater;
            random = new Random();
            GenerateFloodPositions();
            //Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
            // do something with the tiles here
            // This is currently just a placeholder
            
//            var tile = map.GetTile<MapTile>(new Vector3Int(0, 0, 0));
//            tile.Structure = null;
//            tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean,sprites);
//
//            var tile2 = map.GetTile<MapTile>(new Vector3Int(1, 0, 0));
//            tile2.Structure = null;
//            tile2.Terrain = new Terrain(Terrain.TerrainTypes.Ocean,sprites);
            

            // We want to randomly place the water in a section


            Destroy(StoryManager.storyManagerGameObject.GetComponent<CircusEvent>());
        }

        private void GenerateFloodPositions()
        {
            // First generate the first position
            // TODO: Grab the values for the city class/city map class instead of hard coding it here
            int height = 30;
            int width = 40;
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
            Tilemap map = StoryManager.city.Map.map;

            if (tempFloodTiles == null)
            {
                tempFloodTiles = new List<Stack<Vector3Int>>();
            }
            // Choose between 1-6 flood patches
            int numberOfPatches = random.Next(5, 20);

            for (int i = 0; i < numberOfPatches; i++)
            {
                int x = random.Next(-width/2  + 1, width/2 + 1);
                int y = random.Next(-height/2 + 1, height/2 + 1);
                Vector3Int position = new Vector3Int(x, y, 0);
                
                var tile = map.GetTile<MapTile>(position);
                tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean,sprites);
                tile.Structure = null;
                Stack<Vector3Int> newStack = new Stack<Vector3Int>();
                newStack.Push(position);
                tempFloodTiles.Add(newStack);
                
                GenerateSurroundingWater(100,i);
                
                
            }
            
            // For each of the initial patches, increase the surrounding water
        }

        private void GenerateSurroundingWater(int probabilityToIncrease, int listPosition)
        {
            
            int generatedValue = random.Next(0, 101);
            if (generatedValue < probabilityToIncrease)
            {
                Tilemap map = StoryManager.city.Map.map;
                Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
                
                // Generate next position
                probabilityToIncrease -= 1;
                Debug.Log("listPosition: "+ listPosition);
                Debug.Log("Size of list: " + tempFloodTiles.Count);
                Stack<Vector3Int> current = tempFloodTiles[listPosition];
                Vector3Int topPosition = current.Peek();
                // Get the next position for the surroundings
                int addX = random.Next(-1,1);
                int addY = random.Next(-1,1);
                int nextX = topPosition.x + addX;
                int nextY = topPosition.y + addY;

                int height = StoryManager.city.Map.HEIGHT;
                int width = StoryManager.city.Map.WIDTH;


                if (nextX < -width / 2 + 1 || nextX > width / 2 + 1)
                {
                    return;
                }

                if (nextY < -height / 2 + 1 || nextY > height / 2 + 1)
                {
                    return;
                }
                
                Vector3Int position = new Vector3Int(nextX,nextY , 0);
                
                var tile = map.GetTile<MapTile>(position);
                tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean,sprites);
                tile.Structure = null;
                current.Push(position);
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
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
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
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass,sprites);
                    }
                    
                }
            }
        }
        
        
    }
}