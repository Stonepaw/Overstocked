using System.Collections.Generic;
using Kitchen;
using KitchenData;
using KitchenMods;

namespace KitchenOverstocked
{
    public class AvailableApplianceOptionsSystem : GameSystemBase, IModSystem
    {
        public static List<int> LoadedAvailableApplianceIds = new List<int> { };

        public static List<string> LoadedAvailableApplianceNames = new List<string> { };

        public static Dictionary<string, Dictionary<int, string>> LoadedAvailableAppliances =
            new Dictionary<string, Dictionary<int, string>>();

        private static readonly int[] applianceIds =
        {
            1551609169, // Bin
            -571205127, // BlueprintCabinet
            1139247360, // BlueprintUpgradeDesk
            -773196462, // Buffet Counter
            1648733244, // CoffeeTable
            -1906799936, // Combiner
            -1248669347, // Countertop
            434150763, // DirtyPlateStack
            532998682, // Dumbwaiter
            -1817838704, // ExtraLife
            1351951642, // FloorBufferStation
            1765889988, // FloorProtector
            -1813414500, // FoodDisplayStand
            1921027834, // GasLimiter
            -770041014, // GasSafetyOverride
            862493270, // Hob
            -63118559, // HostStand
            1329097317, // Mixer
            -1993346570, // MopBucket
            -1610332021, // OrderingTerminal
            -1068749602, // Oven
            1313469794, // PlateStack
            -1462602185, // Portioner
            1656358740, // PrepStation
            -2147057861, // RobotMop
            1083874952, // SinkNormal
            209074140, // TableLarge
            -26827118, // UpgradeKit
        };

        private static readonly int[] coffeeAppliances =
        {
            -1609758240, // CoffeeMachine
            801015432, // IceDispenser
            -557736569, // MilkDispenser
            143484231, // SourceCakeStand
        };

        private static readonly int[] tools =
        {
            -2070005162, // Clipboard Stand.
            1286554202, // FireExtinguisherHolder
            1738351766, // RollingPinProvider
            624465484, // ScrubbingBrushProvider
            2023704259, // SharpKnifeProvider
            723626409, // ShoeRackTrainers
            1796077718, // ShoeRackWellies
            230848637, // ShoeRackWorkBoots
            1129858275, // TrayStand
        };

        private static readonly int[] consumables =
        {
            639111696, // BreadstickBox
            1358522063, // CandleBox
            221442949, // FlowerPot
            1528688658, // NapkinBox
            2080633647, // SharpCutlery
            446555792, // SpecialsMenuBox
            -1013770159, // SupplyCabinet
            -940358190, // Leftover Bags Station
        };

        private static readonly int[] decorations =
        {
            668664567, // Painting
            756364626, // Plant
            -648349801, // Rug
        };

        private static readonly int[] bakingTrays =
        {
            -660310536, // Big Cake Tin
            -2135982034, // Brownie Tray
            -1723125645, // Cookie Tray
            -315287689, // Cupcake tray
            2136474391, // Doughnut Tray
            -217313684, // Mixing Bowls
        };

        private static readonly int[] cooking =
        {
            -957949759, // PotStack
            235423916, // ServingBoardStack
            314862254, // WokStack
        };

        private static readonly int[] magic =
        {
            -292467039, // Enchanting Desk
            782648278, // Cauldron
            -1992638820, // Enchanted Broom
            540526865, // Enchanted Plates
            -1946127856, // Ghostly Clipboard
            1313278365, // Ghostly Knife
            689268680, // Ghostly Rolling Pin
            -560953757, // Ghost Scrubber
            -1780646993, // Illusion Wall
            1150470926, // Instant Wand
            2044081363, // Levitation Line
            119166501, // Levitation Station
            267288096, // Magic Apple Tree
            744482650, // Magic Mirror
            -1696198539, // Magic Spring
            29164230, // Pouch of Holding
            423254987, // Preserving Station
            -1688921160, // Table - Sharing Cauldron
            2000892639, // Table - Stone
            1492264331, // Vanishing Circle
        };

        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {
            if (!(Has<CSceneFirstFrame>()))
            {
                return;
            }

            DiscoverAppliances();
        }

        private void DiscoverAppliances()
        {
            Mod.LogInfo("Loading available appliances...");

            LoadedAvailableAppliances.Clear();

            foreach (int applianceId in applianceIds)
            {
                Appliance appliance = GameData.Main.Get<Appliance>(applianceId);

                if (appliance == null)
                {
                    continue;
                }

                var variants = new Dictionary<int, string> { { applianceId, appliance.Name } };

                LoadedAvailableAppliances.Add(appliance.Name, variants);

                foreach (var upgrade in appliance.Upgrades)
                {
                    Mod.LogInfo($"{appliance.Name} - {upgrade.Name}");
                    // The deconstructor mod causes an issue because it loops the blueprint cabinet to a deconstructor and back
                    if (!variants.ContainsKey(upgrade.ID))
                    {
                        variants.Add(upgrade.ID, upgrade.Name);
                    }
                }
            }

            // Belts
            var belts = new Dictionary<int, string>();

            Appliance belt = GameData.Main.Get<Appliance>(1973114260); // Basic Bitch Belt
            belts.Add(belt.ID, belt.Name);

            LoadedAvailableAppliances.Add(belt.Name, belts);

            foreach (var upgrade in belt.Upgrades)
            {
                Mod.LogInfo($"{belt.Name} - {upgrade.Name}");
                if (!belts.ContainsKey(upgrade.ID))
                {
                    belts.Add(upgrade.ID, upgrade.Name);
                }

                var upgradeAppliance = GameData.Main.Get<Appliance>(upgrade.ID);

                foreach (var nestedUpgrade in upgradeAppliance.Upgrades)
                {
                    if (!belts.ContainsKey(nestedUpgrade.ID))
                    {
                        belts.Add(nestedUpgrade.ID, nestedUpgrade.Name);
                    }
                }
            }

            LoadedAvailableAppliances.Add("Baking", CreateApplianceDictionary(bakingTrays));
            LoadedAvailableAppliances.Add("Coffee", CreateApplianceDictionary(coffeeAppliances));
            LoadedAvailableAppliances.Add("Cooking", CreateApplianceDictionary(cooking));
            LoadedAvailableAppliances.Add("Consumables", CreateApplianceDictionary(consumables));
            LoadedAvailableAppliances.Add("Decorations", CreateApplianceDictionary(decorations));
            LoadedAvailableAppliances.Add("Magic", CreateApplianceDictionary(magic));
            LoadedAvailableAppliances.Add("Tools", CreateApplianceDictionary(tools));

            Mod.LogInfo("Found all appliances");
        }

        private static Dictionary<int, string> CreateApplianceDictionary(int[] applianceIds)
        {
            var appliances = new Dictionary<int, string>();

            foreach (var applianceId in applianceIds)
            {
                Appliance appliance = GameData.Main.Get<Appliance>(applianceId);
                if (!appliances.ContainsKey(appliance.ID))
                {
                    appliances.Add(
                        appliance.ID,
                        appliance.Name.Replace("Source -", "").Replace("Provider", "")
                    );
                }
            }

            return appliances;
        }
    }
}
