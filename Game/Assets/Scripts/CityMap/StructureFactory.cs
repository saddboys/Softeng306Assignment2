using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CityMap
{
    public abstract class StructureFactory
    {
        // TODO: Could store the picture associated with the structure (for the toolbar) in here.
        // TODO: When implementing concrete StructureFactories, make them singleton because
        // you'll only ever need a single instance factory for each kind of structure.
        // We can implement the concrete StructureFactory in the same file as the
        // concrete Structure class as well.

        /// <summary>
        /// Create a new Structure of the specific type of the factory.
        /// Useful for the ToolBar for generalising Structure creation.
        /// </summary>
        /// <returns></returns>
        public abstract Structure Create();

        /// <summary>
        /// Implements the logic for determining which terrain this structure can be built on.
        /// </summary>
        /// <param name="terrain"></param>
        /// <returns></returns>
        public abstract bool CanBuildOn(Terrain terrain);
    }
}
