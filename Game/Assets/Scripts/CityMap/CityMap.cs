﻿using System;
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

            if (terrainMap == null)
            {
                terrainMap = new int[width, height];
            }
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
                    
                    int value = random.Next(0,3);
                    
                    // Randomly generate the map with tiles (although the tiles are the same right now)
                    if (value == 0)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Desert);
                    }
                    else if (value == 1)
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass);
                    }
                    else
                    {
                        tile.Terrain = new Terrain(Terrain.TerrainTypes.Ocean);
                    }
                    
                    map.SetTile(vector, tile);
                    // Refresh the tile whenever its sprite changes.
                    tile.SpriteChange += () => map.RefreshTile(vector);
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
    }
}
