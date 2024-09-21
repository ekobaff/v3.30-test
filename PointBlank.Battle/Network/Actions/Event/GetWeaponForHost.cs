// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.GetWeaponForHost
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class GetWeaponForHost
  {
    public static WeaponHost ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
    {
      WeaponHost weaponHost = new WeaponHost()
      {
        DeathType = p.readC(),
        HitPart = p.readC(),
        X = p.readUH(),
        Y = p.readUH(),
        Z = p.readUH(),
        WeaponId = p.readD()
      };
      if (genLog)
        Logger.warning("Slot: " + ac.Slot.ToString() + " Type: " + weaponHost.DeathType.ToString() + " Hit: " + weaponHost.HitPart.ToString() + " X: " + weaponHost.X.ToString() + " Y: " + weaponHost.Y.ToString() + " Z: " + weaponHost.Z.ToString() + " WeaponId: " + weaponHost.WeaponId.ToString());
      return weaponHost;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(13);

    public static void WriteInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      WeaponHost info = GetWeaponForHost.ReadInfo(ac, p, genLog);
      GetWeaponForHost.WriteInfo(s, info);
    }

    public static void WriteInfo(SendPacket s, WeaponHost info)
    {
      s.writeC(info.DeathType);
      s.writeC(info.HitPart);
      s.writeH(info.X);
      s.writeH(info.Y);
      s.writeH(info.Z);
      s.writeD(info.WeaponId);
    }
  }
}
