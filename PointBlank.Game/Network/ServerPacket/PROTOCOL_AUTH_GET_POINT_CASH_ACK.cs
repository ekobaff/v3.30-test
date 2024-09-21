// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_GET_POINT_CASH_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using System.Security.Policy;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_ACK : SendPacket
    {
        private readonly int _erro, _gold, _cash, _tag;

        public PROTOCOL_AUTH_GET_POINT_CASH_ACK(int erro, int gold, int cash, int tag)
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
                writeD(_tag);//tag
            }
        }

    }
}
