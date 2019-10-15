using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Rock : Structure
    {
        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 29, new Vector2(1, 1.5f));
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out details);
            title = "Rocks";
        }
    }

    public class RockFactory : StructureFactory
    {
        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[29];

        protected override Structure Create()
        {
            return new Rock();
        }

        public override void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details)
        {
            base.GetInfoBoxData(out _, out meta, out sprite, out _);
            title = "Place some rocks";
            details = "If you wish, click on a tile to add some rocks.";
        }
    }
}
