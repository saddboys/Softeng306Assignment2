using System.Collections.Generic;

namespace Game.Story.ThanTec
{
    public class ThanTec
    {
        public enum ThanTecEvents
        {
            RESEARCH_FACILITY_REQUEST, GIMME_MONEY_REQUEST, GIANT_COOLER_REQUEST, FINAL_TECH_REQUEST,
            PUSHING_HARDER_REQUEST, BAN_CARS_REQUEST, CALLING_ON_LIFECYCLE_REQUEST, FINAL_REQUEST
        }
        private enum ThanTecDoesNotExistEvents { }
        public List<ThanTecEvents> Events { get; private set; }
        /// <summary>
        /// On the 4th turn, the thantec event will occur
        /// </summary>
        
        public int EventTurn
        {
            get { return 4; }
        }
        
        public bool HasThanTec
        {
            get { return hasThanTec;}
            set
            {
                hasThanTec = value;
                Events = CreateStoryPool();

            }
        }
        private bool hasThanTec = false;

        private List<ThanTecEvents> CreateStoryPool()
        {
            if (hasThanTec)
            {
                
            }
            else
            {
                
            }

            return null;
        }
    }
}