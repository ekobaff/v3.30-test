// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Randombox.RandomBoxModel
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System.Collections.Generic;

namespace PointBlank.Core.Models.Randombox
{
  public class RandomBoxModel
  {
    public int itemsCount;
    public int minPercent;
    public int maxPercent;
    public List<RandomBoxItem> items = new List<RandomBoxItem>();

    public List<RandomBoxItem> getRewardList(List<RandomBoxItem> sortedList, int rnd)
    {
      List<RandomBoxItem> rewardList = new List<RandomBoxItem>();
      if (sortedList.Count > 0)
      {
        int index1 = sortedList[rnd].index;
        for (int index2 = 0; index2 < sortedList.Count; ++index2)
        {
          RandomBoxItem sorted = sortedList[index2];
          if (sorted.index == index1)
            rewardList.Add(sorted);
        }
      }
      return rewardList;
    }

    public List<RandomBoxItem> getSortedList(int percent)
    {
      if (percent < this.minPercent)
        percent = this.minPercent;
      List<RandomBoxItem> sortedList = new List<RandomBoxItem>();
      for (int index = 0; index < this.items.Count; ++index)
      {
        RandomBoxItem randomBoxItem = this.items[index];
        if (percent <= randomBoxItem.percent)
          sortedList.Add(randomBoxItem);
      }
      return sortedList;
    }

    public void SetTopPercent()
    {
      int num1 = 100;
      int num2 = 0;
      for (int index = 0; index < this.items.Count; ++index)
      {
        RandomBoxItem randomBoxItem = this.items[index];
        if (randomBoxItem.percent < num1)
          num1 = randomBoxItem.percent;
        if (randomBoxItem.percent > num2)
          num2 = randomBoxItem.percent;
      }
      this.minPercent = num1;
      this.maxPercent = num2;
    }
  }
}
