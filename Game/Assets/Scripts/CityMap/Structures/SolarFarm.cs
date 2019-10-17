﻿using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class SolarFarm : Structure
    {
        public const int StructCO2 = 0;
        public const int StructReputation = 1;
        public const int StructCost = 2000;
        public const int StructUpkeep = -50;
        public const int StructScore = 900;
        public const int StructPopulation = -3;
        public const int StructElectricity = 20;
        
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
        public SolarFarmFactory(City city) : base(city) { }
        public SolarFarmFactory() : base() { }
        public override int Cost
        {
            get { return SolarFarm.StructCost; }
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
                City.Stats.Population += SolarFarm.StructPopulation;
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
    }
}