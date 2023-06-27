using Unity.Entities;

namespace KitchenOverstocked
{
    internal struct CCrateItemProvider : IComponentData
    {
        public int? applianceId;
    }
}
