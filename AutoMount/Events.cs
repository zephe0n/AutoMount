using Kingmaker.PubSubSystem;

namespace AutoMount.Events
{
    public class OnAreaLoad : IAreaHandler
    {
        public void OnAreaDidLoad()
        {
            Settings.Init();

            if (Settings.IsEnabled(Settings.MountOnAreaEnter))
            {
                Main.ForceMount();
            }
        }

        public void OnAreaBeginUnloading()
        { }
    }
}
