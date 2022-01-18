using HarmonyLib;
using UnityEngine;

namespace GammaMod
{
    [HarmonyPatch]
    public class Patch
    {
        static Color originalColor;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RenderSettings), "set_ambientLight")]
        static void SetLight(ref Color value)
        {
            if (!GammaModSetup.ModEnabled) return;

            originalColor = value;
            value = LightenColor(value);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RenderSettings), "get_ambientLight")]
        static void GetLight(ref Color __result)
        {
            if (!GammaModSetup.ModEnabled) return;

            __result = originalColor;
        }

        static Color LightenColor(Color sourceColor)
        {
            float avgColor = (sourceColor.r + sourceColor.g + sourceColor.b) / 3;

            if (GammaModSetup.MinimalColorValue > avgColor)
            {
                sourceColor *= GammaModSetup.MinimalColorValue / avgColor;
            }

            return sourceColor;
        }
    }
}