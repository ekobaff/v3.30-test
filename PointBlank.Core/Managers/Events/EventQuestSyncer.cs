// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventQuestSyncer
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
  public class EventQuestSyncer
  {
    private static List<QuestModel> _events = new List<QuestModel>();

    public static void GenerateList()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_events_quest";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            QuestModel questModel = new QuestModel()
            {
              startDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(0),
              endDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(1)
            };
            EventQuestSyncer._events.Add(questModel);
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
      EventQuestSyncer._events.Clear();
      EventQuestSyncer.GenerateList();
    }

    public static QuestModel getRunningEvent()
    {
      try
      {
        uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventQuestSyncer._events.Count; ++index)
        {
          QuestModel runningEvent = EventQuestSyncer._events[index];
          if (runningEvent.startDate <= num && num < runningEvent.endDate)
            return runningEvent;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return (QuestModel) null;
    }

    public static void ResetPlayerEvent(long pId, PlayerEvent pE)
    {
      if (pId == 0L)
        return;
      ComDiv.updateDB("player_events", "player_id", (object) pId, new string[2]
      {
        "last_quest_date",
        "last_quest_finish"
      }, (object) (long) pE.LastQuestDate, (object) pE.LastQuestFinish);
    }
  }
}
