using Game;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Game.CityMap
{
    public class Tower : Structure
    {
        public override Stats GetStatsContribution()
        {
            return new Stats
            {
                Wealth = 4,
            };
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 44, new Vector2(1, 1.5f));
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "The... tower...";
        }
    }

    public class TowerFactory : StructureFactory
    {
        public override int Cost
        {
            get { return 2000; }
        }
        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[44];

        protected override Structure Create()
        {
            return new Tower();
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Build a tower";
            details = "The tower... Click on a tile to build a tower.";
        }
    }
}
