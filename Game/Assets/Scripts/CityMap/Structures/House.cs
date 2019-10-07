﻿using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class House : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                Score = 100,
                Reputation = -1,
                Wealth = 0.5,
                CO2 = 1,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = 1,
                Population = -4,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 21, new Vector2(1, 1.5f));
        }
    }

    public class HouseFactory : StructureFactory
    {
        public HouseFactory(City city) : base(city) { }
        public HouseFactory() : base() { }

        public override int Cost
        {
            get { return 500; }
        }

        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[21];

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

            if (tile.Terrain.TerrainType == Terrain.TerrainTypes.Ocean)
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
            }
        }
    }
}