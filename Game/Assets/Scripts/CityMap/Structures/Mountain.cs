using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Mountain : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
            };
        }

        public override Stats GetStatsChangeOnDemolish()
        {
            return new Stats
            {
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            Vector3 positionNew = new Vector3(position.x, position.y + 0.15f, position.z);
            RenderOntoSprite(canvas, positionNew, "Textures/structures/mountain", new Vector2(1, 1.5f));
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Mountain";
        }
    }

    public class MountainFactory : StructureFactory
    {
        public MountainFactory(City city) : base(city) { }
        public MountainFactory() : base() { }

        public override int Cost
        {
            get { return 1000000; }
        }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/mountain");

        protected override Structure Create()
        {
            return new Mountain();
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

            if (tile.Terrain.TerrainType.Equals(Terrain.TerrainTypes.Ocean) || 
                tile.Terrain.TerrainType.Equals(Terrain.TerrainTypes.GrassHill) || 
                tile.Terrain.TerrainType.Equals(Terrain.TerrainTypes.DesertHill) || 
                tile.Terrain.TerrainType.Equals(Terrain.TerrainTypes.Beach ))
            {
                reason = "Mountain only buildable on flat soil";
                return false;
            }

            return true;
        }

        public override void BuildOnto(MapTile tile)
        {
            base.BuildOnto(tile);

            if (City != null)
            {
            }
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "mountain";
            details = "A tall mountain which cannot be used for anything";
        }
    }
}