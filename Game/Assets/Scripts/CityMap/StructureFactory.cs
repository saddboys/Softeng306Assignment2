using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CityMap
{
    public abstract class StructureFactory
    {
        public abstract Structure Create();
    }
}