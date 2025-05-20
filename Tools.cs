using MelonLoader;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MoreGunsMono.Tools;

namespace MoreGunsMono
{
    public static class Tools
    {
        public static class Color
        {
            public static UnityEngine.Color StringRGBAToColor(string rgba)
            {
                string values = rgba.Replace("RGBA(", "").Replace(")", "");
                string[] components = values.Split(',');

                if (!float.TryParse(components[0].Trim(), out float r))
                    throw new ArgumentException($"Could not parse RGBA value");

                if (!float.TryParse(components[1].Trim(), out float g))
                    throw new ArgumentException($"Could not parse RGBA value");

                if (!float.TryParse(components[2].Trim(), out float b))
                    throw new ArgumentException($"Could not parse RGBA value");

                if (!float.TryParse(components[3].Trim(), out float a))
                    throw new ArgumentException($"Could not parse RGBA value");

                return new UnityEngine.Color(r, g, b, a);
            }
        }

        public static class LegalStatus
        {
            public static ELegalStatus StringConvertToELegalStatus(string eLegalStatus)
            {
                if (Enum.TryParse<ELegalStatus>(eLegalStatus, out ELegalStatus result))
                {
                    return result;
                }
                else
                {
                    throw new ArgumentException($"Error in converting ELegalStatus.");
                }
            }
        }

        public static class Rank
        {
            public static FullRank StringConvertToFullRank(string pRank)
            {
                string[] rankData = pRank.Split(' ');

                if (rankData.Length != 2)
                {
                    throw new ArgumentException("Error in amount of arguments for FullRank Convet.");
                }

                string rank = rankData[0].Trim();
                string level = rankData[1].Trim();

                if (!Enum.TryParse<ERank>(rank, out ERank resultRank))
                {
                    throw new ArgumentException($"Error in converting FullRank Rank.");
                }

                int resultLevel = 1;
                switch (level)
                {
                    case "I":
                        resultLevel = 1;
                        break;
                    case "II":
                        resultLevel = 2;
                        break;
                    case "III":
                        resultLevel = 3;
                        break;
                    case "IV":
                        resultLevel = 4;
                        break;
                    case "V":
                        resultLevel = 5;
                        break;
                    default:
                        throw new ArgumentException("Error in converting FullRank Tier. Was not a");
                }

                return new FullRank { Rank = resultRank, Tier = resultLevel };
            }
        }
    }
}
