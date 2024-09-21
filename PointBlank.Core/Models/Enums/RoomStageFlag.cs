// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Enums.RoomStageFlag
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;

namespace PointBlank.Core.Models.Enums
{
  [Flags]
  public enum RoomStageFlag
  {
    NONE = 0,
    RANDOM_MAP = 2,
    PASSWORD = 4,
    OBSERVER_MODE = 8,
    REAL_IP = 16, // 0x00000010
    TEAM_BALANCE = 32, // 0x00000020
    OBSERVER = 64, // 0x00000040
    INTER_ENTER = 128, // 0x00000080
  }
}
