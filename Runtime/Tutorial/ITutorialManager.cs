namespace Lumos
{
    public interface ITutorialManager
    {
        public TutorialTable GetTable();
        public BaseTutorial GetTutorial();
        public void Play(TutorialTable table);
    }
}