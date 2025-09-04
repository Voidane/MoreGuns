using HarmonyLib;
using Il2CppScheduleOne.Dialogue;
using MoreGuns.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class DialoguePatch
    {
        public static bool isInititalized = false;
        private static DialogueChoiceData reloadDialogue;

        [HarmonyPatch(typeof(DialogueHandler), "ShowNode")]
        [HarmonyPrefix]
        public static void Prefix(DialogueHandler __instance, DialogueNodeData node)
        {
            if (!isInititalized)
            {
                reloadDialogue = new DialogueChoiceData() { Guid = "voiddial-stan-guns-opti-reloadmultgn", ChoiceText = "Reload Guns", ChoiceLabel = "Reload Guns" };
                isInititalized = true;
            }

            List<DialogueChoiceData> values = node.choices.ToList();
            if (node.Guid == StanDialogue.stanDialogueMainOptions && __instance.NPC.name == "Stan")
            {
                foreach (DialogueChoiceData choice in values)
                {
                    if (choice.ChoiceLabel == "Reload Guns")
                    {
                        return;
                    }
                }

                values.Add(reloadDialogue);
                node.choices = values.ToArray();
            }
            else
            {
                values.RemoveAll(choice => choice.ChoiceLabel == "Reload Guns");
                node.choices = values.ToArray();
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
