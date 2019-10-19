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
    public class BiomeManager
    {
        private readonly int WIDTH;
        private readonly int HEIGHT;
        private Tilemap map;
        private GameObject parent;
        public Dictionary<int[], Terrain.TerrainTypes> occupiedBiomSpots { get; }
        public List<int[]> biomeAnchors { get; }
        Random random = new Random();


        public BiomeManager(int width, int height, Tilemap m, GameObject p, Dictionary<int[], Terrain.TerrainTypes> obs)
        {
            WIDTH = width;
            HEIGHT = height;
            map = m;
            parent = p;
            occupiedBiomSpots = obs;

            // instantiate biomeAnchors so that there is always one anchor at (0, 0) to reserve
            // the cnetral area for the player's initial structures. This is purely for game 
            // balancing reasons
            biomeAnchors = newBiomeAnchorList();

            // create biomes
            int numBiomes = (int) Mathf.Max(WIDTH, HEIGHT) / 20;
            Debug.Log("Creating biomes");
            
            // ocean
            for (int i = 0; i < numBiomes; i++)
            {
                createBiome(Terrain.TerrainTypes.Ocean);
            }
            // desert
            for (int i = 0; i < Mathf.FloorToInt(numBiomes / 2); i++)
            {
                createBiome(Terrain.TerrainTypes.DesertHill);
            }
            // grass hills
            for (int i = 0; i < numBiomes; i++)
            {
                createBiome(Terrain.TerrainTypes.GrassHill);
            }
            // Mountains
            createBiome(Terrain.TerrainTypes.Grass);
            // for (int i = 0; i < numBiomes; i++)
            // {
            //     createBiome(Terrain.TerrainTypes.Grass);
            // }

            // Populate none biom areas with grass
            Debug.Log("Creating non biomes");
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    MapTile tile = ScriptableObject.CreateInstance<MapTile>();

                    // A vector used for hex position
                    Vector3Int vector = new Vector3Int(-i + WIDTH / 2, -j + HEIGHT / 2, 0);

                    int[] pos = new int[2];
                    pos[0] = i;
                    pos[1] = j;
                    if (getTileTerrain(pos).Equals(Terrain.TerrainTypes.NotSet))
                    {
                        if (nextToOcean(pos))
                        {
                            tile.Terrain = new Terrain(Terrain.TerrainTypes.Beach);
                        }
                        else if (surroundedByDessert(pos))
                        {
                            tile.Terrain = new Terrain(Terrain.TerrainTypes.Desert);
                        }
                        else
                        {
                            tile.Terrain = new Terrain(Terrain.TerrainTypes.Grass);
                        }

                        SetTileTo(vector, tile);
                        // Refresh the tile whenever its sprite changes.
                        tile.SpriteChange += () => map.RefreshTile(vector);
                    }
                }
            }
        }

        /// <summar>
        /// creates a new List<int[]> for the biomeAnchor list.
        /// it will be instantiated with 1 anchor with position (0, 0).
        /// this is so that later when other anchors are creatd they are not created too closely towards the centre.
        /// ultimate it should preserve the player's starting structures for game balancing reasons.
        /// </summary>
        private List<int[]> newBiomeAnchorList() {
            List<int[]> anchorList = new List<int[]>();
            int[] dummyCentreAnchor = new int[2];
            dummyCentreAnchor[0] = 0;
            dummyCentreAnchor[1] = 0;
            anchorList.Add(dummyCentreAnchor);
            return anchorList;
        }

        /// <summary>
        /// checks if the anchor is too close to another anchor point
        /// </summary>
        private Boolean biomeAnchorTooClose(int[] anchor, int length)
        {
            foreach (int[] a in biomeAnchors)
            {
                float dist = Mathf.Sqrt(Mathf.Pow(anchor[0] - a[0], 2) + Mathf.Pow(anchor[1] - a[1], 2));
                if (dist < length * 1.5)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// creates a biome for a given type of terrain
        /// </summary>
        private void createBiome(Terrain.TerrainTypes terrain)
        {   
            // calculate biome half length proportional to the map size
            int biomeLenghtValue = (int) (Mathf.Max(WIDTH, HEIGHT) / 6);
            int biomeHalfLength = random.Next(biomeLenghtValue - 2, biomeLenghtValue + 2);

            // random anchor spot for biome
            // index 0 for x and 1 and y coordinate
            int[] anchor = new int[2];
            anchor[0] = random.Next(0, WIDTH);
            anchor[1] = random.Next(0, HEIGHT);
            
            while (biomeAnchorTooClose(anchor, biomeLenghtValue))
            {
                anchor[0] = random.Next(0, WIDTH);
                anchor[1] = random.Next(0, HEIGHT);
            }
            biomeAnchors.Add(anchor);

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
            else if (terrain.Equals(Terrain.TerrainTypes.DesertHill))
            {
                addDessertBiome(anchor, biomeHalfLength);
            }
            else if (terrain.Equals(Terrain.TerrainTypes.Grass))
            {
                addMountainBiome(anchor, biomeHalfLength);
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
                        if (getTileTerrain(curPos).Equals(Terrain.TerrainTypes.NotSet))
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
                                occupiedBiomSpots[curPos] = terrain;
                                tile.Terrain = new Terrain(terrain);
                                SetTileTo(vector, tile);

                                // add mountains if the biome is just grass
                                if (terrain.Equals(Terrain.TerrainTypes.Grass))
                                {
                                    StructureFactory mountainFactory = new MountainFactory();
                                    if (mountainFactory.CanBuildOnto(tile, out _))
                                    {
                                        mountainFactory.BuildOnto(tile);
                                    }
                                }

                                // Refresh the tile whenever its sprite changes.
                                tile.SpriteChange += () => map.RefreshTile(vector);
                            }
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// adds mountains as a biome, but uses different method since it is a structure and not terrain
        ///</summary>
        private void addMountainBiome(int[] anchor, int curBiomHalfLength)
        {
            int mountainBiomeHalfLength = curBiomHalfLength - 4;
            growBoime(anchor, mountainBiomeHalfLength, Terrain.TerrainTypes.Grass);

        }

        /// <summary>
        /// updates the beach biome by growing the biome further with beach tiles
        /// </summary>
        private void addBeachBiome(int[] anchor, int curBiomHalfLength)
        {
            int beachBiomeHalfLength = curBiomHalfLength + 3;
            growBoime(anchor, beachBiomeHalfLength, Terrain.TerrainTypes.Beach);
        }

        /// <summary>
        /// updates the dessert biome by growing the biome further with dessert tiles
        /// </summary>
        private void addDessertBiome(int[] anchor, int curBiomHalfLength)
        {
            int dessertBiomeHalfLength = curBiomHalfLength + 3;
            growBoime(anchor, dessertBiomeHalfLength, Terrain.TerrainTypes.Desert);
        }

        /// <summary>
        /// checks whether the vacant tile is next to an ocean tile
        /// if yes then it the vacant tile will be a beach tile
        /// </summary>
        /// <returns> Boolean if the vacant tile is next to an ocean tile
        private Boolean nextToOcean(int[] pos)
        {
            // gets adjacent positions
            int[,] adjPos = getNeighbouringTiles(pos);

            // check it's neighbouring tiles
            for (int i = 0; i < adjPos.GetLength(0); i++)
            {
                int[] curPos = new int[2];
                curPos[0] = adjPos[i, 0];
                curPos[1] = adjPos[i, 1];
                if (getTileTerrain(curPos).Equals(Terrain.TerrainTypes.Ocean))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// checks whether the vacant tile is completely surrounded by sand tiles
        /// if all 6 tiles are, then the vacant tile will be sand
        /// </summary>
        /// <returns> Boolean if the vacant tile is completely surrounded by sand tiles
        private Boolean surroundedByDessert(int[] pos)
        {
            // gets adjacent positions
            int[,] adjPos = getNeighbouringTiles(pos);

            // check it's neighbouring tiles
            for (int i = 0; i < adjPos.GetLength(0); i++)
            {
                int[] curPos = new int[2];
                curPos[0] = adjPos[i, 0];
                curPos[1] = adjPos[i, 1];
                if (!getTileTerrain(curPos).Equals(Terrain.TerrainTypes.Desert) || !getTileTerrain(curPos).Equals(Terrain.TerrainTypes.DesertHill))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the given position (int[2]) is in the given the list of int[2]
        /// This is to check if a tile is already occupied
        /// </summary>
        /// <returns> The terrain type if occupied, otherwise null
        private Terrain.TerrainTypes getTileTerrain(int[] pos)
        {
            foreach (int[] occupiedPos in occupiedBiomSpots.Keys)
            {
                if (occupiedPos[0].Equals(pos[0]) && occupiedPos[1].Equals(pos[1]))
                {
                    return occupiedBiomSpots[occupiedPos];
                }
            }
            return Terrain.TerrainTypes.NotSet;
        }

        /// <summary>
        /// For a given position it returns it's neighbouring positions as a 2 * 6 int array
        /// The first 2 coordinates are for top right tile, and the subsequent indexes are clockwise to the central tile
        /// </summary>
        /// <returns> The neighbouring 2 * 6 array
        private int[,] getNeighbouringTiles(int[] pos)
        {
            int[,] adjPos = new int[6, 2];
            // when y coordinate is even
            if (pos[1] % 2 == 0)
            {
                // 1 o'clock position
                adjPos[0, 0] = pos[0];
                adjPos[0, 1] = pos[1] + 1;
                // 3 o'clock position
                adjPos[1, 0] = pos[0] + 1;
                adjPos[1, 1] = pos[1];
                // 5 o'clock position
                adjPos[2, 0] = pos[0];
                adjPos[2, 1] = pos[1] - 1;
                // 7 o'clock position
                adjPos[3, 0] = pos[0] - 1;
                adjPos[3, 1] = pos[1] - 1;
                // 9 o'clock position
                adjPos[4, 0] = pos[0] - 1;
                adjPos[4, 1] = pos[1];
                // 11 o'clock position
                adjPos[5, 0] = pos[0] - 1;
                adjPos[5, 1] = pos[1] + 1;
            }
            else
            {
                // 1 o'clock position
                adjPos[0, 0] = pos[0] + 1;
                adjPos[0, 1] = pos[1] + 1;
                // 3 o'clock position
                adjPos[1, 0] = pos[0] + 1;
                adjPos[1, 1] = pos[1];
                // 5 o'clock position
                adjPos[2, 0] = pos[0] + 1;
                adjPos[2, 1] = pos[1] - 1;
                // 7 o'clock position
                adjPos[3, 0] = pos[0];
                adjPos[3, 1] = pos[1] - 1;
                // 9 o'clock position
                adjPos[4, 0] = pos[0] - 1;
                adjPos[4, 1] = pos[1];
                // 11 o'clock position
                adjPos[5, 0] = pos[0];
                adjPos[5, 1] = pos[1] + 1;
            }
            

            return adjPos;
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
    }
}