using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;
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
    public static class ReloadMessage
    {
        public static GameObject message;
        public static Text messageText;

        public static void Initialize(Transform parent)
        {
            MelonCoroutines.Start(LoadAsset(parent));
        }

        public static IEnumerator LoadAsset(Transform parent)
        {
            Il2CppAssetBundleRequest rqMessage = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"assets/ui/Reload Message.prefab");
            yield return rqMessage;
            UnityEngine.Object UEOMessage = rqMessage.asset;
            GameObject _message = UEOMessage.Cast<GameObject>();
            message = UnityEngine.Object.Instantiate(_message, parent);
            message.SetActive(false);
            messageText = message.GetComponent<Text>();
        }

        public static void Show(bool show)
        {
            message.SetActive(show);
            MelonCoroutines.Start(Fade(message));
        }

        public static IEnumerator Fade(GameObject o)
        {
            yield return new WaitForSeconds(4F);
            o.SetActive(false);
        }

        public static void SetText(string message)
        {
            messageText.text = message;
        }
    }
}
