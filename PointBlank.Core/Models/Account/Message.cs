// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Message
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Enums;
using System;
using System.Globalization;

namespace PointBlank.Core.Models.Account
{
  public class Message
  {
    public int object_id;
    public int clanId;
    public int type;
    public int state;
    public long sender_id;
    public long expireDate;
    public string sender_name = "";
    public string text = "";
    public NoteMessageClan cB;
    public int DaysRemaining;

    public Message()
    {
    }

    public Message(long expire, DateTime start)
    {
      this.expireDate = expire;
      this.SetDaysRemaining(start);
    }

    public Message(double days)
    {
      DateTime end = DateTime.Now.AddDays(days);
      this.expireDate = long.Parse(end.ToString("yyMMddHHmm"));
      this.SetDaysRemaining(end, DateTime.Now);
    }

    private void SetDaysRemaining(DateTime now) => this.SetDaysRemaining(DateTime.ParseExact(this.expireDate.ToString(), "yyMMddHHmm", (IFormatProvider) CultureInfo.InvariantCulture), now);

    private void SetDaysRemaining(DateTime end, DateTime now)
    {
      int num = (int) Math.Ceiling((end - now).TotalDays);
      this.DaysRemaining = num < 0 ? 0 : num;
    }
  }
}
