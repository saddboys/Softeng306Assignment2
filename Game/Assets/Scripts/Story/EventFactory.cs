using Game.Story.Events;
using Game.Story.Events.RandomEvent;
using Game.Story.Events.VerdantLandRequests;
using Story.Events.RandomEvent;
using UnityEngine;

namespace Game.Story
{
    public class EventFactory
    {
        public enum RandomEvents {CONDITIONAL_REQUEST_HOUSE,FLOOD_EVENT, HEATWAVE_EVENT, HURRICANE_EVENT}
        public enum StoryEvents {INITIAL_THANTEC, RESEARCH_FACILITY_REQUEST,GIMME_MONEY_REQUEST, GIANT_COOLER_REQUEST,
                                PUSHING_HARDER_REQUEST, BAN_THE_CARS_REQUEST, CALLING_ON_LIFESTYLE_REQUEST}
        private StoryEvent storyEvent;
        public GameObject ManagerObject { get; set; }
        public StoryEvent CreateStoryEvent(StoryEvents storyEvents)
        {
            switch (storyEvents)
            {
                case StoryEvents.INITIAL_THANTEC:
                    return ManagerObject.AddComponent<CreateThanTecRequest>();
                case StoryEvents.RESEARCH_FACILITY_REQUEST:
                    return ManagerObject.AddComponent<ResearchFacilityRequest>();
                case StoryEvents.GIMME_MONEY_REQUEST:
                    return ManagerObject.AddComponent<GimmeMoneyRequest>();
                case StoryEvents.GIANT_COOLER_REQUEST:
                    return ManagerObject.AddComponent<GiantCoolerRequest>();
                case StoryEvents.PUSHING_HARDER_REQUEST:
                    return ManagerObject.AddComponent<PushingHarderRequest>();
                case StoryEvents.BAN_THE_CARS_REQUEST:
                    return ManagerObject.AddComponent<BanTheCarsRequest>();
                case StoryEvents.CALLING_ON_LIFESTYLE_REQUEST:
                    return ManagerObject.AddComponent<CallingOnLifestyleRequest>();
            }
            return null;
        }

        public StoryEvent CreateRandomEvent(RandomEvents randomEvents)
        {
            switch (randomEvents)
            {
                case RandomEvents.CONDITIONAL_REQUEST_HOUSE:
                    return ManagerObject.AddComponent<MoreHouseRequest>();
                case RandomEvents.FLOOD_EVENT:
                    return ManagerObject.AddComponent<FloodEvent>();
                case RandomEvents.HEATWAVE_EVENT:
                    return ManagerObject.AddComponent<HeatwaveEvent>();
                case RandomEvents.HURRICANE_EVENT:
                    return ManagerObject.AddComponent<HurricaneEvent>();
            }
            return null;
        }
    }
}