using System;
using UnityEngine;

namespace PongClone
{
    public class PlayerUtil
    {
        public static void InitializeForRightHandedness(GameObject leftPlayer, GameObject rightPlayer, Type opponentType, out BasePlayer me, out BasePlayer opponent)
        {
            me = rightPlayer.AddComponent<ManualPlayer>();
            me.id = 1;
            me.facing = Direction.Left;
            opponent = leftPlayer.AddComponent(opponentType) as BasePlayer;
            opponent.id = 0;
            opponent.facing = Direction.Right;
        }

        public static void InitializeForLeftHandedness(GameObject leftPlayer, GameObject rightPlayer, Type opponentType, out BasePlayer me, out BasePlayer opponent)
        {
            me = leftPlayer.AddComponent<ManualPlayer>();
            me.id = 0;
            me.facing = Direction.Right;
            opponent = rightPlayer.AddComponent(opponentType) as BasePlayer;
            opponent.id = 1;
            opponent.facing = Direction.Left;
        }
    }
}