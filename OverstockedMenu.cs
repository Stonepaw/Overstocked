using System.Collections.Generic;
using System.Linq;
using Kitchen;
using Kitchen.Modules;
using Unity.Entities;
using UnityEngine;

namespace KitchenOverstocked
{
    public class OverstockedMenu<T> : Menu<T>
    {
        private Option<string> GroupSelector;
        private int ApplianceId = -571205127; // BlueprintCabinet;

        public OverstockedMenu(Transform container, ModuleList module_list)
            : base(container, module_list) { }

        public override void Setup(int player_id)
        {
            AddLabel("What crate would you like order?");
            var applianceNames =
                AvailableApplianceOptionsSystem.LoadedAvailableAppliances.Keys.ToList();
            applianceNames.Sort();

            GroupSelector = new Option<string>(applianceNames, applianceNames[0], applianceNames);

            GroupSelector.OnChanged += delegate(object _, string value)
            {
                Mod.LogInfo(value);
                Redraw(AvailableApplianceOptionsSystem.LoadedAvailableAppliances[value]);
            };

            Redraw(AvailableApplianceOptionsSystem.LoadedAvailableAppliances[applianceNames[0]]);
        }

        private void Redraw(Dictionary<int, string> variants)
        {
            ModuleList.Clear();

            AddLabel("What crate would you like order?");
            AddSelect(GroupSelector);

            AddLabel("Variant");

            Add(
                new Option<int>(
                    variants.Keys.ToList(),
                    variants.Keys.First(),
                    variants.Values.ToList()
                )
            ).OnChanged += delegate(object _, int value)
            {
                ApplianceId = value;
            };

            AddButton(
                "Order",
                delegate
                {
                    var entityManager = World
                        .DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerManager>()
                        .EntityManager;
                    var entity = entityManager.CreateEntity();
                    entityManager.AddComponentData(
                        entity,
                        new CCreateCrate { applianceId = ApplianceId }
                    );
                }
            );

            ApplianceId = variants.Keys.First();

            AddInfo(
                "Ordered or destroyed crates are not saved automatically. Start a restaurant or combine crates to save the changes to the workshop."
            );
        }
    }
}
