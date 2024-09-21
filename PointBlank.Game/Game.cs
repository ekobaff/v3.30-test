// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Game
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Chat;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace PointBlank.Game
{
  public static class Game
  {
    public static bool WeekEnd = true;
    public static bool Week = true;
    public static async void Update()
    {
      while (true)
      {
        Console.Title = "[CG]GAME [S]: " + GameManager._socketList.Count.ToString() + " [O]: " + ServersXml.getServer(GameConfig.serverId)._LastCount.ToString() + " [R]: " + (GC.GetTotalMemory(true) / 1024L).ToString() + " KB]";
        
        DateTime DateCurrent = DateTime.Now;
        string DailyDayTime = DateCurrent.ToString("HH:mm", CultureInfo.InvariantCulture);
        //string RankedDate = DateCurrent.ToString("yyyy-MM-dd HH:mm:ss");

       if (DailyDayTime == "00:00")   
        {
          foreach (PointBlank.Game.Data.Model.Account account in (IEnumerable<PointBlank.Game.Data.Model.Account>) AccountManager._accounts.Values)
          {
            if (account != null)
              account.Daily = new PlayerDailyRecord();
          }
          foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
          {
            if (gameClient != null && gameClient._player != null && gameClient._player._isOnline)
              gameClient._player.Daily = new PlayerDailyRecord();
          }
          ComDiv.updateDB("player_dailyrecord", "total", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "wins", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "loses", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "draws", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "kills", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "deaths", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "headshots", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "point", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "exp", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "playtime", (object) 0);
          ComDiv.updateDB("player_dailyrecord", "money", (object) 0);

          
           }

                if (DateCurrent.Date == GameManager.Config.RankedDate.Date && GameManager.Config.RankedEnable)
                {
                    foreach (PointBlank.Game.Data.Model.Account account in (IEnumerable<PointBlank.Game.Data.Model.Account>)AccountManager._accounts.Values)
                    {
                        if (account != null)
                            account.Ranked = new PlayerRanked();
                    }
                    foreach (GameClient gameClient in (IEnumerable<GameClient>)GameManager._socketList.Values)
                    {
                        if (gameClient != null && gameClient._player != null && gameClient._player._isOnline)
                            gameClient._player.Ranked = new PlayerRanked();
                            gameClient._player.tourneyLevel = 0;
                    }
                    ComDiv.updateDB("player_ranked", "rank", (object)0);
                    ComDiv.updateDB("player_ranked", "exp", (object)0);
                    ComDiv.updateDB("player_ranked", "matches", (object)0);
                    ComDiv.updateDB("player_ranked", "wins", (object)0);
                    ComDiv.updateDB("player_ranked", "loses", (object)0);
                    ComDiv.updateDB("player_ranked", "kills", (object)0);
                    ComDiv.updateDB("player_ranked", "deaths", (object)0);
                    ComDiv.updateDB("player_ranked", "headshots", (object)0);
                    ComDiv.updateDB("player_ranked", "playtime", (object)0);
                    ComDiv.updateDB("players", "ranked_point", (object)0);
                    GameManager.Config.RankedEnable = false;
                    ComDiv.updateDB("server_settings", "enable_ranked", (object)false);

                    //Logger.LogCMD("Reset Ranked -> " + DateCurrent.ToString("yyyy-MM-dd HH:mm:ss"));
                    
                    Logger.console($"Ranked Finished");
                }       

                if (DateCurrent.DayOfWeek == DayOfWeek.Friday && WeekEnd)
                    {
                    //int[] numbers = { 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000 };
                    int[] numbers = { 10000, 11000, 12000, 13000, 14000, 15000};
                    Random random = new Random();
                    int randomNumber = numbers[random.Next(numbers.Length)];

                    if (ComDiv.updateDB("server_events_rankup", "percent_xp", (object)randomNumber))
                        //Logger.LogCMD($"Event RankUP  EXP: {randomNumber:n0}% -> {DateCurrent.ToString("yyyy-MM-dd HH:mm:ss")} ");
                        Logger.console($"[ON] Event RankUP  EXP: {randomNumber:n0}%");

                    EventRankUpSyncer.ReGenList();
                    WeekEnd = false;
                    Week = true;

                    }
                    else if(DateCurrent.DayOfWeek == DayOfWeek.Monday && Week)
                    {
                    if (ComDiv.updateDB("server_events_rankup", "percent_xp", (object)5000))
                        //Logger.LogCMD($"Event RankUP  Desativado -> {DateCurrent.ToString("yyyy-MM-dd HH:mm:ss")} ");
                        Logger.console($"[OFF] Event RankUP  EXP: {5000:n0}%");
                    EventRankUpSyncer.ReGenList();
                    Week = false;
                    WeekEnd = true;
                     }
                
       await Task.Delay(5000);
      }
    }
  }
}
