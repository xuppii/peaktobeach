using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Zorro.Core;
using UnityEngine;

[BepInPlugin("com.yourname.peakmod", "My Peak Mod", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Log;
    void Awake()
    {
        Log = Logger;
        var harmony = new Harmony("com.yourname.peakmod");
        harmony.PatchAll();
        Logger.LogInfo("My Peak Mod loaded!");
    }

}
[HarmonyPatch(typeof(MapHandler), "InitializeMap")]
class InitializeMapPatch
{
    static void Postfix()
    {
        Plugin.Log.LogInfo("=== InitializeMap was called ===");
        var array = Singleton<MapHandler>.Instance.segments;
        //for (int i = 0; i < array.Length; i++)
        //{
        //    Plugin.Log.LogInfo(i + ": " + array[i].segmentParent.name);
        //}
        array[0].segmentParent.SetActive(true);
        array[1].segmentParent.SetActive(true);
        array[2].segmentParent.SetActive(true);
        array[3].segmentParent.SetActive(true);
        array[4].segmentParent.SetActive(true);

        array[0].segmentCampfire.SetActive(true);
        array[1].segmentCampfire.SetActive(true);
        array[2].segmentCampfire.SetActive(true);
        array[3].segmentCampfire.SetActive(true);
        array[4].segmentCampfire.SetActive(true);

    }
}

[HarmonyPatch(typeof(CharacterSpawner), "SpawnMyPlayerCharacter")]

class SpawnMyPlayerCharacterPatch
{
    static void Prefix(ref Transform spawnOverride)
    {
        Plugin.Log.LogInfo("=== SpawnMyPlayerCharacter was called ===");
        GameObject flag = GameObject.Find("Flag_planted_seagull");
        if (flag != null)
        {

            spawnOverride = flag.transform;
            Plugin.Log.LogInfo("flag found at" + spawnOverride);
        }
        else
        {
            Plugin.Log.LogInfo("flag not found");
        }
    }
}

[HarmonyPatch(typeof(FogSphere), "Start")]
class DisableFogPatch
{
    static void Postfix(FogSphere __instance)
    {

        __instance.gameObject.SetActive(false);
        Debug.Log("FogSphere disabled");
    }
}

//bypass fog error checking
[HarmonyPatch(typeof(Ascents), "fogEnabled", MethodType.Getter)]
class FogEnabledPatch
{
    static bool Prefix(ref bool __result)
    {
        __result = false;
        return false; 
    }
}
[HarmonyPatch(typeof(OrbFogHandler), "Update")]
class OrbFogUpdatePatch
{
    static bool Prefix()
    {
        return false; 
    }
}