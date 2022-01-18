using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;

#if DEBUG
using UnityEngine;
using System;
#endif

namespace GammaMod
{
    [BepInPlugin("ColdSpirit.GammaMod", "GammaMod", "0.1")]
    [BepInProcess("valheim.exe")]
    public class GammaModSetup : BaseUnityPlugin
    {
        readonly Harmony harmony = new Harmony("ColdSpirit.GammaMod");


        // CONFIG

        static ConfigEntry<bool> modEnabledConfig;
        public static bool ModEnabled
        {
            get => modEnabledConfig.Value;
            private set => modEnabledConfig.Value = value;
        }


        static ConfigEntry<float> minimalColorValueConfig;
        public static float MinimalColorValue
        {
            get => minimalColorValueConfig.Value;
            private set => minimalColorValueConfig.Value = value;
        }


        private void Awake()
        {
            modEnabledConfig = Config.Bind(
                "General",
                "ModEnabled",
                true,
                "In-game state of mod"
            );

            minimalColorValueConfig = Config.Bind(
                "General",
                "MinimalColorValue",
                0.6f,
                "All colors will be corrected with that value before pass to RenderSettings.ambientLight. Recommended values in range 0.5-0.6."
            );

            harmony.PatchAll();
        }


        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }


#if DEBUG
        static float colorValueStep = 0.05f;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                ModEnabled = !ModEnabled;
                Debug.Log($"GammaMod {(ModEnabled ? "Enabled" : "Disabled")}");
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                MinimalColorValue -= colorValueStep;
                MinimalColorValue = (float)Math.Round(MinimalColorValue, 2);
                Debug.Log(MinimalColorValue);
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                MinimalColorValue += colorValueStep;
                MinimalColorValue = (float)Math.Round(MinimalColorValue, 2);
                Debug.Log(MinimalColorValue);
            }
        }
#endif
    }
}
