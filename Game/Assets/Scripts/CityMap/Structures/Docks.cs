using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Dock : Structure
    {
        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 10, new Vector2(1, 1.5f));
        }
    }

    public class DockFactory : StructureFactory
    {
        public DockFactory(City city) : base(city) { }
        public DockFactory() : base() { }
        public override int Cost
        {
            get { return 1000; }
        }

        public override Sprite Sprite { get; } =
            Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[10];

        protected override Structure Create()
        {
            return new Dock();
        }
    }
}