// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.GetWeaponForClient
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Configs;
using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class GetWeaponForClient
  {
    public static WeaponClient ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
    {
      WeaponClient weaponClient = new WeaponClient()
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
        Logger.warning("Slot: " + ac.Slot.ToString() + " WeaponId: " + weaponClient.WeaponId.ToString());
      return weaponClient;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(8);

    public static void WriteInfo(SendPacket s, WeaponClient info)
    {
      s.writeC(info.WeaponFlag);
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
      info = (WeaponClient) null;
    }
  }
}
