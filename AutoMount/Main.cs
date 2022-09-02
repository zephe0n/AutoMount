using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using static UnityModManagerNet.UnityModManager.ModEntry;
using Kingmaker.PubSubSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Parts;

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
        private static Guid m_mount_ability_guid = new Guid("d340d820867cf9741903c9be9aed1ccc");

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
            if (Settings.AutoMountHotKey.Up())
            {
                Mount();
            }
        }

        static void Mount()
        {
            foreach (var rider in Game.Instance.Player.Party)
            {
                var pet = rider.GetPet(Kingmaker.Enums.PetType.AnimalCompanion);

                if (pet != null && (rider.State.Size < pet.State.Size))
                {
                    var mount = rider.ActivatableAbilities.Enumerable.Find(a => a.Blueprint.AssetGuid.CompareTo(m_mount_ability_guid) == 0);

                    if (mount != null)
                    {
                        if (mount.IsOn)
                        {
#if DEBUG
                            Logger.Log(rider.CharacterName + " -> " + pet.CharacterName + " OFF");
#endif
                            rider.RiderPart?.Dismount();
                        }
                        else
                        {
#if DEBUG
                            Logger.Log(rider.CharacterName + " -> " + pet.CharacterName + " ON");
#endif
                            rider.Ensure<UnitPartRider>().Mount(pet);
                        }
                    }
                }
            }
        }
    }
}