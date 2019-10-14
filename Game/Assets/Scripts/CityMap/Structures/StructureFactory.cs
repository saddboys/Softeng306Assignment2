using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.CityMap
{
    public abstract class StructureFactory
    {
        // TODO: Could store the picture associated with the structure (for the toolbar) in here.

        /// <summary>
        /// Assume that all structures have a cost, as it is useful to display it to the user.
        /// This is to be set by each concrete StructureFactory implementation.
        /// </summary>
        public virtual int Cost { get; }

        /// <summary>
        /// Sprite to use for the toolbar button.
        /// </summary>
        public virtual Sprite Sprite { get; }

        /// <summary>
        /// Reference to the city for requirement calculations. May be null if requirements
        /// checking is not required for the factory instance (e.g. for CityMap map generation).
        /// </summary>
        protected City City { get; }

        /// <summary>
        /// Create a structure factory with requirements checking.
        /// E.g. use this within ToolBar.
        /// </summary>
        /// <param name="city"></param>
        protected StructureFactory(City city)
        {
            City = city;
        }

        /// <summary>
        /// Create a structure factory without requirements checking.
        /// E.g. use this within CityMap.
        /// </summary>
        protected StructureFactory()
        {
            // Do nothing.
        }

        /// <summary>
        /// Probably only needed internally. To build structures, use <see cref="BuildOn"/>.
        /// To be implemented by the concrete StructureFactories for each kind of structure.
        /// </summary>
        /// <returns></returns>
        protected abstract Structure Create();

        /// <summary>
        /// Check whether the current game state allows the structure to be built.
        /// E.g., The structure may cost money, in which case the StructureFactory
        /// needs to check if the city stats has sufficient funds. It may also
        /// be that the structure requires a certain population, reputation, or
        /// electric capacity to build. This interface opens the possibility for
        /// even more complex requirements such as completion of quests or events.
        /// This is useful for the ToolBar to grey-out the button when requirements
        /// are not met.
        /// The base implementation simply checks that the city has enough money to build it.
        /// </summary>
        /// <param name="reason">If the structure cannot be built, the reason why is written here.</param>
        /// <returns>True if the game state allows this structure to be built.</returns>
        public virtual bool CanBuild(out string reason)
        {
            if (City?.Stats.Wealth < Cost)
            {
                reason = "Not enough money";
                return false;
            }

            reason = "";
            return true;
        }

        /// <summary>
        /// Check whether the structure can be built on a particular tile.
        /// This is useful for giving the user feedback when their cursor is hovering
        /// above a tile that the structure cannot be built on, or perhaps show a
        /// message when the user clicks on the tile trying to build the structure.
        /// Also checks if the game state allows the structure to be built in the
        /// first place.
        /// The base implementation allows the structure to be built regardless of tile.
        /// </summary>
        /// <param name="tile">The tile to test building on.</param>
        /// <param name="reason">Reason if building is disallowed.</param>
        /// <returns>True if building onto the tile is allowed.</returns>
        public virtual bool CanBuildOnto(MapTile tile, out string reason)
        {
            if (!CanBuild(out reason))
            {
                return false;
            }

            if (tile.Structure != null)
            {
                reason = "Cannot build on top of existing structure";
                return false;
            }

            reason = "";
            return true;
        }

        /// <summary>
        /// Build a structure onto the tile.
        /// The base implementation updates the mayor's credit card, but concrete
        /// implementations can add more complex behaviour if needed.
        /// </summary>
        /// <returns></returns>
        public virtual void BuildOnto(MapTile tile)
        {
            Assert.IsTrue(CanBuildOnto(tile, out _));

            tile.Structure = Create();

            if (City != null)
            {
                City.Stats.Wealth -= Cost;
            }
        }

        public Structure CreateGhost()
        {
            return Create();
        }
    }
}
