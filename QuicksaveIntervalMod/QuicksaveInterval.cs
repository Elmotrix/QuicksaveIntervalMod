using Assets.Scripts;
using Assets.Scripts.Serialization;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace QuicksaveIntervalMod
{
    [HarmonyPatch(typeof(XmlSaveLoad), "SaveGame", new Type[] {typeof(DirectoryInfo), typeof(bool) } )]
    public static class xmlSaveLoadSaveGamePatch
    {
        [HarmonyPrefix]
        public static void Prefix(bool autoSave)
        {
            if (QuicksaveInterval.Timer == null)
            {
                QuicksaveInterval.Timer = new Timer(QuicksaveInterval.TimerInterval);
                QuicksaveInterval.Timer.Start();
                QuicksaveInterval.Timer.Elapsed += QuicksaveInterval.Timer_Elapsed;
                ConsoleWindow.Print("QSI: Timer generated");
            }
            if (!autoSave)
            {
                QuicksaveInterval.Timer.Stop();
                QuicksaveInterval.Timer.Start();
                ConsoleWindow.Print("QSI: Timer restarted");
            }
        }

    }
    public static class QuicksaveInterval
    {
        public static Timer Timer;
        public const int TimerInterval = 1800000;
        public static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ConsoleWindow.Print("QSI: Saving Worlds");
            XmlSaveLoad.SaveCurrentWorld();
        }
    }
}
