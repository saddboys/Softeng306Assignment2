using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class PowerPlant : Structure
    {
        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 27, new Vector2(1, 1.5f));
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        protected override Structure Create()
        {
            return new Factory();
        }
    }
}