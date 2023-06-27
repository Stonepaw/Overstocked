using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenMods;
using KitchenStartingMealSelector;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenOverstocked
{
    public class Mod : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "com.stonepaw.overstocked";
        public const string MOD_NAME = "Overstocked";
        public const string MOD_VERSION = "0.2.0";
        public const string MOD_AUTHOR = "Stonepaw";
        public const string MOD_GAMEVERSION = ">=1.1.5";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static bool CreateCrate = false;
        public static int ApplianceId = ApplianceReferences.BlueprintCabinet;
        public static bool AutoRestock = false;
        public static bool AutoRestockChanged = false;

        public static AssetBundle Bundle;

        public static bool RefreshOptions = false;

        public static List<int> LoadedAvailableApplianceIds = new();

        public static List<string> LoadedAvailableApplianceNames = new();

        public static Dictionary<string, Dictionary<int, string>> LoadedAvailableAppliances = new();

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            initPauseMenu();
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
 
          
        }


        private void initPauseMenu()
        {
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(OverstockedMenu<PauseMenuAction>), typeof(PauseMenuAction));
            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (s, args) => {
                args.Menus.Add(typeof(OverstockedMenu<PauseMenuAction>), new OverstockedMenu<PauseMenuAction>(args.Container, args.Module_list));
            };
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
