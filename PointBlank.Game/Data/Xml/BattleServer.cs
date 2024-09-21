// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Xml.BattleServer
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using System.Net;

namespace PointBlank.Game.Data.Xml
{
  public class BattleServer
  {
    public string IP;
    public int Port;
    public int SyncPort;
    public IPEndPoint Connection;

    public BattleServer(string ip, int syncPort)
    {
      this.IP = ip;
      this.SyncPort = syncPort;
      this.Connection = new IPEndPoint(IPAddress.Parse(ip), syncPort);
    }
  }
}
