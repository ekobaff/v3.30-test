using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Rank;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PointBlank.Core.Sql;

namespace PointBlank.Core.Xml
{
    public class RankedXml
    { 
            private static List<RankedModel> RankedRank = new List<RankedModel>();
    private static SortedList<int, List<ItemsModel>> RankedAwards = new SortedList<int, List<ItemsModel>>();

    public static void Load()
    {
        try
        {
            using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
            {
                NpgsqlCommand command = npgsqlConnection.CreateCommand();
                ((DbConnection)npgsqlConnection).Open();
                ((DbCommand)command).CommandText = "SELECT * FROM server_rankeds;";
                ((DbCommand)command).CommandType = CommandType.Text;
                NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
                while (((DbDataReader)npgsqlDataReader).Read())
                        RankedXml.RankedRank.Add(new RankedModel(((DbDataReader)npgsqlDataReader).GetInt32(0), ((DbDataReader)npgsqlDataReader).GetInt32(1), ((DbDataReader)npgsqlDataReader).GetInt32(2), ((DbDataReader)npgsqlDataReader).GetInt32(3), ((DbDataReader)npgsqlDataReader).GetInt32(4)));
                ((Component)command).Dispose();
                ((DbDataReader)npgsqlDataReader).Close();
                ((Component)npgsqlConnection).Dispose();
                ((DbConnection)npgsqlConnection).Close();
            }
        }
        catch (Exception ex)
        {
            Logger.error(ex.ToString());
        }
    }

    public static RankedModel getRanked(int rankId)
    {
        lock (RankedXml.RankedRank)
        {
            for (int index = 0; index < RankedXml.RankedRank.Count; ++index)
            {
               RankedModel rank = RankedXml.RankedRank[index];
                if (rank._id == rankId)
                    return rank;
            }
            return (RankedModel)null;
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
                    xmlDocument.Load((Stream)inStream);
                    for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                    {
                        if ("List".Equals(xmlNode1.Name))
                        {
                            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                            {
                                if ("Rank".Equals(xmlNode2.Name))
                                {
                                    XmlNamedNodeMap attributes = (XmlNamedNodeMap)xmlNode2.Attributes;
                                    RankedXml.RankedRank.Add(new RankedModel(int.Parse(attributes.GetNamedItem("Id").Value), int.Parse(attributes.GetNamedItem("NextLevel").Value), int.Parse(attributes.GetNamedItem("PointUp").Value), int.Parse(attributes.GetNamedItem("CashUp").Value), int.Parse(attributes.GetNamedItem("TagUp").Value)));
                                    using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                                    {
                                        NpgsqlCommand command = npgsqlConnection.CreateCommand();
                                        ((DbConnection)npgsqlConnection).Open();
                                        ((DbCommand)command).CommandText = "INSERT INTO server_rankeds VALUES (@Id, @NextLevel, @PointUp, @CashUp);";
                                        command.Parameters.AddWithValue("Id", (object)int.Parse(attributes.GetNamedItem("Id").Value));
                                        command.Parameters.AddWithValue("NextLevel", (object)int.Parse(attributes.GetNamedItem("NextLevel").Value));
                                        command.Parameters.AddWithValue("PointUp", (object)int.Parse(attributes.GetNamedItem("PointUp").Value));
                                        command.Parameters.AddWithValue("CashUp", (object)int.Parse(attributes.GetNamedItem("CashUp").Value));
                                        command.Parameters.AddWithValue("TagUp", (object)int.Parse(attributes.GetNamedItem("TagUp").Value));
                                        ((DbCommand)command).CommandType = CommandType.Text;
                                        ((DbCommand)command).ExecuteNonQuery();
                                        ((Component)command).Dispose();
                                        ((Component)npgsqlConnection).Dispose();
                                        ((DbConnection)npgsqlConnection).Close();
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

    public static List<ItemsModel> getRankedAwards(int rank)
    {
        lock (RankedXml.RankedAwards)
        {
            List<ItemsModel> awards;
            if (RankedXml.RankedAwards.TryGetValue(rank, out awards))
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
                ((DbConnection)npgsqlConnection).Open();
                ((DbCommand)command).CommandText = "SELECT * FROM server_ranked_awards ORDER BY rank_id ASC";
                ((DbCommand)command).CommandType = CommandType.Text;
                NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
                while (((DbDataReader)npgsqlDataReader).Read())
                    RankedXml.AddItemToList(((DbDataReader)npgsqlDataReader).GetInt32(0), new ItemsModel(((DbDataReader)npgsqlDataReader).GetInt32(1))
                    {
                        _name = ((DbDataReader)npgsqlDataReader).GetString(2),
                        _count = ((DbDataReader)npgsqlDataReader).GetInt64(3),
                        _equip = ((DbDataReader)npgsqlDataReader).GetInt32(4)
                    });
                ((Component)command).Dispose();
                ((DbDataReader)npgsqlDataReader).Close();
                ((Component)npgsqlConnection).Dispose();
                ((DbConnection)npgsqlConnection).Close();
            }
        }
        catch (Exception ex)
        {
            Logger.error(ex.ToString());
        }
    }

    private static void AddItemToList(int rank, ItemsModel item)
    {
        if (RankedXml.RankedAwards.ContainsKey(rank))
            RankedXml.RankedAwards[rank].Add(item);
        else
            RankedXml.RankedAwards.Add(rank, new List<ItemsModel>()
        {
          item
        });
    }
}
}
