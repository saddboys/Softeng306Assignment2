using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Dock : Structure
    {
        public static int StructCO2 = 30;
        public static int StructReputation = 0;
        public static int StructCost = 5000;
        public static int StructUpkeep = 1500;
        public static int StructScore = 2000;
        public static int StructPopulation = -15;
        public static int StructElectricity = -30;
        
        
        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "A dock";
            meta = "Cost: $" + Dock.StructCost + "k" + "\t\t" +
                   "CO2: " + Dock.StructCO2 + "MT" + "\n" +
                   "Electricity: " + Dock.StructElectricity + "\t\t" +
                   "Income: $" + Dock.StructUpkeep + "k";
            details = "This is a huge commercial buildings to help generate money, but it adds a lot of pollution. It requires 15k workers.";
        }
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = StructCO2,
                Wealth = StructUpkeep,
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
                Population = -StructPopulation,
                ElectricCapacity = -StructElectricity,
                Reputation = -StructReputation
            };
        }
        
    }

    public class DockFactory : StructureFactory
    {
        public DockFactory(City city) : base(city) { }
        public DockFactory() : base() { }
        public override int Cost
        {
            get { return Dock.StructCost; }
        }

        public override int Population
        {
            get { return -Dock.StructPopulation; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/dock");

        
        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < -Dock.StructElectricity)
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
                City.Stats.ElectricCapacity -= Dock.StructElectricity;
                City.Stats.Reputation += Dock.StructReputation;
                City.Stats.Score += Dock.StructScore;
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a dock";
            meta = "Cost: $" + Dock.StructCost + "k" + "\t\t" +
                   "CO2: " + Dock.StructCO2 + "MT" + "\n" +
                   "Electricity: " + Dock.StructElectricity + "\t\t" +
                   "Income: $" + Dock.StructUpkeep + "k";
            details = "Requires 15k workers." +
                      "Docks are huge commercial buildings to help generate money, but make a lot of pollution." +
                      " Click on a tile to build a dock.";
        }
    }
}