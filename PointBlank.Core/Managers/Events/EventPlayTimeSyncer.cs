// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventPlayTimeSyncer
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers.Events
{
  public class EventPlayTimeSyncer
  {
    private static List<PlayTimeModel> _events = new List<PlayTimeModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_playtime";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            PlayTimeModel playTimeModel = new PlayTimeModel()
            {
              _startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(0),
              _endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1),
              _title = ((DbDataReader) npgsqlDataReader).GetString(2),
              _time = ((DbDataReader) npgsqlDataReader).GetInt64(3),
              _goodReward1 = ((DbDataReader) npgsqlDataReader).GetInt32(4),
              _goodReward2 = ((DbDataReader) npgsqlDataReader).GetInt32(5),
              _goodCount1 = (long) ((DbDataReader) npgsqlDataReader).GetInt32(6),
              _goodCount2 = (long) ((DbDataReader) npgsqlDataReader).GetInt32(7)
            };
            EventPlayTimeSyncer._events.Add(playTimeModel);
          }
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static void ReGenList()
    {
      EventPlayTimeSyncer._events.Clear();
      EventPlayTimeSyncer.GenerateList();
    }

    public static PlayTimeModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventPlayTimeSyncer._events.Count; ++index)
        {
          PlayTimeModel runningEvent = EventPlayTimeSyncer._events[index];
          if (runningEvent._startDate <= num && num < runningEvent._endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (PlayTimeModel) null;
    }

    public static void ResetPlayerEvent(long pId, PlayerEvent pE)
    {
      if (pId == 0L)
        return;
      ComDiv.updateDB("player_events", "player_id", (object) pId, new string[3]
      {
        "last_playtime_value",
        "last_playtime_finish",
        "last_playtime_date"
      }, (object) pE.LastPlaytimeValue, (object) pE.LastPlaytimeFinish, (object) (long) pE.LastPlaytimeDate);
    }
  }
}
