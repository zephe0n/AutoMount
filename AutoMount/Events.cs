using Kingmaker.PubSubSystem;

namespace AutoMount.Events
{
    public class OnAreaLoad : IAreaHandler
    {
        public void OnAreaDidLoad()
        {
            if (Settings.IsEnabled(Settings.MountOnAreaEnter))
            {
                Main.ForceMount();
            }
        }

        public void OnAreaBeginUnloading()
        { }
    }
}
