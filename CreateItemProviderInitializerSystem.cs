using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.References;
using KitchenLib.Utils;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    class CreateItemProviderInitializerSystem : FranchiseSystem
    {
        private EntityQuery CCrateItemProviders;
        private EntityQuery CrateShelvesWithoutCrateProvider;

        protected override void Initialise()
        {
            base.Initialise();
            CCrateItemProviders = GetEntityQuery(typeof(CCrateItemProvider));
            CrateShelvesWithoutCrateProvider = GetEntityQuery(new QueryHelper().All(typeof(CPersistentItemStorageLocation)).None(typeof(CCrateItemProvider)));
        }
        protected override void OnUpdate()
        {
            if (Mod.AutoRestock)
            {
                this.EnableProviders();
            }
            else
            {
                if (Mod.AutoRestockChanged)
                {
                    this.DisableProviders();
                }
            }

            Mod.AutoRestockChanged = false;
        }

        protected void EnableProviders()
        {
            using var persistanceItemStorageLocations = CrateShelvesWithoutCrateProvider.ToEntityArray(Allocator.Temp);


            foreach (var persistanceItemStorageLocation in persistanceItemStorageLocations)
            {
                if (Require(persistanceItemStorageLocation, out CItemHolder comp))
                {
                    var heldItem = comp.HeldItem;

                    if (heldItem != default(Entity) && Require(heldItem, out CUpgrade crate))
                    {
                        Mod.LogInfo("Setting crate provider to be " + crate.ID);
                        Set(persistanceItemStorageLocation, new CCrateItemProvider
                        {
                            applianceId = crate.ID
                        });
                    }
                }
            }
        }

        protected void DisableProviders()
        {
            Mod.LogInfo("Disabling providers");

            using var crateItemProviders = CCrateItemProviders.ToEntityArray(Allocator.Temp);

            foreach (var createItemProvider in crateItemProviders)
            {
                EntityManager.RemoveComponent<CCrateItemProvider>(createItemProvider);
            }
        }

    }


}
