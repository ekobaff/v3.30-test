// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Room.VoteKick
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System.Collections.Generic;

namespace PointBlank.Core.Models.Room
{
  public class VoteKick
  {
    public int creatorIdx;
    public int victimIdx;
    public int motive;
    public int kikar = 1;
    public int deixar = 1;
    public int allies;
    public int enemys;
    public List<int> _votes = new List<int>();
    public bool[] TotalArray = new bool[16];

    public VoteKick(int creator, int victim)
    {
      this.creatorIdx = creator;
      this.victimIdx = victim;
      this._votes.Add(creator);
      this._votes.Add(victim);
    }

    public int GetInGamePlayers()
    {
      int inGamePlayers = 0;
      for (int index = 0; index < 16; ++index)
      {
        if (this.TotalArray[index])
          ++inGamePlayers;
      }
      return inGamePlayers;
    }
  }
}
