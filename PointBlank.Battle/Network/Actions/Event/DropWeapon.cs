// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.DropWeapon
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Configs;
using PointBlank.Battle.Data.Models.Event;
using System;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class DropWeapon
  {
    public static DropWeaponInfo ReadInfo(ReceivePacket p, bool genLog)
    {
      DropWeaponInfo dropWeaponInfo = new DropWeaponInfo()
      {
        WeaponFlag = p.readC(),
        WeaponId = p.readD(),
        Extensions = p.readC(),
        AmmoPrin = p.readUH(),
        AmmoDual = p.readUH(),
        AmmoTotal = p.readUH(),
        Unk1 = p.readUH(),
        Unk2 = p.readD()
      };
      if (genLog)
      {
        Logger.warning("[ActionBuffer]: " + BitConverter.ToString(p.getBuffer()));
        Logger.warning("[DropWeapon] WeaponId: " + dropWeaponInfo.WeaponId.ToString());
      }
      return dropWeaponInfo;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(8);

    public static void WriteInfo(SendPacket s, DropWeaponInfo info, int count)
    {
      s.writeC((byte) ((uint) info.WeaponFlag + (uint) count));
      s.writeD(info.WeaponId);
      s.writeC(info.Extensions);
      if (BattleConfig.useMaxAmmoInDrop)
      {
        s.writeH(ushort.MaxValue);
        s.writeH(info.AmmoDual);
        s.writeH((short) 10000);
      }
      else
      {
        s.writeH(info.AmmoPrin);
        s.writeH(info.AmmoDual);
        s.writeH(info.AmmoTotal);
      }
      s.writeH(info.Unk1);
      s.writeD(info.Unk2);
      info = (DropWeaponInfo) null;
    }
  }
}
