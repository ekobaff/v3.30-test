// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Room.FragInfos
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace PointBlank.Core.Models.Room
{
  public class FragInfos
  {
    public byte killerIdx;
    public byte killsCount;
    public byte flag;
    public CharaKillType killingType;
    public int weapon;
    public int Score;
    public float x;
    public float y;
    public float z;
    public List<Frag> frags = new List<Frag>();

    public KillingMessage GetAllKillFlags()
    {
      KillingMessage allKillFlags = (KillingMessage) 0;
      for (int index = 0; index < this.frags.Count; ++index)
      {
        Frag frag = this.frags[index];
        if (!allKillFlags.HasFlag((Enum) frag.killFlag))
          allKillFlags |= frag.killFlag;
      }
      return allKillFlags;
    }
  }
}
