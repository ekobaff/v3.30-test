// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_AUTH_GET_POINT_CASH_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_ACK : SendPacket
    {
        private readonly int _erro, _gold, _cash, _tag;

        public PROTOCOL_AUTH_GET_POINT_CASH_ACK(int erro, int gold = 0, int cash = 0, int tag = 0)
        {
            _erro = erro;
            _gold = gold;
            _cash = cash;
            _tag = tag;
        }

        public override void write()
        {
            writeH(1058);
            writeD(_erro);
            if (_erro >= 0)
            {
                writeD(_gold);
                writeD(_cash);
                writeD(_tag);
            }
        }
    }
}
