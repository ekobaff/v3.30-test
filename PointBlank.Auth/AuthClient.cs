using Microsoft.Win32.SafeHandles;
using PointBlank.Auth.Data.Configs;
using PointBlank.Auth.Data.Model;
using PointBlank.Auth.Data.Sync;
using PointBlank.Auth.Data.Sync.Server;
using PointBlank.Auth.Network;
using PointBlank.Auth.Network.ClientPacket;
using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace PointBlank.Auth
{
    public class AuthClient : IDisposable
    {
        public Socket _client;
        public Account _player;
        public DateTime ConnectDate;
        public uint SessionId;
        public ushort SessionSeed;
        public int Shift;
        public int firstPacketId;
        private byte[] lastCompleteBuffer;
        private bool disposed;
        private bool closed;
        private SafeHandle handle = (SafeHandle)new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            try
            {
                if (this.disposed)
                    return;
                if (this._client != null)
                {
                    this._client.Dispose();
                    this._client = (Socket)null;
                }
                this.handle.Dispose();
                this.disposed = true;
                GC.SuppressFinalize((object)this);
            }
            catch
            {
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (this.disposed)
                    return;
                this._player = (Account)null;
                if (this._client != null)
                {
                    this._client.Dispose();
                    this._client = (Socket)null;
                }
                if (disposing)
                    this.handle.Dispose();
                this.disposed = true;
            }
            catch
            {
            }
        }

        public AuthClient(Socket client)
        {
            this._client = client;
            this._client.NoDelay = true;
            this.SessionSeed = IdFactory.GetInstance().NextSeed();
        }

        public void Start()
        {
            ConnectDate = DateTime.Now;
            //SetKey = 319394958;
            this.Shift = (int)(this.SessionId % 7U) + 1;
            new Thread(new ThreadStart(this.Read)).Start();
            new Thread(new ThreadStart(() =>
            {
                Console.WriteLine("PROTOCOL_BASE_CONNECT_ACK");
                SendPacket(new PROTOCOL_BASE_CONNECT_ACK(this));

            })).Start();
            new Thread(new ThreadStart(BeginResult)).Start();
        }

        public string GetIPAddress()
        {
            try
            {
                return this._client != null && this._client.RemoteEndPoint != null ? ((IPEndPoint)this._client.RemoteEndPoint).Address.ToString() : "";
            }
            catch
            {
                return "";
            }
        }

        public IPAddress GetAddress()
        {
            try
            {
                return this._client != null && this._client.RemoteEndPoint != null ? ((IPEndPoint)this._client.RemoteEndPoint).Address : (IPAddress)null;
            }
            catch
            {
                return (IPAddress)null;
            }
        }

        private void Connect() => this.SendPacket((PointBlank.Core.Network.SendPacket)new PROTOCOL_BASE_CONNECT_ACK(this));

        public void SendCompletePacket(byte[] data)
        {
            try
            {
                if (data.Length < 4)
                    return;
                if (AuthConfig.debugMode)
                {
                    ushort uint16 = BitConverter.ToUInt16(data, 2);
                    string str1 = "";
                    string str2 = BitConverter.ToString(data);
                    char[] chArray = new char[5]
                    {
            '-',
            ',',
            '.',
            ':',
            '\t'
                    };
                    foreach (string str3 in str2.Split(chArray))
                        str1 = str1 + " " + str3;
                    Logger.debug("Opcode: [" + uint16.ToString() + "]");
                }
                this._client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), (object)this._client);
            }
            catch
            {
                this.Close(0, true);
            }
        }

        public void SendPacket(byte[] data)
        {
            try
            {
                if (data.Length < 2)
                    return;
                ushort uint16_1 = Convert.ToUInt16(data.Length - 2);
                List<byte> byteList = new List<byte>(data.Length + 2);
                byteList.AddRange((IEnumerable<byte>)BitConverter.GetBytes(uint16_1));
                byteList.AddRange((IEnumerable<byte>)data);
                byte[] array = byteList.ToArray();
                if (AuthConfig.debugMode)
                {
                    ushort uint16_2 = BitConverter.ToUInt16(data, 0);
                    string str1 = "";
                    string str2 = BitConverter.ToString(array);
                    char[] chArray = new char[5]
                    {
            '-',
            ',',
            '.',
            ':',
            '\t'
                    };
                    foreach (string str3 in str2.Split(chArray))
                        str1 = str1 + " " + str3;
                    Logger.debug("Opcode: [" + uint16_2.ToString() + "]");
                }
                if (array.Length != 0)
                    this._client.BeginSend(array, 0, array.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), (object)this._client);
                byteList.Clear();
            }
            catch
            {
                this.Close(0, true);
            }
        }

        public void SendPacket(SendPacket bp)
        {
            try
            {
                using (bp)
                {
                    bp.write();
                    byte[] oldPacketData = bp.mstream.ToArray();
                    if (oldPacketData.Length < 2)
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                    ushort opcode = BitConverter.ToUInt16(oldPacketData, 0);
                    int lengthBasic = oldPacketData.Length + 2;
                    byte[] PacketSize = BitConverter.GetBytes((ushort)lengthBasic); // converte o short em um array de bytes
                    byte[] newPacketData = new byte[oldPacketData.Length + PacketSize.Length];
                    Array.Copy(PacketSize, 0, newPacketData, 0, PacketSize.Length); // copia os bytes do short para o início do novo array
                    Array.Copy(oldPacketData, 0, newPacketData, PacketSize.Length, oldPacketData.Length); // copia os bytes do array original para o novo array após o short


                    byte[] bytesLogics = new byte[5]; // Novos bytes a serem adicionados

                    int tamanhoAntigo = oldPacketData.Length; // Armazena o tamanho antigo do array
                    Array.Resize(ref newPacketData, newPacketData.Length + bytesLogics.Length); // Redimensiona o array para adicionar os novos bytes

                    for (int i = 1; i <= bytesLogics.Length; i++)
                    {
                        newPacketData[tamanhoAntigo + i] = bytesLogics[i - 1]; // Adiciona os novos bytes ao array
                    }

                    Console.WriteLine("ACK >==================================================================================<");
                    Console.WriteLine("ACK >>  Send Opcode: " + opcode);
                    Console.WriteLine("ACK >==================================================================================<");

                    if (newPacketData.Length > 0)
                    {
                        _client.BeginSend(newPacketData, 0, newPacketData.Length, SocketFlags.None, new AsyncCallback(SendCallback), _client);
                    }
                    bp.mstream.Close();
                    newPacketData = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }



        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket asyncState = (Socket)ar.AsyncState;
                if (asyncState == null || !asyncState.Connected)
                    return;
                asyncState.EndSend(ar);
            }
            catch
            {
                this.Close(0, true);
            }
        }

        private void Read()
        {
            try
            {
                AuthClient.StateObject stateObject = new AuthClient.StateObject();
                stateObject.workSocket = this._client;
                this._client.BeginReceive(stateObject.buffer, 0, 8096, SocketFlags.None, new AsyncCallback(this.OnReceiveCallback), (object)stateObject);
            }
            catch
            {
                this.Close(0, true);
            }
        }

        public void Close(int time, bool destroyConnection)
        {
            if (this.closed)
                return;
            try
            {
                this.closed = true;
                Account player = this._player;
                if (destroyConnection)
                {
                    if (player != null)
                    {
                        player.setOnlineStatus(false);
                        if (player._status.serverId == (byte)0)
                            SendRefresh.RefreshAccount(player, false);
                        player._status.ResetData(player.player_id);
                        player.SimpleClear();
                        player.updateCacheInfo();
                        this._player = (Account)null;
                    }
                    this._client.Close(time);
                    Thread.Sleep(time);
                    this.Dispose();
                }
                else if (player != null)
                {
                    player.SimpleClear();
                    player.updateCacheInfo();
                    this._player = (Account)null;
                }
                AuthSync.UpdateGSCount(0);
            }
            catch (Exception ex)
            {
                Logger.warning("AuthClient.Close " + ex.ToString());
            }
        }
        private void BeginResult()
        {
            try
            {
                StateObject obj = new StateObject();
                obj.workSocket = _client;
                _client.BeginReceive(obj.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(OnReceiveCallback), obj);
            }
            catch
            {
            }
        }
        private void OnReceiveCallback(IAsyncResult ar)
        {

            StateObject asyncState = (StateObject)ar.AsyncState;
            try
            {
                int byteSize = asyncState.workSocket.EndReceive(ar);
                if (byteSize > 0)
                {
                    int PacketLengthTotal = BitConverter.ToUInt16(asyncState.buffer, 0) & 0x7FFF;
                    bool EncryptedPackage = byteSize - PacketLengthTotal == 3;

                    if (EncryptedPackage)
                    {
                        byte[] babyBuffer = new byte[byteSize];
                        Array.Copy(asyncState.buffer, 0, babyBuffer, 0, byteSize);

                        //Pacote recebido da client encriptado.
                        byte[] packetDataEncryted = new byte[PacketLengthTotal];
                        Array.Copy(asyncState.buffer, 2, packetDataEncryted, 0, packetDataEncryted.Length);

                        //Pacote recebido da client e decriptado.
                        //int shift = (int)SessionId % 7 + 1;
                        CBitRotDecryptor(packetDataEncryted, 0, 2048, 1);
                        RunPacket(packetDataEncryted);

                        //   CheckOut(babyBuffer, PacketLengthTotal);
                    }
                }
            }
            catch
            {

            }
            new Thread(new ThreadStart(BeginResult)).Start();
        }

        public static void CBitRotDecryptor(IList<byte> rawData, int start, int blockSize, int shift)
        {
            int to = start + blockSize;

            if (rawData.Count < to)
            {
                to = rawData.Count;
            }

            byte last = rawData[to - 1];

            for (int j = to - 1; j >= start; j--)
            {
                int index;
                if (j <= start)
                {
                    index = last;
                }
                else
                {
                    index = (rawData[j - 1] & 255);
                }
                rawData[j] = (byte)((index << (8 - shift)) | ((rawData[j] & 255) >> shift));
            }
        }
        public void CheckOut(byte[] buffer, int FirstLength)
        {
            int length = buffer.Length;
            try
            {
                byte[] numArray = new byte[length - FirstLength - 4];
                Array.Copy((Array)buffer, FirstLength + 4, (Array)numArray, 0, numArray.Length);
                if (numArray.Length == 0)
                    return;
                int FirstLength1 = (int)BitConverter.ToUInt16(numArray, 0) & (int)short.MaxValue;
                byte[] data = new byte[FirstLength1 + 2];
                Array.Copy((Array)numArray, 2, (Array)data, 0, data.Length);
                byte[] buff = new byte[FirstLength1 + 2];
                Array.Copy((Array)ComDiv.Decrypt(data, this.Shift), 0, (Array)buff, 0, buff.Length);
                this.RunPacket(buff);
                this.CheckOut(numArray, FirstLength1);
            }
            catch
            {
            }
        }

        private void FirstPacketCheck(ushort packetId)
        {
            if (this.firstPacketId != 0)
                return;
            this.firstPacketId = (int)packetId;
            if (packetId == (ushort)257 || packetId == (ushort)517)
                return;
            this.Close(0, true);
        }
        private bool PacketCheckAuth(ushort uint16)
        {


            if (uint16 == 257) { return true; }
            else if (uint16 == 515) { return true; }
            else if (uint16 == 517) { return true; }
            else if (uint16 == 520) { return true; }
            else if (uint16 == 522) { return true; }
            else if (uint16 == 524) { return true; }
            else if (uint16 == 526) { return true; }
            else if (uint16 == 528) { return true; }
            else if (uint16 == 530) { return true; }
            else if (uint16 == 536) { return true; }
            else if (uint16 == 540) { return true; }
            else if (uint16 == 666) { return true; }
            else if (uint16 == 1057) { return true; }
            else if (uint16 == 5377) { return true; }
            else
            {


                return false;

            }
        }

        private void RunPacket(byte[] buff)
        {
            ushort uint16 = BitConverter.ToUInt16(buff, 0);
            int firstPacketId = this.firstPacketId;
            this.FirstPacketCheck(uint16);
            //ushort uintOK;
            //if (PacketCheckAuth(uint16)) { uintOK = uint16; } else{ this._client.Close(1000);   uintOK = 517; }
            if (this.closed)
                return;
            ReceivePacket receivePacket = (ReceivePacket)null;
            switch (uint16)
            {
                case 257:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_LOGIN_REQ(this, buff);
                    goto case 517;
                case 515:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_LOGOUT_REQ(this, buff);
                    goto case 517;
                case 517:
                    if (receivePacket == null)
                        break;
                    new Thread(new ThreadStart(receivePacket.run)).Start();
                    break;
                case 520:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GAMEGUARD_REQ(this, buff);
                    goto case 517;
                case 522:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GET_SYSTEM_INFO_REQ(this, buff);
                    goto case 517;
                case 524:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GET_USER_INFO_REQ(this, buff);
                    goto case 517;
                case 526:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GET_INVEN_INFO_REQ(this, buff);
                    goto case 517;
                case 528:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GET_OPTION_REQ(this, buff);
                    goto case 517;
                case 530:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_OPTION_SAVE_REQ(this, buff);
                    goto case 517;
                case 536:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_USER_LEAVE_REQ(this, buff);
                    goto case 517;
                case 540:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GET_CHANNELLIST_REQ(this, buff);
                    goto case 517;
                case 667:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_GET_MAP_INFO_REQ(this, buff);
                    goto case 517;
                case 697:
                    receivePacket = (ReceivePacket)new PROTOCOL_BASE_SERVER_LIST_REFRESH_REQ(this, buff);
                    goto case 517;
                case 1057:
                    receivePacket = (ReceivePacket)new PROTOCOL_AUTH_GET_POINT_CASH_REQ(this, buff);
                    goto case 517;
                case 5377:
                    receivePacket = (ReceivePacket)new PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ(this, buff);
                    goto case 517;
                default:
                    Logger.error("Opcode não encontrada: " + uint16.ToString());
                    goto case 517;
            }
        }

        private class StateObject
        {
            public Socket workSocket;
            public const int BufferSize = 8096;
            public byte[] buffer = new byte[8096];
        }
    }
}
