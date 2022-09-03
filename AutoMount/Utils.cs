
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace AutoMount
{
    public class Utils
    {
        public static void ConsoleLog(string msg, Color color)
        {
            var message = new CombatLogMessage(msg, color, PrefixIcon.RightArrow, null, false);
            var messageLog = LogThreadService.Instance.m_Logs[LogChannelType.Common].First(x => x is MessageLogThread);
            messageLog.AddMessage(message);
        }
    }
}