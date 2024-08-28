using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    internal class MoveCrateItemProviderToHolder : FranchiseSystem, IModSystem
    {
        private EntityQuery crateItemProvidersEntityQuery;

        private int CrateItemReference = -2065950566;

        protected override void Initialise()
        {
            base.Initialise();
            crateItemProvidersEntityQuery = GetEntityQuery(
                typeof(CItemHolder),
                typeof(CCrateItemProvider)
            );
        }

        protected override void OnUpdate()
        {
            using var crateItemProviders = crateItemProvidersEntityQuery.ToEntityArray(
                Allocator.TempJob
            );

            foreach (Entity entity in crateItemProviders)
            {
                if (!Require(entity, out CItemHolder itemHolder))
                {
                    continue;
                }

                if (!Require(entity, out CCrateItemProvider crateItemProvider))
                {
                    continue;
                }

                if (crateItemProvider.applianceId == null)
                {
                    continue;
                }

                if (itemHolder.HeldItem != default(Entity))
                {
                    continue;
                }

                var ctx = new EntityContext(EntityManager);
                var crateEntity = ctx.CreateItem(CrateItemReference);
                Set(
                    crateEntity,
                    new CUpgrade { ID = (int)crateItemProvider.applianceId, IsFromLevel = false }
                );
                ctx.UpdateHolder(crateEntity, entity);
            }
        }
    }
}
