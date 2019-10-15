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

        public Tilemap map;
        public GameObject parent;
        public Text display;
        private int[,] terrainMap;
        private const int HEIGHT = 10;
        private const int WIDTH = 10;
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
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");

            if (terrainMap == null)
            {
                terrainMap = new int[WIDTH, HEIGHT];
            }
            
            for (int j = HEIGHT; j >= 0; j--)
            {
                for (int i = 0; i < WIDTH; i++)             
                {
                    MapTile tile = ScriptableObject.CreateInstance<MapTile>();

                    // A vector used for hex position
                    Vector3Int vector = new Vector3Int(-i + WIDTH / 2, -j + HEIGHT / 2, 0);
                    // Find the real position (the position on the screen)
                    
                    
                    int value = random.Next(0,100);
                    
                    // Randomly generate the map with tiles (although the tiles are the same right now)
                    if (value < 20)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Desert);
                    }
                    else if (value < 40)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass);
                    }
                    else if (value < 50)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Beach);
                    }
                    else if (value < 60)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.GrassHill);  
                    } 
                    else if (value < 70)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.DesertHill);
                    } 
                    else
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean);
                    }
                    SetTileTo(vector, tile);
                    tile.name = "j: " + j + " i: " + i;
                    tile.Canvas = parent;
                    
                }
            }


            // Repeat factories to tune probabilities.
            StructureFactory[] factories =
            {
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new HouseFactory(),
                new FactoryFactory(),
                new FactoryFactory(),
                new ParkFactory(),
                new ParkFactory(),
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
                    Debug.Log("got here");
                }
            }
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
                // Clear tile from the tilemap.
                map.SetTile(pos, null);
            }
            foreach (var t in Tiles)
            {
                if (t == null)
                {
                    continue;
                }
                // Unrender structure.
                t.Structure = null;
                // Remove tile from object graph.
                Destroy(t);
            }
            Generate();
        }

        public void Rotate(bool clockwise)
        {
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
                var rectCoords = new Vector3Int
                {
                    x = hexCoords.x + (hexCoords.z - (hexCoords.z & 1)) / 2,
                    y = hexCoords.z,
                    z = 0,
                };
                SetTileTo(rectCoords, tile);
            }
        }

        private void SetTileTo(Vector3Int position, MapTile tile)
        {
            map.SetTile(position, tile);
            if (tile != null)
            {
                tile.ScreenPosition = map.CellToWorld(position);

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
