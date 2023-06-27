using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.References;
using KitchenLib.Utils;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    class CrateCreatorSystem : FranchiseSystem
    {
        private EntityQuery CPersistentItemStorageLocations;

        protected override void Initialise()
        {
            base.Initialise();
            CPersistentItemStorageLocations = GetEntityQuery(typeof(CPersistentItemStorageLocation));
        }
        protected override void OnUpdate()
        {
            if(!Mod.CreateCrate)
            {
                return;
            }

            var applianceId = Mod.ApplianceId;


            using var garageHolders = CPersistentItemStorageLocations.ToEntityArray(Allocator.Temp);

            CItemHolder? unUsedItemHolder = null;

            foreach(var pedastle in garageHolders)
            {
                if(Require<CItemHolder>(pedastle, out CItemHolder itemHolder)) {
                    if(itemHolder.HeldItem == default)
                    {
                        unUsedItemHolder = itemHolder;
                        break;
                    }
                }
            }

            if(unUsedItemHolder == null)
            {
                Mod.LogInfo("Skipping generate the crate because an empty garage pedastle does not exist.");
                Mod.CreateCrate = false;
                return;

            }

            Mod.LogInfo($"Generating crate with applianceId {applianceId}");

            // We create it similarly to how the TriggerWorkshopCrafting works
            var entityManger = EntityUtils.GetEntityManager();
            var entity = entityManger.CreateEntity();

            Set(entity, new CUpgrade { ID = Mod.ApplianceId });

            Mod.CreateCrate = false;
        }
    }
}
