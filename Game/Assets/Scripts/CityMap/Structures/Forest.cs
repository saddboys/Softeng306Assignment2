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
                CO2 = -5,
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

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Forrest";
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
            
            return true;
        }

        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!base.CanBuildOnto(tile, out reason))
            {
                return false;
            }

            if ((tile.Terrain.TerrainType != Terrain.TerrainTypes.Grass) && (tile.Terrain.TerrainType != Terrain.TerrainTypes.GrassHill))
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

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Grow a forrest";
            details = "Although it may seem expensive, you should grow some forrests to help keep the pollution down.";
        }
    }
}