﻿using BepInEx;
using HarmonyLib;

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

        [HarmonyPatch(typeof(Player), "UpdateTeleport")]
        [HarmonyPrefix]
        static void patchTeleportTimer(ref float dt)
        {
            dt *= 3;
        }
    }
}
