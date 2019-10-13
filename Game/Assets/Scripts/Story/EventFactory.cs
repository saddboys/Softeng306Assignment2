using Game.Story.Events;

namespace Game.Story
{
    public class EventFactory
    {
        public enum RandomEvents {CIRCUS_EVENT,CONDITIONAL_REQUEST_HOUSE}
        public enum StoryEvents {INITIAL_THANTEC, RESEARCH_FACILITY_REQUEST, PUSHING_HARDER_REQUEST}
        private StoryEvent storyEvent;
        public StoryEvent CreateStoryEvent(StoryEvents storyEvents)
        {
            switch (storyEvents)
            {
                case StoryEvents.INITIAL_THANTEC:
                    return new CreateThanTecRequest();
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