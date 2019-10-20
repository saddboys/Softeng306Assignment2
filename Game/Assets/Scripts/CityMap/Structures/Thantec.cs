using UnityEngine;

namespace Game.CityMap
{
    public class Thantec : Structure
    {
        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out title, out meta, out sprite, out details);
            title = "The Thantec office";
            details = "The office for Thantec";
            
        }
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                
                Wealth = 100,
                CO2 = 2,
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
                RenderOntoSprite(canvas, positionNew, "EventSprites/thantec-office", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "EventSprites/thantec-office", new Vector2(1, 1.5f));
            }
        }
        
    }
    
    public class ThantecFactory : StructureFactory
    {
        public ThantecFactory(City city) : base(city) { }
        public ThantecFactory() : base() { }

        public override int Cost
        {
            get { return 300; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("EventSprites/thantec-office");

        protected override Structure Create()
        {
            return new Thantec();
        }

        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < 2)
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
            title = "Build the Thantec office!";
            details = "Thantec needs a location for their office.";
        }
    }
}