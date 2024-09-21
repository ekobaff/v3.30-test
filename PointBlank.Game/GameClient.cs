// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.GameClient
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using Microsoft.Win32.SafeHandles;
using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network;
using PointBlank.Game.Network.ClientPacket;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PointBlank.Game
{
    public class GameClient : IDisposable
    {
        public long player_id;
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
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            try
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
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
                this._player = null;
                if (this._client != null)
                {
                    this._client.Dispose();
                    this._client = null;
                }
                this.player_id = 0L;
                if (disposing)
                    this.handle.Dispose();
                this.disposed = true;
            }
            catch
            {
            }
        }

        public GameClient(Socket client)
        {
            this._client = client;
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

        private void ConnectionCheck()
        {
            Thread.Sleep(10000);
            if (this._client != null)
                return;
            Logger.warning("Connection destroyed due to socket null.");
            GameManager.RemoveSocket(this);
            this.Dispose();
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
                return this._client != null && this._client.RemoteEndPoint != null ? ((IPEndPoint)this._client.RemoteEndPoint).Address : null;
            }
            catch
            {
                return null;
            }
        }

        private void Connect() => this.SendPacket(new PROTOCOL_BASE_CONNECT_ACK(this));

        public bool isSocketConnected() => (!this._client.Poll(1000, SelectMode.SelectRead) || this._client.Available != 0) && this._client.Connected;

        public void SendCompletePacket(byte[] data)
        {
            try
            {
                if (data.Length < 4)
                    return;
                if (GameConfig.debugMode)
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
                this._client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), _client);
            }
            catch
            {
                this.Close(0);
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
                byteList.AddRange(BitConverter.GetBytes(uint16_1));
                byteList.AddRange(data);
                byte[] array = byteList.ToArray();
                if (GameConfig.debugMode)
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
                    this._client.BeginSend(array, 0, array.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), _client);
                byteList.Clear();
            }
            catch
            {
                this.Close(0);
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
                this.Close(0);
            }
        }

        private void Read()
        {
            try
            {
                GameClient.StateObject stateObject = new GameClient.StateObject();
                stateObject.workSocket = this._client;
                this._client.BeginReceive(stateObject.buffer, 0, 8096, SocketFlags.None, new AsyncCallback(this.OnReceiveCallback), stateObject);
            }
            catch
            {
                this.Close(0);
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
                        int shift = (int)SessionId % 7 + 1;
                        CBitRotDecryptor(packetDataEncryted, 0, 2048, shift);
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
        public void Checkout(byte[] Buffer, int FirstLength)
        {
            int length = Buffer.Length;
            try
            {
                byte[] numArray = new byte[length - FirstLength - 4];
                Array.Copy(Buffer, FirstLength + 4, numArray, 0, numArray.Length);
                if (numArray.Length == 0)
                    return;
                int FirstLength1 = BitConverter.ToUInt16(numArray, 0) & short.MaxValue;
                byte[] data = new byte[FirstLength1 + 2];
                Array.Copy(numArray, 2, data, 0, data.Length);
                byte[] buff = new byte[FirstLength1 + 2];
                Array.Copy(ComDiv.Decrypt(data, this.Shift), 0, buff, 0, buff.Length);
                this.RunPacket(buff);
                this.Checkout(numArray, FirstLength1);
            }
            catch
            {
            }
        }

        public void Close(int time, bool kicked = false)
        {
            try
            {
                if (!this.closed)
                {
                    try
                    {
                        this.closed = true;
                        GameManager.RemoveSocket(this);
                        Account player = this._player;
                        if (this.player_id > 0L && player != null)
                        {
                            Channel channel = player.getChannel();
                            Room room = player._room;
                            Match match = player._match;
                            player.setOnlineStatus(false);
                            room?.RemovePlayer(player, false, kicked ? 1 : 0);
                            match?.RemovePlayer(player);
                            channel?.RemovePlayer(player);
                            player._status.ResetData(this.player_id);
                            AllUtils.syncPlayerToFriends(player, false);
                            AllUtils.syncPlayerToClanMembers(player);
                            player.SimpleClear();
                            player.updateCacheInfo();
                            this._player = null;
                        }
                        this.player_id = 0L;
                    }
                    catch (Exception ex)
                    {
                        Logger.warning("PlayerId: " + this.player_id.ToString() + "\n" + ex.ToString());
                    }
                }
                if (this._client != null)
                    this._client.Close(time);
                this.Dispose();
            }
            catch (Exception ex)
            {
                Logger.error("Close: " + ex.ToString());
            }
        }

        private void FirstPacketCheck(ushort packetId)
        {
            if (this.firstPacketId != 0)
                return;
            this.firstPacketId = packetId;
            if (packetId == 538 || packetId == 517)
                return;
            this.Close(0);
            Logger.warning("Connection destroyed due to unknown first packet. [" + packetId.ToString() + "]");
        }
        private bool PacketCheckGame(ushort uint16)
        {

            if (uint16 == 515) { return true; }
            else if (uint16 == 517) { return true; }
            else if (uint16 == 520) { return true; }
            else if (uint16 == 530) { return true; }
            else if (uint16 == 534) { return true; }
            else if (uint16 == 536) { return true; }
            else if (uint16 == 538) { return true; }
            else if (uint16 == 540) { return true; }
            else if (uint16 == 542) { return true; }
            else if (uint16 == 544) { return true; }
            else if (uint16 == 546) { return true; }
            else if (uint16 == 558) { return true; }
            else if (uint16 == 568) { return true; }
            else if (uint16 == 572) { return true; }
            else if (uint16 == 574) { return true; }
            else if (uint16 == 584) { return true; }
            else if (uint16 == 586) { return true; }
            else if (uint16 == 588) { return true; }
            else if (uint16 == 592) { return true; }
            else if (uint16 == 600) { return true; }
            else if (uint16 == 622) { return true; }
            else if (uint16 == 633) { return true; }
            else if (uint16 == 672) { return true; }
            else if (uint16 == 685) { return true; }
            else if (uint16 == 787) { return true; }
            else if (uint16 == 792) { return true; }
            else if (uint16 == 794) { return true; }
            else if (uint16 == 796) { return true; }
            else if (uint16 == 802) { return true; }
            else if (uint16 == 804) { return true; }
            else if (uint16 == 809) { return true; }
            else if (uint16 == 929) { return true; }
            else if (uint16 == 931) { return true; }
            else if (uint16 == 934) { return true; }
            else if (uint16 == 936) { return true; }
            else if (uint16 == 1025) { return true; }
            else if (uint16 == 1027) { return true; }
            else if (uint16 == 1029) { return true; }
            else if (uint16 == 1041) { return true; }
            else if (uint16 == 1043) { return true; }
            else if (uint16 == 1047) { return true; }
            else if (uint16 == 1049) { return true; }
            else if (uint16 == 1053) { return true; }
            else if (uint16 == 1055) { return true; }
            else if (uint16 == 1057) { return true; }
            else if (uint16 == 1061) { return true; }
            else if (uint16 == 1075) { return true; }
            else if (uint16 == 1076) { return true; }
            else if (uint16 == 1084) { return true; }
            else if (uint16 == 1087) { return true; }
            else if (uint16 == 1544) { return true; }
            else if (uint16 == 1546) { return true; }
            else if (uint16 == 1548) { return true; }
            else if (uint16 == 1550) { return true; }
            else if (uint16 == 1553) { return true; }
            else if (uint16 == 1558) { return true; }
            else if (uint16 == 1565) { return true; }
            else if (uint16 == 1567) { return true; }
            else if (uint16 == 1569) { return true; }
            else if (uint16 == 1571) { return true; }
            else if (uint16 == 1576) { return true; }
            else if (uint16 == 1793) { return true; }
            else if (uint16 == 1795) { return true; }
            else if (uint16 == 1797) { return true; }
            else if (uint16 == 1799) { return true; }
            else if (uint16 == 1824) { return true; }
            else if (uint16 == 1826) { return true; }
            else if (uint16 == 1828) { return true; }
            else if (uint16 == 1830) { return true; }
            else if (uint16 == 1832) { return true; }
            else if (uint16 == 1834) { return true; }
            else if (uint16 == 1836) { return true; }
            else if (uint16 == 1838) { return true; }
            else if (uint16 == 1840) { return true; }
            else if (uint16 == 1842) { return true; }
            else if (uint16 == 1844) { return true; }
            else if (uint16 == 1846) { return true; }
            else if (uint16 == 1849) { return true; }
            else if (uint16 == 1852) { return true; }
            else if (uint16 == 1854) { return true; }
            else if (uint16 == 1857) { return true; }
            else if (uint16 == 1860) { return true; }
            else if (uint16 == 1863) { return true; }
            else if (uint16 == 1878) { return true; }
            else if (uint16 == 1880) { return true; }
            else if (uint16 == 1882) { return true; }
            else if (uint16 == 1884) { return true; }
            else if (uint16 == 1892) { return true; }
            else if (uint16 == 1901) { return true; }
            else if (uint16 == 1910) { return true; }
            else if (uint16 == 1912) { return true; }
            else if (uint16 == 1914) { return true; }
            else if (uint16 == 1916) { return true; }
            else if (uint16 == 1936) { return true; }
            else if (uint16 == 1938) { return true; }
            else if (uint16 == 1946) { return true; }
            else if (uint16 == 1954) { return true; }
            else if (uint16 == 1956) { return true; }
            else if (uint16 == 3073) { return true; }
            else if (uint16 == 3075) { return true; }
            else if (uint16 == 3077) { return true; }
            else if (uint16 == 3083) { return true; }
            else if (uint16 == 3093) { return true; }
            else if (uint16 == 3095) { return true; }
            else if (uint16 == 3329) { return true; }
            else if (uint16 == 3331) { return true; }
            else if (uint16 == 3396) { return true; }
            else if (uint16 == 3400) { return true; }
            else if (uint16 == 3841) { return true; }
            else if (uint16 == 3843) { return true; }
            else if (uint16 == 3858) { return true; }
            else if (uint16 == 3860) { return true; }
            else if (uint16 == 3865) { return true; }
            else if (uint16 == 3869) { return true; }
            else if (uint16 == 3875) { return true; }
            else if (uint16 == 3877) { return true; }
            else if (uint16 == 3879) { return true; }
            else if (uint16 == 3881) { return true; }
            else if (uint16 == 3883) { return true; }
            else if (uint16 == 3893) { return true; }
            else if (uint16 == 3910) { return true; }
            else if (uint16 == 3911) { return true; }
            else if (uint16 == 3925) { return true; }
            else if (uint16 == 3927) { return true; }
            else if (uint16 == 3929) { return true; }
            else if (uint16 == 3931) { return true; }
            else if (uint16 == 3933) { return true; }
            else if (uint16 == 3934) { return true; }
            else if (uint16 == 3936) { return true; }
            else if (uint16 == 4097) { return true; }
            else if (uint16 == 4099) { return true; }
            else if (uint16 == 4105) { return true; }
            else if (uint16 == 4107) { return true; }
            else if (uint16 == 4109) { return true; }
            else if (uint16 == 4111) { return true; }
            else if (uint16 == 4113) { return true; }
            else if (uint16 == 4119) { return true; }
            else if (uint16 == 4121) { return true; }
            else if (uint16 == 4122) { return true; }
            else if (uint16 == 4132) { return true; }
            else if (uint16 == 4134) { return true; }
            else if (uint16 == 4142) { return true; }
            else if (uint16 == 4144) { return true; }
            else if (uint16 == 4148) { return true; }
            else if (uint16 == 4150) { return true; }
            else if (uint16 == 4156) { return true; }
            else if (uint16 == 4158) { return true; }
            else if (uint16 == 4164) { return true; }
            else if (uint16 == 4238) { return true; }
            else if (uint16 == 4252) { return true; }
            else if (uint16 == 5377) { return true; }
            else if (uint16 == 6145) { return true; }
            else if (uint16 == 6149) { return true; }
            else if (uint16 == 6151) { return true; }
            else if (uint16 == 6914) { return true; }
            else if (uint16 == 6963) { return true; }
            else if (uint16 == 7429) { return true; }
            else
            {
                return false;
            }



        }

        private void RunPacket(byte[] buff)
        {
            ushort uint16 = BitConverter.ToUInt16(buff, 0);
            this.FirstPacketCheck(uint16);

            //ushort uintOK;
            //if (PacketCheckGame(uint16)) { uintOK = uint16; } else { this._client.Close(1000); uintOK = 515; }


            if (this.closed)
                return;
            ReceivePacket receivePacket = null;
            switch (uint16)
            {
                case 515:
                    receivePacket = new PROTOCOL_BASE_LOGOUT_REQ(this, buff);
                    break;
                case 517:
                    receivePacket = new PROTOCOL_BASE_PACKET_EMPTY_REQ(this, buff);
                    break;
                case 520:
                    receivePacket = new PROTOCOL_BASE_GAMEGUARD_REQ(this, buff);
                    break;
                case 530:
                    receivePacket = new PROTOCOL_BASE_OPTION_SAVE_REQ(this, buff);
                    break;
                case 534:
                    receivePacket = new PROTOCOL_BASE_CREATE_NICK_REQ(this, buff);
                    break;
                case 536:
                    receivePacket = new PROTOCOL_BASE_USER_LEAVE_REQ(this, buff);
                    break;
                case 538:
                    receivePacket = new PROTOCOL_BASE_USER_ENTER_REQ(this, buff);
                    break;
                case 540:
                    receivePacket = new PROTOCOL_BASE_GET_CHANNELLIST_REQ(this, buff);
                    break;
                case 542:
                    receivePacket = new PROTOCOL_BASE_SELECT_CHANNEL_REQ(this, buff);
                    break;
                case 544:
                    receivePacket = new PROTOCOL_BASE_ATTENDANCE_REQ(this, buff);
                    break;
                case 546:
                    receivePacket = new PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ(this, buff);
                    break;
                case 558:
                    receivePacket = new PROTOCOL_BASE_GET_RECORD_INFO_DB_REQ(this, buff);
                    break;
                case 568:
                    receivePacket = new PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ(this, buff);
                    break;
                case 572:
                    receivePacket = new PROTOCOL_BASE_QUEST_BUY_CARD_SET_REQ(this, buff);
                    break;
                case 574:
                    receivePacket = new PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ(this, buff);
                    break;
                case 584:
                    receivePacket = new PROTOCOL_BASE_USER_TITLE_CHANGE_REQ(this, buff);
                    break;
                case 586:
                    receivePacket = new PROTOCOL_BASE_USER_TITLE_EQUIP_REQ(this, buff);
                    break;
                case 588:
                    receivePacket = new PROTOCOL_BASE_USER_TITLE_RELEASE_REQ(this, buff);
                    break;
                case 592:
                    receivePacket = new PROTOCOL_BASE_CHATTING_REQ(this, buff);
                    break;
                case 600:
                    receivePacket = new PROTOCOL_BASE_MISSION_SUCCESS_REQ(this, buff);
                    break;
                case 622:
                    receivePacket = new PROTOCOL_BASE_DAILY_RECORD_REQ(this, buff);
                    break;
                case 633:
                    receivePacket = new PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ(this, buff);
                    break;
                case 672:
                    receivePacket = new PROTOCOL_BASE_PACKET_EMPTY_REQ(this, buff);
                    break;
                case 685:
                    receivePacket = new PROTOCOL_BASE_PACKET_EMPTY_REQ(this, buff);
                    break;
                case 787:
                    receivePacket = new PROTOCOL_AUTH_FRIEND_INVITED_REQUEST_REQ(this, buff);
                    break;
                case 792:
                    receivePacket = new PROTOCOL_AUTH_FRIEND_ACCEPT_REQ(this, buff);
                    break;
                case 794:
                    receivePacket = new PROTOCOL_AUTH_FRIEND_INVITED_REQ(this, buff);
                    break;
                case 796:
                    receivePacket = new PROTOCOL_AUTH_FRIEND_DELETE_REQ(this, buff);
                    break;
                case 802:
                    receivePacket = new PROTOCOL_AUTH_RECV_WHISPER_REQ(this, buff);
                    break;
                case 804:
                    receivePacket = new PROTOCOL_AUTH_SEND_WHISPER_REQ(this, buff);
                    break;
                case 809:
                    receivePacket = new PROTOCOL_AUTH_FIND_USER_REQ(this, buff);
                    break;
                case 929:
                    receivePacket = new PROTOCOL_MESSENGER_NOTE_SEND_REQ(this, buff);
                    break;
                case 931:
                    receivePacket = new PROTOCOL_MESSENGER_NOTE_RECEIVE_REQ(this, buff);
                    break;
                case 934:
                    receivePacket = new PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ(this, buff);
                    break;
                case 936:
                    receivePacket = new PROTOCOL_MESSENGER_NOTE_DELETE_REQ(this, buff);
                    break;
                case 1025:
                    receivePacket = new PROTOCOL_SHOP_ENTER_REQ(this, buff);
                    break;
                case 1027:
                    receivePacket = new PROTOCOL_SHOP_LEAVE_REQ(this, buff);
                    break;
                case 1029:
                    receivePacket = new PROTOCOL_SHOP_GET_SAILLIST_REQ(this, buff);
                    break;
                case 1041:
                    receivePacket = new PROTOCOL_AUTH_SHOP_GET_GIFTLIST_REQ(this, buff);
                    break;
                case 1043:
                    receivePacket = new PROTOCOL_AUTH_SHOP_GOODS_BUY_REQ(this, buff);
                    break;
                case 1047:
                    receivePacket = new PROTOCOL_AUTH_SHOP_ITEM_AUTH_REQ(this, buff);
                    break;
                case 1049:
                    receivePacket = new PROTOCOL_INVENTORY_USE_ITEM_REQ(this, buff);
                    break;
                case 1053:
                    receivePacket = new PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ(this, buff);
                    break;
                case 1055:
                    receivePacket = new PROTOCOL_AUTH_SHOP_DELETE_ITEM_REQ(this, buff);
                    break;
                case 1057:
                    receivePacket = new PROTOCOL_AUTH_GET_POINT_CASH_REQ(this, buff);
                    break;
                case 1061:
                    receivePacket = new PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_REQ(this, buff);
                    break;
                case 1075:
                    receivePacket = new PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_REQ(this, buff);
                    break;
                case 1076:
                    receivePacket = new PROTOCOL_SHOP_REPAIR_REQ(this, buff);
                    break;
                case 1084:
                    receivePacket = new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ(this, buff);
                    break;
                case 1087:
                    receivePacket = new PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_REQ(this, buff);
                    break;
                case 1544:
                    receivePacket = new PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_REQ(this, buff);
                    break;
                case 1546:
                    receivePacket = new PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ(this, buff);
                    break;
                case 1548:
                    receivePacket = new PROTOCOL_CLAN_WAR_JOIN_TEAM_REQ(this, buff);
                    break;
                case 1550:
                    receivePacket = new PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ(this, buff);
                    break;
                case 1553:
                    receivePacket = new PROTOCOL_CLAN_WAR_MATCH_PROPOSE_REQ(this, buff);
                    break;
                case 1558:
                    receivePacket = new PROTOCOL_CLAN_WAR_INVITE_ACCEPT_REQ(this, buff);
                    break;
                case 1565:
                    receivePacket = new PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ(this, buff);
                    break;
                case 1567:
                    receivePacket = new PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ(this, buff);
                    break;
                case 1569:
                    receivePacket = new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ(this, buff);
                    break;
                case 1571:
                    receivePacket = new PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ(this, buff);
                    break;
                case 1576:
                    receivePacket = new PROTOCOL_CLAN_WAR_TEAM_CHATTING_REQ(this, buff);
                    break;
                case 1793:
                    receivePacket = new PROTOCOL_CS_CLIENT_ENTER_REQ(this, buff);
                    break;
                case 1795:
                    receivePacket = new PROTOCOL_CS_CLIENT_LEAVE_REQ(this, buff);
                    break;
                case 1797:
                    receivePacket = new PROTOCOL_CS_CLIENT_CLAN_LIST_REQ(this, buff);
                    break;
                case 1799:
                    receivePacket = new PROTOCOL_CS_CLIENT_CLAN_CONTEXT_REQ(this, buff);
                    break;
                case 1824:
                    receivePacket = new PROTOCOL_CS_DETAIL_INFO_REQ(this, buff);
                    break;
                case 1826:
                    receivePacket = new PROTOCOL_CS_MEMBER_CONTEXT_REQ(this, buff);
                    break;
                case 1828:
                    receivePacket = new PROTOCOL_CS_MEMBER_LIST_REQ(this, buff);
                    break;
                case 1830:
                    receivePacket = new PROTOCOL_CS_CREATE_CLAN_REQ(this, buff);
                    break;
                case 1832:
                    receivePacket = new PROTOCOL_CS_CLOSE_CLAN_REQ(this, buff);
                    break;
                case 1834:
                    receivePacket = new PROTOCOL_CS_CHECK_JOIN_AUTHORITY_ERQ(this, buff);
                    break;
                case 1836:
                    receivePacket = new PROTOCOL_CS_JOIN_REQUEST_REQ(this, buff);
                    break;
                case 1838:
                    receivePacket = new PROTOCOL_CS_CANCEL_REQUEST_REQ(this, buff);
                    break;
                case 1840:
                    receivePacket = new PROTOCOL_CS_REQUEST_CONTEXT_REQ(this, buff);
                    break;
                case 1842:
                    receivePacket = new PROTOCOL_CS_REQUEST_LIST_REQ(this, buff);
                    break;
                case 1844:
                    receivePacket = new PROTOCOL_CS_REQUEST_INFO_REQ(this, buff);
                    break;
                case 1846:
                    receivePacket = new PROTOCOL_CS_ACCEPT_REQUEST_REQ(this, buff);
                    break;
                case 1849:
                    receivePacket = new PROTOCOL_CS_DENIAL_REQUEST_REQ(this, buff);
                    break;
                case 1852:
                    receivePacket = new PROTOCOL_CS_SECESSION_CLAN_REQ(this, buff);
                    break;
                case 1854:
                    receivePacket = new PROTOCOL_CS_DEPORTATION_REQ(this, buff);
                    break;
                case 1857:
                    receivePacket = new PROTOCOL_CS_COMMISSION_MASTER_REQ(this, buff);
                    break;
                case 1860:
                    receivePacket = new PROTOCOL_CS_COMMISSION_STAFF_REQ(this, buff);
                    break;
                case 1863:
                    receivePacket = new PROTOCOL_CS_COMMISSION_REGULAR_REQ(this, buff);
                    break;
                case 1878:
                    receivePacket = new PROTOCOL_CS_CHATTING_REQ(this, buff);
                    break;
                case 1880:
                    receivePacket = new PROTOCOL_CS_CHECK_MARK_REQ(this, buff);
                    break;
                case 1882:
                    receivePacket = new PROTOCOL_CS_REPLACE_NOTICE_REQ(this, buff);
                    break;
                case 1884:
                    receivePacket = new PROTOCOL_CS_REPLACE_INTRO_REQ(this, buff);
                    break;
                case 1892:
                    receivePacket = new PROTOCOL_CS_REPLACE_MANAGEMENT_REQ(this, buff);
                    break;
                case 1901:
                    receivePacket = new PROTOCOL_CS_ROOM_INVITED_REQ(this, buff);
                    break;
                case 1910:
                    receivePacket = new PROTOCOL_CS_PAGE_CHATTING_REQ(this, buff);
                    break;
                case 1912:
                    receivePacket = new PROTOCOL_CS_INVITE_REQ(this, buff);
                    break;
                case 1914:
                    receivePacket = new PROTOCOL_CS_INVITE_ACCEPT_REQ(this, buff);
                    break;
                case 1916:
                    receivePacket = new PROTOCOL_CS_NOTE_REQ(this, buff);
                    break;
                case 1936:
                    receivePacket = new PROTOCOL_CS_CREATE_CLAN_CONDITION_REQ(this, buff);
                    break;
                case 1938:
                    receivePacket = new PROTOCOL_CS_CHECK_DUPLICATE_REQ(this, buff);
                    break;
                case 1946:
                    receivePacket = new PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_REQ(this, buff);
                    break;
                case 1954:
                    receivePacket = new PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_REQ(this, buff);
                    break;
                case 1956:
                    receivePacket = new PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_REQ(this, buff);
                    break;
                case 3073:
                    receivePacket = new PROTOCOL_LOBBY_ENTER_REQ(this, buff);
                    break;
                case 3075:
                    receivePacket = new PROTOCOL_LOBBY_LEAVE_REQ(this, buff);
                    break;
                case 3077:
                    receivePacket = new PROTOCOL_LOBBY_GET_ROOMLIST_REQ(this, buff);
                    break;
                case 3083:
                    receivePacket = new PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ(this, buff);
                    break;
                case 3093:
                    receivePacket = new PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ(this, buff);
                    break;
                case 3095:
                    receivePacket = new PROTOCOL_BASE_SELECT_AGE_REQ(this, buff);
                    break;
                case 3329:
                    receivePacket = new PROTOCOL_INVENTORY_ENTER_REQ(this, buff);
                    break;
                case 3331:
                    receivePacket = new PROTOCOL_INVENTORY_LEAVE_REQ(this, buff);
                    break;
                case 3396:
                    receivePacket = new PROTOCOL_BATTLE_START_KICKVOTE_REQ(this, buff);
                    break;
                case 3400:
                    receivePacket = new PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ(this, buff);
                    break;
                case 3841:
                    receivePacket = new PROTOCOL_ROOM_CREATE_REQ(this, buff);
                    break;
                case 3843:
                    receivePacket = new PROTOCOL_ROOM_JOIN_REQ(this, buff);
                    break;
                case 3858:
                    receivePacket = new PROTOCOL_ROOM_CHANGE_PASSWD_REQ(this, buff);
                    break;
                case 3860:
                    receivePacket = new PROTOCOL_ROOM_CHANGE_SLOT_REQ(this, buff);
                    break;
                case 3865:
                    receivePacket = new PROTOCOL_ROOM_PERSONAL_TEAM_CHANGE_REQ(this, buff);
                    break;
                case 3869:
                    receivePacket = new PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_REQ(this, buff);
                    break;
                case 3875:
                    receivePacket = new PROTOCOL_ROOM_REQUEST_MAIN_REQ(this, buff);
                    break;
                case 3877:
                    receivePacket = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_REQ(this, buff);
                    break;
                case 3879:
                    receivePacket = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ(this, buff);
                    break;
                case 3881:
                    receivePacket = new PROTOCOL_ROOM_CHECK_MAIN_REQ(this, buff);
                    break;
                case 3883:
                    receivePacket = new PROTOCOL_ROOM_TOTAL_TEAM_CHANGE_REQ(this, buff);
                    break;
                case 3893:
                    receivePacket = new PROTOCOL_ROOM_CHANGE_ROOMINFO_REQ(this, buff);
                    break;
                case 3910:
                    receivePacket = new PROTOCOL_BASE_PLAYTIME_REWARD_REQ(this, buff);
                    break;
                case 3911:
                    receivePacket = new PROTOCOL_ROOM_LOADING_START_REQ(this, buff);
                    break;
                case 3925:
                    receivePacket = new PROTOCOL_ROOM_INFO_ENTER_REQ(this, buff);
                    break;
                case 3927:
                    receivePacket = new PROTOCOL_ROOM_INFO_LEAVE_REQ(this, buff);
                    break;
                case 3929:
                    receivePacket = new PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ(this, buff);
                    break;
                case 3931:
                    receivePacket = new PROTOCOL_ROOM_CHANGE_COSTUME_REQ(this, buff);
                    break;
                case 3933:
                    receivePacket = new PROTOCOL_ROOM_SELECT_SLOT_CHANGE_REQ(this, buff);
                    break;
                case 3934:
                    receivePacket = new PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_REQ(this, buff);
                    break;
                case 3936:
                    receivePacket = new PROTOCOL_BATTLE_ACE_MODE_SLOT_REQ(this, buff);
                    break;
                case 4097:
                    receivePacket = new PROTOCOL_BATTLE_HOLE_CHECK_REQ(this, buff);
                    break;
                case 4099:
                    receivePacket = new PROTOCOL_BATTLE_READYBATTLE_REQ(this, buff);
                    break;
                case 4105:
                    receivePacket = new PROTOCOL_BATTLE_PRESTARTBATTLE_REQ(this, buff);
                    break;
                case 4107:
                    receivePacket = new PROTOCOL_BATTLE_STARTBATTLE_REQ(this, buff);
                    break;
                case 4109:
                    receivePacket = new PROTOCOL_BATTLE_GIVEUPBATTLE_REQ(this, buff);
                    break;
                case 4111:
                    receivePacket = new PROTOCOL_BATTLE_DEATH_REQ(this, buff);
                    break;
                case 4113:
                    receivePacket = new PROTOCOL_BATTLE_RESPAWN_REQ(this, buff);
                    break;
                case 4119:
                    receivePacket = new PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ(this, buff);
                    break;
                case 4121:
                    receivePacket = new PROTOCOL_BASE_PACKET_EMPTY_REQ(this, buff);
                    break;
                case 4122:
                    receivePacket = new PROTOCOL_BATTLE_SENDPING_REQ(this, buff);
                    break;
                case 4132:
                    receivePacket = new PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_REQ(this, buff);
                    break;
                case 4134:
                    receivePacket = new PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_REQ(this, buff);
                    break;
                case 4142:
                    receivePacket = new PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_REQ(this, buff);
                    break;
                case 4144:
                    receivePacket = new PROTOCOL_BATTLE_TIMERSYNC_REQ(this, buff);
                    break;
                case 4148:
                    receivePacket = new PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_REQ(this, buff);
                    break;
                case 4150:
                    receivePacket = new PROTOCOL_BATTLE_RESPAWN_FOR_AI_REQ(this, buff);
                    break;
                case 4156:
                    receivePacket = new PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_REQ(this, buff);
                    break;
                case 4158:
                    receivePacket = new PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_REQ(this, buff);
                    break;
                case 4164:
                    receivePacket = new PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_REQ(this, buff);
                    break;
                case 4238:
                    receivePacket = new PROTOCOL_BATTLE_NEW_JOIN_ROOM_SCORE_REQ(this, buff);
                    break;
                case 4252:
                    receivePacket = new PROTOCOL_BATTLE_USER_SOPETYPE_REQ(this, buff);
                    break;
                case 5377:
                    receivePacket = new PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ(this, buff);
                    break;
                case 6145:
                    receivePacket = new PROTOCOL_CHAR_CREATE_CHARA_REQ(this, buff);
                    break;
                case 6149:
                    receivePacket = new PROTOCOL_CHAR_CHANGE_EQUIP_REQ(this, buff);
                    break;
                case 6151:
                    receivePacket = new PROTOCOL_CHAR_DELETE_CHARA_REQ(this, buff);
                    break;
                case 6914:
                    receivePacket = new PROTOCOL_CLAN_WAR_MATCH_TEAM_COUNT_REQ(this, buff);
                    break;
                case 6963:
                    receivePacket = new PROTOCOL_CLAN_WAR_RESULT_REQ(this, buff);
                    break;
                case 7429:
                    receivePacket = new PROTOCOL_BATTLEBOX_AUTH_REQ(this, buff);
                    break;
                default:
                    Logger.warning("Opcode not found: " + uint16.ToString());
                    Console.WriteLine(this.ConvertToHex(buff));
                    break;
            }
            if (receivePacket == null)
                return;
            if (GameConfig.debugMode)
                Logger.debug("Opcode: [" + uint16.ToString() + "]");
            new Thread(new ThreadStart(receivePacket.run)).Start();
        }

        private string ConvertToHex(byte[] buffer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = buffer.Length;
            int num1 = 0;
            stringBuilder.AppendLine("|--------------------------------------------------------------------------|");
            stringBuilder.AppendLine("|       00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F                    |");
            stringBuilder.AppendLine("|--------------------------------------------------------------------------|");
            for (int index1 = 0; index1 < length; ++index1)
            {
                if (num1 % 16 == 0)
                    stringBuilder.Append("| " + index1.ToString("X4") + ": ");
                stringBuilder.Append(buffer[index1].ToString("X2") + " ");
                ++num1;
                if (num1 == 16)
                {
                    stringBuilder.Append("   ");
                    int num2 = index1 - 15;
                    for (int index2 = 0; index2 < 16; ++index2)
                    {
                        byte num3 = buffer[num2++];
                        if (num3 > 31 && num3 < 128)
                            stringBuilder.Append((char)num3);
                        else
                            stringBuilder.Append('.');
                    }
                    stringBuilder.Append("\n");
                    num1 = 0;
                }
            }
            int num4 = length % 16;
            if (num4 > 0)
            {
                for (int index = 0; index < 17 - num4; ++index)
                    stringBuilder.Append("   ");
                int num5 = length - num4;
                for (int index = 0; index < num4; ++index)
                {
                    byte num6 = buffer[num5++];
                    if (num6 > 31 && num6 < 128)
                        stringBuilder.Append((char)num6);
                    else
                        stringBuilder.Append('.');
                }
                stringBuilder.Append("\n");
            }
            stringBuilder.AppendLine("|--------------------------------------------------------------------------|");
            return stringBuilder.ToString().TrimEnd('\n');
        }

        private class StateObject
        {
            public Socket workSocket;
            public const int BufferSize = 8096;
            public byte[] buffer = new byte[8096];
        }
    }
}
