// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Auth
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using System;
using System.Threading.Tasks;

namespace PointBlank.Auth
{
  public class Auth
  {
    public static async void Update()
    {
      while (true)
      {
        Console.Title = "[CG]AUTH [R]: " + (GC.GetTotalMemory(true) / 1024L).ToString() + " KB]";
        await Task.Delay(5000);
      }
    }
  }
}
