using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class House : Structure
    {
        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 35, new Vector2(1, 1.5f));
        }
    }

    public class HouseFactory : StructureFactory
    {
        protected override Structure Create()
        {
            return new House();
        }
    }
}