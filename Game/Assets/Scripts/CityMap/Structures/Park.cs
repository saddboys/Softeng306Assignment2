using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Park : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                Reputation = 0,
                Score = 500,
                Wealth = -2,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 0, new Vector2(1, 1.5f));
        }
    }

    public class ParkFactory : StructureFactory
    {
        public ParkFactory(City city) : base(city) { }
        public ParkFactory() : base() { }

        public override int Cost
        {
            get { return 300; }
        }

        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[0];

        protected override Structure Create()
        {
            return new Park();
        }
        
        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (tile.Terrain.TerrainType != Terrain.TerrainTypes.Grass)
            {
                reason = "Parks can only be built on grassland";
                return false;
            }
            return base.CanBuildOnto(tile, out reason);
        }
    }
}