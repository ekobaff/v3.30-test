// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Filters.NickFilter
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace PointBlank.Core.Filters
{
  public static class NickFilter
  {
    public static List<string> _filter = new List<string>();

    public static void Load()
    {
      if (File.Exists("Data/Filters/Nicks.txt"))
      {
        try
        {
          using (StreamReader streamReader = new StreamReader("Data/Filters/Nicks.txt"))
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
              NickFilter._filter.Add(str);
            streamReader.Close();
          }
        }
        catch (Exception ex)
        {
          Logger.error("Filter: " + ex.ToString());
        }
      }
      else
        Logger.warning("Filter file does not exist.");
    }
  }
}
