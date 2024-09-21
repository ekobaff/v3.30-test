// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Managers.Events.EventLoader
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Managers.Events
{
  public static class EventLoader
  {
    public static void LoadAll()
    {
      EventVisitSyncer.GenerateList();
      EventLoginSyncer.GenerateList();
      EventMapSyncer.GenerateList();
      EventPlayTimeSyncer.GenerateList();
      EventQuestSyncer.GenerateList();
      EventRankUpSyncer.GenerateList();
      EventXmasSyncer.GenerateList();
    }

    public static void ReloadEvent(int index)
    {
      switch (index)
      {
        case 0:
          EventVisitSyncer.ReGenList();
          break;
        case 1:
          EventLoginSyncer.ReGenList();
          break;
        case 2:
          EventMapSyncer.ReGenList();
          break;
        case 3:
          EventPlayTimeSyncer.ReGenList();
          break;
        case 4:
          EventQuestSyncer.ReGenList();
          break;
        case 5:
          EventRankUpSyncer.ReGenList();
          break;
        case 6:
          EventXmasSyncer.ReGenList();
          break;
      }
    }

    public static void ReloadAll()
    {
      EventVisitSyncer.ReGenList();
      EventLoginSyncer.ReGenList();
      EventMapSyncer.ReGenList();
      EventPlayTimeSyncer.ReGenList();
      EventQuestSyncer.ReGenList();
      EventRankUpSyncer.ReGenList();
      EventXmasSyncer.ReGenList();
    }
  }
}
