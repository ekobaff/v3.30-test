// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Friend
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Account.Players;

namespace PointBlank.Core.Models.Account
{
  public class Friend
  {
    public long player_id;
    public int state;
    public bool removed;
    public PlayerInfo player;

    public Friend(long player_id)
    {
      this.player_id = player_id;
      this.player = new PlayerInfo(player_id);
    }

    public Friend(
      long player_id,
      int rank,
      int name_color,
      string name,
      bool isOnline,
      AccountStatus status)
    {
      this.player_id = player_id;
      this.SetModel(player_id, rank, name_color, name, isOnline, status);
    }

    public void SetModel(
      long player_id,
      int rank,
      int name_color,
      string name,
      bool isOnline,
      AccountStatus status)
    {
      this.player = new PlayerInfo(player_id, rank, name_color, name, isOnline, status);
    }
  }
}
