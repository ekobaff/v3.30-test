// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Mission.MissionAwards
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Models.Account.Mission
{
  public class MissionAwards
  {
    public int _id;
    public int _blueOrder;
    public int _exp;
    public int _gp;

    public MissionAwards(int id, int blueOrder, int exp, int gp)
    {
      this._id = id;
      this._blueOrder = blueOrder;
      this._exp = exp;
      this._gp = gp;
    }
  }
}
