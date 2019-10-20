using Game;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Game.CityMap
{
    public class House : Structure
    {
        public static int StructCO2 = 1;
        public static int StructReputation = -1;
        public static int StructCost = 250;
        public static int StructUpkeep = 50;
        public static int StructScore = 100;
        public static int StructPopulation = 5;
        public static int StructElectricity = -1;
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                
                Wealth = StructUpkeep,
                CO2 = StructCO2,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = -StructElectricity,
                Population = -StructPopulation,
                Reputation = -StructReputation
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {

            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/House", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.1f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/House", new Vector2(1, 1.5f));
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out _, out sprite, out _);
            title = "A house";
            meta = "Cost: $" + House.StructCost + "k" + "\t\t" +
                   "CO2: " + House.StructCO2 + "MT" + "\n" +
                   "Electricity: " + House.StructElectricity + "\t\t" +
                   "Income: $" + House.StructUpkeep + "k";
            details = "Provides 5k workers." +
                      "They will pay you tax, but use some electricity and generate some pollution. " +
                      "Building too many without parks will make your city unhappy";
        }
    }

    public class HouseFactory : StructureFactory
    {
        public HouseFactory(City city) : base(city) { }
        public HouseFactory() : base() { }

        public override int Cost
        {
            get { return House.StructCost; }
        }
        
        public override int Population
        {
            get { return -House.StructPopulation; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/House");

        protected override Structure Create()
        {
            return new House();
        }

        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City?.Stats.ElectricCapacity < -House.StructElectricity)
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
                City.Stats.ElectricCapacity += House.StructElectricity;
                City.Stats.Reputation += House.StructReputation;
                City.Stats.Score += House.StructScore;
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out _, out sprite, out _);
            title = "Build a house";
            meta = "Cost: $" + House.StructCost + "k" + "\t\t" +
                   "CO2: " + House.StructCO2 + "MT" + "\n" +
                   "Electricity: " + House.StructElectricity + "\t\t" +
                   "Income: $" + House.StructUpkeep + "k";
            details = "Provides 5k workers." +
                      "They will pay you tax, but use some electricity and generate some pollution. " +
                      "Building too many without parks will make your city unhappy";
        }
    }
}