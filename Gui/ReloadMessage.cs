using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace MoreGunsMono.Gui
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
            AssetBundleRequest rqMessage = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"assets/ui/Reload Message.prefab");
            yield return rqMessage;
            GameObject _Message = rqMessage.asset as GameObject;
            message = UnityEngine.GameObject.Instantiate(_Message, parent);

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
