using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

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
            ApplianceReferences.Dumbwaiter,
            ApplianceReferences.ExtraLife,
            ApplianceReferences.FloorBufferStation,
            ApplianceReferences.FloorProtector,
            ApplianceReferences.FoodDisplayStand,
            ApplianceReferences.GasLimiter,
            ApplianceReferences.GasSafetyOverride,
            ApplianceReferences.Hob,
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

        private static int[] tools =
        {
            ApplianceReferences.FireExtinguisherHolder,
            ApplianceReferences.ScrubbingBrushProvider,
            ApplianceReferences.SharpKnifeProvider,
            ApplianceReferences.ShoeRackTrainers,
            ApplianceReferences.ShoeRackWellies,
            ApplianceReferences.ShoeRackWorkBoots,
            ApplianceReferences.TrayStand,
        };

        private static int[] consumables =
        {
            ApplianceReferences.BreadstickBox,
            ApplianceReferences.CandleBox,
            ApplianceReferences.NapkinBox,
            ApplianceReferences.SharpCutlery,
            ApplianceReferences.SpecialsMenuBox,
            ApplianceReferences.SupplyCabinet,
        };

        private static int[] decorations =
        {
            ApplianceReferences.Painting,
            ApplianceReferences.Plant,
            ApplianceReferences.Rug,
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

            Mod.LogWarning("Loading available appliances...");

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
                    variants.Add(upgrade.ID, upgrade.Name);
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
                belts.Add(upgrade.ID, upgrade.Name);

                var upgradeAppliance = (Appliance)GDOUtils.GetExistingGDO(upgrade.ID);

                foreach (var nestedUpgrade in upgradeAppliance.Upgrades)
                {
                    belts.Add(nestedUpgrade.ID, nestedUpgrade.Name);
                }
            }

            var consumableVariants = new Dictionary<int, string>();

            foreach (var consumableId in consumables)
            {
                Appliance appliance = (Appliance)GDOUtils.GetExistingGDO(consumableId);
                consumableVariants.Add(appliance.ID, appliance.Name);
            }

            Mod.LoadedAvailableAppliances.Add("Consumables", consumableVariants);

            var toolsVariants = new Dictionary<int, string>();

            foreach (var toolId in tools)
            {
                Appliance appliance = (Appliance)GDOUtils.GetExistingGDO(toolId);
                toolsVariants.Add(appliance.ID, appliance.Name);
            }

            Mod.LoadedAvailableAppliances.Add("Tools", toolsVariants);

            var decorationVariants = new Dictionary<int, string>();

            foreach (var decorationId in decorations)
            {
                Appliance appliance = (Appliance)GDOUtils.GetExistingGDO(decorationId);
                decorationVariants.Add(appliance.ID, appliance.Name);
            }

            Mod.LoadedAvailableAppliances.Add("Decorations", decorationVariants);
        }
    }
}
