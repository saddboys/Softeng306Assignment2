using Game;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Game.CityMap
{
    public class Tower : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stat
            {
                Wealth = 4,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 44, new Vector2(1, 1.5f));
        }
    }

    public class TowerFactory : StructureFactory
    {
        public int Cost
        {
            get { return 2000; }
        }

        protected override Structure Create()
        {
            return new Tower();
        }
    }
}
