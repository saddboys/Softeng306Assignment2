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

        private readonly int WIDTH = 40;
        private readonly int HEIGHT = 40;

        Random random = new Random();

        private AudioClip rotateSound;

        public MapTile[] Tiles
        {
            get
            {
                BoundsInt cellBounds = map.cellBounds;
                return Array.ConvertAll(map.GetTilesBlock(cellBounds),
                    tileBase => (MapTile)tileBase);
            }
        }
        private Vector3 mouseDownPosition;

        // Start is called before the first frame update
        void Start()
        {
            rotateSound = Resources.Load<AudioClip>("SoundEffects/CameraRotate");
            Generate(1);
        }
        // Update is called once per frame
        void Update()
        {
            CheckTileClick();
            CheckTileHover();
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
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
                    //Debug.Log("position of tile is: " + position.x + ", " + position.y + ", " + position.z);
                    TileMouseEnterEvent?.Invoke(this, new TileClickArgs(currentTile));
                    // Debug.Log("pos: " + position);
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

                // Focus onto structure
                if (someOtherTile.Structure != null)
                {
                    camera.GetComponent<CameraDrag>().PanTo(new Vector3(worldPoint.x, worldPoint.y, 0));
                }
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
        private void Generate(int level)
        {

            if (terrainMap == null)
            {
                terrainMap = new int[WIDTH, HEIGHT];
            }

            BiomeManager biomeManager = new BiomeManager(WIDTH, HEIGHT, map, parent);
            biomeManager.start();

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

            // Reset camera for new maps.
            camera.GetComponent<CameraDrag>().PanTo(new Vector3(0, 0, 0));
            camera.GetComponent<CameraZoom>().ResetZoom();
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

        public void Regenerate(int level)
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

            DestroyRest();
            Generate(level);
        }

        private void DestroyRest()
        {
            foreach (Transform child in parent.transform)
            { 
                Destroy(child.gameObject);
            }
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

            // Perform the swapping in two stages. Can't be done in a single pass.

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
            camera.GetComponent<CameraDrag>().TeleportTo(newPos);

            // Nice swoosh sound.
            GameObject.FindObjectOfType<AudioBehaviour>().Play(rotateSound);
        }

        public Vector3Int RotateCellPosition(Vector3Int pos, bool clockwise)
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
