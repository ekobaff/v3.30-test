// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.BanManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public static class BanManager
  {
    public static BanHistory GetAccountBan(long object_id)
    {
      BanHistory accountBan = new BanHistory();
      if (object_id == 0L)
        return accountBan;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@obj", (object) object_id);
          ((DbCommand) command).CommandText = "SELECT * FROM ban_history WHERE object_id=@obj";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            accountBan.object_id = object_id;
            accountBan.provider_id = ((DbDataReader) npgsqlDataReader).GetInt64(1);
            accountBan.type = ((DbDataReader) npgsqlDataReader).GetString(2);
            accountBan.value = ((DbDataReader) npgsqlDataReader).GetString(3);
            accountBan.reason = ((DbDataReader) npgsqlDataReader).GetString(4);
            accountBan.startDate = ((DbDataReader) npgsqlDataReader).GetDateTime(5);
            accountBan.endDate = ((DbDataReader) npgsqlDataReader).GetDateTime(6);
          }
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
        return (BanHistory) null;
      }
      return accountBan;
    }

    public static List<string> GetHwIdList()
    {
      List<string> hwIdList = new List<string>();
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM ban_hwid";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            string str = ((DbDataReader) npgsqlDataReader).GetString(0);
            if (str != null || str.Length != 0)
              hwIdList.Add(str);
          }
          ((Component) command).Dispose();
          ((DbDataReader) npgsqlDataReader).Close();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
        return (List<string>) null;
      }
      return hwIdList;
    }

    public static void GetBanStatus(string mac, string ip, out bool validMac, out bool validIp)
    {
      validMac = false;
      validIp = false;
      try
      {
        DateTime now = DateTime.Now;
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@mac", (object) mac);
          command.Parameters.AddWithValue("@ip", (object) ip);
          ((DbCommand) command).CommandText = "SELECT * FROM ban_history WHERE value in (@mac, @ip)";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            string str1 = ((DbDataReader) npgsqlDataReader).GetString(2);
            string str2 = ((DbDataReader) npgsqlDataReader).GetString(3);
            if (!(((DbDataReader) npgsqlDataReader).GetDateTime(6) < now))
            {
              if (str1 == "MAC" && str2 == mac)
                validMac = true;
              else if (str1 == "IP" && str2 == ip)
                validIp = true;
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
        Logger.warning(ex.ToString());
      }
    }

    public static BanHistory SaveHistory(long provider, string type, string value, DateTime end)
    {
      BanHistory banHistory = new BanHistory()
      {
        provider_id = provider,
        type = type,
        value = value,
        endDate = end
      };
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          command.Parameters.AddWithValue("@provider", (object) banHistory.provider_id);
          command.Parameters.AddWithValue("@type", (object) banHistory.type);
          command.Parameters.AddWithValue("@value", (object) banHistory.value);
          command.Parameters.AddWithValue("@reason", (object) banHistory.reason);
          command.Parameters.AddWithValue("@start", (object) banHistory.startDate);
          command.Parameters.AddWithValue("@end", (object) banHistory.endDate);
          ((DbCommand) command).CommandText = "INSERT INTO ban_history(provider_id,type,value,reason,start_date,expire_date)VALUES(@provider,@type,@value,@reason,@start,@end) RETURNING object_id";
          object obj = ((DbCommand) command).ExecuteScalar();
          banHistory.object_id = (long) obj;
          ((Component) command).Dispose();
          ((Component) npgsqlConnection).Dispose();
          ((DbConnection) npgsqlConnection).Close();
          return banHistory;
        }
      }
      catch
      {
        return (BanHistory) null;
      }
    }

    public static bool SaveBanReason(long object_id, string reason) => ComDiv.updateDB("ban_history", nameof (reason), (object) reason, nameof (object_id), (object) object_id);
  }
}
