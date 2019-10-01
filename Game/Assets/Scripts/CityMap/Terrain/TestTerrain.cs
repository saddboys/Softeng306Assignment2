using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.CityMap
{
    /// <summary>
    /// A placeholder class for a terrain
    /// </summary>
    public class TestTerrain : Terrain
    {
        public TestTerrain()
        {
            Sprite[] sprites = GetSprites();
            Sprite = sprites[1];
        }
    }
}