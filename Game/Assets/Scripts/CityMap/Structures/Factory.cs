using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Factory : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats()
            {
                CO2 = 10,
                Score = 500,
                Reputation = 3,
                Wealth = 10,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOntoSprite(canvas, position, "Textures/structures/FactorySprite", new Vector2(1, 1.5f));
        }
    }

    public class FactoryFactory : StructureFactory
    {
        public override int Cost {
            get
            {
                return 3000;
            }
        }

        protected override Structure Create()
        {
            return new Factory();
        }
        
        public override bool CanBuild(out string reason)
        {
            if (!base.CanBuild(out reason))
            {
                return false;
            }
            if (City.Stats.ElectricCapacity < 5)
            {
                reason = "Not enough electric capacity";
                return false;
            }
            return true;
        }
        
        public override void BuildOnto(MapTile tile)
        {
            City.Stats.ElectricCapacity -= 5;
            City.Stats.Wealth -= 15;
            base.BuildOnto(tile);
        }


    }
}