// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Network.TimerState
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;
using System.Threading;

namespace PointBlank.Core.Network
{
  public class TimerState
  {
    public Timer Timer = (Timer) null;
    public DateTime EndDate = new DateTime();
    private object sync = new object();

    public void Start(int period, TimerCallback callback)
    {
      lock (this.sync)
      {
        this.Timer = new Timer(callback, (object) this, period, -1);
        this.EndDate = DateTime.Now.AddMilliseconds((double) period);
      }
    }

    public int getTimeLeft()
    {
      if (this.Timer == null)
        return 0;
      int totalSeconds = (int) (this.EndDate - DateTime.Now).TotalSeconds;
      return totalSeconds < 0 ? 0 : totalSeconds;
    }
  }
}
