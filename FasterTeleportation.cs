using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Diagnostics;

namespace Faster_Teleportation
{
    [BepInPlugin("com.lvh-it.valheim.fasterteleportation", "Faster Teleportation", "0.3.1.0")]
    [BepInProcess("valheim.exe")]
    public class FasterTeleportation : BaseUnityPlugin
    {
        private static bool timerStarted = false;
        private static Stopwatch stopwatch = new Stopwatch();
        private static ManualLogSource logger;
        private static bool playerIsTeleporting = false;

        void Main()
        {
            logger = Logger;

            Harmony.CreateAndPatchAll(typeof(FasterTeleportation));
            Logger.LogInfo("Patched Teleport Timer");
        }

        [HarmonyPatch(typeof(Player), "UpdateTeleport")]
        [HarmonyPrefix]
        static void patchTeleportTimer(ref bool ___m_teleporting, ref float ___m_teleportTimer, ref float dt)
        {
            playerIsTeleporting = ___m_teleporting;

            if (___m_teleporting && !timerStarted)
            {
                stopwatch.Start();
                timerStarted = true;
            }
            else if (!___m_teleporting && timerStarted)
            {
                stopwatch.Stop();
                logger.LogInfo("Teleporting took "+ stopwatch.ElapsedMilliseconds + " ms");
                stopwatch.Reset();
                timerStarted = false;
            }

            dt *= 3;
            if (___m_teleportTimer > 0f && ___m_teleportTimer < 2f)
            {
                ___m_teleportTimer = 2f;
            }
        }

        [HarmonyPatch(typeof(Hud), "UpdateBlackScreen")]
        [HarmonyPrefix]
        static void patchUpdateBlackScreen(ref UnityEngine.CanvasGroup ___m_loadingScreen)
        {
            if(playerIsTeleporting)
            {
                ___m_loadingScreen.alpha = 1f;
            }
        }
    }
}
