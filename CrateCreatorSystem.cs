using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    class CrateCreatorSystem : FranchiseSystem, IModSystem
    {
        private EntityQuery garageItemHolders;
        private EntityQuery createCrates;

        protected override void Initialise()
        {
            base.Initialise();
            garageItemHolders = GetEntityQuery(new QueryHelper().All(typeof(CPersistentItemStorageLocation), typeof(CItemHolder)));
            createCrates = GetEntityQuery(typeof(CCreateCrate));
            RequireForUpdate(createCrates);
        }
        protected override void OnUpdate()
        {
            using var currentCreateCreates = createCrates.ToComponentDataArray<CCreateCrate>(Allocator.Temp);

            foreach (var item in currentCreateCreates)
            {

                using var currentGarageItemHolders = this.garageItemHolders.ToComponentDataArray<CItemHolder>(Allocator.Temp);

                CItemHolder? unUsedItemHolder = null;

                foreach (var pedastle in currentGarageItemHolders)
                {

                    if (pedastle.HeldItem == default)
                    {
                        unUsedItemHolder = pedastle;
                        break;
                    }

                }

                if (unUsedItemHolder == null)
                {
                    Mod.LogInfo("Skipping generate the crate because an empty garage pedastle does not exist.");
                    break;

                }

                var applianceId = item.applianceId;

                Mod.LogInfo($"Generating crate with applianceId {applianceId}");

                // We create it similarly to how the TriggerWorkshopCrafting works, the garage seems to just pickup the new crate and put it on a shelf
                var entity = EntityManager.CreateEntity();
                Set(entity, new CUpgrade { ID = applianceId });
            }

            if (currentCreateCreates.Length > 0)
            {
                EntityManager.DestroyEntity(createCrates);
            }
        }
    }
}
