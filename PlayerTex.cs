using MelonLoader;
using HarmonyLib;
using UnityEngine;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(Player_Textures.PlayerTex), "Player Textures", "1.0.0", "Lilly", null)]
[assembly: MelonGame("KisSoft", "ATLYSS")]

namespace Player_Textures
{
    public class PlayerTex : MelonMod
    {
        private byte[] Bytes;
        static PlayerTex instance;
        public Player Localplayer;
        string[][] paths = new string[5][];

        public override void OnInitializeMelon()
        {
            instance = this;

            paths[0] = new string[4];
            paths[0][0] = "Imp/Head.png";
            paths[0][1] = "Imp/Chest.png";
            paths[0][2] = "Imp/Legs.png";
            paths[0][3] = "Imp/Tail.png";

            paths[1] = new string[4];
            paths[1][0] = "Poon/Head.png";
            paths[1][1] = "Poon/Chest.png";
            paths[1][2] = "Poon/Legs.png";
            paths[1][3] = "Poon/Tail.png";

            paths[2] = new string[4];
            paths[2][0] = "Kobold/Head.png";
            paths[2][1] = "Kobold/Chest.png";
            paths[2][2] = "Kobold/Legs.png";
            paths[2][3] = "Kobold/Tail.png";

            paths[3] = new string[4];
            paths[3][0] = "Byrdle/Head.png";
            paths[3][1] = "Byrdle/Chest.png";
            paths[3][2] = "Byrdle/Legs.png";
            paths[3][3] = "Byrdle/Tail.png";

            paths[4] = new string[4];
            paths[4][0] = "Chang/Head.png";
            paths[4][1] = "Chang/Chest.png";
            paths[4][2] = "Chang/Legs.png";
            paths[4][3] = "Chang/Tail.png";
        }

        public void reTex(string race)
        {
            PlayerRaceModel vis = Localplayer._pVisual._playerRaceModel;
            int raceInt = 0;
            MelonLogger.Msg(race);
            if (race == "imp")
                raceInt = 0;
            else if (race == "poon")
                raceInt = 1;
            else if (race == "kobold")
                raceInt = 2;
            else if (race == "byrdle")
                raceInt = 3;
            else if (race == "chang")
                raceInt = 4;

            Texture2D tex = null;


            for (int i = 0; i < paths.Length - 2; i++)
            {
                if (System.IO.File.Exists(MelonEnvironment.UserDataDirectory + "/Skins/" + paths[raceInt][i]))
                {
                    if(i == 3)
                    {
                        foreach (SkinnedMeshRenderer tail in vis._tailDisplays)
                        {
                            tex = tail.materials[0].mainTexture as Texture2D;
                            tex.LoadImage(System.IO.File.ReadAllBytes(MelonEnvironment.UserDataDirectory + "/Skins/" + paths[raceInt][i]));
                        }
                    }
                    else
                    {
                        tex = vis._baseBodyMesh.materials[i].mainTexture as Texture2D;
                        tex.LoadImage(System.IO.File.ReadAllBytes(MelonEnvironment.UserDataDirectory + "/Skins/" + paths[raceInt][i]));
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player), "OnGameConditionChange")]
        public static class playerspawn
        {
            private static void Postfix(ref Player __instance)
            {
                if(__instance._currentGameCondition == GameCondition.IN_GAME && __instance.isLocalPlayer)
                {
                    PlayerTex.instance.Localplayer = __instance;
                    PlayerTex.instance.reTex(__instance._pVisual._playerRaceModel._scriptablePlayerRace._raceName.ToLower());
                }
            }
        }
    }
}