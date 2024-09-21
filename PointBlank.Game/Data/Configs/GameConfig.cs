// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Configs.GameConfig
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using System.Collections.Generic;
using System.Text;

namespace PointBlank.Game.Data.Configs
{
    public static class GameConfig
    {
        public static string passw;
        public static string gameIp;
        public static bool isTestMode;
        public static bool debugMode;
        public static bool winCashPerBattle;
        public static bool showCashReceiveWarn;
        public static bool EnableClassicRules;
        public static UdpState udpType;
        public static float maxClanPoints;
        public static int serverId;
        public static int configId;
        public static int ruleId;
        public static int maxBattleLatency;
        public static int maxRepeatLatency;
        public static int syncPort;
        public static int maxActiveClans;
        public static int minRankVote;
        public static int maxNickSize;
        public static int minNickSize;
        public static int maxBattleXP;
        public static int maxBattleGP;
        public static int maxBattleMY;
        public static int maxChannelPlayers;
        public static int gamePort;
        public static int minCreateGold;
        public static int minCreateRank;
        public static bool RewardPerBattle;


        public static void Load()
        {
            ConfigFile configFile1 = new ConfigFile("Config/Database.ini");
            Config.dbHost = configFile1.readString("Host", "localhost");
            Config.dbName = configFile1.readString("Name", "");
            Config.dbUser = configFile1.readString("User", "root");
            Config.dbPass = configFile1.readString("Pass", "");
            Config.dbPort = configFile1.readInt32("Port", 0);
            Config.EncodeText = Encoding.GetEncoding(configFile1.readInt32("EncodingPage", 0));
            ConfigFile configFile2 = new ConfigFile("Config/Game.ini");
            serverId = configFile2.readInt32("ServerId", -1);
            configId = configFile2.readInt32("ConfigId", 0);
            gameIp = configFile2.readString("GameIp", "127.0.0.1");
            gamePort = configFile2.readInt32("GamePort", 39190);
            syncPort = configFile2.readInt32("SyncPort", 0);
            debugMode = configFile2.readBoolean("Debug", true);
            isTestMode = configFile2.readBoolean("Test", true);
            winCashPerBattle = configFile2.readBoolean("WinCashPerBattle", true);
            showCashReceiveWarn = configFile2.readBoolean("ShowCashReceiveWarn", true);
            EnableClassicRules = configFile2.readBoolean("EnableClassicRules", false);
            minCreateRank = configFile2.readInt32("MinCreateRank", 15);
            minCreateGold = configFile2.readInt32("MinCreatePoint", 7500);
            maxClanPoints = configFile2.readFloat("MaxClanPoints", 0.0f);
            passw = configFile2.readString("Password", "");
            maxChannelPlayers = configFile2.readInt32("MaxChannelPlayers", 100);
            maxBattleXP = configFile2.readInt32("MaxBattleXP", 1000);
            maxBattleGP = configFile2.readInt32("MaxBattlePoint", 1000);
            maxBattleMY = configFile2.readInt32("MaxBattleMY", 1000);
            udpType = (UdpState)configFile2.readByte("UdpType", (byte)1);
            minNickSize = configFile2.readInt32("MinNickSize", 0);
            maxNickSize = configFile2.readInt32("MaxNickSize", 0);
            minRankVote = configFile2.readInt32("MinRankVote", 0);
            maxActiveClans = configFile2.readInt32("MaxActiveClans", 0);
            maxBattleLatency = configFile2.readInt32("MaxBattleLatency", 0);
            maxRepeatLatency = configFile2.readInt32("MaxRepeatLatency", 0);
            ruleId = configFile2.readInt32("RuleId", 0);
            RewardPerBattle = configFile2.readBoolean("RewardPerBattle", false);

        }
    }
}
