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
            RenderOnto(canvas, position, 1, new Vector2(1, 1.5f));
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        public PowerPlantFactory(City city) : base(city) { }
        public PowerPlantFactory() : base() { }
        public override int Cost
        {
            get { return 4000; }
        }

        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[1];

        protected override Structure Create()
        {
            return new PowerPlant();
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += 5;
            }
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