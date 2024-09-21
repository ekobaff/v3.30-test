// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.BasicInventoryXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Xml
{
  public class BasicInventoryXml
  {
    public static List<ItemsModel> basic = new List<ItemsModel>();
    public static List<ItemsModel> creationAwards = new List<ItemsModel>();
    public static List<ItemsModel> Characters = new List<ItemsModel>();

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_inventory_template";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            int int32 = ((DbDataReader) npgsqlDataReader).GetInt32(0);
            ItemsModel itemsModel = new ItemsModel(((DbDataReader) npgsqlDataReader).GetInt32(1))
            {
              _name = ((DbDataReader) npgsqlDataReader).GetString(2),
              _count = ((DbDataReader) npgsqlDataReader).GetInt64(3),
              _equip = ((DbDataReader) npgsqlDataReader).GetInt32(4)
            };
            switch (int32)
            {
              case 0:
                BasicInventoryXml.basic.Add(itemsModel);
                break;
              case 1:
                BasicInventoryXml.creationAwards.Add(itemsModel);
                break;
              case 2:
                BasicInventoryXml.Characters.Add(itemsModel);
                break;
            }
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
  }
}
