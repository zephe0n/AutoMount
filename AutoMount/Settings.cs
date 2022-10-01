using AutoMount;
using Epic.OnlineServices;
using Kingmaker.Localization;
using Kingmaker.UI;
using ModMenu.Settings;
using UnityEngine;
using UnityModManagerNet;
using KeyBinding = ModMenu.Settings.KeyBinding;

namespace AutoMount
{
    //public class Settings : UnityModManager.ModSettings
    //{
    //    public UnityModManagerNet.KeyBinding MountHotKey = new() { keyCode = KeyCode.A, modifiers = 1 | 2 }; // ctrl+shift+A
    //    public UnityModManagerNet.KeyBinding DismountHotKey = new() { keyCode = KeyCode.D, modifiers = 1 | 2 }; // ctrl+shift+D
    //    public bool MountOnAreaEnter = true;
    //    public bool ConsoleOutput = false;

    //    public override void Save(UnityModManager.ModEntry modEntry)
    //    {
    //        Save(this, modEntry);
    //    }

    //    public static void Draw()
    //    {
    //        GUILayout.Space(10);

    //        GUILayout.BeginHorizontal(GUILayout.Width(300));
    //        GUILayout.Label("Mount hotkey");
    //        UnityModManager.UI.DrawKeybinding(ref Main.Settings.MountHotKey, "Mount hotkey");
    //        GUILayout.EndHorizontal();

    //        GUILayout.BeginHorizontal(GUILayout.Width(300));
    //        GUILayout.Label("Dismount hotkey");
    //        UnityModManager.UI.DrawKeybinding(ref Main.Settings.DismountHotKey, "Dismount hotkey");
    //        GUILayout.EndHorizontal();

    //        GUILayout.BeginHorizontal();
    //        Main.Settings.MountOnAreaEnter = GUILayout.Toggle(Main.Settings.MountOnAreaEnter, " Always mount when entering area");
    //        GUILayout.EndHorizontal();

    //        GUILayout.BeginHorizontal();
    //        Main.Settings.ConsoleOutput = GUILayout.Toggle(Main.Settings.ConsoleOutput, " Enable console output");
    //        GUILayout.EndHorizontal();


    //        GUILayout.Space(10);
    //    }
    //}

    public static class Settings
    {
        private static bool Initialized = false;

        /* Keys */
        private static readonly string KeyRoot = "automount";
        private static readonly string Hotkeys = "hotkeys";
        private static readonly string Whitelist = "whitelist";
        public static readonly string MountOnAreaEnter = "areaentermount";
        public static readonly string ConsoleOutput = "consoleoutput";
        public static readonly string MountHotKey = $"{Hotkeys}.mounthotkey";
        public static readonly string DismountHotKey = $"{Hotkeys}.dismounthotkey";

        private static SettingsBuilder settings = SettingsBuilder.New(KeyRoot, GetString(GetKey("title"), "Auto Mount"));

        public static void Init()
        {
            if (Initialized)
            {
                Main.Logger.Log("Settings already initialized");
                return;
            }

            Main.Logger.Log("Initializing Settings");

            /* Main settings */
            settings.AddToggle(
                Toggle.New(
                    GetKey(MountOnAreaEnter),
                    true,
                    GetString(GetKey(MountOnAreaEnter), "Always mount when entering area")));

            settings.AddToggle(
                Toggle.New(
                    GetKey(ConsoleOutput),
                    true,
                    GetString(GetKey(ConsoleOutput), "Enable console output")));

            /* Hotkeys */
            var hotkeys = settings.AddSubHeader(GetString(GetKey(Hotkeys), "Hotkeys"), true);
            hotkeys.AddKeyBinding(KeyBinding.New(GetKey(MountHotKey), KeyboardAccess.GameModesGroup.All, GetString(GetKey(MountHotKey), "Mount")).SetPrimaryBinding(KeyCode.A, withCtrl:true, withShift:true), () => Main.Mount(true));
            hotkeys.AddKeyBinding(KeyBinding.New(GetKey(DismountHotKey), KeyboardAccess.GameModesGroup.All, GetString(GetKey(DismountHotKey), "Dismount")).SetPrimaryBinding(KeyCode.D, withCtrl: true, withShift: true), () => Main.Mount(false));

            ModMenu.ModMenu.AddSettings(settings);

            Initialized = true;

            Main.Logger.Log("Settings Initialized");
        }

        //public static void CreateWhitelist()
        //{
        //    settings.
        //}

        internal static bool IsEnabled(string key)
        {
            return ModMenu.ModMenu.GetSettingValue<bool>(GetKey(key));
        }

        private static string GetKey(string partialKey)
        {
            return $"{KeyRoot}.{partialKey}";
        }

        private static LocalizedString GetString(string partialKey, string text)
        {
            return Helpers.CreateString(GetKey(partialKey), text);
        }
    }
}
