using BepInEx;
using HarmonyLib;
using System.Diagnostics;

namespace Faster_Teleportation
{
    

    [BepInPlugin("com.lvh-it.valheim.fasterteleportation", "Faster Teleportation", "0.2.0.0")]
    [BepInProcess("valheim.exe")]
    public class FasterTeleportation : BaseUnityPlugin
    {
        void Main()
        {
            Harmony.CreateAndPatchAll(typeof(FasterTeleportation));
            Logger.LogInfo("Patched Teleport Timer");
        }

        bool timerStarted = false;
        Stopwatch stopwatch = new Stopwatch();
        static FasterTeleportation FT = new FasterTeleportation();

        [HarmonyPatch(typeof(Player), "UpdateTeleport")]
        [HarmonyPrefix]
        static void patchTeleportTimer(ref bool ___m_teleporting, ref float ___m_teleportTimer, ref float dt)
        {
            if(___m_teleporting)
            {
                if(!FT.timerStarted)
                {
                    FT.stopwatch.Start();
                    FT.timerStarted = true;
                }
            } 
            else
            {
                if(FT.timerStarted)
                {
                    FT.stopwatch.Stop();
                    FT.Logger.LogInfo("Teleporting took "+ FT.stopwatch.ElapsedMilliseconds + " ms");
                    FT.stopwatch.Reset();
                    FT.timerStarted = false;
                }
            }

            dt *= 3;
            if (___m_teleportTimer > 0f && ___m_teleportTimer <= 2f)
            {
                ___m_teleportTimer = 3f;
            }
        }
    }
}
