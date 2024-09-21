// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.ClanRankXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Rank;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Xml
{
  public class ClanRankXml
  {
    private static List<RankModel> _ranks = new List<RankModel>();

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_clan_ranks;";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            ClanRankXml._ranks.Add(new RankModel(((DbDataReader) npgsqlDataReader).GetInt32(0), ((DbDataReader) npgsqlDataReader).GetInt32(1), 0, ((DbDataReader) npgsqlDataReader).GetInt32(2)));
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

    public static RankModel getRank(int rankId)
    {
      lock (ClanRankXml._ranks)
      {
        for (int index = 0; index < ClanRankXml._ranks.Count; ++index)
        {
          RankModel rank = ClanRankXml._ranks[index];
          if (rank._id == rankId)
            return rank;
        }
        return (RankModel) null;
      }
    }
  }
}
