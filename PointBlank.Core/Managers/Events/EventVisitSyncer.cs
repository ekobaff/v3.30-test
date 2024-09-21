// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventVisitSyncer
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers.Events
{
  public class EventVisitSyncer
  {
    private static List<EventVisitModel> _events = new List<EventVisitModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_visit";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            EventVisitModel eventVisitModel = new EventVisitModel()
            {
              id = ((DbDataReader) npgsqlDataReader).GetInt32(0),
              startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1),
              endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(2),
              title = ((DbDataReader) npgsqlDataReader).GetString(3),
              checks = ((DbDataReader) npgsqlDataReader).GetInt32(4)
            };
            string str1 = ((DbDataReader) npgsqlDataReader).GetString(5);
            string str2 = ((DbDataReader) npgsqlDataReader).GetString(6);
            string str3 = ((DbDataReader) npgsqlDataReader).GetString(7);
            string str4 = ((DbDataReader) npgsqlDataReader).GetString(8);
            string[] strArray1 = str1.Split(',');
            string[] strArray2 = str3.Split(',');
            for (int index = 0; index < strArray1.Length; ++index)
              eventVisitModel.box.Add(new VisitBox()
              {
                reward1 = {
                  good_id = int.Parse(strArray1[index])
                }
              });
            for (int index = 0; index < strArray2.Length; ++index)
              eventVisitModel.box[index].reward2.good_id = int.Parse(strArray2[index]);
            string[] strArray3 = str2.Split(',');
            string[] strArray4 = str4.Split(',');
            for (int index = 0; index < strArray3.Length; ++index)
              eventVisitModel.box[index].reward1.SetCount(strArray3[index]);
            for (int index = 0; index < strArray4.Length; ++index)
              eventVisitModel.box[index].reward2.SetCount(strArray4[index]);
            eventVisitModel.SetBoxCounts(strArray1.Length);
            EventVisitSyncer._events.Add(eventVisitModel);
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
      EventVisitSyncer._events.Clear();
      EventVisitSyncer.GenerateList();
    }

    public static EventVisitModel getEvent(int eventId)
    {
      try
      {
        for (int index = 0; index < EventVisitSyncer._events.Count; ++index)
        {
          EventVisitModel eventVisitModel = EventVisitSyncer._events[index];
          if (eventVisitModel.id == eventId)
            return eventVisitModel;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (EventVisitModel) null;
    }

    public static EventVisitModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventVisitSyncer._events.Count; ++index)
        {
          EventVisitModel runningEvent = EventVisitSyncer._events[index];
          if (runningEvent.startDate <= num && num < runningEvent.endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (EventVisitModel) null;
    }

    public static void ResetPlayerEvent(long pId, int eventId)
    {
      if (pId == 0L)
        return;
      ComDiv.updateDB("player_events", "player_id", (object) pId, new string[4]
      {
        "last_visit_event_id",
        "last_visit_sequence1",
        "last_visit_sequence2",
        "next_visit_date"
      }, (object) eventId, (object) 0, (object) 0, (object) 0);
    }
  }
}
