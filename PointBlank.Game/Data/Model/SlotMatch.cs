// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Model.SlotMatch
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Enums;

namespace PointBlank.Game.Data.Model
{
  public class SlotMatch
  {
    public SlotMatchState state;
    public long _playerId;
    public long _id;

    public SlotMatch(int slot) => this._id = (long) slot;
  }
}
