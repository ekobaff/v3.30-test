// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Enums.KillingMessage
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Models.Enums
{
  public enum KillingMessage
  {
    PiercingShot = 1,
    MassKill = 2,
    ChainStopper = 4,
    Headshot = 8,
    ChainHeadshot = 16, // 0x00000010
    ChainSlugger = 32, // 0x00000020
    Suicide = 64, // 0x00000040
    ObjectDefense = 128, // 0x00000080
  }
}
