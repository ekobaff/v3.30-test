using PointBlank.Auth.Data.Configs;
using PointBlank.Auth.Data.Sync;
using PointBlank.Auth.Data.Xml;
using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using System;
using System.Reflection;
using System.Threading;

namespace PointBlank.Auth
{
  public class Programm
  {
    private static void Main(string[] args)
    {
      string TTT =  ComDiv.GetLinkerTime(Assembly.GetExecutingAssembly()).ToString("dd/MM/yyyy HH:mm");
      Logger.StartedFor = "Auth";
      Logger.checkDirectorys();
      Console.Clear();
      StartConsole();
      AuthConfig.Load();
      ServerConfigSyncer.GenerateConfig(AuthConfig.configId);
      EventLoader.LoadAll();
      BasicInventoryXml.Load();
      CafeInventoryXml.Load();
      ServersXml.Load();
      ChannelsXml.Load(AuthConfig.serverId);
      MissionCardXml.LoadBasicCards(2);
      MapsXml.Load();
      ShopManager.Load(1);
      ShopManager.Load(2);
      //WeaponExpXml.Load();
      RankXml.Load();
      RankXml.LoadAwards();
      RankedXml.Load();
      RankedXml.LoadAwards();
      CouponEffectManager.LoadCouponFlags();
      QuickStartXml.Load();
      MissionsXml.Load();
      ICafeManager.GetList();
      AuthSync.Start();
      Logger.debug(Programm.StartSuccess());
      bool flag = AuthManager.Start();
      if (Logger.erro)
      {
        Logger.error("Check your configuration.");
        Thread.Sleep(5000);
        Environment.Exit(0);
      }
      if (flag)
        PointBlank.Auth.Auth.Update();
            
           while (true) { Console.ReadLine(); }
        }
        private static void StartConsole()
        {
            Logger.LogYaz("" + Environment.NewLine, ConsoleColor.Cyan);
            Logger.LogYaz(@" ____  ____  _  _      _____    ____  _     ____  _      _  __   _    _____    ____     _ ", ConsoleColor.DarkCyan);
            Logger.LogYaz(@"/  __\/  _ \/ \/ \  /|/__ __\  /  __\/ \   /  _ \/ \  /|/ |/ /  / \ |\\__  \  /_   \/\ / |", ConsoleColor.DarkCyan);
            Logger.LogYaz(@"|  \/|| / \|| || |\ ||  / \    | | //| |   | / \|| |\ |||   /   | | //  /  |   /   /\_\| |", ConsoleColor.DarkCyan);
            Logger.LogYaz(@"|  __/| \_/|| || | \||  | |    | |_\\| |_/\| |-||| | \|||   \   | \//  _\  |__/   /_   | |", ConsoleColor.DarkCyan);
            Logger.LogYaz(@"\_/   \____/\_/\_/  \|  \_/    \____/\____/\_/ \|\_/  \|\_|\_\  \__/  /____/\/\____/   \_|", ConsoleColor.DarkCyan);
            Logger.LogYaz("" + Environment.NewLine, ConsoleColor.Cyan);
        }
        private static string StartSuccess() => Logger.erro ? "Startup failed." : "Servidor Iniciado. (" + DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + ")";
  }
}
