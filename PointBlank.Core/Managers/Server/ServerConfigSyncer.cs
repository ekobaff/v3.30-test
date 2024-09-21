// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Server.ServerConfigSyncer
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers.Server
{
  public static class ServerConfigSyncer
  {
    public static ServerConfig GenerateConfig(int configId)
    {
      ServerConfig config = (ServerConfig) null;
      if (configId == 0)
        return config;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@cfg", (object) configId);
          ((DbCommand) command).CommandText = "SELECT * FROM server_settings WHERE config_id=@cfg";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            config = new ServerConfig()
            {
              configId = configId,
              onlyGM = ((DbDataReader) npgsqlDataReader).GetBoolean(1),
              missions = ((DbDataReader) npgsqlDataReader).GetBoolean(2),
              UserFileList = ((DbDataReader) npgsqlDataReader).GetString(3),
              ClientVersion = ((DbDataReader) npgsqlDataReader).GetString(4),
              GiftSystem = ((DbDataReader) npgsqlDataReader).GetBoolean(5),
              ExitURL = ((DbDataReader) npgsqlDataReader).GetString(6),
              ChatColor = ((DbDataReader) npgsqlDataReader).GetInt32(7),
              AnnouceColor = ((DbDataReader) npgsqlDataReader).GetInt32(8),
              Chat = ((DbDataReader) npgsqlDataReader).GetString(9),
              Annouce = ((DbDataReader) npgsqlDataReader).GetString(10),
              ClanEnable = ((DbDataReader) npgsqlDataReader).GetBoolean(11),
              BloodEnable = ((DbDataReader) npgsqlDataReader).GetBoolean(12),
              RankedEnable = ((DbDataReader) npgsqlDataReader).GetBoolean(13),
              RankedDate = npgsqlDataReader.IsDBNull(14) ? default : npgsqlDataReader.GetDateTime(14)
                };
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
      return config;
    }

    public static bool updateMission(ServerConfig cfg, bool mission)
    {
      cfg.missions = mission;
      return ComDiv.updateDB("server_settings", "missions", (object) mission, "config_id", (object) cfg.configId);
    }
  }
}
