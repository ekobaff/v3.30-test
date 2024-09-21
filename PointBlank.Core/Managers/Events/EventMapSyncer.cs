// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventMapSyncer
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers.Events
{
  public class EventMapSyncer
  {
    private static List<EventMapModel> _events = new List<EventMapModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_mapbonus";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            EventMapModel eventMapModel = new EventMapModel()
            {
              _startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(0),
              _endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1),
              _mapId = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              _stageType = ((DbDataReader) npgsqlDataReader).GetInt32(3),
              _percentXp = ((DbDataReader) npgsqlDataReader).GetInt32(4),
              _percentGp = ((DbDataReader) npgsqlDataReader).GetInt32(5)
            };
            EventMapSyncer._events.Add(eventMapModel);
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
      EventMapSyncer._events.Clear();
      EventMapSyncer.GenerateList();
    }

    public static EventMapModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventMapSyncer._events.Count; ++index)
        {
          EventMapModel runningEvent = EventMapSyncer._events[index];
          if (runningEvent._startDate <= num && num < runningEvent._endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (EventMapModel) null;
    }

    public static bool EventIsValid(EventMapModel ev, int map, int stageType) => ev != null && (ev._mapId == map || ev._stageType == stageType);
  }
}
