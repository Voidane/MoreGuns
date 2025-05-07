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
    class GunSettings : MonoBehaviour
    {
        public Il2CppValueField<bool> isAutomatic;

        public GunSettings(IntPtr ptr) : base(ptr) { }

        public GunSettings() : base(ClassInjector.DerivedConstructorPointer<GunSettings>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }
    }
}
