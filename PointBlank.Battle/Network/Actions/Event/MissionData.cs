// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.MissionData
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Enums;
using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Models.Event;
using PointBlank.Battle.Data.Sync;
using System;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class MissionData
  {
    public static MissionDataInfo ReadInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog,
      float time,
      bool OnlyBytes = false)
    {
      MissionDataInfo missionDataInfo = new MissionDataInfo()
      {
        Bomb = (int) p.readC(),
        PlantTime = p.readT()
      };
      if (!OnlyBytes)
      {
        missionDataInfo.BombEnum = (BOMB_FLAG) (missionDataInfo.Bomb & 15);
        missionDataInfo.BombId = missionDataInfo.Bomb >> 4;
      }
      if (genLog)
        Logger.warning("Slot: " + ac.Slot.ToString() + " Bomb: " + missionDataInfo.BombEnum.ToString() + " Id: " + missionDataInfo.BombId.ToString() + " PlantTime: " + missionDataInfo.PlantTime.ToString() + " Time: " + time.ToString());
      return missionDataInfo;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(5);

    public static void SendC4UseSync(Room room, Player pl, MissionDataInfo info)
    {
      if (pl == null)
        return;
      int type = info.BombEnum.HasFlag((Enum) BOMB_FLAG.DEFUSE) ? 1 : 0;
      BattleSync.SendBombSync(room, pl, type, info.BombId);
    }

    public static void WriteInfo(
      SendPacket s,
      ActionModel ac,
      ReceivePacket p,
      bool genLog,
      float pacDate,
      float plantDuration)
    {
      MissionDataInfo info = MissionData.ReadInfo(ac, p, genLog, pacDate);
      if ((double) info.PlantTime > 0.0 && (double) pacDate >= (double) info.PlantTime + (double) plantDuration && !info.BombEnum.HasFlag((Enum) BOMB_FLAG.STOP))
        info.Bomb += 2;
      MissionData.WriteInfo(s, info);
    }

    public static void WriteInfo(SendPacket s, MissionDataInfo info)
    {
      s.writeC((byte) info.Bomb);
      s.writeT(info.PlantTime);
    }
  }
}
