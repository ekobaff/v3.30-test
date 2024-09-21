// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Program
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Configs;

using PointBlank.Battle.Data.Sync;
using PointBlank.Battle.Data.Xml;
using PointBlank.Battle.Network;
using System;
using System.Threading.Tasks;

namespace PointBlank.Battle
{
  internal class Program
  {
    protected static void Main(string[] args)
    {
      BattleConfig.Load();
      Logger.checkDirectory();
      Console.Clear();
      StartConsole();
      MapXml.Load();
      CharaXml.Load();
      MeleeExceptionsXml.Load();
      ServersXml.Load();
      BattleSync.Start();
      BattleManager.Connect();
      Program.Update();

            while (true) { Console.ReadLine(); }
        }

    protected static async void Update()
    {
      while (true)
      {
        Console.Title = "[CG]BATTLE [R]" + (GC.GetTotalMemory(true) / 1024L).ToString() + " KB]";
                
                await Task.Delay(5000);
      }
    }
        private static void StartConsole()
        {
            Logger.LogYaz("" + Environment.NewLine, ConsoleColor.Red);
            Logger.LogYaz(@" ____  ____  _  _      _____    ____  _     ____  _      _  __   _    _____    ____     _ ", ConsoleColor.DarkRed);
            Logger.LogYaz(@"/  __\/  _ \/ \/ \  /|/__ __\  /  __\/ \   /  _ \/ \  /|/ |/ /  / \ |\\__  \  /_   \/\ / |", ConsoleColor.DarkRed);
            Logger.LogYaz(@"|  \/|| / \|| || |\ ||  / \    | | //| |   | / \|| |\ |||   /   | | //  /  |   /   /\_\| |", ConsoleColor.DarkRed);
            Logger.LogYaz(@"|  __/| \_/|| || | \||  | |    | |_\\| |_/\| |-||| | \|||   \   | \//  _\  |__/   /_   | |", ConsoleColor.DarkRed);
            Logger.LogYaz(@"\_/   \____/\_/\_/  \|  \_/    \____/\____/\_/ \|\_/  \|\_|\_\  \__/  /____/\/\____/   \_|", ConsoleColor.DarkRed);
            Logger.LogYaz("" + Environment.NewLine, ConsoleColor.Red);
        }
    }
}
