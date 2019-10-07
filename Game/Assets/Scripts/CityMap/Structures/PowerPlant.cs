using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class PowerPlant : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = 1,
                Wealth = 10,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 27, new Vector2(1, 1.5f));
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        public int Cost
        {
            get { return 4000; }
        }

        protected override Structure Create()
        {
            return new Factory();
        }

        public void BuildOnto(MapTile tile)
        {
            City.Stats.ElectricCapacity += 5;
            base.BuildOnto(tile);
        }
    }
}