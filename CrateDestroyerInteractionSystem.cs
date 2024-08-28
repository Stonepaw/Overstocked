using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace KitchenOverstocked
{
    internal class CrateDestroyerInteractionSystem : ItemInteractionSystem, IModSystem
    {
        protected override InteractionType RequiredType => InteractionType.Act;
        protected override bool RequireHold => false;

        protected override void Initialise()
        {
            base.Initialise();
            // Only run in franchise mode
            RequireSingletonForUpdate<SFranchiseMarker>();
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            if (!Mod.DestroyCratesEnabled)
            {
                return false;
            }

            if (
                Has<CPersistentItemStorageLocation>(data.Target)
                && Require<CItemHolder>(data.Target, out CItemHolder holder)
                && holder.HeldItem != default(Entity)
            )
            {
                return true;
            }

            return false;
        }

        protected override void Perform(ref InteractionData data)
        {
            if (Has<CCrateItemProvider>(data.Target))
            {
                data.Context.Remove<CCrateItemProvider>(data.Target);
            }

            if (Require<CItemHolder>(data.Target, out CItemHolder holder))
            {
                var item = holder.HeldItem;

                if (item != default(Entity))
                {
                    data.Context.Destroy(item);
                }
            }
        }
    }
}
