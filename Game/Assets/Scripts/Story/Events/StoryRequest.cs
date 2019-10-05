namespace Game.Story.Events
{
    
    /// <summary>
    /// A story request is an event but with an option to decline.
    /// </summary>
    public abstract class StoryRequest : StoryEvent
    {
        public abstract void OnNoClick();

        public override EventTypes EventType
        {
            get { return EventTypes.Request; }
        }
    }
}