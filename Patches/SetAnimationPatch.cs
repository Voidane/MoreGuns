using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.PlayerScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class SetAnimationPatch
    {
        // Store original animations to restore them later
        private static Dictionary<int, Dictionary<string, AnimationClip>> originalAnimations = new Dictionary<int, Dictionary<string, AnimationClip>>();

        // Cache override controllers to avoid creating new ones every time
        private static Dictionary<int, AnimatorOverrideController> cachedOverrideControllers = new Dictionary<int, AnimatorOverrideController>();

        // Animation names that will be overridden
        private static readonly string[] animationsToOverride = new string[] { "BothHand_Grip_Lowered", "BothHand_Grip_Raised", "BothHand_Grip_Recoil" };

        // Lock object for thread safety
        private static readonly object dictionaryLock = new object();

        // Track active animators to handle cleanup
        private static HashSet<int> activeAnimatorIds = new HashSet<int>();

        /**
        
        [HarmonyPatch(typeof(MelonMod), "OnApplicationQuit")]
        [HarmonyPostfix]
        public static void OnApplicationQuit()
        {
            // Clean up resources when application quits
            lock (dictionaryLock)
            {
                originalAnimations.Clear();
                cachedOverrideControllers.Clear();
                activeAnimatorIds.Clear();
            }
        }

        [HarmonyPatch(typeof(ScheduleOne.AvatarFramework.Avatar), "OnDestroy")]
        [HarmonyPrefix]
        public static void OnAvatarDestroy(ScheduleOne.AvatarFramework.Avatar __instance)
        {
            if (__instance?.Anim?.animator == null) return;

            int animatorId = __instance.Anim.animator.GetInstanceID();
            CleanupAnimator(animatorId);
        }

        **/

        private static void CleanupAnimator(int animatorId)
        {
            lock (dictionaryLock)
            {
                originalAnimations.Remove(animatorId);
                cachedOverrideControllers.Remove(animatorId);
                activeAnimatorIds.Remove(animatorId);
            }
        }

        [HarmonyPatch(typeof(AvatarEquippable), "SetTrigger")]
        [HarmonyPostfix]
        public static void Postfix(AvatarEquippable __instance, string anim)
        {
            try
            {
                // Get avatar and animator, with null checks
                var avatar = AccessTools.Field(typeof(AvatarEquippable), "avatar").GetValue(__instance) as ScheduleOne.AvatarFramework.Avatar;
                if (avatar?.Anim?.animator == null)
                {
                    MelonLogger.Warning("[MoreGuns] Null animator detected in SetTrigger");
                    return;
                }

                Animator animator = avatar.Anim.animator;
                int animatorId = animator.GetInstanceID();

                // Get or create override controller
                AnimatorOverrideController animatorOverrideController;

                lock (dictionaryLock)
                {
                    // Register this animator as active
                    activeAnimatorIds.Add(animatorId);

                    // Get or create the override controller
                    if (!cachedOverrideControllers.TryGetValue(animatorId, out animatorOverrideController))
                    {
                        if (animator.runtimeAnimatorController == null)
                        {
                            return;
                        }

                        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                        cachedOverrideControllers[animatorId] = animatorOverrideController;
                    }
                }

                // Extract weapon name safely
                string weaponName = "";
                if (__instance?.gameObject != null)
                {
                    weaponName = __instance.gameObject.name;
                    weaponName = weaponName.Replace("(Clone)", "").Trim().ToLower();
                }
                else
                {
                    return;
                }

                // Get current overrides
                List<KeyValuePair<AnimationClip, AnimationClip>> anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                animatorOverrideController.GetOverrides(anims);

                // Store original animations if not already stored
                lock (dictionaryLock)
                {
                    if (!originalAnimations.ContainsKey(animatorId))
                    {
                        originalAnimations[animatorId] = new Dictionary<string, AnimationClip>();
                    }

                    foreach (var animName in animationsToOverride)
                    {
                        for (int i = 0; i < anims.Count; i++)
                        {
                            if (anims[i].Key != null && animName == anims[i].Key.name && !originalAnimations[animatorId].ContainsKey(animName))
                            {
                                originalAnimations[animatorId][animName] = anims[i].Value;
                            }
                        }
                    }
                }

                bool animationsReplaced = false;

                // Attempt to get weapon-specific animations
                if (!string.IsNullOrEmpty(weaponName) && WeaponBase.weaponsByName.TryGetValue(weaponName, out WeaponBase weapon))
                {
                    // Check if we have specific animation matching for this weapon and trigger
                    if (weapon?.animations != null && weapon.animations.TryGetValue(anim, out AnimationClip weaponAnim) && weaponAnim != null)
                    {
                        // Apply weapon-specific animation to appropriate animation slots
                        foreach (var animName in animationsToOverride)
                        {
                            for (int i = 0; i < anims.Count; i++)
                            {
                                if (anims[i].Key != null && anims[i].Key.name == animName)
                                {
                                    anims[i] = new KeyValuePair<AnimationClip, AnimationClip>(anims[i].Key, weaponAnim);
                                    animationsReplaced = true;
                                }
                            }
                        }

                        if (animationsReplaced)
                        {
                            animatorOverrideController.ApplyOverrides(anims);
                            animator.runtimeAnimatorController = animatorOverrideController;
                        }
                    }
                    else
                    {
                        // No specific animation for this trigger, reset to original
                        ResetAnimations(animator, anims, animatorId, animatorOverrideController);
                    }
                }
                else
                {
                    // Not a known weapon, reset animations
                    ResetAnimations(animator, anims, animatorId, animatorOverrideController);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MoreGuns] Exception in SetAnimationPatch: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void ResetAnimations(Animator animator, List<KeyValuePair<AnimationClip, AnimationClip>> anims,
                                        int animatorId, AnimatorOverrideController animatorOverrideController)
        {
            try
            {
                if (animator == null || animatorOverrideController == null)
                {
                    MelonLogger.Warning("[MoreGuns] Null animator or controller in ResetAnimations");
                    return;
                }

                bool animationsReset = false;

                lock (dictionaryLock)
                {
                    if (originalAnimations.TryGetValue(animatorId, out Dictionary<string, AnimationClip> originalClips))
                    {
                        foreach (var animName in animationsToOverride)
                        {
                            if (originalClips.TryGetValue(animName, out AnimationClip originalClip))
                            {
                                for (int i = 0; i < anims.Count; i++)
                                {
                                    if (anims[i].Key != null && anims[i].Key.name == animName)
                                    {
                                        anims[i] = new KeyValuePair<AnimationClip, AnimationClip>(anims[i].Key, originalClip);
                                        animationsReset = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (animationsReset)
                {
                    animatorOverrideController.ApplyOverrides(anims);
                    animator.runtimeAnimatorController = animatorOverrideController;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MoreGuns] Exception in ResetAnimations: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Optional: Periodic cleanup method to be called from somewhere in your mod
        public static void PerformPeriodicCleanup()
        {
            lock (dictionaryLock)
            {
                // Create a list of animator IDs to remove
                List<int> animatorsToRemove = new List<int>();

                foreach (int animatorId in originalAnimations.Keys)
                {
                    if (!activeAnimatorIds.Contains(animatorId))
                    {
                        animatorsToRemove.Add(animatorId);
                    }
                }

                // Remove any stale entries
                foreach (int id in animatorsToRemove)
                {
                    CleanupAnimator(id);
                }
            }
        }
    }
}