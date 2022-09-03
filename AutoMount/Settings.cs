using AutoMount;
using UnityEngine;
using UnityModManagerNet;

namespace AutoMount
{
    public class Settings : UnityModManager.ModSettings
    {
        public KeyBinding MountHotKey = new() { keyCode = KeyCode.A, modifiers = 1 | 2 }; // ctrl+shift+A
        public KeyBinding DismountHotKey = new() { keyCode = KeyCode.D, modifiers = 1 | 2 }; // ctrl+shift+D
        public bool MountOnAreaEnter = true;
        public bool ConsoleOutput = false;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public static void Draw()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal(GUILayout.Width(300));
            GUILayout.Label("Mount hotkey");
            UnityModManager.UI.DrawKeybinding(ref Main.Settings.MountHotKey, "Mount hotkey");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(300));
            GUILayout.Label("Dismount hotkey");
            UnityModManager.UI.DrawKeybinding(ref Main.Settings.DismountHotKey, "Dismount hotkey");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            Main.Settings.MountOnAreaEnter = GUILayout.Toggle(Main.Settings.MountOnAreaEnter, " Always mount when entering area");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            Main.Settings.ConsoleOutput = GUILayout.Toggle(Main.Settings.ConsoleOutput, " Enable console output");
            GUILayout.EndHorizontal();


            GUILayout.Space(10);
        }
    }
}
