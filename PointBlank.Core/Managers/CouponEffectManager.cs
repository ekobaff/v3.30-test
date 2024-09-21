// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.CouponEffectManager
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace PointBlank.Core.Managers
{
  public static class CouponEffectManager
  {
    private static List<CouponFlag> Effects = new List<CouponFlag>();

    public static void LoadCouponFlags()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          ((DbConnection) npgsqlConnection).Open();
          ((DbCommand) command).CommandText = "SELECT * FROM server_effect_flags";
          ((DbCommand) command).CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (((DbDataReader) npgsqlDataReader).Read())
          {
            CouponFlag couponFlag = new CouponFlag()
            {
              ItemId = ((DbDataReader) npgsqlDataReader).GetInt32(0),
              EffectFlag = (CouponEffects) ((DbDataReader) npgsqlDataReader).GetInt64(1)
            };
            CouponEffectManager.Effects.Add(couponFlag);
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

    public static CouponFlag getCouponEffect(int id)
    {
      for (int index = 0; index < CouponEffectManager.Effects.Count; ++index)
      {
        CouponFlag effect = CouponEffectManager.Effects[index];
        if (effect.ItemId == id)
          return effect;
      }
      return (CouponFlag) null;
    }
  }
}
