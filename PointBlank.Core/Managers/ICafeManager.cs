// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.ICafeManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public static class ICafeManager
  {
    public static List<ICafe> GetList()
    {
      List<ICafe> list = new List<ICafe>();
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM pc_icafe";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            ICafe icafe = new ICafe(((DbDataReader) npgsqlDataReader).GetString(2));
            list.Add(icafe);
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
      return list;
    }

    public static bool GetCafe(string Ip)
    {
      bool cafe = false;
      if (Ip == "")
        cafe = false;
      for (int index = 0; index < ICafeManager.GetList().Count; ++index)
      {
        ICafe icafe = ICafeManager.GetList()[index];
        cafe = Ip == icafe.Ip;
      }
      return cafe;
    }
  }
}
