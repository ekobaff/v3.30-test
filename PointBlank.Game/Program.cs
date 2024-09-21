// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Programm
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using Game.data.managers;
using PointBlank.Core;
using PointBlank.Core.Filters;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace PointBlank.Game
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            ComDiv.GetLinkerTime(Assembly.GetExecutingAssembly()).ToString("dd/MM/yyyy HH:mm");
            Logger.StartedFor = "Game";
            Logger.checkDirectorys();
            Console.Clear();
            StartConsole();
            GameConfig.Load();
           
            BasicInventoryXml.Load();
            CafeInventoryXml.Load();
            ServerConfigSyncer.GenerateConfig(GameConfig.configId);
            ServersXml.Load();
            ChannelsXml.Load(GameConfig.serverId);
            EventLoader.LoadAll();
            TitlesXml.Load();
            TitleAwardsXml.Load();
            ClanManager.Load();
            NickFilter.Load();
            MissionCardXml.LoadBasicCards(1);
            RankedXml.Load();
            RankedXml.LoadAwards();
            RankXml.Load();
            WeaponExpXml.Load();
            BattleServerXml.Load();
            RankXml.LoadAwards();
            ClanRankXml.Load();
            MissionAwardsXml.Load();
            MissionsXml.Load();
            Translation.Load();
            ShopManager.Load(1);
            BattleBoxManager.Load();
            ClassicModeManager.LoadList();
            MapsXml.Load();
            RandomBoxXml.LoadBoxes();
            ICafeManager.GetList();
            CouponEffectManager.LoadCouponFlags();
            GameSync.Start();

            if (Logger.erro)
            {
                Logger.error("Check your configuration.");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
            if (GameManager.Start())
                PointBlank.Game.Game.Update();


            while (true) { Console.ReadLine(); }
        } 
        private static void StartConsole()
        {
            Logger.LogYaz("" + Environment.NewLine, ConsoleColor.Green);
            Logger.LogYaz(@" ____  ____  _  _      _____    ____  _     ____  _      _  __   _    _____    ____     _ ", ConsoleColor.DarkGreen);
            Logger.LogYaz(@"/  __\/  _ \/ \/ \  /|/__ __\  /  __\/ \   /  _ \/ \  /|/ |/ /  / \ |\\__  \  /_   \/\ / |", ConsoleColor.DarkGreen);
            Logger.LogYaz(@"|  \/|| / \|| || |\ ||  / \    | | //| |   | / \|| |\ |||   /   | | //  /  |   /   /\_\| |", ConsoleColor.DarkGreen);
            Logger.LogYaz(@"|  __/| \_/|| || | \||  | |    | |_\\| |_/\| |-||| | \|||   \   | \//  _\  |__/   /_   | |", ConsoleColor.DarkGreen);
            Logger.LogYaz(@"\_/   \____/\_/\_/  \|  \_/    \____/\____/\_/ \|\_/  \|\_|\_\  \__/  /____/\/\____/   \_|", ConsoleColor.DarkGreen);
            Logger.LogYaz("" + Environment.NewLine, ConsoleColor.Green);
        }
    }
}
