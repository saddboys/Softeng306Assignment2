using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class WindFarm : Structure
    {
        
        public static int StructCO2 = 0;
        public static int StructReputation = 1;
        public static int StructCost = 2000;
        public static int StructUpkeep = -50;
        public static int StructScore = 900;
        public static int StructPopulation = -3;
        public static int StructElectricity = 20;
        
        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "A wind farm";
            meta = "Cost: $" + WindFarm.StructCost + "k" + "\t\t" +
                   "CO2: " + WindFarm.StructCO2 + "MT" + "\n" +
                   "Electricity: " + WindFarm.StructElectricity + "\t\t" +
                   "Upkeep: $" + -WindFarm.StructUpkeep + "k";
            details = "Currently providing work for 3k citizens. Although expensive and less effective that power plants, solar farms can produce electricity " +
                      "for your town without adding pollution.";
        }
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = StructCO2,
                Wealth = StructUpkeep,
            };
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
            Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/windmill", new Vector2(1, 1.5f));
            
        }
    }

    public class WindFarmFactory : StructureFactory
    {
        public WindFarmFactory(City city) : base(city)
        {
            buildSound = Resources.Load<AudioClip>("SoundEffects/PowerPlant");
        }
        public WindFarmFactory() : base() { }
        public override int Cost
        {
            get { return WindFarm.StructCost; }
        }
        public override int Population
        {
            get { return -WindFarm.StructPopulation; }
        }


        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/windmill");

        protected override Structure Create()
        {
            return new WindFarm();
        }

        public override void BuildOnto(MapTile tile)
        {
            
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += WindFarm.StructElectricity;
                City.Stats.Score += WindFarm.StructScore;
                City.Stats.Reputation += WindFarm.StructReputation;
            }
        }

        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!base.CanBuildOnto(tile, out reason))
            {
                return false;
            }

            if (tile.Terrain.TerrainType != Terrain.TerrainTypes.DesertHill && tile.Terrain.TerrainType != Terrain.TerrainTypes.GrassHill )
            {
                reason = "Can only build on hills";
                return false;
            }

            return true;
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a wind farm";
            meta = "Cost: $" + WindFarm.StructCost + "k" + "\t\t" +
                   "CO2: " + WindFarm.StructCO2 + "MT" + "\n" +
                   "Electricity: " + WindFarm.StructElectricity + "\t\t" +
                   "Upkeep: $" + -WindFarm.StructUpkeep + "k";
            details = "Requires 3k workers. " +
                      "Although expensive and less effective, wind farms can produce electricity for your town without adding pollution.";
        }
    }
}