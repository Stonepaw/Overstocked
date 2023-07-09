using Kitchen;
using Unity.Entities;

namespace KitchenOverstocked
{
    internal class InsertIntoCrateItemProvider : ItemInteractionSystem
    {
        private CCrateItemProvider Provider;

        private Entity TargetItem;

        private bool HasMadeChanges;

        protected override bool AllowActOrGrab => true;

        protected override bool BeforeRun()
        {
            HasMadeChanges = false;
            return true;
        }

        protected override void Perform(ref InteractionData data)
        {
            HasMadeChanges = true;
            data.Context.Destroy(TargetItem);
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            if (HasMadeChanges)
            {
                return false;
            }

            if (!Require<CCrateItemProvider>(data.Target, out Provider))
            {
                return false;
            }


            if (!Require<CItemHolder>(data.Interactor, out CItemHolder itemHolder) || itemHolder.HeldItem == default(Entity))
            {

                return false;

            }

            if (!Require<CUpgrade>(itemHolder.HeldItem, out CUpgrade cUpgrade))
            {
                return false;

            }


            if(cUpgrade.ID != Provider.applianceId)
            {
                return false;
            }

            TargetItem = itemHolder.HeldItem;

            return true;
        }
    }
}
