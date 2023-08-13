using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.References;
using KitchenLib.Utils;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    class DisableCrateItemProviderSystem : FranchiseSystem
    {
        private EntityQuery crateItemProviders;

        protected override void Initialise()
        {
            base.Initialise();
            crateItemProviders = GetEntityQuery(typeof(CCrateItemProvider));
            RequireForUpdate(crateItemProviders);
        }
        protected override void OnUpdate()
        {
            if (!Mod.AutoRestock.Get())
            {
                Mod.LogInfo("Disabling providers");
                EntityManager.RemoveComponent<CCrateItemProvider>(crateItemProviders);
            }
        }
    }
}
