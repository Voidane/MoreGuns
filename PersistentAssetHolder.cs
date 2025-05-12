using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.ItemFramework;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGuns
{
    // Persistent object that holds asset references across scene transitions
    [RegisterTypeInIl2Cpp]
    public class PersistentAssetHolder : MonoBehaviour
    {
        // We need to keep static references because we can't rely on the GameObject surviving
        public static GameObject AK47EquippablePrefab;
        public static IntegerItemDefinition AK47IntItemDef;
        public static IntegerItemDefinition AK47MagazineIntItemDef;
        public static GameObject AK47HandGunPrefab;
        public static GameObject AK47MagazineEquippablePrefab;
        public static bool AssetsLoaded = false;

        // Required for IL2CPP injection
        public PersistentAssetHolder(IntPtr ptr) : base(ptr) { }

        public PersistentAssetHolder() : base(ClassInjector.DerivedConstructorPointer<PersistentAssetHolder>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        public void Awake()
        {
            // Make this object persist across scene loads
            DontDestroyOnLoad(gameObject);
            MelonLogger.Msg("PersistentAssetHolder created and set to not destroy on load");
        }
    }
}
