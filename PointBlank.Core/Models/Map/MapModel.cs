// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Map.MapModel
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System.Collections.Generic;
using System.Linq;

namespace PointBlank.Core.Models.Map
{
  public static class MapModel
  {
    public static List<MapRule> Rules = new List<MapRule>();
    public static List<MapMatch> Matchs = new List<MapMatch>();

        // Token: 0x0600011F RID: 287 RVA: 0x00009214 File Offset: 0x00007414
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int limit)
        {
            return from x in list.Select((T item, int inx) => new
            {
                item,
                inx
            })
                   group x by x.inx / limit into g
                   select
                       from x in g
                       select x.item;
        }
        public static MapRule getRule(int Mode)
    {
      for (int index = 0; index < MapModel.Rules.Count; ++index)
      {
        MapRule rule = MapModel.Rules[index];
        if (rule != null && rule.Id == Mode)
          return rule;
      }
      return (MapRule) null;
    }
  }
}
