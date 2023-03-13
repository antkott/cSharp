using System.Media;

namespace QMaticWebBookingParser.Helpers
{
    public static class SoundHelper
    {
        public static void PlayFindAlarm() {
            for (var i = 0; i < 6; i++)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.FindAlarm);
                player.Play();
                Thread.Sleep(1000);
            }
        }

        public static void PlayAttention()
        {
            for (var i = 0; i < 1; i++)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.Attention);
                player.Play();
                Thread.Sleep(1000);
            }
        }
    }
}
