using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.CityMap
{
    public class TestTerrain : Terrain
    {
        public TestTerrain()
        {
            Sprite[] sprites = GetSprites();
            Sprite = sprites[1];
        }
    }
}