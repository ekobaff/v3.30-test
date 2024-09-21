// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventXmasSyncer
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
  public class EventXmasSyncer
  {
    private static List<EventXmasModel> _events = new List<EventXmasModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_xmas";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            EventXmasModel eventXmasModel = new EventXmasModel()
            {
              startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(0),
              endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1)
            };
            EventXmasSyncer._events.Add(eventXmasModel);
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
      EventXmasSyncer._events.Clear();
      EventXmasSyncer.GenerateList();
    }

    public static EventXmasModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventXmasSyncer._events.Count; ++index)
        {
          EventXmasModel runningEvent = EventXmasSyncer._events[index];
          if (runningEvent.startDate <= num && num < runningEvent.endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (EventXmasModel) null;
    }
  }
}
