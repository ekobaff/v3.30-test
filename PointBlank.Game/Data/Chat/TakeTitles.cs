// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Chat.TakeTitles
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Title;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Chat
{
  public static class TakeTitles
  {
    public static string GetAllTitles(PointBlank.Game.Data.Model.Account p)
    {
      if (p._titles.ownerId == 0L)
      {
        TitleManager.getInstance().CreateTitleDB(p.player_id);
        p._titles = new PlayerTitles()
        {
          ownerId = p.player_id
        };
      }
      PlayerTitles titles = p._titles;
      int num = 0;
      for (int titleId = 1; titleId <= 44; ++titleId)
      {
        TitleQ title = TitlesXml.getTitle(titleId);
        if (title != null && !titles.Contains(title._flag))
        {
          ++num;
          titles.Add(title._flag);
          if (titles.Slots < title._slot)
            titles.Slots = title._slot;

        }
      }
      if (num > 0)
      {
        ComDiv.updateDB("player_titles", "titleslots", (object) titles.Slots, "owner_id", (object) p.player_id);
        TitleManager.getInstance().updateTitlesFlags(p.player_id, titles.Flags);
        p.SendPacket((SendPacket) new PROTOCOL_BASE_USER_TITLE_INFO_ACK(p));

                //Adicionado
                p.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, new ItemsModel(2700014, "Title Reward", 3, 1)));
                p.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, new ItemsModel(2700016, "Title Reward", 3, 1)));
                p.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, new ItemsModel(2700017, "Title Reward", 3, 1)));
                p.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, new ItemsModel(2700015, "Title Reward", 3, 1)));
                p.SendPacket((SendPacket)new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, new ItemsModel(2700018, "Title Reward", 3, 1)));

        return "Titulos liberado [Relogue]";
        }
        else
        {
            return "Você já liberou seus titulos";
        }
      //return Translation.GetLabel("TitleAcquisiton", (object) num);
     
    }
  }
}
