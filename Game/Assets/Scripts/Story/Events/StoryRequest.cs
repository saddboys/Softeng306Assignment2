namespace Game.Story.Events
{
    public abstract class StoryRequest : StoryEvent
    {
        public abstract void OnNoClick();
        public abstract void OnYesClick();
    }
}