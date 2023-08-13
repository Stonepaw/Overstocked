using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine.UIElements.Experimental;

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
            -2070005162, // Clipboard Stand. TODO: Update when kitchenlib updates
            ApplianceReferences.FireExtinguisherHolder,
            ApplianceReferences.RollingPinProvider,
            ApplianceReferences.ScrubbingBrushProvider,
            ApplianceReferences.SharpKnifeProvider,
            ApplianceReferences.ShoeRackTrainers,
            ApplianceReferences.ShoeRackWellies,
            ApplianceReferences.ShoeRackWorkBoots,
            ApplianceReferences.TrayStand,
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
            Mod.LoadedAvailableAppliances.Add("Consumables", CreateApplianceDictionary(consumables));
            Mod.LoadedAvailableAppliances.Add("Decorations", CreateApplianceDictionary(decorations));
            Mod.LoadedAvailableAppliances.Add("Tools", CreateApplianceDictionary(tools));
        }


        private Dictionary<int, string> CreateApplianceDictionary(int[] applianceIds)
        {
            var appliances = new Dictionary<int, string>();

            foreach (var applianceId in applianceIds)
            {
                Appliance appliance = (Appliance)GDOUtils.GetExistingGDO(applianceId);
                if (!appliances.ContainsKey(appliance.ID))
                {
                    appliances.Add(appliance.ID, appliance.name.Replace("Source -", "").Replace("Provider", ""));
                }
            }

            return appliances;
        }
    }



}
