using Game;
using Game.CityMap;
using UnityEngine;

namespace Game.CityMap
{
    public class DemolishFactory : StructureFactory
    {
        public override int Cost
        {
            get
            {
                return 100;
            }
        }
        public DemolishFactory(City city) : base(city) { }
        public DemolishFactory() : base() { }

        public override Sprite Sprite { get; } =
            Resources.Load<Sprite>("Textures/structures/Demolish");

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

            if (tile.Structure == null)
            {
                reason = "Nothing to demolish here";
                return false;
            }

            reason = "";
            return true;
        }

        public override void BuildOnto(MapTile tile)
        {
            // Note: Get structure before it is demolished.
            if (City != null)
            {
                City.Stats.UpdateContribution(tile.Structure.GetStatsChangeOnDemolish());
            }

            base.BuildOnto(tile);
        }
    }
}