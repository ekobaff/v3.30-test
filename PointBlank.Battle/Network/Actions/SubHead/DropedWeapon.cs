// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.SubHead.DropedWeapon
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.SubHead;
using SharpDX;
using System;

namespace PointBlank.Battle.Network.Actions.SubHead
{
  public class DropedWeapon
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(15);

    public static DropedWeaponInfo ReadInfo(ReceivePacket p, bool genLog)
    {
      DropedWeaponInfo dropedWeaponInfo = new DropedWeaponInfo()
      {
        WeaponFlag = p.readC(),
        X = p.readUH(),
        Y = p.readUH(),
        Z = p.readUH(),
        Unk1 = p.readUH(),
        Unk2 = p.readUH(),
        Unk3 = p.readUH(),
        Unk4 = p.readUH()
      };
      if (genLog)
      {
        Vector3 vector3 = (Vector3) new Half3(dropedWeaponInfo.X, dropedWeaponInfo.Y, dropedWeaponInfo.Z);
        Logger.warning("[WeaponSync] " + BitConverter.ToString(p.getBuffer()));
        Logger.warning("[WeaponSync] Flag: " + dropedWeaponInfo.WeaponFlag.ToString());
        Logger.warning("[WeaponSync] X: " + vector3.X.ToString() + " Y: " + vector3.Y.ToString() + " Z: " + vector3.Z.ToString());
      }
      return dropedWeaponInfo;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p) => s.writeB(DropedWeapon.ReadInfo(p));

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      DropedWeaponInfo dropedWeaponInfo = DropedWeapon.ReadInfo(p, genLog);
      s.writeC(dropedWeaponInfo.WeaponFlag);
      s.writeH(dropedWeaponInfo.X);
      s.writeH(dropedWeaponInfo.Y);
      s.writeH(dropedWeaponInfo.Z);
      s.writeH(dropedWeaponInfo.Unk1);
      s.writeH(dropedWeaponInfo.Unk2);
      s.writeH(dropedWeaponInfo.Unk3);
      s.writeH(dropedWeaponInfo.Unk4);
    }
  }
}
