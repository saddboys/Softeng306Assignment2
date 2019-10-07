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
    }

    public class RockFactory : StructureFactory
    {
        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[29];

        protected override Structure Create()
        {
            return new Rock();
        }
    }
}
