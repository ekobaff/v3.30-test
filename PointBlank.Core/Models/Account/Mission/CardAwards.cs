// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Mission.CardAwards
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Models.Account.Mission
{
  public class CardAwards
  {
    public int _id;
    public int _card;
    public int _insignia;
    public int _medal;
    public int _brooch;
    public int _exp;
    public int _gp;

    public bool Unusable() => this._insignia == 0 && this._medal == 0 && this._brooch == 0 && this._exp == 0 && this._gp == 0;
  }
}
