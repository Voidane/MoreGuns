using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Dialogue;
using ScheduleOne.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class DialoguePatch
    {
        public static bool called = false;

        [HarmonyPatch(typeof(DialogueHandler), "ShowNode")]
        [HarmonyPrefix]
        public static void Prefix(DialogueHandler __instance, DialogueNodeData node)
        {
            if (!called)
            {
                if (node.Guid == StanDialogue.allWeaponCategoryOptions)
                {
                    List<DialogueChoiceData> values = node.choices.ToList();
                    MelonLogger.Msg($"Move: {values[3].ChoiceText}");
                    DialogueChoiceData nevermind = values[3];
                    values[3] = new DialogueChoiceData() { Guid = "voiddial-stan-guns-opti-reloadmultgn", ChoiceText = "Reload Guns", ChoiceLabel = "Reload Guns" };
                    values.Add(nevermind);
                    node.choices = values.ToArray();
                    called = true;
                }
            }
        }

        [HarmonyPatch(typeof(DialogueController_ArmsDealer), "ChoiceCallback")]
        [HarmonyPrefix]
        public static void Prefix(DialogueController_ArmsDealer __instance, string choiceLabel)
        {
            if (choiceLabel == "Reload Guns")
            {
                StanDialogue.StartSpecialGunReloads();
            }
        }
    }
}
