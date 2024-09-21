// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_AUTH_GET_POINT_CASH_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Network.ServerPacket;

namespace PointBlank.Auth.Network.ClientPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_REQ : ReceivePacket
    {
        public PROTOCOL_AUTH_GET_POINT_CASH_REQ(AuthClient Client, byte[] Buffer) => makeme(Client, Buffer);

        public override void read()
        {
        }

        public override void run()
        {
            if (_client._player == null)
                return;
            _client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, _client._player._gp, _client._player._money, _client._player._tag));
        }
    }
}
