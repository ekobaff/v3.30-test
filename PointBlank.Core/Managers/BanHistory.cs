// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.BanHistory
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;

namespace PointBlank.Core.Managers
{
  public class BanHistory
  {
    public long object_id;
    public long provider_id;
    public string type;
    public string value;
    public string reason;
    public DateTime startDate;
    public DateTime endDate;

    public BanHistory()
    {
      this.startDate = DateTime.Now;
      this.type = "";
      this.value = "";
      this.reason = "";
    }
  }
}
