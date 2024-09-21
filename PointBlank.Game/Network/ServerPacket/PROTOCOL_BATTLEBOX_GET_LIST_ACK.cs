// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BATTLEBOX_GET_LIST_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

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
            writeD(585); //seed do shop
        }
    }
}
