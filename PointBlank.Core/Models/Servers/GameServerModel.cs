// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Servers.GameServerModel
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System.Net;

namespace PointBlank.Core.Models.Servers
{
  public class GameServerModel
  {
    public int _state;
    public int _id;
    public int _type;
    public int _LastCount;
    public int _maxPlayers;
    public string _ip;
    public ushort _port;
    public ushort _syncPort;
    public IPEndPoint Connection;

    public GameServerModel(string ip, ushort syncPort)
    {
      this._ip = ip;
      this._syncPort = syncPort;
      this.Connection = new IPEndPoint(IPAddress.Parse(ip), (int) syncPort);
    }
  }
}
