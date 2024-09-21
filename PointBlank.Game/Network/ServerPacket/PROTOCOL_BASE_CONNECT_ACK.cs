using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_CONNECT_ACK : SendPacket
    {
        private uint SessionId;
        private ushort SessionSeed;

        public PROTOCOL_BASE_CONNECT_ACK(GameClient Client)
        {
            SessionId = Client.SessionId;
            SessionSeed = Client.SessionSeed;
        }

        public override void write()
        {
            writeH(514);
            writeH(2);
            writeC(11);
            for (int i = 0; i < 11; i++)
            {
                writeC(0);
            }
            writeH(6);
            writeH(4);
            writeD(0);//SHIFT
            writeC(3);
            writeH(30);
            writeD(SessionSeed);
            writeD(SessionId);
        }
    }
}