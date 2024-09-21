// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Sql.SqlConnection
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using Npgsql;
using PointBlank.Battle.Data.Configs;
using System.Runtime.Remoting.Contexts;

namespace PointBlank.Battle.Data.Sql
{
  [Synchronization]
  public class SqlConnection
  {
    private static SqlConnection sql = new SqlConnection();
    protected NpgsqlConnectionStringBuilder connBuilder;

    public SqlConnection() => this.connBuilder = new NpgsqlConnectionStringBuilder()
    {
      Database = BattleConfig.dbName,
      Host = BattleConfig.dbHost,
      Username = BattleConfig.dbUser,
      Password = BattleConfig.dbPass,
      Port = BattleConfig.dbPort
    };

    public static SqlConnection getInstance() => SqlConnection.sql;

    public NpgsqlConnection conn() => new NpgsqlConnection(this.connBuilder.ConnectionString);
  }
}
