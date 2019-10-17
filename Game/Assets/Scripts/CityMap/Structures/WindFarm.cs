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
                Wealth = -50,
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
            Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/windmill", new Vector2(1, 1.5f));
            
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Wind farm";
        }
    }

    public class WindFarmFactory : StructureFactory
    {
        public WindFarmFactory(City city) : base(city) { }
        public WindFarmFactory() : base() { }
        public override int Cost
        {
            get { return 2000; }
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
                City.Stats.Score += 900;
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

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a wind farm";
            details = "Although expensive and less effective, wind farms can produce electricity for your town without adding pollution.";
        }
    }
}