using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class PowerPlant : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                CO2 = 1,
                Wealth = 10,
                ElectricCapacity = 0,
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
                ElectricCapacity = -20,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            
            Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/powerPlant", new Vector2(1, 1.5f));
        }
    }

    public class PowerPlantFactory : StructureFactory
    {
        public PowerPlantFactory(City city) : base(city) { }
        public PowerPlantFactory() : base() { }
        public override int Cost
        {
            get { return 4000; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/powerPlant");

        protected override Structure Create()
        {
            return new PowerPlant();
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
                City.Stats.ElectricCapacity += 20;
            }
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
    }
}