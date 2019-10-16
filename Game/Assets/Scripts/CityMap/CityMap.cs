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

        public event EventHandler<TileClickArgs> TileMouseEnterEvent;
        public event EventHandler<TileClickArgs> TileMouseLeaveEvent;
        private MapTile previousHoveredTile;

        // Camera to reposition after rotation.
        [SerializeField]
        private GameObject camera;

        public Tilemap map;
        public GameObject parent;
        private int[,] terrainMap;
        private int WIDTH = 40;
        private int HEIGHT = 30;
        private List<int[]> occupiedBiomSpots = new List<int[]>();
        Random random = new Random();


        public MapTile[] Tiles
        {
            get
            {
                BoundsInt cellBounds = map.cellBounds;
                // Debug.Log("Cell bounds are" + cellBounds);
                // cellBounds.size = new Vector3Int(WIDTH,HEIGHT,2);
                // Debug.Log("Bounds are" + cellBounds);
                return Array.ConvertAll(map.GetTilesBlock(cellBounds),
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
            CheckTileHover();
            if (Input.GetKeyDown(KeyCode.R))
            {
                Rotate(true);
            }
        }

        private void CheckTileHover()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = map.WorldToCell(worldPoint);
            MapTile currentTile = map.GetTile<MapTile>(position);
            if (EventSystem.current.IsPointerOverGameObject())
            {
                currentTile = null;
            }
            if (currentTile != previousHoveredTile)
            {
                if (previousHoveredTile != null)
                {
                    TileMouseLeaveEvent?.Invoke(this, new TileClickArgs(previousHoveredTile));
                }
                if (currentTile != null)
                {
                    TileMouseEnterEvent?.Invoke(this, new TileClickArgs(currentTile));
                }
            }
            previousHoveredTile = currentTile;
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

            // Vector3 test = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log("World point:" + test);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = map.WorldToCell(worldPoint);
            MapTile someOtherTile = GetTileWithZ(position);
            if (someOtherTile != null)
            {
                // Notify the click event for things like the ToolBar or other user feedback.
                TileClickedEvent?.Invoke(this, new TileClickArgs(someOtherTile));
            }
        }

        private MapTile GetTileWithZ(Vector3Int position)
        {
            for (int z = 0; z < HEIGHT; z++)
            {
                position.z = z;
                MapTile someTile = map.GetTile<MapTile>(position);
                
                if (someTile != null)
                {
                    return someTile;
                }
            }

            return null;
        }
        

        /// <summary>
        /// Generates the Tilemap by randomly allocating terrains to tiles and sometimes
        /// adding structures to certain tiles.
        /// </summary>
        private void Generate()
        {
            Debug.Log("Camera dimensions: " + Camera.main.pixelWidth +" , " + Camera.main.pixelHeight);

            if (terrainMap == null)
            {
                terrainMap = new int[WIDTH, HEIGHT];
            }

            // create biomes
            int numBiomes = (int) Mathf.Max(WIDTH, HEIGHT) / 13;

            createBiome(Terrain.TerrainTypes.Desert);
            for (int i = 0; i < numBiomes; i++) {
                createBiome(Terrain.TerrainTypes.Ocean);
            }
            


            // Populate none biom areas with grass
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    MapTile tile = ScriptableObject.CreateInstance<MapTile>();

                    // A vector used for hex position
                    Vector3Int vector = new Vector3Int(-i + WIDTH / 2, -j + HEIGHT / 2, 0);
                    // Find the real position (the position on the screen)
                    
                    
                    int value = random.Next(0,100);
                    int[] pos = new int[2];
                    pos[0] = i;
                    pos[1] = j;
                    if (!TileOccupied(occupiedBiomSpots, pos))
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass);
                        SetTileTo(vector, tile);
                        // Refresh the tile whenever its sprite changes.
                        tile.SpriteChange += () => map.RefreshTile(vector);
                    }
                }
            }


            // Repeat factories to tune probabilities.
            StructureFactory[] factories =
            {
                new MountainFactory(),
                new MountainFactory(), 
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new ForestFactory(),
                new ForestFactory(),
                new ForestFactory(),
                new ForestFactory(),
                new ForestFactory(),
                new ForestFactory(),
                new ForestFactory(),
                new FactoryFactory(),
                new FactoryFactory(),
                new PowerPlantFactory(),
            };

            for (int i = 0; i < 50; i++)
            {
                // Cluster them close to the centre.
                int x = (int)(Mathf.Clamp(NextNormalRandom() * WIDTH, -WIDTH, WIDTH) / 2.0f);
                int y = (int)(Mathf.Clamp(NextNormalRandom() * HEIGHT, -HEIGHT, HEIGHT) / 2.0f);

                var tile = GetTileWithZ(new Vector3Int(x, y, 0));
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

            // Start off at an angle to further enhance 2.5D effect.
            Rotate(true);
        }
        
        /// <summary>
        /// creates a biome for a given type of terrain
        /// </summary>
        private void createBiome(Terrain.TerrainTypes terrain)
        {
            // calculate biome half length proportional to the map size
            int biomeLenghtValue = (int) (Mathf.Max(WIDTH, HEIGHT) / 6);
            int biomeHalfLength = random.Next(biomeLenghtValue - 4, biomeLenghtValue + 4);

            // random anchor spot for biome
            // index 0 for x and 1 and y coordinate
            int[] anchor = new int[2];
            anchor[0] = random.Next(0, WIDTH);
            anchor[1] = random.Next(0, HEIGHT);
            
            while (TileOccupied(occupiedBiomSpots, anchor))
            {
                anchor[0] = random.Next(0, WIDTH);
                anchor[1] = random.Next(0, HEIGHT);
            }
            Debug.Log("Anchor: X: " + anchor[0] + ", Y: " + anchor[1]);

            // adding anchor to screen
            MapTile anchorTile = ScriptableObject.CreateInstance<MapTile>();
            Vector3Int anchorVector = new Vector3Int(-anchor[0] + WIDTH / 2, -anchor[1] + HEIGHT / 2, 0);
            Vector3 anchorMappedVector = map.CellToWorld(anchorVector);

            anchorTile.Canvas = parent;
            anchorTile.ScreenPosition = anchorMappedVector;

            anchorTile.Terrain = new Terrain(terrain);

            growBoime(anchor, biomeHalfLength, terrain);

            // create beach biom if the biome type is Ocean
            if (terrain.Equals(Terrain.TerrainTypes.Ocean))
            {
                addBeachBiome(anchor, biomeHalfLength);
            }
            
        }

        /// <summary>
        /// grows a biome
        /// </summary>
        private void growBoime(int[] anchor, int biomHalfLength, Terrain.TerrainTypes terrain)
        {
            // constants that will be used further down the line
            float k = Mathf.Sqrt(Mathf.Pow(biomHalfLength, 2) * 2);
            float a = (float) 2.0f / Mathf.Log10(Mathf.Pow(k, 2) - 2.0f);

            // growing biom
            for (int i = 0; i < biomHalfLength * 2; i++) 
            {
                for (int j = 0; j < biomHalfLength * 2; j++)
                {
                    // current position array where index 0 is X and index 1 is Y coordinate
                    int[] curPos = new int[2];
                    curPos[0] = anchor[0] - biomHalfLength + i;
                    curPos[1] = anchor[1] - biomHalfLength + j;

                    // check if X and Y values are within the map
                    if (curPos[0] < WIDTH && curPos[0] >= 0 && curPos[1] < HEIGHT && curPos[1] >= 0)
                    {
                        MapTile tile = ScriptableObject.CreateInstance<MapTile>();
                        // A vector used for hex position
                        Vector3Int vector = new Vector3Int(-curPos[0] + WIDTH / 2, -curPos[1] + HEIGHT / 2, 0);
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
                                tile.Terrain = new Terrain(terrain);
                                SetTileTo(vector, tile);
                                // Refresh the tile whenever its sprite changes.
                                tile.SpriteChange += () => map.RefreshTile(vector);
                            }
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// updates the beach biome by growing the biome further beach tiles
        /// </summary>
        private void addBeachBiome(int[] anchor, int curBiomHalfLength)
        {
            int beachBiomeHalfLength = curBiomHalfLength + 3;
            growBoime(anchor, beachBiomeHalfLength, Terrain.TerrainTypes.Beach);
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
                if (t == null)
                {
                    continue;
                }
                sum += t.GetStatsContribution();
            }
            return sum;
        }

        public void Regenerate()
        {
            foreach (Vector3Int pos in map.cellBounds.allPositionsWithin)
            {
                var t = map.GetTile<MapTile>(pos);

                // Clear tile from the tilemap.
                map.SetTile(pos, null);

                if (t == null)
                {
                    continue;
                }
                // Unrender structure.
                t.Structure = null;
                // Remove tile from object graph.
                Destroy(t);
            }

            occupiedBiomSpots.Clear();

            Generate();
        }

        public void Rotate(bool clockwise)
        {
            // Find tile location that camera is currently centred at.
            var cameraFocus = map.WorldToCell(camera.transform.position);
            var cameraZPos = camera.transform.position.z;

            var centre = map.cellBounds.min + map.cellBounds.max;
            centre.x /= 2;
            centre.y /= 2;
            centre.z = 0;
            MapTile test = map.GetTile<MapTile>(new Vector3Int(0, 0, 0));
            List<ValueTuple<Vector3Int, MapTile>> tiles = new List<(Vector3Int, MapTile)>();
            foreach (Vector3Int pos in map.cellBounds.allPositionsWithin)
            {
                tiles.Add((pos, map.GetTile<MapTile>(pos)));

                // Remove the tile at the old position.
                map.SetTile(pos, null);
            }
            foreach (var (pos, tile) in tiles)
            {
                SetTileTo(RotateCellPosition(pos, clockwise), tile);
            }

            // Shrink bounds to where tiles exist.
            map.CompressBounds();

            // Recenter camera to previous tile.
            Vector3 newPos = map.CellToWorld(RotateCellPosition(cameraFocus, clockwise));
            newPos.z = cameraZPos;
            camera.transform.position = newPos;
        }

        private Vector3Int RotateCellPosition(Vector3Int pos, bool clockwise)
        {
            // Transform into hexagonal coordinate system.
            var hexCoords = new Vector3Int
            {
                x = pos.x - (pos.y - (pos.y & 1)) / 2,
                y = (pos.y - (pos.y & 1)) / 2 - pos.x - pos.y,
                z = pos.y,
            };

            // Rotate in the hexagonal coordinate system.
            if (clockwise)
            {
                hexCoords = new Vector3Int
                {
                    x = -hexCoords.z,
                    y = -hexCoords.x,
                    z = -hexCoords.y,
                };
            }
            else
            {
                hexCoords = new Vector3Int
                {
                    x = -hexCoords.y,
                    y = -hexCoords.z,
                    z = -hexCoords.x,
                };
            }

            // Transform back to grid's rectangular coordinate system and apply.
            return new Vector3Int
            {
                x = hexCoords.x + (hexCoords.z - (hexCoords.z & 1)) / 2,
                y = hexCoords.z,
                z = 0,
            };
        }

        private void SetTileTo(Vector3Int position, MapTile tile)
        {
            map.SetTile(position, tile);
            if (tile != null)
            {
                tile.ScreenPosition = map.CellToWorld(position);
                tile.Canvas = parent;

                // Refresh the tile whenever its sprite changes.
                tile.SpriteChange += () => map.RefreshTile(position);
            }
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
