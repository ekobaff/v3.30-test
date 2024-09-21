// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.TitleManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account.Title;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public class TitleManager
  {
    private static TitleManager acm = new TitleManager();

    public static TitleManager getInstance() => TitleManager.acm;

    public bool CreateTitleDB(long player_id)
    {
      if (player_id == 0L)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) player_id);
          ((DbCommand) command).CommandText = "INSERT INTO player_titles (owner_id) VALUES (@owner)";
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return true;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        return false;
      }
    }

    public PlayerTitles getTitleDB(long pId)
    {
      PlayerTitles titleDb = new PlayerTitles();
      if (pId == 0L)
        return titleDb;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) pId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_titles WHERE owner_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            titleDb.ownerId = pId;
            titleDb.Equiped1 = ((DbDataReader) npgsqlDataReader).GetInt32(1);
            titleDb.Equiped2 = ((DbDataReader) npgsqlDataReader).GetInt32(2);
            titleDb.Equiped3 = ((DbDataReader) npgsqlDataReader).GetInt32(3);
            titleDb.Flags = ((DbDataReader) npgsqlDataReader).GetInt64(4);
            titleDb.Slots = ((DbDataReader) npgsqlDataReader).GetInt32(5);
          }
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar os títulos!\r\n" + ex.ToString());
      }
      return titleDb;
    }

    public bool updateEquipedTitle(long player_id, int index, int titleId) => ComDiv.updateDB("player_titles", "titleequiped" + (index + 1).ToString(), (object) titleId, "owner_id", (object) player_id);

    public void updateTitlesFlags(long player_id, long flags) => ComDiv.updateDB("player_titles", "titleflags", (object) flags, "owner_id", (object) player_id);

    public void updateRequi(
      long player_id,
      int medalhas,
      int insignias,
      int ordens_azuis,
      int broche)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@pid", (object) player_id);
          command.Parameters.AddWithValue("@broche", (object) broche);
          command.Parameters.AddWithValue("@insignias", (object) insignias);
          command.Parameters.AddWithValue("@medalhas", (object) medalhas);
          command.Parameters.AddWithValue("@ordensazuis", (object) ordens_azuis);
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).CommandText = "UPDATE players SET brooch=@broche, insignia=@insignias, medal=@medalhas, blue_order=@ordensazuis WHERE player_id=@pid";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
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
