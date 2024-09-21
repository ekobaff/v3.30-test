// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Xml.Card
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Enums;

namespace PointBlank.Core.Xml
{
  public class Card
  {
    public ClassType _weaponReq;
    public MissionType _missionType;
    public int _missionId;
    public int _mapId;
    public int _weaponReqId;
    public int _missionLimit;
    public int _missionBasicId;
    public int _cardBasicId;
    public int _arrayIdx;
    public int _flag;

    public Card(int cardBasicId, int missionBasicId)
    {
      this._cardBasicId = cardBasicId;
      this._missionBasicId = missionBasicId;
      this._arrayIdx = this._cardBasicId * 4 + this._missionBasicId;
      this._flag = 15 << 4 * this._missionBasicId;
    }
  }
}
