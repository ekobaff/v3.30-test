// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.FireDataOnObject
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class FireDataOnObject
  {
    public static FireDataObjectInfo ReadInfo(ReceivePacket p, bool genLog) => new FireDataObjectInfo()
    {
      ShotId = p.readUH()
    };

    public static void ReadInfo(ReceivePacket p) => p.Advance(2);

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      FireDataObjectInfo fireDataObjectInfo = FireDataOnObject.ReadInfo(p, genLog);
      s.writeH(fireDataObjectInfo.ShotId);
    }
  }
}
