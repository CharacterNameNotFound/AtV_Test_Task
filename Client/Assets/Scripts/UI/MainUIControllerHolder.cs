namespace UI
{
    // with no proper UI system we need to hack this
    public class MainUIControllerHolder
    {
        public MainUIController MainUI { get; private set; }

        public void Set(MainUIController mainUIController)
        {
            MainUI = mainUIController;
        }

    }
}