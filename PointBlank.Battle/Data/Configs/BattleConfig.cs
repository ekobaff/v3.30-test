// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Configs.BattleConfig
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using System.Text;

namespace PointBlank.Battle.Data.Configs
{
  public static class BattleConfig
  {
    public static string dbName;
    public static string dbHost;
    public static string dbUser;
    public static string dbPass;
    public static string hosIp;
    public static string serverIp;
    public static string udpVersion;
    public static int dbPort;
    public static ushort hosPort;
    public static ushort maxDrop;
    public static ushort syncPort;
    public static bool isTestMode;
    public static bool sendInfoToServ;
    public static bool sendFailMsg;
    public static bool enableLog;
    public static bool useMaxAmmoInDrop;
    public static float plantDuration;
    public static float defuseDuration;
    public static Encoding EncodeText;

    public static void Load()
    {
      ConfigFile configFile1 = new ConfigFile("Config/Database.ini");
      BattleConfig.dbHost = configFile1.readString("Host", "localhost");
      BattleConfig.dbName = configFile1.readString("Name", "");
      BattleConfig.dbUser = configFile1.readString("User", "root");
      BattleConfig.dbPass = configFile1.readString("Pass", "");
      BattleConfig.dbPort = configFile1.readInt32("Port", 0);
      BattleConfig.EncodeText = Encoding.GetEncoding(configFile1.readInt32("EncodingPage", 0));
      ConfigFile configFile2 = new ConfigFile("Config/Battle.ini");
      BattleConfig.hosIp = configFile2.readString("UdpIp", "0.0.0.0");
      BattleConfig.serverIp = configFile2.readString("ServerIp", "0.0.0.0");
      BattleConfig.hosPort = configFile2.readUInt16("UdpPort", (ushort) 40000);
      BattleConfig.isTestMode = configFile2.readBoolean("Test", false);
      BattleConfig.sendInfoToServ = configFile2.readBoolean("SendInfoToServer", true);
      BattleConfig.sendFailMsg = configFile2.readBoolean("SendFailMsg", true);
      BattleConfig.enableLog = configFile2.readBoolean("EnableLog", false);
      BattleConfig.maxDrop = configFile2.readUInt16("MaxDrop", (ushort) 0);
      BattleConfig.syncPort = configFile2.readUInt16("SyncPort", (ushort) 0);
      BattleConfig.plantDuration = 5.5f;
      BattleConfig.defuseDuration = 6.5f;
      BattleConfig.useMaxAmmoInDrop = configFile2.readBoolean("UseMaxAmmoInDrop", false);
      BattleConfig.udpVersion = configFile2.readString("UdpVersion", "0.0");
    }
  }
}
