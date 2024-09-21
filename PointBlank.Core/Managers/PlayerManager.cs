// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.PlayerManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace PointBlank.Core.Managers
{
  public static class PlayerManager
  {
    public static void updatePlayerBonus(long pId, int bonuses, int freepass)
    {
      if (pId == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@id", (object) pId);
          command.Parameters.AddWithValue("@bonuses", (object) bonuses);
          command.Parameters.AddWithValue("@freepass", (object) freepass);
          ((DbCommand) command).CommandText = "UPDATE player_bonus SET bonuses=@bonuses, freepass=@freepass WHERE player_id=@id";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static void updateCupomEffects(long id, CouponEffects effects)
    {
      if (id == 0L)
        return;
      ComDiv.updateDB("players", nameof (effects), (object) (long) effects, "player_id", (object) id);
    }

    public static PlayerBonus getPlayerBonusDB(long id)
    {
      PlayerBonus playerBonusDb = new PlayerBonus();
      if (id == 0L)
        return playerBonusDb;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@id", (object) id);
          ((DbCommand) command).CommandText = "SELECT * FROM player_bonus WHERE player_id=@id";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            playerBonusDb.ownerId = id;
            playerBonusDb.bonuses = ((DbDataReader) npgsqlDataReader).GetInt32(1);
            playerBonusDb.sightColor = ((DbDataReader) npgsqlDataReader).GetInt32(2);
            playerBonusDb.freepass = ((DbDataReader) npgsqlDataReader).GetInt32(3);
            playerBonusDb.fakeRank = ((DbDataReader) npgsqlDataReader).GetInt32(4);
            playerBonusDb.fakeNick = ((DbDataReader) npgsqlDataReader).GetString(5);
            playerBonusDb.muzzle = ((DbDataReader) npgsqlDataReader).GetInt32(6);
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
      return playerBonusDb;
    }

    public static PlayerDailyRecord getPlayerDailyRecord(long PlayerId)
    {
      PlayerDailyRecord playerDailyRecord = (PlayerDailyRecord) null;
      if (PlayerId == 0L)
        return (PlayerDailyRecord) null;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@PlayerId", (object) PlayerId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_dailyrecord WHERE player_id=@PlayerId";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            playerDailyRecord = new PlayerDailyRecord();
            playerDailyRecord.PlayerId = PlayerId;
            playerDailyRecord.Total = ((DbDataReader) npgsqlDataReader).GetInt32(1);
            playerDailyRecord.Wins = ((DbDataReader) npgsqlDataReader).GetInt32(2);
            playerDailyRecord.Loses = ((DbDataReader) npgsqlDataReader).GetInt32(3);
            playerDailyRecord.Draws = ((DbDataReader) npgsqlDataReader).GetInt32(4);
            playerDailyRecord.Kills = ((DbDataReader) npgsqlDataReader).GetInt32(5);
            playerDailyRecord.Deaths = ((DbDataReader) npgsqlDataReader).GetInt32(6);
            playerDailyRecord.Headshots = ((DbDataReader) npgsqlDataReader).GetInt32(7);
            playerDailyRecord.Point = ((DbDataReader) npgsqlDataReader).GetInt32(8);
            playerDailyRecord.Exp = ((DbDataReader) npgsqlDataReader).GetInt32(9);
            playerDailyRecord.Playtime = ((DbDataReader) npgsqlDataReader).GetInt32(10);
            playerDailyRecord.Money = ((DbDataReader) npgsqlDataReader).GetInt32(11);
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
        return (PlayerDailyRecord) null;
      }
      return playerDailyRecord;
    }

        //NEW PLAYER RANKED 
        public static PlayerRanked getPlayerRanked(long PlayerId)
        {
            PlayerRanked PlayerRankedRecord = (PlayerRanked)null;
            if (PlayerId == 0L)
                return (PlayerRanked)null;
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    ((DbConnection)npgsqlConnection).Open();
                    command.Parameters.AddWithValue("@PlayerId", (object)PlayerId);
                    ((DbCommand)command).CommandText = "SELECT * FROM player_ranked WHERE player_id=@PlayerId";
                    ((DbCommand)command).CommandType = CommandType.Text;
                    NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
                    while (((DbDataReader)npgsqlDataReader).Read())
                    {
                        PlayerRankedRecord = new PlayerRanked();
                        PlayerRankedRecord.PlayerId = PlayerId;
                        PlayerRankedRecord.Rank = ((DbDataReader)npgsqlDataReader).GetInt32(1);
                        PlayerRankedRecord.Exp = ((DbDataReader)npgsqlDataReader).GetInt32(2);
                        PlayerRankedRecord.Matches = ((DbDataReader)npgsqlDataReader).GetInt32(3);
                        PlayerRankedRecord.Wins = ((DbDataReader)npgsqlDataReader).GetInt32(4);
                        PlayerRankedRecord.Loses = ((DbDataReader)npgsqlDataReader).GetInt32(5);
                        PlayerRankedRecord.Wins = ((DbDataReader)npgsqlDataReader).GetInt32(6);
                        PlayerRankedRecord.Deaths = ((DbDataReader)npgsqlDataReader).GetInt32(7);
                        PlayerRankedRecord.Headshots = ((DbDataReader)npgsqlDataReader).GetInt32(8);
                        PlayerRankedRecord.Playtime = ((DbDataReader)npgsqlDataReader).GetInt32(9);
                       
                    }
                  ((Component)command).Dispose();
                    ((DbDataReader)npgsqlDataReader).Close();
                    ((Component)npgsqlConnection).Dispose();
                    ((DbConnection)npgsqlConnection).Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
                return (PlayerRanked)null;
            }
            return PlayerRankedRecord;
        }

        public static List<PlayerItemTopup> getPlayerTopups(long id)
    {
      List<PlayerItemTopup> playerTopups = new List<PlayerItemTopup>();
      if (id == 0L)
        return playerTopups;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@id", (object) id);
          ((DbCommand) command).CommandText = "SELECT * FROM player_topups WHERE player_id=@id";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            playerTopups.Add(new PlayerItemTopup()
            {
              ObjectId = ((DbDataReader) npgsqlDataReader).GetInt64(0),
              PlayerId = ((DbDataReader) npgsqlDataReader).GetInt64(1),
              ItemId = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              ItemName = ((DbDataReader) npgsqlDataReader).GetString(3),
              Count = ((DbDataReader) npgsqlDataReader).GetInt64(4),
              Equip = ((DbDataReader) npgsqlDataReader).GetInt32(5)
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
      return playerTopups;
    }

    public static bool DeletePlayerTopup(long ObjectId, long PlayerId) => ObjectId != 0L && PlayerId != 0L && ComDiv.deleteDB("player_topups", "object_id", (object) ObjectId, "player_id", (object) PlayerId);

        //=================
        public static List<PlayerMakeVip> getPlayerMakeVip(long id)
        {
            List<PlayerMakeVip> playervip = new List<PlayerMakeVip>();
            if (id == 0L)
                return playervip;
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    ((DbConnection)npgsqlConnection).Open();
                    command.Parameters.AddWithValue("@id", (object)id);
                    ((DbCommand)command).CommandText = "SELECT * FROM player_vip_add WHERE player_id=@id";
                    ((DbCommand)command).CommandType = CommandType.Text;
                    NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
                    while (((DbDataReader)npgsqlDataReader).Read())
                        playervip.Add(new PlayerMakeVip()
                        {
                            ObjectId = ((DbDataReader)npgsqlDataReader).GetInt32(0),
                            PlayerId = ((DbDataReader)npgsqlDataReader).GetInt64(1),
                            VipType = ((DbDataReader)npgsqlDataReader).GetInt32(2),
                            VipGold = ((DbDataReader)npgsqlDataReader).GetInt32(3),
                            VipCash = ((DbDataReader)npgsqlDataReader).GetInt32(4),
                            VipTag = ((DbDataReader)npgsqlDataReader).GetInt32(5),
                            
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
            return playervip;
        }

        public static bool DeletePlayerMakeVip(int ObjectId, long PlayerId) => ObjectId != 0L && PlayerId != 0L && ComDiv.deleteDB("player_vip_add", "object_id", (object)ObjectId, "player_id", (object)PlayerId);

        //=================



        public static void CreatePlayerDailyRecord(long id)
    {
      if (id == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@id", (object) id);
          ((DbCommand) command).CommandText = "INSERT INTO player_dailyrecord(player_id)VALUES(@id)";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

        public static void CreatePlayerRanked(long id)
        {
            if (id == 0L)
                return;
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    ((DbConnection)npgsqlConnection).Open();
                    ((DbCommand)command).CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", (object)id);
                    ((DbCommand)command).CommandText = "INSERT INTO player_ranked(player_id)VALUES(@id)";
                    ((DbCommand)command).ExecuteNonQuery();
                    ((Component)command).Dispose();
                    ((Component)npgsqlConnection).Dispose();
                    ((DbConnection)npgsqlConnection).Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }

        public static void CreatePlayerBonusDB(long id)
    {
      if (id == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@id", (object) id);
          ((DbCommand) command).CommandText = "INSERT INTO player_bonus(player_id)VALUES(@id)";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static bool DeleteItem(long objId, long ownerId) => objId != 0L && ownerId != 0L && ComDiv.deleteDB("player_items", "object_id", (object) objId, "owner_id", (object) ownerId);

    public static int CheckEquipedItems(
      PlayerEquipedItems items,
      List<ItemsModel> inventory,
      bool BattleRules = false)
    {
      int num = 0;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      bool flag9 = false;
      bool flag10 = false;
      if (items._primary == 0)
        flag1 = true;
      if (BattleRules)
      {
        if (!flag1 && (items._primary == 105025 || items._primary == 106007))
          flag1 = true;
        if (!flag3 && items._melee == 323001)
          flag3 = true;
      }
      if (items._beret == 0)
        flag9 = true;
      lock (inventory)
      {
        foreach (ItemsModel itemsModel in inventory)
        {
          if (itemsModel._count > 0L)
          {
            if (itemsModel._id == items._primary)
              flag1 = true;
            else if (itemsModel._id == items._secondary)
              flag2 = true;
            else if (itemsModel._id == items._melee)
              flag3 = true;
            else if (itemsModel._id == items._grenade)
              flag4 = true;
            else if (itemsModel._id == items._special)
              flag5 = true;
            else if (itemsModel._id == items._red)
              flag6 = true;
            else if (itemsModel._id == items._blue)
              flag7 = true;
            else if (itemsModel._id == items._helmet)
              flag8 = true;
            else if (itemsModel._id == items._beret)
              flag9 = true;
            else if (itemsModel._id == items._dino)
              flag10 = true;
            if (flag1 & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag8 & flag9 & flag10)
              break;
          }
        }
      }
      if (!flag1 || !flag2 || !flag3 || !flag4 || !flag5)
        num += 2;
      if (!flag6 || !flag7 || !flag8 || !flag9 || !flag10)
        ++num;
      if (!flag1)
        items._primary = 103004;
      if (!flag2)
        items._secondary = 202003;
      if (!flag3)
        items._melee = 301001;
      if (!flag4)
        items._grenade = 407001;
      if (!flag5)
        items._special = 508001;
      if (!flag6)
        items._red = 601001;
      if (!flag7)
        items._blue = 602002;
      if (!flag8)
        items._helmet = 1000800000;
      if (!flag9)
        items._beret = 0;
      if (!flag10)
        items._dino = 1500511;
      return num;
    }

    public static void updateWeapons(
      PlayerEquipedItems source,
      PlayerEquipedItems items,
      DBQuery query)
    {
      if (items._primary != source._primary)
        query.AddQuery("weapon_primary", (object) source._primary);
      if (items._secondary != source._secondary)
        query.AddQuery("weapon_secondary", (object) source._secondary);
      if (items._melee != source._melee)
        query.AddQuery("weapon_melee", (object) source._melee);
      if (items._grenade != source._grenade)
        query.AddQuery("weapon_thrown_normal", (object) source._grenade);
      if (items._special == source._special)
        return;
      query.AddQuery("weapon_thrown_special", (object) source._special);
    }

    public static void updateChars(
      PlayerEquipedItems source,
      PlayerEquipedItems items,
      DBQuery query)
    {
      if (items._red != source._red)
        query.AddQuery("char_red", (object) source._red);
      if (items._blue != source._blue)
        query.AddQuery("char_blue", (object) source._blue);
      if (items._helmet != source._helmet)
        query.AddQuery("char_helmet", (object) source._helmet);
      if (items._beret != source._beret)
        query.AddQuery("char_beret", (object) source._beret);
      if (items._dino != source._dino)
        query.AddQuery("char_dino", (object) source._dino);
      if (items.face != source.face)
        query.AddQuery("face", (object) source.face);
      if (items.jacket != source.jacket)
        query.AddQuery("jacket", (object) source.jacket);
      if (items.poket != source.poket)
        query.AddQuery("poket", (object) source.poket);
      if (items.glove != source.glove)
        query.AddQuery("glove", (object) source.glove);
      if (items.belt != source.belt)
        query.AddQuery("belt", (object) source.belt);
      if (items.holster != source.holster)
        query.AddQuery("holster", (object) source.holster);
      if (items.skin == source.skin)
        return;
      query.AddQuery("skin", (object) source.skin);
    }

    public static void updateCharSlots(
      PlayerEquipedItems source,
      PlayerEquipedItems items,
      DBQuery query)
    {
      if (items._red != source._red)
        query.AddQuery("char_red", (object) source._red);
      if (items._blue != source._blue)
        query.AddQuery("char_blue", (object) source._blue);
      if (items._dino == source._dino)
        return;
      query.AddQuery("char_dino", (object) source._dino);
    }

    public static void updateWeapons(PlayerEquipedItems items, DBQuery query)
    {
      query.AddQuery("weapon_primary", (object) items._primary);
      query.AddQuery("weapon_secondary", (object) items._secondary);
      query.AddQuery("weapon_melee", (object) items._melee);
      query.AddQuery("weapon_thrown_normal", (object) items._grenade);
      query.AddQuery("weapon_thrown_special", (object) items._special);
    }

    public static void updateChars(PlayerEquipedItems items, DBQuery query)
    {
      query.AddQuery("char_red", (object) items._red);
      query.AddQuery("char_blue", (object) items._blue);
      query.AddQuery("char_helmet", (object) items._helmet);
      query.AddQuery("char_beret", (object) items._beret);
      query.AddQuery("char_dino", (object) items._dino);
      query.AddQuery("face", (object) items.face);
      query.AddQuery("jacket", (object) items.jacket);
      query.AddQuery("poket", (object) items.poket);
      query.AddQuery("glove", (object) items.glove);
      query.AddQuery("belt", (object) items.belt);
      query.AddQuery("holster", (object) items.holster);
      query.AddQuery("skin", (object) items.skin);
    }

    public static bool updateFights(
      int partidas,
      int partidas_ganhas,
      int partidas_perdidas,
      int partidas_empatadas,
      int todas,
      long id)
    {
      if (id == 0L)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) id);
          command.Parameters.AddWithValue("@partidas", (object) partidas);
          command.Parameters.AddWithValue("@ganhas", (object) partidas_ganhas);
          command.Parameters.AddWithValue("@perdidas", (object) partidas_perdidas);
          command.Parameters.AddWithValue("@empates", (object) partidas_empatadas);
          command.Parameters.AddWithValue("@todaspartidas", (object) todas);
          ((DbCommand) command).CommandText = "UPDATE players SET fights=@partidas, fights_win=@ganhas, fights_lost=@perdidas, fights_draw=@empates, totalfights_count=@todaspartidas WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateAccountCashing(long player_id, int gold, int cash)
    {
      if (player_id == 0L || gold == -1 && cash == -1)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) player_id);
          string str = "";
          if (gold > -1)
          {
            command.Parameters.AddWithValue("@gold", (object) gold);
            str += "gp=@gold";
          }
          if (cash > -1)
          {
            command.Parameters.AddWithValue("@cash", (object) cash);
            str = str + (str != "" ? ", " : "") + "money=@cash";
          }
          ((DbCommand) command).CommandText = "UPDATE players SET " + str + " WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateAccountAccess(long player_id, int Vip)
    {
      if (player_id == 0L || Vip == -1)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) player_id);
          command.Parameters.AddWithValue("@access_level", (object) Vip);
          ((DbCommand) command).CommandText = "UPDATE players SET access_level=@access_level WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateAccountVip(long player_id, int Vip)
    {
      if (player_id == 0L || Vip == -1)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) player_id);
          command.Parameters.AddWithValue("@pc_cafe", (object) Vip);
          ((DbCommand) command).CommandText = "UPDATE players SET pc_cafe=@pc_cafe WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateAccountCash(long player_id, int cash)
    {
      if (player_id == 0L || cash == -1)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) player_id);
          command.Parameters.AddWithValue("@cash", (object) cash);
          ((DbCommand) command).CommandText = "UPDATE players SET money=@cash WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateAccountGold(long player_id, int gold)
    {
      if (player_id == 0L || gold == -1)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) player_id);
          command.Parameters.AddWithValue("@gold", (object) gold);
          ((DbCommand) command).CommandText = "UPDATE players SET gp=@gold WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateAccountTag(long player_id, int tag)
    {
            if (player_id == 0L || tag == -1)
                return false;
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    ((DbConnection)npgsqlConnection).Open();
                    ((DbCommand)command).CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@owner", (object)player_id);
                    command.Parameters.AddWithValue("@tag", (object)tag);
                    ((DbCommand)command).CommandText = "UPDATE players SET tag=@tag WHERE player_id=@owner";
                    ((DbCommand)command).ExecuteNonQuery();
                    ((Component)command).Dispose();
                    ((Component)npgsqlConnection).Dispose();
                    ((DbConnection)npgsqlConnection).Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
                return false;
            }
    }

        public static bool updateKD(long player_id, int kills, int death, int hs, int total)
    {
      if (player_id == 0L)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) player_id);
          command.Parameters.AddWithValue("@deaths", (object) death);
          command.Parameters.AddWithValue("@kills", (object) kills);
          command.Parameters.AddWithValue("@hs", (object) hs);
          command.Parameters.AddWithValue("@total", (object) total);
          ((DbCommand) command).CommandText = "UPDATE players SET kills_count=@kills, deaths_count=@deaths, headshots_count=@hs, totalkills_count=@total WHERE player_id=@owner";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool updateMissionId(long player_id, int value, int index) => ComDiv.updateDB("players", "mission_id" + (index + 1).ToString(), (object) value, nameof (player_id), (object) player_id);

    public static bool isPlayerNameExist(string name)
    {
      if (string.IsNullOrEmpty(name))
        return true;
      try
      {
        int num = 0;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@name", (object) name);
          ((DbCommand) command).CommandText = "SELECT COUNT(*) FROM players WHERE player_name=@name";
          num = Convert.ToInt32(((DbCommand) command).ExecuteScalar());
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return num > 0;
      }
      catch
      {
        return true;
      }
    }

    public static bool DeleteFriend(long friendId, long pId) => ComDiv.deleteDB("player_friends", "friend_id", (object) friendId, "owner_id", (object) pId);

    public static void UpdateFriendState(long ownerId, Friend friend) => ComDiv.updateDB("player_friends", "state", (object) friend.state, "owner_id", (object) ownerId, "friend_id", (object) friend.player_id);

    public static void UpdateFriendBlock(long ownerId, Friend friend) => ComDiv.updateDB("player_friends", "removed", (object) friend.removed, "owner_id", (object) ownerId, "friend_id", (object) friend.player_id);

    public static List<Friend> getFriendList(long ownerId)
    {
      List<Friend> friendList = new List<Friend>();
      if (ownerId == 0L)
        return friendList;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) ownerId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_friends WHERE owner_id=@owner ORDER BY friend_id";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            friendList.Add(new Friend(((DbDataReader) npgsqlDataReader).GetInt64(1))
            {
              state = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              removed = ((DbDataReader) npgsqlDataReader).GetBoolean(3)
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
      return friendList;
    }

    public static bool CreateItem(ItemsModel item, long playerId)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) playerId);
          command.Parameters.AddWithValue("@itmId", (object) item._id);
          command.Parameters.AddWithValue("@ItmNm", (object) item._name);
          command.Parameters.AddWithValue("@count", (object) item._count);
          command.Parameters.AddWithValue("@equip", (object) item._equip);
          command.Parameters.AddWithValue("@category", (object) item._category);
          ((DbCommand) command).CommandText = "INSERT INTO player_items(owner_id,item_id,item_name,count,equip,category)VALUES(@owner,@itmId,@ItmNm,@count,@equip,@category) RETURNING object_id";
          object obj = ((DbCommand) command).ExecuteScalar();
          item._objId = (long) obj;
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return true;
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
        return false;
      }
    }

    public static bool CreateClan(
      out int clanId,
      string name,
      long ownerId,
      string clan_info,
      int date)
    {
      try
      {
        clanId = -1;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@owner", (object) ownerId);
          command.Parameters.AddWithValue("@name", (object) name);
          command.Parameters.AddWithValue("@date", (object) date);
          command.Parameters.AddWithValue("@info", (object) clan_info);
          command.Parameters.AddWithValue("@best", (object) "0-0");
          ((DbCommand) command).CommandText = "INSERT INTO clans(clan_name,owner_id,create_date,clan_info,best_exp,best_participation,best_wins,best_kills,best_headshot)VALUES(@name,@owner,@date,@info,@best,@best,@best,@best,@best) RETURNING clan_id";
          object obj = ((DbCommand) command).ExecuteScalar();
          clanId = (int) obj;
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return true;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        clanId = -1;
        return false;
      }
    }

    public static bool updateClanInfo(
      int clan_id,
      int autoridade,
      int limite_rank,
      int limite_idade,
      int limite_idade2)
    {
      if (clan_id == 0)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@clan", (object) clan_id);
          command.Parameters.AddWithValue("@autoridade", (object) autoridade);
          command.Parameters.AddWithValue("@limite_rank", (object) limite_rank);
          command.Parameters.AddWithValue("@limite_idade", (object) limite_idade);
          command.Parameters.AddWithValue("@limite_idade2", (object) limite_idade2);
          ((DbCommand) command).CommandText = "UPDATE clans SET autoridade=@autoridade, limite_rank=@limite_rank, limite_idade=@limite_idade, limite_idade2=@limite_idade2 WHERE clan_id=@clan";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
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

    public static bool updateClanLogo(int clan_id, uint logo) => clan_id != 0 && ComDiv.updateDB("clans", nameof (logo), (object) (long) logo, nameof (clan_id), (object) clan_id);

    public static bool updateClanPoints(int clan_id, float pontos) => clan_id != 0 && ComDiv.updateDB("clans", nameof (pontos), (object) pontos, nameof (clan_id), (object) clan_id);

    public static bool updateClanExp(int clan_id, int exp) => clan_id != 0 && ComDiv.updateDB("clans", "clan_exp", (object) exp, nameof (clan_id), (object) clan_id);

    public static bool updateClanRank(int clan_id, int rank) => clan_id != 0 && ComDiv.updateDB("clans", "clan_rank", (object) rank, nameof (clan_id), (object) clan_id);

    public static bool updateClanBattles(int clan_id, int partidas, int vitorias, int derrotas)
    {
      if (clan_id == 0)
        return false;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandType = CommandType.Text;
          command.Parameters.AddWithValue("@clan", (object) clan_id);
          command.Parameters.AddWithValue("@partidas", (object) partidas);
          command.Parameters.AddWithValue("@vitorias", (object) vitorias);
          command.Parameters.AddWithValue("@derrotas", (object) derrotas);
          ((DbCommand) command).CommandText = "UPDATE clans SET partidas=@partidas, vitorias=@vitorias, derrotas=@derrotas WHERE clan_id=@clan";
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

    public static int getClanPlayers(int clanId)
    {
      int clanPlayers = 0;
      if (clanId == 0)
        return clanPlayers;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@clan", (object) clanId);
          ((DbCommand) command).CommandText = "SELECT COUNT(*) FROM players WHERE clan_id=@clan";
          clanPlayers = Convert.ToInt32(((DbCommand) command).ExecuteScalar());
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
      return clanPlayers;
    }

    public static List<ItemsModel> getInventoryItems(long player_id)
    {
      List<ItemsModel> inventoryItems = new List<ItemsModel>();
      if (player_id == 0L)
        return inventoryItems;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) player_id);
          ((DbCommand) command).CommandText = "SELECT * FROM player_items WHERE owner_id=@owner ORDER BY object_id ASC;";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            inventoryItems.Add(new ItemsModel(((DbDataReader) npgsqlDataReader).GetInt32(2), ((DbDataReader) npgsqlDataReader).GetInt32(5), ((DbDataReader) npgsqlDataReader).GetString(3), ((DbDataReader) npgsqlDataReader).GetInt32(6), ((DbDataReader) npgsqlDataReader).GetInt64(4), ((DbDataReader) npgsqlDataReader).GetInt64(0)));
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
      return inventoryItems;
    }

    public static bool CheckWeaponPermanent(int ItemId)
    {
      foreach (ItemRepair itemRepair in ShopManager.ItemRepairs)
      {
        if (ItemId == itemRepair.ItemId)
          return false;
      }
      return true;
    }

    public static void tryCreateItem(ItemsModel modelo, PlayerInventory inventory, long pId)
    {
      try
      {
        ItemsModel itemsModel = inventory.getItem(modelo._id);
        if (itemsModel == null)
        {
          if (!PlayerManager.CreateItem(modelo, pId))
            return;
          inventory.AddItem(modelo);
        }
        else
        {
          modelo._objId = itemsModel._objId;
          if (itemsModel._equip == 1)
          {
            if (PlayerManager.CheckWeaponPermanent(modelo._id))
            {
              modelo._count += itemsModel._count;
              ComDiv.updateDB("player_items", "count", (object) modelo._count, "owner_id", (object) pId, "item_id", (object) modelo._id);
            }
            else
            {
              modelo._count = 100L;
              ComDiv.updateDB("player_items", "count", (object) modelo._count, "owner_id", (object) pId, "item_id", (object) modelo._id);
            }
          }
          else if (itemsModel._equip == 2)
          {
            DateTime exact = DateTime.ParseExact(itemsModel._count.ToString(), "yyMMddHHmm", (IFormatProvider) CultureInfo.InvariantCulture);
            if (modelo._category != 3)
            {
              modelo._equip = 2;
              modelo._count = Convert.ToInt64(exact.AddSeconds((double) modelo._count).ToString("yyMMddHHmm"));
            }
            else
            {
              TimeSpan timeSpan = DateTime.ParseExact(modelo._count.ToString(), "yyMMddHHmm", (IFormatProvider) CultureInfo.InvariantCulture) - DateTime.Now;
              modelo._equip = 2;
              modelo._count = Convert.ToInt64(exact.AddDays(timeSpan.TotalDays).ToString("yyMMddHHmm"));
            }
            ComDiv.updateDB("player_items", "count", (object) modelo._count, "owner_id", (object) pId, "item_id", (object) modelo._id);
          }
          itemsModel._equip = modelo._equip;
          itemsModel._count = modelo._count;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static PlayerConfig getConfigDB(long playerId)
    {
      PlayerConfig configDb = (PlayerConfig) null;
      if (playerId == 0L)
        return (PlayerConfig) null;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) playerId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_configs WHERE owner_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            configDb = new PlayerConfig()
            {
              config = ((DbDataReader) npgsqlDataReader).GetInt32(1),
              blood = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              sight = ((DbDataReader) npgsqlDataReader).GetInt32(3),
              hand = ((DbDataReader) npgsqlDataReader).GetInt32(4),
              audio1 = ((DbDataReader) npgsqlDataReader).GetInt32(5),
              audio2 = ((DbDataReader) npgsqlDataReader).GetInt32(6),
              audio_enable = ((DbDataReader) npgsqlDataReader).GetInt32(7),
              sensibilidade = ((DbDataReader) npgsqlDataReader).GetInt32(8),
              fov = ((DbDataReader) npgsqlDataReader).GetInt32(9),
              mouse_invertido = ((DbDataReader) npgsqlDataReader).GetInt32(10),
              msgConvite = ((DbDataReader) npgsqlDataReader).GetInt32(11),
              chatSussurro = ((DbDataReader) npgsqlDataReader).GetInt32(12),
              macro = ((DbDataReader) npgsqlDataReader).GetInt32(13),
              macro_1 = ((DbDataReader) npgsqlDataReader).GetString(14),
              macro_2 = ((DbDataReader) npgsqlDataReader).GetString(15),
              macro_3 = ((DbDataReader) npgsqlDataReader).GetString(16),
              macro_4 = ((DbDataReader) npgsqlDataReader).GetString(17),
              macro_5 = ((DbDataReader) npgsqlDataReader).GetString(18)
            };
            ((DbDataReader) npgsqlDataReader).GetBytes(19, 0L, configDb.keys, 0, 235);
          }
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return configDb;
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar as configurações!\r\n" + ex.ToString());
        return (PlayerConfig) null;
      }
    }

    public static bool CreateConfigDB(long player_id)
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
          ((DbCommand) command).CommandText = "INSERT INTO player_configs (owner_id) VALUES (@owner)";
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
          return true;
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        return false;
      }
    }

    public static void updateConfigs(DBQuery query, PlayerConfig config)
    {
      query.AddQuery("mira", (object) config.sight);
      query.AddQuery("audio1", (object) config.audio1);
      query.AddQuery("audio2", (object) config.audio2);
      query.AddQuery("sensibilidade", (object) config.sensibilidade);
      query.AddQuery("sangue", (object) config.blood);
      query.AddQuery("visao", (object) config.fov);
      query.AddQuery("mao", (object) config.hand);
      query.AddQuery("audio_enable", (object) config.audio_enable);
      query.AddQuery(nameof (config), (object) config.config);
      query.AddQuery("mouse_invertido", (object) config.mouse_invertido);
      query.AddQuery("msgconvite", (object) config.msgConvite);
      query.AddQuery("chatsussurro", (object) config.chatSussurro);
      query.AddQuery("macro", (object) config.macro);
    }

    public static void updateMacros(DBQuery query, PlayerConfig config, int value)
    {
      if ((value & 1) == 1)
        query.AddQuery("macro_1", (object) config.macro_1);
      if ((value & 2) == 2)
        query.AddQuery("macro_2", (object) config.macro_2);
      if ((value & 4) == 4)
        query.AddQuery("macro_3", (object) config.macro_3);
      if ((value & 8) == 8)
        query.AddQuery("macro_4", (object) config.macro_4);
      if ((value & 16) != 16)
        return;
      query.AddQuery("macro_5", (object) config.macro_5);
    }

    public static PlayerEvent getPlayerEventDB(long pId)
    {
      PlayerEvent playerEventDb = (PlayerEvent) null;
      if (pId == 0L)
        return (PlayerEvent) null;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@id", (object) pId);
          ((DbCommand) command).CommandText = "SELECT * FROM player_events WHERE player_id=@id";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            playerEventDb = new PlayerEvent()
            {
              LastVisitEventId = ((DbDataReader) npgsqlDataReader).GetInt32(1),
              LastVisitSequence1 = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              LastVisitSequence2 = ((DbDataReader) npgsqlDataReader).GetInt32(3),
              NextVisitDate = ((DbDataReader) npgsqlDataReader).GetInt32(4),
              LastXmasRewardDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(5),
              LastPlaytimeDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(6),
              LastPlaytimeValue = ((DbDataReader) npgsqlDataReader).GetInt64(7),
              LastPlaytimeFinish = ((DbDataReader) npgsqlDataReader).GetInt32(8),
              LastLoginDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(9),
              LastQuestDate = (uint) ((DbDataReader) npgsqlDataReader).GetInt64(10),
              LastQuestFinish = ((DbDataReader) npgsqlDataReader).GetInt32(11)
            };
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return playerEventDb;
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar os eventos!\r\n" + ex.ToString());
        return (PlayerEvent) null;
      }
    }

    public static void addEventDB(long pId)
    {
      if (pId == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@id", (object) pId);
          ((DbCommand) command).CommandText = "INSERT INTO player_events (player_id) VALUES (@id)";
          ((DbCommand) command).CommandType = CommandType.Text;
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

    public static List<ClanInvite> getClanRequestList(int clan_id)
    {
      List<ClanInvite> clanRequestList = new List<ClanInvite>();
      if (clan_id == 0)
        return clanRequestList;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@clan", (object) clan_id);
          ((DbCommand) command).CommandText = "SELECT * FROM clan_invites WHERE clan_id=@clan";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            clanRequestList.Add(new ClanInvite()
            {
              clan_id = clan_id,
              player_id = ((DbDataReader) npgsqlDataReader).GetInt64(1),
              inviteDate = ((DbDataReader) npgsqlDataReader).GetInt32(2),
              text = ((DbDataReader) npgsqlDataReader).GetString(3)
            });
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return clanRequestList;
    }

    public static int getRequestCount(int clanId)
    {
      int requestCount = 0;
      if (clanId == 0)
        return requestCount;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@clan", (object) clanId);
          ((DbCommand) command).CommandText = "SELECT COUNT(*) FROM clan_invites WHERE clan_id=@clan";
          requestCount = Convert.ToInt32(((DbCommand) command).ExecuteScalar());
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
      return requestCount;
    }

    public static int getRequestClanId(long player_id)
    {
      int requestClanId = 0;
      if (player_id == 0L)
        return requestClanId;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@owner", (object) player_id);
          ((DbCommand) command).CommandText = "SELECT clan_id FROM clan_invites WHERE player_id=@owner";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          if (((DbDataReader) npgsqlDataReader).Read())
            requestClanId = ((DbDataReader) npgsqlDataReader).GetInt32(0);
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
      return requestClanId;
    }

    public static string getRequestText(int clan_id, long player_id)
    {
      if (clan_id == 0 || player_id == 0L)
        return (string) null;
      try
      {
        string requestText = (string) null;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@clan", (object) clan_id);
          command.Parameters.AddWithValue("@player", (object) player_id);
          ((DbCommand) command).CommandText = "SELECT text FROM clan_invites WHERE clan_id=@clan AND player_id=@player";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          if (((DbDataReader) npgsqlDataReader).Read())
            requestText = ((DbDataReader) npgsqlDataReader).GetString(0);
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((DbConnection) npgsqlConnection).Close();
        }
        return requestText;
      }
      catch
      {
        return (string) null;
      }
    }

    public static bool DeleteInviteDb(int clanId, long pId) => pId != 0L && clanId != 0 && ComDiv.deleteDB("clan_invites", "clan_id", (object) clanId, "player_id", (object) pId);

    public static bool DeleteInviteDb(long pId) => pId != 0L && ComDiv.deleteDB("clan_invites", "player_id", (object) pId);

    public static bool CreateInviteInDb(ClanInvite invite)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@clan", (object) invite.clan_id);
          command.Parameters.AddWithValue("@player", (object) invite.player_id);
          command.Parameters.AddWithValue("@date", (object) invite.inviteDate);
          command.Parameters.AddWithValue("@text", (object) invite.text);
          ((DbCommand) command).CommandText = "INSERT INTO clan_invites(clan_id, player_id, dateInvite, text)VALUES(@clan,@player,@date,@text)";
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static void updateBestPlayers(PointBlank.Core.Models.Account.Clan.Clan clan)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@id", (object) clan._id);
          command.Parameters.AddWithValue("@bp1", (object) clan.BestPlayers.Exp.GetSplit());
          command.Parameters.AddWithValue("@bp2", (object) clan.BestPlayers.Participation.GetSplit());
          command.Parameters.AddWithValue("@bp3", (object) clan.BestPlayers.Wins.GetSplit());
          command.Parameters.AddWithValue("@bp4", (object) clan.BestPlayers.Kills.GetSplit());
          command.Parameters.AddWithValue("@bp5", (object) clan.BestPlayers.Headshot.GetSplit());
          ((DbCommand) command).CommandType = CommandType.Text;
          ((DbCommand) command).CommandText = "UPDATE clans SET best_exp=@bp1, best_participation=@bp2, best_wins=@bp3, best_kills=@bp4, best_headshot=@bp5 WHERE clan_id=@id";
          ((DbCommand) command).ExecuteNonQuery();
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static int getClanIdByName(string name)
    {
      if (string.IsNullOrEmpty(name))
        return 0;
      int clanIdByName = 0;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@name", (object) name);
          ((DbCommand) command).CommandText = "SELECT clan_id FROM clans WHERE clan_name=@name";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
            clanIdByName = ((DbDataReader) npgsqlDataReader).GetInt32(0);
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
      return clanIdByName;
    }
  }
}
