using UnityEngine;

namespace Game.CityMap
{
    
    /// <summary>
    /// A factory class which dynamically creates terrains
    /// </summary>
    public class TerrainFactory
    {
        public enum TerrainTypes { Beach, Desert, DesertHills, Grass, Hills, Mountain, Sea, Test }
        
        private const string TERRAIN_PATH = "Textures/terrain";
        private Sprite[] sprites;

        public TerrainFactory()
        {
            sprites = Resources.LoadAll<Sprite>(TERRAIN_PATH);
        }
        
        /// <summary>
        /// Terrain creation method that allows for dynamic terrain
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Terrain CreateTerrain(TerrainTypes type)
        {
            switch (type)
            {
                case TerrainTypes.Beach:
                    return null;
                case TerrainTypes.Desert:
                    return null;
                case TerrainTypes.DesertHills:
                    return null;
                case TerrainTypes.Grass:
                    Grass grass = new Grass();
                    grass.Sprite = sprites[12];
                    return grass;
                case TerrainTypes.Hills:
                    return null;
                case TerrainTypes.Mountain:
                    return null;
                case TerrainTypes.Sea:
                    return null;
                case TerrainTypes.Test:
                    TestTerrain test = new TestTerrain();
                    test.Sprite = sprites[3];
                    return test;
                default:
                    TestTerrain terrain = new TestTerrain();
                    terrain.Sprite = sprites[3];
                    return terrain;
            }
        }
    }
}