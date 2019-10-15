using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Dock : Structure
    {
        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOntoSprite(canvas, position, "Textures/structures/dock", new Vector2(1, 1.5f));
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = 1,
                Reputation = -5
            };
        }
    }

    public class DockFactory : StructureFactory
    {
        public DockFactory(City city) : base(city) { }
        public DockFactory() : base() { }
        public override int Cost
        {
            get { return 1000; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/dock");

        
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

            if (tile.Terrain.TerrainType!=(Terrain.TerrainTypes.Ocean))
            {
                reason = "Cannot build onto land";
                return false;
            }

            return true;
        }
        
        protected override Structure Create()
        {
            return new Dock();
        }
        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity -= 1;
                City.Stats.Reputation += 5;
            }
        }
    }
}