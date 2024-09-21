// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.PosRotation
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class PosRotation
  {
    public static PosRotationInfo ReadInfo(ReceivePacket p, bool genLog)
    {
      PosRotationInfo posRotationInfo = new PosRotationInfo()
      {
        RotationX = p.readUH(),
        RotationY = p.readUH(),
        RotationZ = p.readUH(),
        CameraX = p.readUH(),
        CameraY = p.readUH(),
        Area = p.readUH()
      };
      if (!genLog)
        ;
      return posRotationInfo;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      PosRotationInfo info = PosRotation.ReadInfo(p, genLog);
      PosRotation.WriteInfo(s, info);
    }

    public static void WriteInfo(SendPacket s, PosRotationInfo info)
    {
      s.writeH(info.RotationX);
      s.writeH(info.RotationY);
      s.writeH(info.RotationZ);
      s.writeH(info.CameraX);
      s.writeH(info.CameraY);
      s.writeH(info.Area);
    }
  }
}
