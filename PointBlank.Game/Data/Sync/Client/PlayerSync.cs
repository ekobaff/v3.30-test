// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Client.PlayerSync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Data.Sync.Client
{
  public static class PlayerSync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      int num1 = (int) p.readC();
      int num2 = (int) p.readC();
      int num3 = p.readD();
      int num4 = p.readD();
      Account account = AccountManager.getAccount(id, true);
      if (account == null || num1 != 0)
        return;
      account._rank = num2;
      account._gp = num3;
      account._money = num4;
    }

        public static void ExIP(ReceiveGPacket p)
        {
            string ip = p.readS(p.readC());
            foreach (var client in GameManager._socketList.Values)
            {
                Account account = client._player;

                if (account != null && account._isOnline && account.PublicIP.ToString() == ip)
                {
                    account.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(0));
                    string str = account.player_name;
            
                    account.Close(1000);
                    if (ComDiv.updateDB("players", "access_level", -1, "player_id", account.player_id))
                    {
                        Logger.info($" {str} foi desconectado, tentando bugar cash/gold e patente");
                    }
                }
            }
        }
    }
}
