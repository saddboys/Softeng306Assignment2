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
                ElectricCapacity = 10
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 27, new Vector2(1, 1.5f));
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        public PowerPlantFactory(City city) : base(city) { }
        public PowerPlantFactory() : base() { }
        public int Cost
        {
            get { return 4000; }
        }

        protected override Structure Create()
        {
            return new PowerPlant();
        }

        public override void BuildOnto(MapTile tile)
        {
            if (City != null)
            {
                City.Stats.ElectricCapacity += 5;
            }
            base.BuildOnto(tile);
        }

        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!base.CanBuildOnto(tile, out reason))
            {
                return false;
            }

            if (tile.Terrain.TerrainType == Terrain.TerrainTypes.Ocean)
            {
                reason = "Cannot build onto water";
                return false;
            }

            return true;
        }
    }
}