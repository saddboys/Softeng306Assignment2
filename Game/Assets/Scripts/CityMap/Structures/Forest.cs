using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Forest : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                Score = 100,
                CO2 = -2,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                Reputation = -1
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOntoSprite(canvas, position, "Textures/structures/objectForest", new Vector2(1, 1.5f));
        }
    }

    public class ForestFactory : StructureFactory
    {
        public ForestFactory(City city) : base(city) { }
        public ForestFactory() : base() { }

        public override int Cost
        {
            get { return 500; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/objectForest");

        protected override Structure Create()
        {
            return new Forest();
        }

        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < 1)
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

            if (tile.Terrain.TerrainType != Terrain.TerrainTypes.Grass)
            {
                reason = "Cannot build onto anything other than grass";
                return false;
            }

            return true;
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.Reputation += 1;
            }
        }
    }
}