// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.VisitItem
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Models.Account
{
  public class VisitItem
  {
    public int good_id;
    public long count;
    public bool IsReward;

    public void SetCount(string text)
    {
      this.count = long.Parse(text);
      if (this.count <= 0L)
        return;
      this.IsReward = true;
    }
  }
}
