// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.CharacterManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public class CharacterManager
  {
    public static List<Character> getCharacters(long PlayerId)
    {
      if (PlayerId == 0L)
        return (List<Character>) null;
      List<Character> characters = new List<Character>();
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@PlayerId", (object) PlayerId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_characters WHERE player_id=@PlayerId ORDER BY slot ASC;";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            characters.Add(new Character()
            {
              ObjId = ((DbDataReader) npgsqlDataReader).GetInt64(0),
              Id = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              Slot = ((DbDataReader) npgsqlDataReader).GetInt32(3),
              Name = ((DbDataReader) npgsqlDataReader).GetString(4),
              CreateDate = ((DbDataReader) npgsqlDataReader).GetInt32(5),
              PlayTime = ((DbDataReader) npgsqlDataReader).GetInt32(6)
            });
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("was a problem Loading (Character)!\r\n" + ex.ToString());
      }
      return characters;
    }

    public static bool Create(Character Model, long PlayerId)
    {
      if (PlayerId == 0L)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@player_id", (object) PlayerId);
          command.Parameters.AddWithValue("@id", (object) Model.Id);
          command.Parameters.AddWithValue("@slot", (object) Model.Slot);
          command.Parameters.AddWithValue("@name", (object) Model.Name);
          command.Parameters.AddWithValue("@createdate", (object) Model.CreateDate);
          command.Parameters.AddWithValue("@playtime", (object) Model.PlayTime);
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).CommandText = "INSERT INTO player_characters(player_id, id, slot, name, createdate, playtime)VALUES(@player_id, @id, @slot, @name, @createdate, @playtime) RETURNING object_id";
          object obj = ((DbCommand) command).ExecuteScalar();
          Model.ObjId = (long) obj;
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
          return true;
        }
      }
      catch (Exception ex)
      {
        Logger.error("was a problem Create (Characater)!\r\n" + ex.ToString());
        return false;
      }
    }

    public static bool Delete(long ObjectId, long PlayerId) => ObjectId != 0L && PlayerId != 0L && ComDiv.deleteDB("player_characters", "object_id", (object) ObjectId, "player_id", (object) PlayerId);

    public static void Update(int Slot, long ObjectId)
    {
      if (ObjectId == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@ObjectId", (object) ObjectId);
          command.Parameters.AddWithValue("@Slot", (object) Slot);
          ((DbCommand) command).CommandText = "UPDATE player_characters SET slot=@Slot WHERE object_id=@ObjectId";
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("was a problem Update (Characater)!\r\n" + ex.ToString());
      }
    }

    public static int getSlots(long PlayerId)
    {
      int slots = 0;
      if (PlayerId == 0L)
        return slots;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@PlayerId", (object) PlayerId);
          ((DbCommand) command).CommandText = "SELECT slot FROM player_characters WHERE player_id=@PlayerId ORDER BY slot ASC;";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            slots = ((DbDataReader) npgsqlDataReader).GetInt32(0);
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("was a problem Slots (Characater)!\r\n" + ex.ToString());
      }
      return slots;
    }
  }
}
