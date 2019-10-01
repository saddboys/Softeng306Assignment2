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
                MapTile someOtherTile = map.GetTile<MapTile>(position);
                if (someOtherTile.Structure != null)
                {
                    // Testing if the structures correctly store the cost
                    display.text = someOtherTile.Structure.Cost.ToString();
                }
                someOtherTile.Terrain.Sprite = Resources.LoadAll<Sprite>("Textures/terrain")[0];
                map.RefreshTile(position);
            }
        }

        /// <summary>
        /// Generates the Tilemap by randomly allocating terrains to tiles and sometimes
        /// adding structures to certain tiles.
        /// </summary>
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
                    Random random = new Random();
                    int value =  random.Next(0,2);
                    // A vector used for hex position
                    Vector3Int vector = new Vector3Int(-i + width/2, -j + height/2, 0);
                    // Find the real position (the position on the screen)
                    Vector3 mappedVector = map.CellToWorld(vector);
                    // Randomly generate the map with tiles (although the tiles are the same right now)
                    if (value == 0)
                    {
                        Tower tower = new Tower(parent,mappedVector);
                        tile.Structure = tower;
                        map.SetTile(vector, tile);
                    }
                    else
                    {
                        // Only random tiles have the mountain on it
                        if (random.Next(1, 6) == 4)
                        {
                            Rock rocks = new Rock(parent,mappedVector);
                            tile.Structure = rocks;
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
