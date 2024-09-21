// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.ServersXml
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Xml
{
  public class ServersXml
  {
    public static List<GameServerModel> _servers = new List<GameServerModel>();

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
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM gameservers ORDER BY id ASC";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            GameServerModel gameServerModel = new GameServerModel(((DbDataReader) npgsqlDataReader).GetString(3), (ushort) ((DbDataReader) npgsqlDataReader).GetInt32(5))
            {
              _id = ((DbDataReader) npgsqlDataReader).GetInt32(0),
              _state = ((DbDataReader) npgsqlDataReader).GetInt32(1),
              _type = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              _port = (ushort) ((DbDataReader) npgsqlDataReader).GetInt32(4),
              _maxPlayers = ((DbDataReader) npgsqlDataReader).GetInt32(6)
            };
            ServersXml._servers.Add(gameServerModel);
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

    public static void UpdateServer(int serverId)
    {
      GameServerModel server = ServersXml.getServer(serverId);
      if (server == null)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@id", (object) serverId);
          ((DbCommand) command).CommandText = "SELECT * FROM gameservers WHERE id=@id";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            server._state = ((DbDataReader) npgsqlDataReader).GetInt32(1);
            server._type = ((DbDataReader) npgsqlDataReader).GetInt32(2);
            server._ip = ((DbDataReader) npgsqlDataReader).GetString(3);
            server._port = (ushort) ((DbDataReader) npgsqlDataReader).GetInt32(4);
            server._syncPort = (ushort) ((DbDataReader) npgsqlDataReader).GetInt32(5);
            server._maxPlayers = ((DbDataReader) npgsqlDataReader).GetInt32(6);
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
