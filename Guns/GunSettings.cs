using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Guns
{
    public class GunSettings : MonoBehaviour
    {
        public bool isAutomatic;
        public float speedMultiplier = 1.0F;
        public bool cameraJolt = true;
        public bool requiredWindup;
        public float windupTime = 0F;
        public bool canManuallyReload = true;
    }
}
