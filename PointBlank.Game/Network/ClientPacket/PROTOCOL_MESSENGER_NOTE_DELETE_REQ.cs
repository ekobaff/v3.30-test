// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_MESSENGER_NOTE_DELETE_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_MESSENGER_NOTE_DELETE_REQ : ReceivePacket
  {
    private uint erro;
    private List<object> objs = new List<object>();

    public PROTOCOL_MESSENGER_NOTE_DELETE_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      int num = (int) this.readC();
      for (int index = 0; index < num; ++index)
        this.objs.Add((object) this.readD());
    }

    public override void run()
    {
      if (this._client._player == null)
        return;
      try
      {
        if (!MessageManager.DeleteMessages(this.objs, this._client.player_id))
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new PROTOCOL_MESSENGER_NOTE_DELETE_ACK(this.erro, this.objs));
        this.objs = (List<object>) null;
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_MESSENGER_NOTE_DELETE_REQ: " + ex.ToString());
      }
    }
  }
}
