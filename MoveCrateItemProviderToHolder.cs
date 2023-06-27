using Kitchen;
using KitchenLib.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenOverstocked
{
    internal class MoveCrateItemProviderToHolder : FranchiseSystem
    {

        private EntityQuery crateItemProvidersEntityQuery;

        protected override void Initialise()
        {
            base.Initialise();
            crateItemProvidersEntityQuery = GetEntityQuery(typeof(CItemHolder), typeof(CCrateItemProvider));

        }
        protected override void OnUpdate()
        {
            using var crateItemProviders = crateItemProvidersEntityQuery.ToEntityArray(Allocator.TempJob);

            foreach (Entity entity in crateItemProviders)
            {
                if (!Require(entity, out CItemHolder itemHolder))
                {
                    continue;
                }

                if (!Require(entity, out CCrateItemProvider crateItemProvider)) {
                    continue;
                }

                if(crateItemProvider.applianceId == null)
                {
                    continue;
                }

                if (itemHolder.HeldItem != default(Entity)) {
                    continue;
                }

                var ctx = new EntityContext(EntityManager);
                var crateEntity = ctx.CreateItem(ItemReferences.Crate);
                Set(crateEntity, new CUpgrade { ID = (int)crateItemProvider.applianceId, IsFromLevel = false });
                ctx.UpdateHolder(crateEntity, entity);
            }
        }
    }
}
