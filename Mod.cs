using Kitchen;
using KitchenMods;
using PreferenceSystem;
using PreferenceSystem.Event;

// Namespace should have "Kitchen" in the beginning
namespace KitchenOverstocked
{
    public class Mod : IModInitializer
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "com.stonepaw.overstocked";
        public const string MOD_NAME = "Overstocked";
        public const string MOD_VERSION = "2.0.0";
        public const string MOD_AUTHOR = "Stonepaw";
        public const string MOD_GAMEVERSION = ">=1.2.0";

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static PreferenceSystemManager PrefManager;

        public static bool AutoRestock
        {
            get { return PrefManager.Get<bool>(AutoRestockKey); }
        }

        private static readonly string AutoRestockKey = "auto_restock";

        public static bool DestroyCratesEnabled
        {
            get { return PrefManager.Get<bool>(DestroyCratesEnabledKey); }
        }

        private static readonly string DestroyCratesEnabledKey = "destroy_crates_enabled";

        public Mod() { }

        public void PostActivate(KitchenMods.Mod mod)
        {
            PrefManager = new PreferenceSystemManager(MOD_GUID, MOD_NAME);

            // Register our custom ordering sub menu
            Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(
                    args.instance,
                    new object[]
                    {
                        typeof(OverstockedMenu<MenuAction>),
                        new OverstockedMenu<MenuAction>(
                            args.instance.ButtonContainer,
                            args.module_list
                        ),
                    }
                );
            };

            // Build the preferences system menu
            PrefManager
                .AddLabel("Auto Restock")
                .AddOption(
                    AutoRestockKey,
                    true,
                    new bool[] { false, true },
                    new string[] { "Disabled", "Enabled" }
                )
                .AddLabel("Destroy Crates with Act")
                .AddOption(
                    DestroyCratesEnabledKey,
                    false,
                    new bool[] { false, true },
                    new string[] { "Disabled", "Enabled" }
                )
                .AddLabel("Order Additional Crates")
                .AddSelfRegisteredSubmenu<OverstockedMenu<MenuAction>>("Order")
                .AddInfo(
                    "Ordered or destroyed crates are not saved automatically. Start a restaurant or combine crates to save the changes to the workshop."
                );

            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.PauseMenu);
        }

        public void PreInject() { }

        public void PostInject() { }

        #region Logging
        internal static void LogInfo(string _log)
        {
            Debug.Log($"[{MOD_NAME}] " + _log);
        }

        internal static void LogWarning(string _log)
        {
            Debug.LogWarning($"[{MOD_NAME}] " + _log);
        }

        internal static void LogError(string _log)
        {
            Debug.LogError($"[{MOD_NAME}] " + _log);
        }

        #endregion
    }
}
