// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_CHATTING_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using Game.data.managers;
using Npgsql;
using PointBlank.Core;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Chat;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Data;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_CHATTING_REQ : ReceivePacket
    {
        private string text;
        private ChattingType type;
        private bool SendAll;
        private bool SendMe;

        public PROTOCOL_BASE_CHATTING_REQ(GameClient client, byte[] data) => this.makeme(client, data);

        public override void read()
        {
            this.type = (ChattingType)this.readH();
            this.text = this.readUnicode((int)this.readH() * 2);
            this.SendAll = false;
        }

        public override void run()
        {
            try
            {
                Account player = this._client._player;
                if (player == null || string.IsNullOrEmpty(this.text) || this.text.Length > 60 || player.player_name.Length == 0)
                    return;

                ///CASO DE BUG REMOVER
                if (AntiCrash(text))
                    return;
                ///----------------------
                PointBlank.Game.Data.Model.Room room = player._room;
                switch (this.type)
                {
                    case 0:
                        if (!ServerCommands(player, room))
                        {
                            Slot slot3 = room._slots[player._slotId];

                            byte[] completeBytes2 = new PROTOCOL_ROOM_CHATTING_ACK((int)type, slot3._id, player.UseChatGM(), text).GetCompleteBytes("PROTOCOL_BASE_CHATTING_REQ-0");
                            lock (room._slots)
                            {
                                for (int k = 0; k < 16; k++)
                                {
                                    Slot slot5 = room._slots[k];
                                    Account playerBySlot3 = room.getPlayerBySlot(slot5);
                                    if (playerBySlot3 != null && slot5.state >= SlotState.LOAD)
                                    {
                                        playerBySlot3.SendCompletePacket(completeBytes2);
                                    }
                                }
                                break;
                            }
                        }
                        else
                            _client.SendPacket(new PROTOCOL_ROOM_CHATTING_ACK((int)type, player._slotId, GM: true, text));
                        break;
                    case ChattingType.All:
                    case ChattingType.Lobby:
                        if (room != null)
                        {
                            if (!this.ServerCommands(player, room))
                            {
                                Slot slot1 = room._slots[player._slotId];
                                if (this.SendAll)
                                {
                                    for (int index = 0; index < 16; ++index)
                                    {
                                        Slot slot2 = room._slots[index];
                                        Account playerBySlot = room.getPlayerBySlot(slot2);
                                        if (playerBySlot != null && this.SlotValidMessage(slot1, slot2))
                                            playerBySlot.SendPacket((SendPacket)new PROTOCOL_LOBBY_CHATTING_ACK("Room", 0U, 5, false, this.text));
                                    }
                                    break;
                                }
                                if (this.SendMe)
                                {
                                    this._client.SendPacket((SendPacket)new PROTOCOL_LOBBY_CHATTING_ACK("Room", 0U, 5, false, this.text));
                                    break;
                                }
                                using (PROTOCOL_ROOM_CHATTING_ACK protocolRoomChattingAck = new PROTOCOL_ROOM_CHATTING_ACK((int)this.type, slot1._id, player.UseChatGM(), this.text))
                                {
                                    byte[] completeBytes = protocolRoomChattingAck.GetCompleteBytes("PROTOCOL_BASE_CHATTING_REQ-2");
                                    lock (room._slots)
                                    {
                                        for (int index = 0; index < 16; ++index)
                                        {
                                            Slot slot3 = room._slots[index];
                                            Account playerBySlot = room.getPlayerBySlot(slot3);
                                            if (playerBySlot != null && this.SlotValidMessage(slot1, slot3))
                                                playerBySlot.SendCompletePacket(completeBytes);
                                        }
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                this._client.SendPacket((SendPacket)new PROTOCOL_ROOM_CHATTING_ACK((int)this.type, player._slotId, true, this.text));
                                break;
                            }
                        }
                        else
                        {
                            Channel channel = player.getChannel();
                            if (channel == null)
                                break;
                            if (!this.ServerCommands(player, room))
                            {
                                using (PROTOCOL_LOBBY_CHATTING_ACK lobbyChattingAck = new PROTOCOL_LOBBY_CHATTING_ACK(player, this.text))
                                {
                                    channel.SendPacketToWaitPlayers((SendPacket)lobbyChattingAck);
                                    break;
                                }
                            }
                            else
                            {
                                this._client.SendPacket((SendPacket)new PROTOCOL_LOBBY_CHATTING_ACK(player, this.text, true));
                                break;
                            }
                        }
                    case ChattingType.Team:
                        if (room == null)
                            break;
                        Slot slot4 = room._slots[player._slotId];
                        int[] teamArray = room.GetTeamArray(slot4._team);
                        using (PROTOCOL_ROOM_CHATTING_ACK protocolRoomChattingAck = new PROTOCOL_ROOM_CHATTING_ACK((int)this.type, slot4._id, player.UseChatGM(), this.text))
                        {
                            byte[] completeBytes = protocolRoomChattingAck.GetCompleteBytes("PROTOCOL_BASE_CHATTING_REQ-1");
                            lock (room._slots)
                            {
                                for (int index1 = 0; index1 < teamArray.Length; ++index1)
                                {
                                    int index2 = teamArray[index1];
                                    Slot slot5 = room._slots[index2];
                                    Account playerBySlot = room.getPlayerBySlot(slot5);
                                    if (playerBySlot != null && this.SlotValidMessage(slot4, slot5))
                                        playerBySlot.SendCompletePacket(completeBytes);
                                }
                                break;
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }

        private bool ServerCommands(Account player, PointBlank.Game.Data.Model.Room room)
        {
            try
            {

                string str = this.text.Substring(1);
                if (!this.text.StartsWith("#") && !this.text.StartsWith("\\") && !this.text.StartsWith("#"))
                    return false;


                if (str.StartsWith("dano"))
                    this.text = DamageView.SyncDAMAGE(player, room);
                else if (str.StartsWith("mytitles"))
                    this.text = TakeTitles.GetAllTitles(player);

                if (player.pc_cafe > 0)
                {
                    if (str.StartsWith("giftcash ") && player.pc_cafe >= 1 && player.pc_cafe <= 5)
                        text = NewCommands.SendCashVip(str, player);
                    else if (str.StartsWith("giftgold ") && player.pc_cafe >= 1 && player.pc_cafe <= 5)
                        text = NewCommands.SendGoldVip(str, player);
                    else if (str.StartsWith("viprank") && player.pc_cafe >= 1 && player.pc_cafe <= 6)
                        this.text = NewCommands.RankFakeVip(player, room);
                }

                if (player.access >= AccessLevel.Streamer)
                {

                    if (str.StartsWith("changenick ") && player.access >= AccessLevel.GameMaster)
                        this.text = GMDisguises.SetFakeNick(str, player, room);
                    else if (str.StartsWith("fakerank ") && player.access >= AccessLevel.GameMaster)// novo
                        this.text = GMDisguises.SetFakeRank(str, player, room);
                    else if (str.StartsWith("ban ") && player.access >= AccessLevel.GameMaster)
                        text = NewCommands.BanNew(str.Substring(4));
                    else if (str.StartsWith("uban ") && player.access >= AccessLevel.GameMaster)
                        text = NewCommands.uBanNew(str.Substring(5));
                    else if (str.StartsWith("kp ") && player.access >= AccessLevel.GameMaster)
                        this.text = KickPlayer.KickByNick(str, player);
                    else if (str.StartsWith("kp2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = KickPlayer.KickById(str, player);
                    else if (str.StartsWith("hcn") && player.access >= AccessLevel.Devolper)
                        this.text = GMDisguises.SetHideColor(player);
                    else if (str.StartsWith("antikick") && player.access >= AccessLevel.Devolper)
                        this.text = GMDisguises.SetAntiKick(player);
                    else if (str.StartsWith("roomunlock ") && player.access >= AccessLevel.GameMaster)
                        this.text = ChangeRoomInfos.UnlockById(str, player);
                    else if (str.StartsWith("afkcount ") && player.access >= AccessLevel.Devolper)
                        this.text = AFKInteraction.GetAFKCount(str);
                    else if (str.StartsWith("afkkick ") && player.access >= AccessLevel.Devolper)
                        this.text = AFKInteraction.KickAFKPlayers(str);
                    else if (str.StartsWith("players1") && player.access >= AccessLevel.GameMaster)
                        this.text = PlayersCountInServer.GetMyServerPlayersCount();
                    else if (str.StartsWith("players2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = PlayersCountInServer.GetServerPlayersCount(str);
                    else if (str.StartsWith("ping") && player.access >= AccessLevel.GameMaster)
                        this.text = LatencyAnalyze.StartAnalyze(player, room);
                    else if (str.StartsWith("send ") && player.access >= AccessLevel.Admin)
                        this.text = SendMsgToPlayers.SendToAll(str);
                    else if (str.StartsWith("sendr ") && player.access >= AccessLevel.GameMaster)
                        this.text = SendMsgToPlayers.SendToRoom(str, room);
                    else if (str.StartsWith("cp ") && player.access >= AccessLevel.GameMaster)
                        this.text = SendCashToPlayer.SendByNick(str);
                    else if (str.StartsWith("cp2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = SendCashToPlayer.SendById(str);
                    else if (str.StartsWith("gp ") && player.access >= AccessLevel.GameMaster)
                        this.text = SendGoldToPlayer.SendByNick(str);
                    else if (str.StartsWith("gp2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = SendGoldToPlayer.SendById(str);
                    else if (str.StartsWith("ka") && player.access >= AccessLevel.Admin)
                        this.text = KickAllPlayers.KickPlayers();
                    else if (str.StartsWith("banS ") && player.access >= AccessLevel.GameMaster)
                        this.text = Ban.BanNormalNick(str, player, true);
                    else if (str.StartsWith("banS2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = Ban.BanNormalId(str, player, true);
                    else if (str.StartsWith("banA ") && player.access >= AccessLevel.GameMaster)
                        this.text = Ban.BanNormalNick(str, player, false);
                    else if (str.StartsWith("banA2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = Ban.BanNormalId(str, player, false);
                    else if (str.StartsWith("unb ") && player.access >= AccessLevel.Admin)
                        this.text = UnBan.UnbanByNick(str, player);
                    else if (str.StartsWith("unb2 ") && player.access >= AccessLevel.Admin)
                        this.text = UnBan.UnbanById(str, player);
                    else if (str.StartsWith("reason ") && player.access >= AccessLevel.Admin)
                        this.text = Ban.UpdateReason(str);
                    else if (str.StartsWith("getip ") && player.access >= AccessLevel.Admin)
                        this.text = GetAccountInfo.getByIPAddress(str, player);
                    else if (str.StartsWith("get1 ") && player.access >= AccessLevel.Admin)
                        this.text = GetAccountInfo.getById(str, player);
                    else if (str.StartsWith("get2 ") && player.access >= AccessLevel.Admin)
                        this.text = GetAccountInfo.getByNick(str, player);
                    else if (str.StartsWith("open1 ") && player.access >= AccessLevel.Admin)
                        this.text = OpenRoomSlot.OpenSpecificSlot(str, player, room);
                    else if (str.StartsWith("open2 ") && player.access >= AccessLevel.GameMaster)
                        this.text = OpenRoomSlot.OpenRandomSlot(str, player);

                    else if (str.StartsWith("seeid ") && player.access >= AccessLevel.GameMaster)
                        text = NewCommands.ViewUser(str.Substring(6));

                    else if (str.StartsWith("open3 ") && player.access >= AccessLevel.Admin)
                        this.text = OpenRoomSlot.OpenAllSlots(str, player);
                    //else if (str.StartsWith("taketitles") && player.access >= AccessLevel.Devolper)
                    //this.text = TakeTitles.GetAllTitles(player);
                    else if (str.StartsWith("changerank ") && player.access >= AccessLevel.Admin)
                        this.text = ChangePlayerRank.SetPlayerRank(str);
                    else if (str.StartsWith("banSE ") && player.access >= AccessLevel.Admin)
                        this.text = Ban.BanForeverNick(str, player, true);
                    else if (str.StartsWith("banSE2 ") && player.access >= AccessLevel.Admin)
                        this.text = Ban.BanForeverId(str, player, true);
                    else if (str.StartsWith("banAE ") && player.access >= AccessLevel.Admin)
                        this.text = Ban.BanForeverNick(str, player, false);
                    else if (str.StartsWith("banAE2 ") && player.access >= AccessLevel.Admin)
                        this.text = Ban.BanForeverId(str, player, false);
                    else if (str.StartsWith("getban ") && player.access >= AccessLevel.Admin)
                        this.text = Ban.GetBanData(str, player);
                    else if (str.StartsWith("sunb ") && player.access >= AccessLevel.Admin)
                        this.text = UnBan.SuperUnbanByNick(str, player);
                    else if (str.StartsWith("sunb2 ") && player.access >= AccessLevel.Admin)
                        this.text = UnBan.SuperUnbanById(str, player);
                    else if (str.StartsWith("rshop1") && player.access >= AccessLevel.Admin)
                        this.text = RefillShop.SimpleRefill(player);
                    else if (str.StartsWith("rshop2") && player.access >= AccessLevel.Admin)
                        this.text = RefillShop.InstantRefill(player);
                    else if (str.StartsWith("setgold ") && player.access >= AccessLevel.Admin)
                        this.text = SetGoldToPlayer.SetGdToPlayer(str);
                    else if (str.StartsWith("setcash ") && player.access >= AccessLevel.Admin)
                        this.text = SetCashToPlayer.SetCashPlayer(str);
                    else if (str.StartsWith("gpd ") && player.access >= AccessLevel.Admin)
                        this.text = SendGoldToPlayerDev.SendGoldToPlayer(str);
                    else if (str.StartsWith("cpd ") && player.access >= AccessLevel.Admin)
                        this.text = SendCashToPlayerDev.SendCashToPlayer(str);
                    else if (str.StartsWith("rrules") && player.access >= AccessLevel.Devolper)
                    {
                        ClassicModeManager.RealodCamp();
                        this.text = "Regras atualizada!";

                    }
                    else if (str.StartsWith("rrankup") && player.access >= AccessLevel.Devolper)
                    {
                        EventRankUpSyncer.ReGenList();
                        EventUpModel runningEvent = EventRankUpSyncer.getRunningEvent();
                        this.text = runningEvent != null ? string.Format("Event RankUP atualizado. XP {0}% e GP {1}%.", (object)runningEvent._percentXp, (object)runningEvent._percentGp) : "Nenhum evento ativo encontrado.";
                    }
                    else if (str.StartsWith("end") && player.access >= AccessLevel.Devolper)
                    {
                        if (room != null)
                        {
                            if (room.isPreparing())
                            {
                                AllUtils.EndBattle(room);
                                this.text = Translation.GetLabel("EndRoomSuccess");
                            }
                            else
                                this.text = Translation.GetLabel("EndRoomFail1");
                        }
                        else
                            this.text = Translation.GetLabel("GeneralRoomInvalid");
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_BASE_CHATTING_REQ: " + ex.ToString());
                this.text = Translation.GetLabel("CrashProblemCmd");
                return true;
            }
        }

        private bool AntiCrash(string txt) ///Remover aqui
        {
            if (txt.Contains("@@@@@@@@@@") && txt.Length > 20
                || txt.Contains("|H|K|M|N|T|R@@@@@@@@@") && txt.Length > 20
                || txt.Contains("IHIKIMINITIR@@@@@@@@@") && txt.Length > 20
                || txt.Contains("@@@@DC@@@@@@") && txt.Length > 20)
            {
                return true;
            }
            return false;
        }

        private bool SlotValidMessage(Slot sender, Slot receiver)
        {
            if ((sender.state == SlotState.NORMAL || sender.state == SlotState.READY) && (receiver.state == SlotState.NORMAL || receiver.state == SlotState.READY))
                return true;
            if (sender.state < SlotState.LOAD || receiver.state < SlotState.LOAD)
                return false;
            if (receiver.specGM || sender.specGM || sender._deathState.HasFlag((Enum)DeadEnum.UseChat) || sender._deathState.HasFlag((Enum)DeadEnum.Dead) && receiver._deathState.HasFlag((Enum)DeadEnum.Dead) || sender.espectador && receiver.espectador)
                return true;
            if (!sender._deathState.HasFlag((Enum)DeadEnum.Alive) || !receiver._deathState.HasFlag((Enum)DeadEnum.Alive))
                return false;
            if (sender.espectador && receiver.espectador)
                return true;
            return !sender.espectador && !receiver.espectador;
        }
    }
}
