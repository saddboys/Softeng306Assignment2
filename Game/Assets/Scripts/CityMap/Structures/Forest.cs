﻿using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Forest : Structure
    {
        public const int StructCO2 = -5;
        public const int StructReputation = 0;
        public const int StructCost = 500;
        public const int StructUpkeep = 0;
        public const int StructScore = 100;
        public const int StructPopulation = 0;
        public const int StructElectricity = 0;
        
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = StructCO2,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                Reputation = -StructReputation
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            
            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/objectForest", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.1f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/objectForest", new Vector2(1, 1.5f));
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Forrest";
        }
    }

    public class ForestFactory : StructureFactory
    {
        public ForestFactory(City city) : base(city) { }
        public ForestFactory() : base() { }

        public override int Cost
        {
            get { return Forest.StructCost; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/objectForest");

        protected override Structure Create()
        {
            return new Forest();
        }

        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
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

            if ((tile.Terrain.TerrainType != Terrain.TerrainTypes.Grass) && (tile.Terrain.TerrainType != Terrain.TerrainTypes.GrassHill))
            {
                reason = "Cannot build onto anything other than grass";
                return false;
            }

            return true;
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.Reputation += Forest.StructReputation;
                City.Stats.Score += Forest.StructScore;
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Grow a forrest";
            details = "Although it may seem expensive, you should grow some forrests to help keep the pollution down.";
        }
    }
}