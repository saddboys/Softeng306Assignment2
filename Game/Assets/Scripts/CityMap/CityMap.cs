using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.CityMap
{
    public class CityMap : MonoBehaviour
    {
        /// <summary>
        /// Subscribe to this event (map.TileClickedEvent += YourListener) to
        /// get notified when the user clicks on any tile. Useful for implementing
        /// things like the toolbar when adding structures.
        /// </summary>
        public event EventHandler<TileClickArgs> TileClickedEvent;

        public Tilemap map;
        public GameObject parent;
        public Text display;
        private int[,] terrainMap;

        Random random = new Random();


        public MapTile[] Tiles
        {
            get
            {
                return Array.ConvertAll(map.GetTilesBlock(map.cellBounds),
                    tileBase => (MapTile)tileBase);
            }
        }

        private Vector3 mouseDownPosition;

        // Start is called before the first frame update
        void Start()
        {
            Generate();
        }
        // Update is called once per frame
        void Update()
        {
            CheckTileClick();
        }

        private void CheckTileClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPosition = Input.mousePosition;
            }

            // Check left click released.
            if (!Input.GetMouseButtonUp(0)) return;

            // Check for drag.
            if (Input.mousePosition != mouseDownPosition) return;

            // Check UI click-through.
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Check camera dragging
            //if (cameraDrag.WasDragging) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = map.WorldToCell(worldPoint);
            MapTile someOtherTile = map.GetTile<MapTile>(position);
            if (someOtherTile != null)
            {
                // Notify the click event for things like the ToolBar or other user feedback.
                TileClickedEvent?.Invoke(this, new TileClickArgs(someOtherTile));

                // For testing purposes:
                //someOtherTile.Structure = new Rock();
                //someOtherTile.Terrain.Sprite = Resources.LoadAll<Sprite>("Textures/terrain")[0];
            }
        }

        /// <summary>
        /// Generates the Tilemap by randomly allocating terrains to tiles and sometimes
        /// adding structures to certain tiles.
        /// </summary>
        private void Generate()
        {
            Debug.Log("Camera dimensions: " + Camera.main.pixelWidth +" , " + Camera.main.pixelHeight);
            int width = 40;
            int height = 30;
            List<int[]> occupiedBiomSpots = new List<int[]>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");

            if (terrainMap == null)
            {
                terrainMap = new int[width, height];
            }

            for (int i = 0; i < 2; i++) {
                createBiome(Terrain.TerrainTypes.GrassHill, occupiedBiomSpots, sprites, width, height);
                createBiome(Terrain.TerrainTypes.Desert, occupiedBiomSpots, sprites, width, height);
                createBiome(Terrain.TerrainTypes.Ocean, occupiedBiomSpots, sprites, width, height);
            }
            


            // Populate none biom areas with grass
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    MapTile tile = ScriptableObject.CreateInstance<MapTile>();
                    // A vector used for hex position
                    Vector3Int vector = new Vector3Int(-i + width / 2, -j + height / 2, 0);
                    // Find the real position (the position on the screen)
                    Vector3 mappedVector = map.CellToWorld(vector);
                    

                    tile.Canvas = parent;
                    tile.ScreenPosition = mappedVector;
                    
                    int value = random.Next(0,100);

                    int[] pos = new int[2];
                    pos[0] = i;
                    pos[1] = j;
                    if (!TileOccupied(occupiedBiomSpots, pos))
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass, sprites);
                        map.SetTile(vector, tile);
                        // Refresh the tile whenever its sprite changes.
                        tile.SpriteChange += () => map.RefreshTile(vector);
                    } 
                }
            }

            // Repeat factories to tune probabilities.
            StructureFactory[] factories =
            {
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new FactoryFactory(),
                new FactoryFactory(),
                new FactoryFactory(),
                new FactoryFactory(),
                new ParkFactory(),
            };

            for (int i = 0; i < 50; i++)
            {
                // Cluster them close to the centre.
                int x = (int)(Mathf.Clamp(NextNormalRandom() * width, -width, width) / 2.0f);
                int y = (int)(Mathf.Clamp(NextNormalRandom() * height, -height, height) / 2.0f);

                var tile = map.GetTile<MapTile>(new Vector3Int(x, y, 0));
                if (tile == null)
                {
                    continue;
                }

                var randomFactory = factories[random.Next(0, factories.Length)];
                if (randomFactory.CanBuildOnto(tile, out _))
                {
                    randomFactory.BuildOnto(tile);
                }
            }
        }
        
        /// <summary>
        /// creates a biome for a given type of terrain
        /// </summary>
        private void createBiome(Terrain.TerrainTypes terrain, List<int[]> occupiedBiomSpots, Sprite[] sprites, int width, int height)
        {
            // random anchor spot for biome
            // index 0 for x and 1 and y coordinate
            int[] anchor = new int[2];
            anchor[0] = random.Next(0, width);
            anchor[1] = random.Next(0, height);
            
            while (TileOccupied(occupiedBiomSpots, anchor))
            {
                anchor[0] = random.Next(0, width);
                anchor[1] = random.Next(0, height);
            }
            Debug.Log("Anchor: X: " + anchor[0] + ", Y: " + anchor[1]);

            // adding anchor to screen
            MapTile anchorTile = ScriptableObject.CreateInstance<MapTile>();
            Vector3Int anchorVector = new Vector3Int(-anchor[0] + width / 2, -anchor[1] + height / 2, 0);
            Vector3 anchorMappedVector = map.CellToWorld(anchorVector);

            anchorTile.Canvas = parent;
            anchorTile.ScreenPosition = anchorMappedVector;

            anchorTile.Terrain = new Terrain(terrain, sprites);

            int biomHalfLength = 7;
            // constants that will be used further down the line
            float k = Mathf.Sqrt(Mathf.Pow(biomHalfLength, 2) * 2);
            float a = (float) 2.0f / Mathf.Log10(Mathf.Pow(k, 2) - 2.0f);

            // growing sand biom
            for (int i = 0; i < biomHalfLength * 2; i++) 
            {
                for (int j = 0; j < biomHalfLength * 2; j++)
                {
                    // current position array where index 0 is X and index 1 is Y coordinate
                    int[] curPos = new int[2];
                    curPos[0] = anchor[0] - biomHalfLength + i;
                    curPos[1] = anchor[1] - biomHalfLength + j;

                    // check if X and Y values are within the map
                    if (curPos[0] < 40 && curPos[0] >= 0 && curPos[1] < 30 && curPos[1] >= 0)
                    {
                        MapTile tile = ScriptableObject.CreateInstance<MapTile>();
                        // A vector used for hex position
                        Vector3Int vector = new Vector3Int(-curPos[0] + width / 2, -curPos[1] + height / 2, 0);
                        // Find the real position (the position on the screen)
                        Vector3 mappedVector = map.CellToWorld(vector);

                        tile.Canvas = parent;
                        tile.ScreenPosition = mappedVector;

                        // check if terrain is vacant
                        if (!TileOccupied(occupiedBiomSpots, curPos))
                        {
                            // Debug.Log("Cur: X: " + curX + ", Y: " + curY);

                            // weighted random terrain allocation depending on distance from anchor
                            int value = random.Next(0, 100);

                            // negative linear weighting
                            // if (value < 100 - ((int) Mathf.Abs(sandAnchorXValue - curPos[0]) + (int) Mathf.Abs(sandAnchorYValue - curPos[1])) * 7)
                            // {
                            //     occupiedBiomSpots.Add(curPos);
                            //     sandTile.Terrain = new Terrain(Terrain.TerrainTypes.Desert, sprites);
                            //     map.SetTile(vector, sandTile);
                            //     // Refresh the tile whenever its sprite changes.
                            //     sandTile.SpriteChange += () => map.RefreshTile(vector);
                            // }

                            // weighting is done in such a that:
                            // P = (k^2 - d^2)^a,
                            //    where:
                            //       P = probability of the biome tile being set
                            //       k = a constant (calculated earlier before the nested for loops)
                            //       d = absolute distance between the anchor and any potential biome tile
                            //       a = a constant (calculated earlier before the nested for loops)
                            //
                            // the formula satisfies the two equations below:
                            //    100 = (k^2 - 2)^a, this ensures that the biome is atleast 3x3
                            //    0 = (k^2 - d^2)^a, where d is a distance for an arbitrary square just outside the biome half length
                            //
                            // so if biome half length is 7 then k = 11.3 and a = 0.484
                            float dist = Mathf.Sqrt(Mathf.Pow(anchor[0] - curPos[0], 2) + Mathf.Pow(anchor[1] - curPos[1], 2));
                            double prob = Mathf.Pow(Mathf.Pow(k, 2) - Mathf.Pow(dist, 2), a);
                            if (value < prob)
                            {
                                occupiedBiomSpots.Add(curPos);
                                tile.Terrain = new Terrain(terrain, sprites);
                                map.SetTile(vector, tile);
                                // Refresh the tile whenever its sprite changes.
                                tile.SpriteChange += () => map.RefreshTile(vector);
                            }
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// Checks if the given position (int[2]) is in the given the list of int[2]
        /// This is to check if a tile is already occupied
        /// </summary>
        /// <returns> Boolean if a position is occupied
        private Boolean TileOccupied(List<int[]> occupiedPosList, int[] pos)
        {
            foreach(int[] occupiedPos in occupiedPosList)
            {
                if (occupiedPos[0].Equals(pos[0]) && occupiedPos[1].Equals(pos[1]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Accumulate all the stats contributions (e.g. the CO2, the profits, etc.)
        /// of al the tiles in the map at the current state.
        /// </summary>
        /// <returns>The overall stats contribution.</returns>
        public Stats GetStatsContribution()
        {
            // Get stats from its tiles.
            Stats sum = new Stats();
            foreach (var t in Tiles) {
                sum += t.GetStatsContribution();
            }
            return sum;
        }

        public void Regenerate()
        {
            foreach (var t in Tiles)
            {
                // Unrender structure.
                t.Structure = null;
                // Remove tile from object graph.
                Destroy(t);
            }
            Generate();
        }

        private float NextNormalRandom()
        {
            const int samples = 100;
            float sum = 0;
            for (int i = 0; i < samples; i++)
            {
                sum += (float)random.NextDouble();
            }
            return 5 * (sum / samples - 0.5f);
        }
    }
}
