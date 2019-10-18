using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class SolarFarm : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = 0,
                Wealth = -50,
                ElectricCapacity = 0,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = -10,
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
            get { return 2000; }
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
                City.Stats.ElectricCapacity += 10;
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