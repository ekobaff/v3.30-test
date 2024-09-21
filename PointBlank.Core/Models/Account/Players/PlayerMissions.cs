// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Account.Players.PlayerMissions
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Network;

namespace PointBlank.Core.Models.Account.Players
{
  public class PlayerMissions
  {
    public byte[] list1 = new byte[40];
    public byte[] list2 = new byte[40];
    public byte[] list3 = new byte[40];
    public byte[] list4 = new byte[40];
    public int actualMission;
    public int card1;
    public int card2;
    public int card3;
    public int card4;
    public int mission1;
    public int mission2;
    public int mission3;
    public int mission4;
    public bool selectedCard;

    public PlayerMissions DeepCopy() => (PlayerMissions) this.MemberwiseClone();

    public byte[] getCurrentMissionList()
    {
      if (this.actualMission == 0)
        return this.list1;
      if (this.actualMission == 1)
        return this.list2;
      return this.actualMission == 2 ? this.list3 : this.list4;
    }

    public int getCurrentCard() => this.getCard(this.actualMission);

    public int getCard(int index)
    {
      switch (index)
      {
        case 0:
          return this.card1;
        case 1:
          return this.card2;
        case 2:
          return this.card3;
        default:
          return this.card4;
      }
    }

    public int getCurrentMissionId()
    {
      if (this.actualMission == 0)
        return this.mission1;
      if (this.actualMission == 1)
        return this.mission2;
      return this.actualMission == 2 ? this.mission3 : this.mission4;
    }

    public void UpdateSelectedCard()
    {
      int currentCard = this.getCurrentCard();
      if (ushort.MaxValue != ComDiv.getCardFlags(this.getCurrentMissionId(), currentCard, this.getCurrentMissionList()))
        return;
      this.selectedCard = true;
    }
  }
}
