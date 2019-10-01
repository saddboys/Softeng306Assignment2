using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.CityMap
{
    public class CityMap : MonoBehaviour
    {
        /// <summary>
        /// Subscribe to this event (map.StructureBuildRequestEvent += YourHandler) to
        /// add logic to test whether structures can be built on certain tiles.
        /// </summary>
        public event EventHandler<StructureBuildRequestArgs> StructureBuildRequestEvent;

        /// <summary>
        /// Subscribe to this event (map.StructureBuildEvent += YourListener) to
        /// get notified when a new structure is built.
        /// </summary>
        public event EventHandler<StructureBuildRequestArgs> StructureBuiltEvent;

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
        public MapTile[] Tiles { get; }

        // Start is called before the first frame update
        void Start()
        {
            Generate();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
                Vector3Int position = map.WorldToCell(worldPoint);
                Debug.Log("Position of: " + position.x +" , " +  position.y);
                MapTile someOtherTile = map.GetTile<MapTile>(position);

                Debug.Log("fak" + someOtherTile.Structure);
                if (someOtherTile.Structure != null)
                {
                    Debug.Log("goes here!!!");
                    display.text = someOtherTile.Structure.Cost.ToString();
                }
                someOtherTile.Terrain.Sprite = Resources.LoadAll<Sprite>("Textures/terrain")[0];
                map.RefreshTile(position);
                //  test();
            
            }
        }

        private void Generate()
        {
            int width = 10;
            int height = 10;

            if (terrainMap == null)
            {
                terrainMap = new int[width,height];
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    MapTile tile = ScriptableObject.CreateInstance<MapTile>();
                    tile.Terrain = new TestTerrain();
//                    RandomTile tile2 = ScriptableObject.CreateInstance<RandomTile>();
//                    AnotherRandomTile tile1 = ScriptableObject.CreateInstance<AnotherRandomTile>();
                    Random random = new Random();
                    int value =  random.Next(0,2);
                    Debug.Log(value);
                    Vector3Int vector = new Vector3Int(-i + width/2, -j + height/2, 0);
                    Vector3 mappedVector = map.CellToWorld(vector);
                    if (value == 0)
                    {
                        Tower tower = new Tower(parent,mappedVector);
                        tile.Structure = tower;
                        map.SetTile(vector, tile);
                        
                        Debug.Log("Position i: " + vector.x +" , " + "j: " + vector.y );
                    }
                    else
                    {
                        // Only random tiles have the mountain on it
                        if (random.Next(1, 6) == 4)
                        {
                            Rock rocks = new Rock(parent,mappedVector);
                            // Rock rocks = obj.AddComponent<Rock>(parent,mappedVector);
                            tile.Structure = rocks;
                            Debug.Log("yeS"+ tile.Structure);
                        }

                        map.SetTile(vector, tile);
                    }
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
            throw new System.NotImplementedException();
        }
    }
}
