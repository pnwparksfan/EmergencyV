﻿namespace EmergencyV
{
    // System
    using System;
    using System.Linq;

    // RPH
    using Rage;
    using Rage.Native;

    public enum FirefighterRole
    {
        None = 0,
        Engine,
        Battalion,
        Rescue,
    }

    public enum PlayerStateType
    {
        Normal = 0,
        Firefighter = 1,
        EMS = 2,
    }

    internal class PlayerManager
    {
        private static PlayerManager instance;
        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerManager();
                return instance;
            }
        }

        private Model normalStateModel;

        private PlayerStateType playerState;
        public PlayerStateType PlayerState { get { return playerState; } }

        public bool IsEMS { get { return playerState == PlayerStateType.EMS; } }

        public bool IsFirefighter { get { return playerState == PlayerStateType.Firefighter; } }
        public FirefighterRole FirefighterRole { get; set; }

        private PlayerManager()
        {
            playerState = PlayerStateType.Normal;
            normalStateModel = Plugin.LocalPlayer.Model;
        }

        public void Update()
        {
        }

        public void SetPlayerToState(PlayerStateType type)
        {
            if (playerState == type)
                return;

            switch (type)
            {
                default:
                case PlayerStateType.Normal:
                    Plugin.LocalPlayer.Model = normalStateModel;
                    FirefighterRole = FirefighterRole.None;
                    break;

                case PlayerStateType.Firefighter:
                    normalStateModel = Plugin.LocalPlayer.Model;
                    Plugin.LocalPlayer.Model = Plugin.UserSettings.PEDS.FIREFIGHTER_MODEL;
                    FirefighterRole = FirefighterRole.Engine;
                    break;
                case PlayerStateType.EMS:
                    normalStateModel = Plugin.LocalPlayer.Model;
                    Plugin.LocalPlayer.Model = Plugin.UserSettings.PEDS.EMS_MODEL;
                    FirefighterRole = FirefighterRole.None;
                    break;
            }

            API.Functions.OnPlayerStateChanged(type, playerState);
            playerState = type;
        }
    }
}
