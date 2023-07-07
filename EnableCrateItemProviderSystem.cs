using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.References;
using KitchenLib.Utils;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    class EnableCrateItemProviderSystem : FranchiseSystem
    {
        private EntityQuery crateShelvesWithoutCrateProvider;

        protected override void Initialise()
        {
            base.Initialise();
            crateShelvesWithoutCrateProvider = GetEntityQuery(new QueryHelper().All(typeof(CPersistentItemStorageLocation), typeof(CItemHolder)).None(typeof(CCrateItemProvider)));
            RequireForUpdate(crateShelvesWithoutCrateProvider);
        }
        protected override void OnUpdate()
        {
            if (Mod.AutoRestock)
            {
                using var garageShelves = crateShelvesWithoutCrateProvider.ToEntityArray(Allocator.Temp);

                foreach (var garageShelve in garageShelves)
                {
                    if (Require(garageShelve, out CItemHolder itemHolder) && itemHolder.HeldItem != default(Entity) && Require(itemHolder.HeldItem, out CUpgrade crate))
                    {
                        Mod.LogInfo("Setting crate provider to be " + crate.ID);
                        Set(garageShelve, new CCrateItemProvider
                        {
                            applianceId = crate.ID
                        });
                    }
                }
            }
        }
    }
}
