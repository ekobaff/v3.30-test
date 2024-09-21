// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Data.Configs.AuthConfig
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointBlank.Auth.Data.Configs
{
  public static class AuthConfig
  {
    public static string authIp;
    public static bool isTestMode;
    public static bool Outpost;
    public static bool AUTO_ACCOUNTS;
    public static bool debugMode;
    public static bool ClearToken;
    public static int syncPort;
    public static int configId;
    public static int maxNickSize;
    public static int minNickSize;
    public static int minTokenSize;
    public static int authPort;
    public static int serverId;
    public static int maxChannelPlayers;
    public static ulong LauncherKey;
    public static List<ClientLocale> GameLocales;

    public static void Load()
    {
      ConfigFile configFile1 = new ConfigFile("Config/Database.ini");
      Config.dbHost = configFile1.readString("Host", "localhost");
      Config.dbName = configFile1.readString("Name", "");
      Config.dbUser = configFile1.readString("User", "root");
      Config.dbPass = configFile1.readString("Pass", "");
      Config.dbPort = configFile1.readInt32("Port", 0);
      Config.EncodeText = Encoding.GetEncoding(configFile1.readInt32("EncodingPage", 0));
      ConfigFile configFile2 = new ConfigFile("Config/Auth.ini");
      AuthConfig.configId = configFile2.readInt32("ConfigId", 0);
      AuthConfig.serverId = configFile2.readInt32("ServerId", -1);
      AuthConfig.authIp = configFile2.readString("AuthIp", "127.0.0.1");
      AuthConfig.authPort = configFile2.readInt32("AuthPort", 39190);
      AuthConfig.syncPort = configFile2.readInt32("SyncPort", 0);
      AuthConfig.AUTO_ACCOUNTS = configFile2.readBoolean("AutoAccounts", false);
      AuthConfig.debugMode = configFile2.readBoolean("Debug", true);
      AuthConfig.isTestMode = configFile2.readBoolean("Test", true);
      AuthConfig.maxChannelPlayers = configFile2.readInt32("MaxChannelPlayers", 100);
      AuthConfig.Outpost = configFile2.readBoolean("Outpost", false);
      AuthConfig.LauncherKey = configFile2.readUInt64("LauncherKey", 0UL);
      AuthConfig.minNickSize = configFile2.readInt32("MinNickSize", 0);
      AuthConfig.maxNickSize = configFile2.readInt32("MaxNickSize", 0);
      AuthConfig.minTokenSize = configFile2.readInt32("MinTokenSize", 0);
      AuthConfig.ClearToken = configFile2.readBoolean("ClearToken", true);
      AuthConfig.GameLocales = new List<ClientLocale>();
      string str1 = configFile2.readString("GameLocales", "None");
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        ClientLocale result;
        Enum.TryParse<ClientLocale>(str2, out result);
        AuthConfig.GameLocales.Add(result);
      }
    }
  }
}
