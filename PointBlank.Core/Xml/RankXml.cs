// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.RankXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Rank;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;

namespace PointBlank.Core.Xml
{
  public class RankXml
  {
    private static List<RankModel> _ranks = new List<RankModel>();
    private static SortedList<int, List<ItemsModel>> _awards = new SortedList<int, List<ItemsModel>>();

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_ranks;";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            RankXml._ranks.Add(new RankModel(((DbDataReader) npgsqlDataReader).GetInt32(0), ((DbDataReader) npgsqlDataReader).GetInt32(1), ((DbDataReader) npgsqlDataReader).GetInt32(2), ((DbDataReader) npgsqlDataReader).GetInt32(3)));
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
      lock (RankXml._ranks)
      {
        for (int index = 0; index < RankXml._ranks.Count; ++index)
        {
          RankModel rank = RankXml._ranks[index];
          if (rank._id == rankId)
            return rank;
        }
        return (RankModel) null;
      }
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream inStream = new FileStream(path, FileMode.Open))
      {
        if (inStream.Length == 0L)
        {
          Logger.error("File is empty: " + path);
        }
        else
        {
          try
          {
            xmlDocument.Load((Stream) inStream);
            for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
            {
              if ("List".Equals(xmlNode1.Name))
              {
                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                {
                  if ("Rank".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    RankXml._ranks.Add(new RankModel(int.Parse(attributes.GetNamedItem("Id").Value), int.Parse(attributes.GetNamedItem("NextLevel").Value), int.Parse(attributes.GetNamedItem("PointUp").Value), int.Parse(attributes.GetNamedItem("AllExp").Value)));
                    using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                    {
                      NpgsqlCommand command = npgsqlConnection.CreateCommand();
                      ((DbConnection) npgsqlConnection).Open();
                      ((DbCommand) command).CommandText = "INSERT INTO server_ranks VALUES (@Id, @NextLevel, @PointUp, @AllExp);";
                      command.Parameters.AddWithValue("Id", (object) int.Parse(attributes.GetNamedItem("Id").Value));
                      command.Parameters.AddWithValue("NextLevel", (object) int.Parse(attributes.GetNamedItem("NextLevel").Value));
                      command.Parameters.AddWithValue("PointUp", (object) int.Parse(attributes.GetNamedItem("PointUp").Value));
                      command.Parameters.AddWithValue("AllExp", (object) int.Parse(attributes.GetNamedItem("AllExp").Value));
                      ((DbCommand) command).CommandType = CommandType.Text;
                      ((DbCommand) command).ExecuteNonQuery();
                      ((Component) command).Dispose();
                      ((Component) npgsqlConnection).Dispose();
                      ((DbConnection) npgsqlConnection).Close();
                    }
                  }
                }
              }
            }
          }
          catch (XmlException ex)
          {
            Logger.warning(ex.ToString());
          }
        }
        inStream.Dispose();
        inStream.Close();
      }
    }

    public static List<ItemsModel> getAwards(int rank)
    {
      lock (RankXml._awards)
      {
        List<ItemsModel> awards;
        if (RankXml._awards.TryGetValue(rank, out awards))
          return awards;
      }
      return new List<ItemsModel>();
    }

    public static void LoadAwards()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_rank_awards ORDER BY rank_id ASC";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            RankXml.AddItemToList(((DbDataReader) npgsqlDataReader).GetInt32(0), new ItemsModel(((DbDataReader) npgsqlDataReader).GetInt32(1))
            {
              _name = ((DbDataReader) npgsqlDataReader).GetString(2),
              _count = ((DbDataReader) npgsqlDataReader).GetInt64(3),
              _equip = ((DbDataReader) npgsqlDataReader).GetInt32(4)
            });
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

    private static void AddItemToList(int rank, ItemsModel item)
    {
      if (RankXml._awards.ContainsKey(rank))
        RankXml._awards[rank].Add(item);
      else
        RankXml._awards.Add(rank, new List<ItemsModel>()
        {
          item
        });
    }
  }
}
