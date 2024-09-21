using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Game.Network.ServerPacket
{
    internal class PROTOCOL_BATTLEBOX_AUTH_ACK : SendPacket
    {
        private uint _erro;
        private int _itemid, _count, _tags;

        public PROTOCOL_BATTLEBOX_AUTH_ACK(uint erro)
        {
            _erro = erro;
        }

        public PROTOCOL_BATTLEBOX_AUTH_ACK(int ItemID, int Count, int Tags)
        {
            _erro = 0x00000001;
            _itemid = ItemID;
            _count = Count;
            _tags = Tags;
        }

        public override void write()
        {
            writeH(7430);
            writeH(0); //Independente do valor a ação segue normal
            writeD(_erro);//error
            if (_erro == 0x00000001)
            {
                writeD(_itemid); //item 


                writeC(0); //?
                writeD(_count); //count ou oque?

                writeD(_tags); //Tags restante após a compra

            }
        }
    }
}
