using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using static UnityModManagerNet.UnityModManager.ModEntry;
using Kingmaker;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;
using AutoMount.Events;
using Kingmaker.PubSubSystem;

namespace AutoMount
{
#if DEBUG
    [EnableReloading]
#endif
    public static class Main
    {
        public static Settings Settings;
        public static bool Enabled;
        public static ModLogger Logger;

        private static Harmony m_harmony;
        private static OnAreaLoad m_area_load_handler;
        private static Guid m_mount_ability_guid = new Guid("d340d820867cf9741903c9be9aed1ccc");
        private static bool m_force_mount = false;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            Settings = Settings.Load<Settings>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUpdate = OnUpdate;
#if DEBUG
            modEntry.OnUnload = Unload;
#endif
            m_harmony = new Harmony(modEntry.Info.Id);
            m_harmony.PatchAll(Assembly.GetExecutingAssembly());

            m_area_load_handler = new OnAreaLoad();
            EventBus.Subscribe(m_area_load_handler);

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

#if DEBUG
        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            m_harmony.UnpatchAll();
            EventBus.Unsubscribe(m_area_load_handler);
            return true;
        }
#endif

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Draw();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float delta)
        {
            if (Main.m_force_mount)
            {
                Mount(true);
                Main.m_force_mount = false;
            }
            else
            {
                if (Settings.MountHotKey.Up())
                {
                    Mount(true);
                }

                if (Settings.DismountHotKey.Up())
                {
                    Mount(false);
                }
            }
        }

        public static void ForceMount()
        {
            Main.m_force_mount = true;
        }

        public static void Mount(bool on)
        {
            foreach (var rider in Game.Instance.Player.Party)
            {
                var pet = rider.GetPet(Kingmaker.Enums.PetType.AnimalCompanion);

                if (pet != null && (rider.State.Size < pet.State.Size))
                {
                    var mount = rider.ActivatableAbilities.Enumerable.Find(a => a.Blueprint.AssetGuid.CompareTo(m_mount_ability_guid) == 0);

                    if (mount != null)
                    {
                        if (mount.IsOn && !on)
                        {
                            rider.RiderPart?.Dismount();

                            if (Settings.ConsoleOutput)
                            {
                                Utils.ConsoleLog(rider.CharacterName + " -> " + pet.CharacterName + " OFF", Color.blue);
                            }
                        }
                        else if (!mount.IsOn && on)
                        {
                            rider.Ensure<UnitPartRider>().Mount(pet);

                            if (Settings.ConsoleOutput)
                            {
                                Utils.ConsoleLog(rider.CharacterName + " -> " + pet.CharacterName + " ON", Color.blue);
                            }
                        }
                    }
                }
            }
        }
    }
}