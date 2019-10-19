using UnityEngine;

namespace Game.CityMap
{
    public class GiantCooler : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                
                Wealth = 50,
                CO2 = 1,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = 1,
                Population = -4,
                Reputation = 1
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {

            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "EventSprites/cooler", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "EventSprites/cooler", new Vector2(1, 1.5f));
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Thantec";
        }
    }
    
    public class GiantCoolerFactory : StructureFactory
    {
        public GiantCoolerFactory(City city) : base(city) { }
        public GiantCoolerFactory() : base() { }

        public override int Cost
        {
            get { return 250; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("EventSprites/cooler");

        protected override Structure Create()
        {
            return new GiantCooler();
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

            if (tile.Terrain.TerrainType==(Terrain.TerrainTypes.Ocean))
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
                City.Stats.ElectricCapacity -= 1;
                City.Stats.Population += 4;
                City.Stats.Reputation -= 1;
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build the cooler!";
            details = "Thantec has developed a new machine to cool down the town!";
        }
    }
}