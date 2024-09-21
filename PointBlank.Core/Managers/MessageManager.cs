// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.MessageManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public static class MessageManager
  {
    public static Message getMessage(int objId, long pId)
    {
      Message message = (Message) null;
      if (pId == 0L || objId == 0)
        return (Message) null;
      try
      {
        DateTime now = DateTime.Now;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@obj", (object) objId);
          command.Parameters.AddWithValue("@owner", (object) pId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_messages WHERE object_id=@obj AND owner_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            message = new Message(((DbDataReader) npgsqlDataReader).GetInt64(8), now)
            {
              object_id = objId,
              sender_id = ((DbDataReader) npgsqlDataReader).GetInt64(2),
              clanId = ((DbDataReader) npgsqlDataReader).GetInt32(3),
              sender_name = ((DbDataReader) npgsqlDataReader).GetString(4),
              text = ((DbDataReader) npgsqlDataReader).GetString(5),
              type = ((DbDataReader) npgsqlDataReader).GetInt32(6),
              state = ((DbDataReader) npgsqlDataReader).GetInt32(7),
              cB = (NoteMessageClan) ((DbDataReader) npgsqlDataReader).GetInt32(9)
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
        return (Message) null;
      }
      return message;
    }

    public static List<Message> getGifts(long owner_id)
    {
      List<Message> gifts = new List<Message>();
      if (owner_id == 0L)
        return gifts;
      try
      {
        DateTime now = DateTime.Now;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) owner_id);
          ((DbCommand) command).CommandText = "SELECT * FROM player_messages WHERE owner_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            int int32 = ((DbDataReader) npgsqlDataReader).GetInt32(6);
            if (int32 == 2)
            {
              Message message = new Message(((DbDataReader) npgsqlDataReader).GetInt64(8), now)
              {
                object_id = ((DbDataReader) npgsqlDataReader).GetInt32(0),
                sender_id = ((DbDataReader) npgsqlDataReader).GetInt64(2),
                clanId = ((DbDataReader) npgsqlDataReader).GetInt32(3),
                sender_name = ((DbDataReader) npgsqlDataReader).GetString(4),
                text = ((DbDataReader) npgsqlDataReader).GetString(5),
                type = int32,
                state = ((DbDataReader) npgsqlDataReader).GetInt32(7),
                cB = (NoteMessageClan) ((DbDataReader) npgsqlDataReader).GetInt32(9)
              };
              gifts.Add(message);
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
      return gifts;
    }

    public static List<Message> getMessages(long owner_id)
    {
      List<Message> messages = new List<Message>();
      if (owner_id == 0L)
        return messages;
      try
      {
        DateTime now = DateTime.Now;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) owner_id);
          ((DbCommand) command).CommandText = "SELECT * FROM player_messages WHERE owner_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            int int32 = ((DbDataReader) npgsqlDataReader).GetInt32(6);
            if (int32 != 2)
            {
              Message message = new Message(((DbDataReader) npgsqlDataReader).GetInt64(8), now)
              {
                object_id = ((DbDataReader) npgsqlDataReader).GetInt32(0),
                sender_id = ((DbDataReader) npgsqlDataReader).GetInt64(2),
                clanId = ((DbDataReader) npgsqlDataReader).GetInt32(3),
                sender_name = ((DbDataReader) npgsqlDataReader).GetString(4),
                text = ((DbDataReader) npgsqlDataReader).GetString(5),
                type = int32,
                state = ((DbDataReader) npgsqlDataReader).GetInt32(7),
                cB = (NoteMessageClan) ((DbDataReader) npgsqlDataReader).GetInt32(9)
              };
              messages.Add(message);
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
      return messages;
    }

    public static bool messageExists(int objId, long owner_id)
    {
      if (owner_id == 0L || objId == 0)
        return false;
      try
      {
        int num = 0;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@obj", (object) objId);
          command.Parameters.AddWithValue("@owner", (object) owner_id);
          ((DbCommand) command).CommandText = "SELECT COUNT(*) FROM player_messages WHERE object_id=@obj AND owner_id=@owner";
          num = Convert.ToInt32(((DbCommand) command).ExecuteScalar());
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return num > 0;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return false;
    }

    public static int getMsgsCount(long owner_id)
    {
      int msgsCount = 0;
      if (owner_id == 0L)
        return msgsCount;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) owner_id);
          ((DbCommand) command).CommandText = "SELECT COUNT(*) FROM player_messages WHERE owner_id=@owner";
          msgsCount = Convert.ToInt32(((DbCommand) command).ExecuteScalar());
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return msgsCount;
    }

    public static bool CreateMessage(long owner_id, Message msg)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) owner_id);
          command.Parameters.AddWithValue("@sendid", (object) msg.sender_id);
          command.Parameters.AddWithValue("@clan", (object) msg.clanId);
          command.Parameters.AddWithValue("@sendname", (object) msg.sender_name);
          command.Parameters.AddWithValue("@text", (object) msg.text);
          command.Parameters.AddWithValue("@type", (object) msg.type);
          command.Parameters.AddWithValue("@state", (object) msg.state);
          command.Parameters.AddWithValue("@expire", (object) msg.expireDate);
          command.Parameters.AddWithValue("@cb", (object) (int) msg.cB);
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).CommandText = "INSERT INTO player_messages(owner_id,sender_id,clan_id,sender_name,text,type,state,expire,cb)VALUES(@owner,@sendid,@clan,@sendname,@text,@type,@state,@expire,@cb) RETURNING object_id";
          object obj = ((DbCommand) command).ExecuteScalar();
          msg.object_id = (int) obj;
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
          return true;
        }
      }
      catch
      {
        return false;
      }
    }

    public static void updateState(int objId, long owner, int value) => ComDiv.updateDB("player_messages", "state", (object) value, "object_id", (object) objId, "owner_id", (object) owner);

    public static void updateExpireDate(int objId, long owner, long date) => ComDiv.updateDB("player_messages", "expire", (object) date, "object_id", (object) objId, "owner_id", (object) owner);

    public static bool DeleteMessage(int objId, long owner) => owner != 0L && objId != 0 && ComDiv.deleteDB("player_messages", "object_id", (object) objId, "owner_id", (object) owner);

    public static bool DeleteMessages(List<object> objs, long owner) => owner != 0L && objs.Count != 0 && ComDiv.deleteDB("player_messages", "object_id", objs.ToArray(), "owner_id", (object) owner);

    public static void RecicleMessages(long owner_id, List<Message> msgs)
    {
      List<object> objs = new List<object>();
      for (int index = 0; index < msgs.Count; ++index)
      {
        Message msg = msgs[index];
        if (msg.DaysRemaining == 0)
        {
          objs.Add((object) msg.object_id);
          msgs.RemoveAt(index--);
        }
      }
      MessageManager.DeleteMessages(objs, owner_id);
    }
  }
}
