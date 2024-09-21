// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ : ReceivePacket
  {
    private int formacao;

    public PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.formacao = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Match match = player._match;
        if (match != null && player.matchSlot == match._leader)
        {
          match.formação = this.formacao;
          using (PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK mercenaryReceiverAck = new PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK(0U, this.formacao))
            match.SendPacketToPlayers((SendPacket) mercenaryReceiverAck);
        }
        else
          this._client.SendPacket((SendPacket) new PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK(2147483648U));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
