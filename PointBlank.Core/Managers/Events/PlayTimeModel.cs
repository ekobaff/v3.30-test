// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.PlayTimeModel
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;

namespace PointBlank.Core.Managers.Events
{
  public class PlayTimeModel
  {
    public int _goodReward1;
    public int _goodReward2;
    public long _goodCount1;
    public long _goodCount2;
    public uint _startDate;
    public uint _endDate;
    public string _title = "";
    public long _time;

    public bool EventIsEnabled()
    {
      uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      return this._startDate <= num && num < this._endDate;
    }

    public long GetRewardCount(int goodId) => goodId == this._goodReward1 ? this._goodCount1 : (goodId == this._goodReward2 ? this._goodCount2 : 0L);
  }
}
