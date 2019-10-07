using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Factory : Structure
    {
        private Sprite sprite = Resources.Load<Sprite>("Textures/structures/FactorySprite");
        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOntoSprite(canvas, position, sprite, new Vector2(1, 1.5f));
        }
    }

    public class FactoryFactory : StructureFactory
    {
        public FactoryFactory(City city) : base(city) { }
        public FactoryFactory() : base() { }

        protected override Structure Create()
        {
            return new Factory();
        }
    }
}