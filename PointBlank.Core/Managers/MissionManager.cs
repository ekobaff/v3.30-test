// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.MissionManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public class MissionManager
  {
    private static MissionManager acm = new MissionManager();

    public static MissionManager getInstance() => MissionManager.acm;

    public void addMissionDB(long player_id)
    {
      if (player_id == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) player_id);
          ((DbCommand) command).CommandText = "INSERT INTO player_missions (owner_id) VALUES (@owner)";
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public PlayerMissions getMission(
      long pId,
      int mission1,
      int mission2,
      int mission3,
      int mission4)
    {
      if (pId == 0L)
        return (PlayerMissions) null;
      PlayerMissions mission = (PlayerMissions) null;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) pId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_missions WHERE owner_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            mission = new PlayerMissions()
            {
              actualMission = ((DbDataReader) npgsqlDataReader).GetInt32(1),
              card1 = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              card2 = ((DbDataReader) npgsqlDataReader).GetInt32(3),
              card3 = ((DbDataReader) npgsqlDataReader).GetInt32(4),
              card4 = ((DbDataReader) npgsqlDataReader).GetInt32(5),
              mission1 = mission1,
              mission2 = mission2,
              mission3 = mission3,
              mission4 = mission4
            };
            ((DbDataReader) npgsqlDataReader).GetBytes(6, 0L, mission.list1, 0, 40);
            ((DbDataReader) npgsqlDataReader).GetBytes(7, 0L, mission.list2, 0, 40);
            ((DbDataReader) npgsqlDataReader).GetBytes(8, 0L, mission.list3, 0, 40);
            ((DbDataReader) npgsqlDataReader).GetBytes(9, 0L, mission.list4, 0, 40);
            mission.UpdateSelectedCard();
          }
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return mission;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        return (PlayerMissions) null;
      }
    }

    public void updateCurrentMissionList(long player_id, PlayerMissions mission)
    {
      byte[] currentMissionList = mission.getCurrentMissionList();
      ComDiv.updateDB("player_missions", nameof (mission) + (mission.actualMission + 1).ToString(), (object) currentMissionList, "owner_id", (object) player_id);
    }
  }
}
