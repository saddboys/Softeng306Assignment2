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
                CO2 = 10,
                Score = 500,
                Reputation = 3,
                Wealth = 10,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            Vector3 positionNew = new Vector3(position.x, position.y - 0f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/FactorySprite", new Vector2(1, 1.2f));
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
            Resources.Load<Sprite>("Textures/structures/FactorySprite");
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
            }
        }


    }
}