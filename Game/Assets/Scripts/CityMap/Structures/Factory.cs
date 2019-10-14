using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Factory : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats()
            {
                CO2 = 20,
                Score = 500,
                Wealth = 30,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = 5,
                Reputation = -3
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/FactoryNew", new Vector2(5f, 5f));
        }
    }

    public class FactoryFactory : StructureFactory
    {
        public override int Cost
        {
            get
            {
                return 3000;
            }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/FactoryNew");
        public FactoryFactory(City city) : base(city) { }
        public FactoryFactory() : base() { }

        protected override Structure Create()
        {
            return new Factory();
        }
        
        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < 5)
            {
                reason = "Not enough electric capacity";
                return false;
            }
            return true;
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

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity -= 5;
                City.Stats.Reputation += 3;
            }
        }


    }
}