using Game;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Game
{
    public class Tower : Structure
    {
        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 44, new Vector2(1, 1.5f));
        }
    }

    public class TowerFactory : StructureFactory
    {
        protected override Structure Create()
        {
            return new Tower();
        }
    }
}
