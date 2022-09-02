using AutoMount;
using UnityEngine;
using UnityModManagerNet;

namespace AutoMount
{
    public class Settings : UnityModManager.ModSettings
    {
        public KeyBinding AutoMountHotKey = new() { keyCode = KeyCode.A, modifiers = 1 | 2 }; // ctrl+shift+A

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public static void Draw()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal(GUILayout.Width(300));
            GUILayout.Label("Hotkey");
            UnityModManager.UI.DrawKeybinding(ref Main.Settings.AutoMountHotKey, "Hotkey");
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
    }
}
