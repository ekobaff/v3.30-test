// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.GrenadeHit
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data;
using PointBlank.Battle.Data.Enums;
using PointBlank.Battle.Data.Models.Event;
using SharpDX;
using System.Collections.Generic;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class GrenadeHit
  {
    public static List<GrenadeHitInfo> ReadInfo(
      ReceivePacket p,
      bool genLog,
      bool OnlyBytes = false)
    {
      return GrenadeHit.BaseReadInfo(p, OnlyBytes, genLog);
    }

    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(32 * num);
    }

    private static List<GrenadeHitInfo> BaseReadInfo(
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      List<GrenadeHitInfo> grenadeHitInfoList = new List<GrenadeHitInfo>();
      int num1 = (int) p.readC();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        GrenadeHitInfo grenadeHitInfo = new GrenadeHitInfo()
        {
          HitInfo = p.readUD(),
          BoomInfo = p.readUH(),
          PlayerPos = p.readUHVector(),
          Extensions = p.readC(),
          WeaponId = p.readD(),
          DeathType = p.readC(),
          FirePos = p.readUHVector(),
          HitPos = p.readUHVector(),
          GrenadesCount = p.readUH()
        };
        if (!OnlyBytes)
        {
          grenadeHitInfo.HitEnum = (HIT_TYPE) AllUtils.getHitHelmet(grenadeHitInfo.HitInfo);
          if (grenadeHitInfo.BoomInfo > (ushort) 0)
          {
            grenadeHitInfo.BoomPlayers = new List<int>();
            for (int index2 = 0; index2 < 16; ++index2)
            {
              int num2 = 1 << index2;
              if (((int) grenadeHitInfo.BoomInfo & num2) == num2)
                grenadeHitInfo.BoomPlayers.Add(index2);
            }
          }
          grenadeHitInfo.WeaponClass = (CLASS_TYPE) AllUtils.getIdStatics(grenadeHitInfo.WeaponId, 2);
        }
        if (genLog)
        {
          string[] strArray1 = new string[6];
          strArray1[0] = "[Player Postion] X: ";
          Half half = grenadeHitInfo.FirePos.X;
          strArray1[1] = half.ToString();
          strArray1[2] = "; Y: ";
          half = grenadeHitInfo.FirePos.Y;
          strArray1[3] = half.ToString();
          strArray1[4] = "; Z: ";
          half = grenadeHitInfo.FirePos.Z;
          strArray1[5] = half.ToString();
          Logger.warning(string.Concat(strArray1));
          string[] strArray2 = new string[6];
          strArray2[0] = "[Object Postion] X: ";
          half = grenadeHitInfo.HitPos.X;
          strArray2[1] = half.ToString();
          strArray2[2] = "; Y: ";
          half = grenadeHitInfo.HitPos.Y;
          strArray2[3] = half.ToString();
          strArray2[4] = "; Z: ";
          half = grenadeHitInfo.HitPos.Z;
          strArray2[5] = half.ToString();
          Logger.warning(string.Concat(strArray2));
        }
        grenadeHitInfoList.Add(grenadeHitInfo);
      }
      return grenadeHitInfoList;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<GrenadeHitInfo> hits = GrenadeHit.ReadInfo(p, genLog, true);
      GrenadeHit.WriteInfo(s, hits);
    }

    public static void WriteInfo(SendPacket s, List<GrenadeHitInfo> hits)
    {
      s.writeC((byte) hits.Count);
      for (int index = 0; index < hits.Count; ++index)
      {
        GrenadeHitInfo hit = hits[index];
        s.writeD(hit.HitInfo);
        s.writeH(hit.BoomInfo);
        s.writeHVector(hit.PlayerPos);
        s.writeC(hit.Extensions);
        s.writeD(hit.WeaponId);
        s.writeC(hit.DeathType);
        s.writeHVector(hit.FirePos);
        s.writeHVector(hit.HitPos);
        s.writeH(hit.GrenadesCount);
      }
    }
  }
}
