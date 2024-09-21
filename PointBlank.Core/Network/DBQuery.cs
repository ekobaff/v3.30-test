// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Network.DBQuery
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System.Collections.Generic;

namespace PointBlank.Core.Network
{
  public class DBQuery
  {
    private List<string> tables;
    private List<object> values;

    public DBQuery()
    {
      this.tables = new List<string>();
      this.values = new List<object>();
    }

    public void AddQuery(string table, object value)
    {
      this.tables.Add(table);
      this.values.Add(value);
    }

    public string[] GetTables() => this.tables.ToArray();

    public object[] GetValues() => this.values.ToArray();
  }
}
