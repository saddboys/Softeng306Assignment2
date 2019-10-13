using Game.Story.Events;
using Game.Story.Events.VerdantLandRequests;

namespace Game.Story
{
    public class EventFactory
    {
        public enum RandomEvents {CIRCUS_EVENT,CONDITIONAL_REQUEST_HOUSE}
        public enum StoryEvents {INITIAL_THANTEC, RESEARCH_FACILITY_REQUEST,GIMME_MONEY_REQUEST, GIANT_COOLER_REQUEST,
                                PUSHING_HARDER_REQUEST, BAN_THE_CARS_REQUEST, CALLING_ON_LIFESTYLE_REQUEST}
        private StoryEvent storyEvent;
        public StoryEvent CreateStoryEvent(StoryEvents storyEvents)
        {
            switch (storyEvents)
            {
                case StoryEvents.INITIAL_THANTEC:
                    return new CreateThanTecRequest();
                case StoryEvents.RESEARCH_FACILITY_REQUEST:
                    return new ResearchFacilityRequest();
                case StoryEvents.GIMME_MONEY_REQUEST:
                    return new GimmeMoneyRequest();
                case StoryEvents.GIANT_COOLER_REQUEST:
                    return new GiantCoolerRequest();
                case StoryEvents.PUSHING_HARDER_REQUEST:
                    return new PushingHarderRequest();
                case StoryEvents.BAN_THE_CARS_REQUEST:
                    return new BanTheCarsRequest();
                case StoryEvents.CALLING_ON_LIFESTYLE_REQUEST:
                    return new CallingOnLifestyleRequest();
            }
            return null;
        }

        public StoryEvent CreateRandomEvent(RandomEvents randomEvents)
        {
            switch (randomEvents)
            {
                case RandomEvents.CONDITIONAL_REQUEST_HOUSE:
                    return new MoreHouseRequest();
                case RandomEvents.CIRCUS_EVENT:
                    return new CircusEvent();
            }
            return null;
        }
    }
}