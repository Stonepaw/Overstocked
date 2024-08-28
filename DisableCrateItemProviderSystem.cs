using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace KitchenOverstocked
{
    class DisableCrateItemProviderSystem : FranchiseSystem, IModSystem
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
            if (!Mod.AutoRestock)
            {
                Mod.LogInfo("Disabling providers");
                EntityManager.RemoveComponent<CCrateItemProvider>(crateItemProviders);
            }
        }
    }
}
