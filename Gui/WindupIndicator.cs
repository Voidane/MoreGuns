using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MoreGuns.Gui
{
    public static class WindupIndicator
    {
        public static GameObject windupIndicator;
        public static Slider windupIndicatorSlider;
        public static Image backgroundImage;
        public static Image fillImage;

        public static void Initialize(Transform parent)
        {
            MelonCoroutines.Start(LoadAsset(parent));
        }

        public static IEnumerator LoadAsset(Transform parent)
        {
            Il2CppAssetBundleRequest rqWindupIndicator = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"assets/ui/Windup Indicator.prefab");
            yield return rqWindupIndicator;
            UnityEngine.Object UEOWindupIndicator = rqWindupIndicator.asset;
            GameObject _windupIndicator = UEOWindupIndicator.Cast<GameObject>();
            windupIndicator = UnityEngine.Object.Instantiate(_windupIndicator, parent);
            windupIndicator.SetActive(false);

            windupIndicatorSlider = windupIndicator.GetComponent<Slider>();
            backgroundImage = windupIndicator.transform.GetChild(0).GetComponent<Image>();
            fillImage = windupIndicator.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }

        public static void Show(bool shown)
        {
            windupIndicator.SetActive(shown);
        }

        public static void SetValueByTime(float from, float to)
        {
            if (from >= to)
            {
                SetValue(100);
            }
            else
            {
                int value = (int)((from * 100) / to);
                SetValue(value);
            }
        }

        public static void SetValue(int value)
        {
            windupIndicatorSlider.value = value;

            if (value == 100)
            {
                Show(false);
            }
            else
            {
                Show(true);
            }
        }
    }
}
