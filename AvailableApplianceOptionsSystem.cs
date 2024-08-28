using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenOverstocked
{
    public class AvailableApplianceOptionsSystem : GameSystemBase
    {

        private static readonly int[] applianceIds = {
            ApplianceReferences.Bin,
            ApplianceReferences.BlueprintCabinet,
            ApplianceReferences.BlueprintUpgradeDesk,
            ApplianceReferences.CoffeeTable,
            ApplianceReferences.Combiner,
            ApplianceReferences.Countertop,
            ApplianceReferences.DirtyPlateStack,
            ApplianceReferences.Dumbwaiter,
            ApplianceReferences.ExtraLife,
            ApplianceReferences.FloorBufferStation,
            ApplianceReferences.FloorProtector,
            ApplianceReferences.FoodDisplayStand,
            ApplianceReferences.GasLimiter,
            ApplianceReferences.GasSafetyOverride,
            ApplianceReferences.Hob,
            ApplianceReferences.HostStand,
            ApplianceReferences.Mixer,
            ApplianceReferences.MopBucket,
            ApplianceReferences.OrderingTerminal,
            ApplianceReferences.Oven,
            ApplianceReferences.PlateStack,
            ApplianceReferences.Portioner,
            ApplianceReferences.PrepStation,
            ApplianceReferences.RobotMop,
            ApplianceReferences.SinkNormal,
            ApplianceReferences.TableLarge,
            ApplianceReferences.UpgradeKit,
        };


        private static readonly int[] coffeeAppliances =
        {
            ApplianceReferences.CoffeeMachine,
            ApplianceReferences.IceDispenser,
            ApplianceReferences.MilkDispenser,
            ApplianceReferences.SourceCakeStand,

        };

        private static readonly int[] tools =
        {
            -2070005162, // Clipboard Stand.
            ApplianceReferences.FireExtinguisherHolder,
            ApplianceReferences.RollingPinProvider,
            ApplianceReferences.ScrubbingBrushProvider,
            ApplianceReferences.SharpKnifeProvider,
            ApplianceReferences.ShoeRackTrainers,
            ApplianceReferences.ShoeRackWellies,
            ApplianceReferences.ShoeRackWorkBoots,
            ApplianceReferences.TrayStand,
            -940358190, // Leftover Bags Station
        };

        private static readonly int[] consumables =
        {
            ApplianceReferences.BreadstickBox,
            ApplianceReferences.CandleBox,
            ApplianceReferences.FlowerPot,
            ApplianceReferences.NapkinBox,
            ApplianceReferences.SharpCutlery,
            ApplianceReferences.SpecialsMenuBox,
            ApplianceReferences.SupplyCabinet,
        };

        private static readonly int[] decorations =
        {
            ApplianceReferences.Painting,
            ApplianceReferences.Plant,
            ApplianceReferences.Rug,
        };

        private static readonly int[] bakingTrays =
        {
           -660310536, // Big Cake Tin
            -2135982034, // Brownie Tray
            -1723125645, // Cookie Tray
            -315287689, // Cupcake tray
            2136474391, // Doughnut Tray
        };

        private static readonly int[] cooking =
        {
            ApplianceReferences.PotStack,
            ApplianceReferences.ServingBoardStack,
            ApplianceReferences.WokStack,
        };

        private static readonly int[] magic = {
            -292467039, // Enchanting Desk
            782648278, // Cauldron
            -1992638820 , // Enchanted Broom
            540526865, // Enchanted Plates
            -1946127856, // Ghostly Clipboard
            1313278365, // Ghostly Knife
            689268680, // Ghostly Rolling Pin
            -560953757, // Ghost Scrubber
            -1780646993 , // Illusion Wall
            1150470926 , // Instant Wand
            2044081363 , // Levitation Line
            119166501 , // Levitation Station
            267288096 , // Magic Apple Tree
            744482650 , // Magic Mirror
            -1696198539 , // Magic Spring
            29164230 , // Pouch of Holding
            423254987 , // Preserving Station
            -1688921160 , // Table - Sharing Cauldron
            2000892639 , // Table - Stone
            1492264331 , // Vanishing Circle
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

            Mod.LogInfo("Loading available appliances...");

            Mod.LoadedAvailableAppliances.Clear();


            foreach (int applianceId in applianceIds)
            {
                Appliance appliance = (Appliance)GDOUtils.GetExistingGDO(applianceId);


                if (appliance == null)
                {
                    continue;
                }

                var variants = new Dictionary<int, string>();
                variants.Add(applianceId, appliance.Name);

                Mod.LoadedAvailableAppliances.Add(appliance.Name, variants);


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

            Appliance belt = (Appliance)GDOUtils.GetExistingGDO(ApplianceReferences.Belt);
            belts.Add(belt.ID, belt.Name);

            Mod.LoadedAvailableAppliances.Add(belt.Name, belts);

            foreach (var upgrade in belt.Upgrades)
            {
                Mod.LogInfo($"{belt.Name} - {upgrade.Name}");
                if (!belts.ContainsKey(upgrade.ID))
                {
                    belts.Add(upgrade.ID, upgrade.Name);
                }

                var upgradeAppliance = (Appliance)GDOUtils.GetExistingGDO(upgrade.ID);

                foreach (var nestedUpgrade in upgradeAppliance.Upgrades)
                {
                    if (!belts.ContainsKey(nestedUpgrade.ID))
                    {
                        belts.Add(nestedUpgrade.ID, nestedUpgrade.Name);
                    }
                }
            }

            Mod.LoadedAvailableAppliances.Add("Baking", CreateApplianceDictionary(bakingTrays));
            Mod.LoadedAvailableAppliances.Add("Coffee", CreateApplianceDictionary(coffeeAppliances));
            Mod.LoadedAvailableAppliances.Add("Cooking", CreateApplianceDictionary(cooking));
            Mod.LoadedAvailableAppliances.Add("Consumables", CreateApplianceDictionary(consumables));
            Mod.LoadedAvailableAppliances.Add("Decorations", CreateApplianceDictionary(decorations));
            Mod.LoadedAvailableAppliances.Add("Magic", CreateApplianceDictionary(magic));
            Mod.LoadedAvailableAppliances.Add("Tools", CreateApplianceDictionary(tools));

            Mod.LogInfo("Found all appliances");
        }


        private Dictionary<int, string> CreateApplianceDictionary(int[] applianceIds)
        {
            var appliances = new Dictionary<int, string>();

            foreach (var applianceId in applianceIds)
            {
                Appliance appliance = (Appliance)GDOUtils.GetExistingGDO(applianceId);
                if (!appliances.ContainsKey(appliance.ID))
                {
                    appliances.Add(appliance.ID, appliance.Name.Replace("Source -", "").Replace("Provider", ""));
                }
            }

            return appliances;
        }
    }



}
