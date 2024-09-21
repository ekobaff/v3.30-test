// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_AUTH_GET_POINT_CASH_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_REQ : ReceivePacket
    {
        public PROTOCOL_AUTH_GET_POINT_CASH_REQ(GameClient client, byte[] data) => makeme(client, data);

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player != null)
                {
                    _client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, player._gp, player._money, player._tag));
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
