﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Sql.SqlConnection
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using Npgsql;
using System.Data.Common;
using System.Runtime.Remoting.Contexts;

namespace PointBlank.Core.Sql
{
  [Synchronization]
  public class SqlConnection
  {
    private static SqlConnection sql = new SqlConnection();
    protected NpgsqlConnectionStringBuilder connBuilder;

    public SqlConnection() => this.connBuilder = new NpgsqlConnectionStringBuilder()
    {
      Database = Config.dbName,
      Host = Config.dbHost,
      Username = Config.dbUser,
      Password = Config.dbPass,
      Port = Config.dbPort
    };

    public static SqlConnection getInstance() => SqlConnection.sql;

    public NpgsqlConnection conn() => new NpgsqlConnection(((DbConnectionStringBuilder) this.connBuilder).ConnectionString);
  }
}
