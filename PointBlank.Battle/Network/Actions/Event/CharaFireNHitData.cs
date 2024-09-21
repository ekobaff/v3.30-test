// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.CharaFireNHitData
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.Event;
using System;
using System.Collections.Generic;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class CharaFireNHitData
  {
    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(17 * num);
    }

    public static List<CharaFireNHitDataInfo> ReadInfo(
      ReceivePacket p,
      bool genLog)
    {
      List<CharaFireNHitDataInfo> fireNhitDataInfoList = new List<CharaFireNHitDataInfo>();
      int num = (int) p.readC();
      for (int index = 0; index < num; ++index)
      {
        CharaFireNHitDataInfo fireNhitDataInfo = new CharaFireNHitDataInfo()
        {
          HitInfo = p.readUD(),
          Extensions = p.readC(),
          WeaponId = p.readD(),
          Unk = p.readUH(),
          X = p.readUH(),
          Y = p.readUH(),
          Z = p.readUH()
        };
        if (genLog)
        {
          Logger.warning("X: " + fireNhitDataInfo.X.ToString() + " Y: " + fireNhitDataInfo.Y.ToString() + " Z: " + fireNhitDataInfo.Z.ToString());
          Logger.warning("[" + index.ToString() + "] Hit: " + BitConverter.ToString(p.getBuffer()));
        }
        fireNhitDataInfoList.Add(fireNhitDataInfo);
      }
      return fireNhitDataInfoList;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<CharaFireNHitDataInfo> fireNhitDataInfoList = CharaFireNHitData.ReadInfo(p, genLog);
      s.writeC((byte) fireNhitDataInfoList.Count);
      for (int index = 0; index < fireNhitDataInfoList.Count; ++index)
      {
        CharaFireNHitDataInfo fireNhitDataInfo = fireNhitDataInfoList[index];
        s.writeD(fireNhitDataInfo.HitInfo);
        s.writeC(fireNhitDataInfo.Extensions);
        s.writeD(fireNhitDataInfo.WeaponId);
        s.writeH(fireNhitDataInfo.Unk);
        s.writeH(fireNhitDataInfo.X);
        s.writeH(fireNhitDataInfo.Y);
        s.writeH(fireNhitDataInfo.Z);
      }
    }
  }
}
