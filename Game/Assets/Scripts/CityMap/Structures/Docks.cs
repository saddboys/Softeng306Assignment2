﻿using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class Dock : Structure
    {
        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }

        public override void RenderOnto(GameObject canvas, Vector3 position)
        {
            RenderOnto(canvas, position, 33, new Vector2(1, 1.5f));
        }
    }

    public class DockFactory : StructureFactory
    {
        protected override Structure Create()
        {
            return new Dock();
        }
    }
}