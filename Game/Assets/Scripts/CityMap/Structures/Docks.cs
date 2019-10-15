using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Dock : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = 10,
                Wealth = 250,
            };
        }
        
        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            
            Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/dock", new Vector2(1, 1.5f));
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = 1,
                Reputation = -5
            };
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Dock";
        }
    }

    public class DockFactory : StructureFactory
    {
        public DockFactory(City city) : base(city) { }
        public DockFactory() : base() { }
        public override int Cost
        {
            get { return 2000; }
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

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a dock";
            details = "Click on a tile to build a dock.";
        }
    }
}