// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Xml.ChannelsXml
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using Npgsql;
using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using PointBlank.Game.Data.Model;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Data.Xml
{
  public static class ChannelsXml
  {
    public static List<Channel> _channels = new List<Channel>();

    public static void Load(int serverId)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@server", (object) serverId);
          command.CommandText = "SELECT * FROM channels WHERE server_id=@server ORDER BY channel_id ASC";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
            ChannelsXml._channels.Add(new Channel()
            {
              serverId = npgsqlDataReader.GetInt32(0),
              _id = npgsqlDataReader.GetInt32(1),
              _type = npgsqlDataReader.GetInt32(2)
            });
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

    public static Channel getChannel(int id)
    {
      try
      {
        return ChannelsXml._channels[id];
      }
      catch
      {
        return (Channel) null;
      }
    }

    public static List<Channel> getChannels(int ServerId)
    {
      List<Channel> channelList = new List<Channel>();
      for (int index = 0; index < ChannelsXml._channels.Count; ++index)
      {
        Channel channel = ChannelsXml._channels[index];
        if (channel.serverId == ServerId)
          channelList.Add(channel);
      }
      return channelList;
    }

    public static bool updateNotice(int serverId, int channelId, string text) => ComDiv.updateDB("channels", "announce", (object) text, "server_id", (object) serverId, "channel_id", (object) channelId);

    public static bool updateNotice(string text) => ComDiv.updateDB("channels", "announce", (object) text);
  }
}
