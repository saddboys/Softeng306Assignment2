using Game;
using Game.CityMap;
using UnityEngine;

namespace Game
{
    public class Rock : Structure
    {
        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 29, new Vector2(1, 1.5f));
        }
    }

    public class RockFactory : StructureFactory
    {
        protected override Structure Create()
        {
            return new Rock();
        }
    }
}
