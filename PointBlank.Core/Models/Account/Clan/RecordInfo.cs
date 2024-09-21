// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Clan.RecordInfo
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Models.Account.Clan
{
  public class RecordInfo
  {
    public long PlayerId;
    public int RecordValue;

    public RecordInfo(string[] split)
    {
      this.PlayerId = this.GetPlayerId(split);
      this.RecordValue = this.GetPlayerValue(split);
    }

    public long GetPlayerId(string[] split)
    {
      try
      {
        return long.Parse(split[0]);
      }
      catch
      {
        return 0;
      }
    }

    public int GetPlayerValue(string[] split)
    {
      try
      {
        return int.Parse(split[1]);
      }
      catch
      {
        return 0;
      }
    }

    public string GetSplit() => this.PlayerId.ToString() + "-" + this.RecordValue.ToString();
  }
}
