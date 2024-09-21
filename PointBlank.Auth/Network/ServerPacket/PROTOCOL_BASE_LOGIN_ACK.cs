using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_LOGIN_ACK : SendPacket
    {
        private uint _result;
        private string _login;
        private long _pId;

        public PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum result, string login, long pId)
        {
            _result = (uint)result;
            _login = login;
            _pId = pId;
        }

        public PROTOCOL_BASE_LOGIN_ACK(uint result, string login, long pId)
        {
            _result = result;
            _login = login;
            _pId = pId;
        }

        public PROTOCOL_BASE_LOGIN_ACK(int result, string login, long pId)
        {
            _result = (uint)result;
            _login = login;
            _pId = pId;
        }
        public enum AccountFeatures : uint
        {
            ALL = 2389079934,
            TOKEN_ONLY = 2121728000, //no killcam
            TOKEN_CLAN = 0x7E779934, //no killcam
        }
        public override void write()
        {
            writeH(259);
            writeH(0);
            writeD((uint)AccountFeatures.ALL);
            writeH(1402); //killcam (1402 enabled)
            writeB(new byte[19]);
            if (_result == 0)
            {
                writeC((byte)_login.Length);
                writeS(_login, _login.Length);
                writeQ(_pId);
            }
            else
            {
                writeC(0);
                writeS("");
                writeQ(0);
            }
            writeD(_result);
        }
    }
}