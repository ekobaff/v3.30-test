// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Gift.TicketModel
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Enums;

namespace PointBlank.Core.Models.Gift
{
  public class TicketModel
  {
    public string Ticket;
    public int ItemId;
    public string ItemName;
    public int Count;
    public int Equip;
    public int Point;
    public int Cash;
    public int Tag;
    public int MaxUse;
    public TicketType Type;

    public TicketModel(TicketType Type, string Ticket)
    {
      this.Type = Type;
      this.Ticket = Ticket;
    }
  }
}
