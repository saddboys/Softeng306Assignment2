using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class SolarFarm : Structure
    {
        public static int StructCO2 = 0;
        public static int StructReputation = 1;
        public static int StructCost = 2000;
        public static int StructUpkeep = -50;
        public static int StructScore = 900;
        public static int StructPopulation = -3;
        public static int StructElectricity = 20;
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = StructCO2,
                Wealth = StructUpkeep,
            };
        }
        
        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "A solar farm";
            meta = "Cost: $" + SolarFarm.StructCost + "k" + "\t\t" +
                   "CO2: " + SolarFarm.StructCO2 + "MT" + "\n" +
                   "Electricity: " + SolarFarm.StructElectricity + "\t\t" +
                   "Upkeep: $" + -SolarFarm.StructUpkeep + "k";
            details = "Requires 3k workers. " +
                      "Although expensive and less effective, solar farms can produce electricity " +
                      "for your town without adding pollution.";
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

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/solarPanel", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/solarPanel", new Vector2(1, 1.5f));
            }
        }
    }

    public class SolarFarmFactory : StructureFactory
    {
        public SolarFarmFactory(City city) : base(city)
        {
            buildSound = Resources.Load<AudioClip>("SoundEffects/PowerPlant");
        }
        public SolarFarmFactory() : base() { }
        public override int Cost
        {
            get { return SolarFarm.StructCost; }
        }

        public override int Population
        {
            get { return -SolarFarm.StructPopulation; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/solarPanel");

        protected override Structure Create()
        {
            return new SolarFarm();
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += SolarFarm.StructElectricity;
                City.Stats.Score += SolarFarm.StructScore;
                City.Stats.Reputation += SolarFarm.StructReputation;
            }
        }

        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!base.CanBuildOnto(tile, out reason))
            {
                return false;
            }

            if (tile.Terrain.TerrainType != Terrain.TerrainTypes.DesertHill && tile.Terrain.TerrainType != Terrain.TerrainTypes.Desert
                                                                            && tile.Terrain.TerrainType != Terrain.TerrainTypes.Beach)
            {
                reason = "Can only build on desert";
                return false;
            }

            return true;
        }
        
        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a solar farm";
            meta = "Cost: $" + SolarFarm.StructCost + "k" + "\t\t" +
                   "CO2: " + SolarFarm.StructCO2 + "MT" + "\n" +
                   "Electricity: " + SolarFarm.StructElectricity + "\t\t" +
                   "Upkeep: $" + -SolarFarm.StructUpkeep + "k";
            details = "Requires 3k workers. " +
                      "Although expensive and less effective, solar farms can produce electricity " +
                      "for your town without adding pollution.";
        }
    }
}