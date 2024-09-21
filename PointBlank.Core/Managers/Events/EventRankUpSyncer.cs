// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventRankUpSyncer
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
  public class EventRankUpSyncer
  {
    private static List<EventUpModel> _events = new List<EventUpModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_rankup";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            EventUpModel eventUpModel = new EventUpModel()
            {
              _startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(0),
              _endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1),
              _percentXp = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              _percentGp = ((DbDataReader) npgsqlDataReader).GetInt32(3)
            };
            EventRankUpSyncer._events.Add(eventUpModel);
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
      EventRankUpSyncer._events.Clear();
      EventRankUpSyncer.GenerateList();
    }

    public static EventUpModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventRankUpSyncer._events.Count; ++index)
        {
          EventUpModel runningEvent = EventRankUpSyncer._events[index];
          if (runningEvent._startDate <= num && num < runningEvent._endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (EventUpModel) null;
    }
  }
}
