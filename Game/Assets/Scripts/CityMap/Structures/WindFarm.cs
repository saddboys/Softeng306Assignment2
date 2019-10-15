using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class WindFarm : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = 0,
                Wealth = 0,
                ElectricCapacity = 0,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = -10,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            
            Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/windmill", new Vector2(1, 1.5f));
        }
    }

    public class WindFarmFactory : StructureFactory
    {
        public WindFarmFactory(City city) : base(city) { }
        public WindFarmFactory() : base() { }
        public override int Cost
        {
            get { return 3000; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/windmill");

        protected override Structure Create()
        {
            return new WindFarm();
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += 10;
            }
        }

        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!base.CanBuildOnto(tile, out reason))
            {
                return false;
            }

            if (tile.Terrain.TerrainType != Terrain.TerrainTypes.DesertHill && tile.Terrain.TerrainType != Terrain.TerrainTypes.GrassHill )
            {
                reason = "Can only build on hills";
                return false;
            }

            return true;
        }
    }
}