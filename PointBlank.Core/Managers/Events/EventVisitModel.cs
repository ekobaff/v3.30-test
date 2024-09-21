// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventVisitModel
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Account;
using System;
using System.Collections.Generic;

namespace PointBlank.Core.Managers.Events
{
  public class EventVisitModel
  {
    public int id;
    public int checks = 7;
    public uint startDate;
    public uint endDate;
    public string title = "";
    public List<VisitBox> box = new List<VisitBox>();

    public bool EventIsEnabled()
    {
      uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      return this.startDate <= num && num < this.endDate;
    }

    public VisitItem getReward(int idx, int rewardIdx)
    {
      try
      {
        return rewardIdx == 0 ? this.box[idx].reward1 : this.box[idx].reward2;
      }
      catch
      {
        return (VisitItem) null;
      }
    }

    public void SetBoxCounts(int count)
    {
      for (int index = 0; index < count; ++index)
        this.box[index].SetCount();
    }
  }
}
