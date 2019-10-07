using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class DemolishFactory : StructureFactory
    {
        protected override Structure Create()
        {
            return null;
        }
        public override bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!CanBuild(out reason))
            {
                return false;
            }

            reason = "";
            return true;
        }
    }
}