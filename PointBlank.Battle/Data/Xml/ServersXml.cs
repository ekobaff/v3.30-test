// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Xml.ServersXml
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using Npgsql;
using PointBlank.Battle.Data.Models;
using PointBlank.Battle.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PointBlank.Battle.Data.Xml
{
  public class ServersXml
  {
    private static List<GameServerModel> _servers = new List<GameServerModel>();

    public static GameServerModel getServer(int id)
    {
      lock (ServersXml._servers)
      {
        for (int index = 0; index < ServersXml._servers.Count; ++index)
        {
          GameServerModel server = ServersXml._servers[index];
          if (server._id == id)
            return server;
        }
        return (GameServerModel) null;
      }
    }

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.CommandText = "SELECT * FROM gameservers ORDER BY id ASC";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            GameServerModel gameServerModel = new GameServerModel(npgsqlDataReader.GetString(3), (ushort) npgsqlDataReader.GetInt32(5))
            {
              _id = npgsqlDataReader.GetInt32(0),
              _state = npgsqlDataReader.GetInt32(1),
              _type = npgsqlDataReader.GetInt32(2),
              _port = (ushort) npgsqlDataReader.GetInt32(4),
              _maxPlayers = npgsqlDataReader.GetInt32(6)
            };
            ServersXml._servers.Add(gameServerModel);
          }
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }
  }
}
