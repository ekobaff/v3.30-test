// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.Event.GrenadeHitInfo
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Enums;
using SharpDX;
using System.Collections.Generic;

namespace PointBlank.Battle.Data.Models.Event
{
  public class GrenadeHitInfo
  {
    public byte DeathType;
    public byte Extensions;
    public ushort BoomInfo;
    public ushort GrenadesCount;
    public uint HitInfo;
    public int WeaponId;
    public List<int> BoomPlayers;
    public Half3 FirePos;
    public Half3 HitPos;
    public Half3 PlayerPos;
    public HIT_TYPE HitEnum;
    public CLASS_TYPE WeaponClass;
  }
}
