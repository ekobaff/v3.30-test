// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.MissionsXml
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

namespace PointBlank.Core.Xml
{
  public class MissionsXml
  {
    public static uint _missionPage1;
    public static uint _missionPage2;
    private static List<MissionModel> Missions = new List<MissionModel>();

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_cards ORDER BY mission_id ASC";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            bool boolean = ((DbDataReader) npgsqlDataReader).GetBoolean(2);
            MissionModel missionModel = new MissionModel()
            {
              id = ((DbDataReader) npgsqlDataReader).GetInt32(0),
              price = ((DbDataReader) npgsqlDataReader).GetInt32(1)
            };
            uint num1 = (uint) (1 << missionModel.id);
            int num2 = (int) Math.Ceiling((double) missionModel.id / 32.0);
            if (boolean)
            {
              if (num2 == 1)
                MissionsXml._missionPage1 += num1;
              else if (num2 == 2)
                MissionsXml._missionPage2 += num1;
            }
            MissionsXml.Missions.Add(missionModel);
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

    public static int GetMissionPrice(int id)
    {
      for (int index = 0; index < MissionsXml.Missions.Count; ++index)
      {
        MissionModel mission = MissionsXml.Missions[index];
        if (mission.id == id)
          return mission.price;
      }
      return -1;
    }
  }
}
