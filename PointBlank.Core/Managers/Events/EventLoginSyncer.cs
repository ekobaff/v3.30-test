// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventLoginSyncer
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers.Events
{
  public class EventLoginSyncer
  {
    private static List<EventLoginModel> _events = new List<EventLoginModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_login";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            EventLoginModel eventLoginModel = new EventLoginModel()
            {
              startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(0),
              endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1),
              _rewardId = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              _count = ((DbDataReader) npgsqlDataReader).GetInt64(3)
            };
            eventLoginModel._category = ComDiv.GetItemCategory(eventLoginModel._rewardId);
            if (eventLoginModel._rewardId < 100000)
              Logger.error("Event with incorrect reward! [Id: " + eventLoginModel._rewardId.ToString() + "]");
            else
              EventLoginSyncer._events.Add(eventLoginModel);
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
      EventLoginSyncer._events.Clear();
      EventLoginSyncer.GenerateList();
    }

    public static EventLoginModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventLoginSyncer._events.Count; ++index)
        {
          EventLoginModel runningEvent = EventLoginSyncer._events[index];
          if (runningEvent.startDate <= num && num < runningEvent.endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (EventLoginModel) null;
    }
  }
}
