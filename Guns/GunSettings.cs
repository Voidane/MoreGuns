using Il2CppInterop.Runtime.InteropTypes.Fields;
using Il2CppInterop.Runtime.Attributes;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;

namespace MoreGuns.Guns
{
    [RegisterTypeInIl2Cpp]
    public class GunSettings : MonoBehaviour
    {
        public Il2CppValueField<bool> isAutomatic;
        public Il2CppValueField<float> speedMultiplier;
        public Il2CppValueField<bool> cameraJolt;
        public Il2CppValueField<bool> requireWindup;
        public Il2CppValueField<float> windupTime;
        public Il2CppValueField<bool> canManualyReload;

        public GunSettings(IntPtr ptr) : base(ptr) { }

        public GunSettings() : base(ClassInjector.DerivedConstructorPointer<GunSettings>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }
    }
}
