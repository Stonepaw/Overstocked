using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace KitchenOverstocked
{
    public class OverstockedMenu<T> : KLMenu<T>
    {

        private Option<string> GroupSelector;
        private Option<bool> AutoRestockOption;

        public OverstockedMenu(Transform container, ModuleList module_list) : base(container, module_list)
        { }

        public override void Setup(int player_id)
        {
            AddLabel("What crate would you like generate?");
            var applianceNames = Mod.LoadedAvailableAppliances.Keys.ToList();
            applianceNames.Sort();

            GroupSelector = new Option<string>(applianceNames, applianceNames[0], applianceNames);

            GroupSelector.OnChanged += delegate (object _, string value)
                            {
                                Mod.LogInfo(value);
                                Redraw(Mod.LoadedAvailableAppliances[value]);
                            };

            AutoRestockOption = new Option<bool>(new List<bool> { true, false }, Mod.AutoRestock, new List<string> { "Enabled", "Disabled" });
            AutoRestockOption.OnChanged += delegate (object _, bool value)
            {
                Mod.LogInfo("Setting autostocked to " + value);
                Mod.AutoRestock = value;
                Mod.AutoRestockChanged = true;
            };


            Redraw(Mod.LoadedAvailableAppliances[applianceNames[0]]);
        }

        private void Redraw(Dictionary<int, string> variants)
        {
            ModuleList.Clear();

            AddLabel("What crate would you like generate?");
            AddSelect(GroupSelector);

            AddLabel("Variant");

            Add(new Option<int>(variants.Keys.ToList(), variants.Keys.First(), variants.Values.ToList())).OnChanged += delegate (object _, int value)
            {
                Mod.ApplianceId = value;
            };

            AddButton("Create", delegate
            {
                Mod.CreateCrate = true;
            });

            //AddButton("Refresh Options", delegate {
            //    Mod.RefreshOptions = true;
            //    RequestPreviousMenu();
            //});

            Mod.ApplianceId = variants.Keys.First();

            AddLabel("Auto Restock");
            AddSelect(AutoRestockOption);
        }

    }
}
