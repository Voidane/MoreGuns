using MelonLoader;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Gui
{
    public static class Reticle
    {

        public static GameObject reticle;

        public static void Initialize()
        {
            MelonCoroutines.Start(FindAndInstantiateCrosshair());
        }

        public static IEnumerator FindAndInstantiateCrosshair()
        {
            if (true)
            {
                if (HUD.Instance.crosshair == null)
                {
                    MelonLogger.Msg("Searching for crosshair in scene...");
                    yield return new WaitForSeconds(0.5F);
                }
                else
                {
                    reticle = UnityEngine.Object.Instantiate(HUD.Instance.crosshair.gameObject, HUD.Instance.crosshair.transform.parent);
                    reticle.SetActive(false);
                }
            }
        }
    }
}
