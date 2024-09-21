using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
    internal class PROTOCOL_BATTLEBOX_GET_LIST_ACK : SendPacket
    {
        private readonly Tuple<int, short>[] _list;

        public PROTOCOL_BATTLEBOX_GET_LIST_ACK(Tuple<int, short>[] list) => _list = list;

        public override void write()
        {
            writeH(7426);

            writeD(_list.Length);
            writeD(_list.Length);
            writeD(0);
            for (int i = 0; i < _list.Length; i++)
            {
                writeD(_list[i].Item1);
                writeH(_list[i].Item2);
                writeC(0);
            }
            writeB(new byte[] { 0x50, 0x61, 0x75, 0x6C });
        }
    }
}
