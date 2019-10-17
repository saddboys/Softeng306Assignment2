using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Park : Structure
    {
        public const int StructCO2 = 0;
        public const int StructReputation = 5;
        public const int StructCost = 1000;
        public const int StructUpkeep = -100;
        public const int StructScore = 250;
        public const int StructPopulation = 0;
        public const int StructElectricity = 0;
        
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
            if (Tile.Terrain.TerrainType == Terrain.TerrainTypes.DesertHill 
                || Tile.Terrain.TerrainType == Terrain.TerrainTypes.GrassHill)
            {
                
                Vector3 positionNew = new Vector3(position.x, position.y + 0.3f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/park", new Vector2(1, 1.5f));
            }
            else
            {
                Vector3 positionNew = new Vector3(position.x, position.y + 0.1f, position.z);
                RenderOntoSprite(canvas, positionNew, "Textures/structures/park", new Vector2(1, 1.5f));
            }

        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats()
            {
                Population = -StructPopulation,
                Reputation = -StructReputation,
            };
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Park";
        }
    }

    public class ParkFactory : StructureFactory
    {
        public ParkFactory(City city) : base(city) { }
        public ParkFactory() : base() { }

        public override int Cost
        {
            get { return Park.StructCost; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/park");

        protected override Structure Create()
        {
            return new Park();
        }
        
        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (tile.Terrain.TerrainType != Terrain.TerrainTypes.Grass && tile.Terrain.TerrainType != Terrain.TerrainTypes.GrassHill)
            {
                reason = "Parks can only be built on grassland";
                return false;
            }
            return base.CanBuildOnto(tile, out reason);
        }
        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.Reputation += Park.StructReputation;
                City.Stats.Score += Park.StructScore;
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a park";
            details = "Add a park to your town. Make it a wonderful town to live in. Click on a tile to build a park.";
        }
    }
}