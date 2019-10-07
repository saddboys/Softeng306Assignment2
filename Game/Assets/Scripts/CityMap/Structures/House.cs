using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class House : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                Score = 100,
                Wealth = 0.5,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 35, new Vector2(1, 1.5f));
        }
    }

    public class HouseFactory : StructureFactory
    {
        public HouseFactory(City city) : base(city) { }
        public HouseFactory() : base() { }

        public int Cost
        {
            get { return 500; }
        }

        protected override Structure Create()
        {
            return new House();
        }

        public bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City.Stats.ElectricCapacity < 1)
            {
                reason = "Not enough electric capacity";
                return false;
            }
            return true;
        }

        public void BuildOnto(MapTile tile)
        {
            City.Stats.ElectricCapacity -= 1;
            base.BuildOnto(tile);
        }
    }
}