// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.MissionAwardsXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Mission;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Xml
{
  public class MissionAwardsXml
  {
    private static List<MissionAwards> _awards = new List<MissionAwards>();

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_cards_awards WHERE id > 0;";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            MissionAwardsXml._awards.Add(new MissionAwards(((DbDataReader) npgsqlDataReader).GetInt32(0), ((DbDataReader) npgsqlDataReader).GetInt32(1), ((DbDataReader) npgsqlDataReader).GetInt32(2), ((DbDataReader) npgsqlDataReader).GetInt32(3)));
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

    public static MissionAwards getAward(int mission)
    {
      lock (MissionAwardsXml._awards)
      {
        for (int index = 0; index < MissionAwardsXml._awards.Count; ++index)
        {
          MissionAwards award = MissionAwardsXml._awards[index];
          if (award._id == mission)
            return award;
        }
        return (MissionAwards) null;
      }
    }
  }
}
