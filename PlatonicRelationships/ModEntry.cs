﻿using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using HarmonyLib;
using PlatonicRelationships.Framework;
using StardewModdingAPI.Events;

namespace PlatonicRelationships
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        private readonly AddDatingPrereq Editor = new AddDatingPrereq();

        public override void Entry(IModHelper helper)
        {
            Config = this.Helper.ReadConfig<ModConfig>();
            if (Config.AddDatingRequirementToRomanticEvents)
                helper.Events.Content.AssetRequested += OnAssetRequested;

            //apply harmony patches
            ApplyPatches();
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (Editor.CanEdit(e.NameWithoutLocale))
                e.Edit(Editor.Edit);
        }

        public void ApplyPatches()
        {
            var harmony = new Harmony("cherry.platonicrelationships");

            try
            {
                this.Monitor.Log("Transpile patching SocialPage.drawNPCSlotHeart");
                harmony.Patch(
                    original: AccessTools.Method(typeof(SocialPage), name: "drawNPCSlotHeart"),
                    prefix: new HarmonyMethod(methodType: typeof(PatchDrawNpcSlotHeart), nameof(PatchDrawNpcSlotHeart.Prefix))
                );
            }
            catch (Exception e)
            {
                Monitor.Log($"Failed in Patching SocialPage.drawNPCSlotHeart: \n{e}", LogLevel.Error);
                return;
            }

            try
            {
                this.Monitor.Log("Postfix patching Utility.GetMaximumHeartsForCharacter");
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), name: "GetMaximumHeartsForCharacter"),
                    postfix: new HarmonyMethod(typeof(patchGetMaximumHeartsForCharacter), nameof(patchGetMaximumHeartsForCharacter.Postfix))
                );
            }
            catch (Exception e)
            {
                Monitor.Log($"Failed in Patching Utility.GetMaximumHeartsForCharacter: \n{e}", LogLevel.Error);
                return;
            }
        }
    }
}