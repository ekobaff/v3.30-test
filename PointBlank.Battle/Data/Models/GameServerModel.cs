// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.GameServerModel
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using System.Net;

namespace PointBlank.Battle.Data.Models
{
  public class GameServerModel
  {
    public int _state;
    public int _id;
    public int _type;
    public int _lastCount;
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
