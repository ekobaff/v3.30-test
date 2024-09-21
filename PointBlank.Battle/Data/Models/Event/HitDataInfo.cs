// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.Event.HitDataInfo
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Enums;
using SharpDX;
using System.Collections.Generic;

namespace PointBlank.Battle.Data.Models.Event
{
  public class HitDataInfo
  {
    public byte Extensions;
    public ushort BoomInfo;
    public uint HitIndex;
    public int WeaponId;
    public Half3 StartBullet;
    public Half3 EndBullet;
    public List<int> BoomPlayers;
    public HIT_TYPE HitEnum;
    public CLASS_TYPE WeaponClass;
  }
}
