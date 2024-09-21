// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.SubHead.GrenadeSync
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.SubHead;
using System;

namespace PointBlank.Battle.Network.Actions.SubHead
{
  public class GrenadeSync
  {
    public static GrenadeInfo ReadInfo(ReceivePacket p, bool genLog, bool OnlyBytes = false) => GrenadeSync.BaseReadInfo(p, OnlyBytes, genLog);

    private static GrenadeInfo BaseReadInfo(
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      GrenadeInfo grenadeInfo = new GrenadeInfo()
      {
        Extensions = p.readC(),
        WeaponId = p.readD(),
        BoomInfo = p.readUH(),
        ObjPos_X = p.readUH(),
        ObjPos_Y = p.readUH(),
        ObjPos_Z = p.readUH(),
        Unk1 = p.readUH(),
        Unk2 = p.readUH(),
        Unk3 = p.readUH(),
        GrenadesCount = p.readUH(),
        Unk4 = p.readUH(),
        Unk5 = p.readUH(),
        Unk6 = p.readUH()
      };
      if (genLog)
      {
        Logger.warning("[GrenadeSync] " + BitConverter.ToString(p.getBuffer()));
        Logger.warning("[GrenadeSync] WeaponId: " + grenadeInfo.WeaponId.ToString());
      }
      return grenadeInfo;
    }

    public static byte[] ReadInfo(ReceivePacket p) => p.readB(27);

    public static void WriteInfo(SendPacket s, ReceivePacket p) => s.writeB(GrenadeSync.ReadInfo(p));

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      GrenadeInfo grenadeInfo = GrenadeSync.ReadInfo(p, genLog, true);
      s.writeC(grenadeInfo.Extensions);
      s.writeD(grenadeInfo.WeaponId);
      s.writeH(grenadeInfo.BoomInfo);
      s.writeH(grenadeInfo.ObjPos_X);
      s.writeH(grenadeInfo.ObjPos_Y);
      s.writeH(grenadeInfo.ObjPos_Z);
      s.writeH(grenadeInfo.Unk1);
      s.writeH(grenadeInfo.Unk2);
      s.writeH(grenadeInfo.Unk3);
      s.writeH(grenadeInfo.GrenadesCount);
      s.writeH(grenadeInfo.Unk4);
      s.writeH(grenadeInfo.Unk5);
      s.writeH(grenadeInfo.Unk6);
    }
  }
}
