﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.ShopSearch
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Chat
{
  public static class ShopSearch
  {
    public static string SearchGoods(string str, Account player)
    {
      string str1 = str.Substring(6);
      int num = 0;
      string msg = Translation.GetLabel("SearchGoodTitle");
      foreach (GoodItem shopBuyable in ShopManager.ShopBuyableList)
      {
        if (shopBuyable._item._name.Contains(str1))
        {
          msg = msg + "\n" + Translation.GetLabel("SearchGoodInfo", (object) shopBuyable.id, (object) shopBuyable._item._name);
          if (++num >= 15)
            break;
        }
      }
      player.SendPacket((SendPacket) new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(msg));
      return Translation.GetLabel("SearchGoodSuccess", (object) num);
    }
  }
}
