// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_TIMERSYNC_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_TIMERSYNC_REQ : ReceivePacket
    {
        private float Value;
        private uint TimeRemaining;
        private int Ping;
        private int Hack;
        private int Latency;
        private int Round;
        private DateTime LastTimeSync;

        public PROTOCOL_BATTLE_TIMERSYNC_REQ(GameClient client, byte[] data) => this.makeme(client, data);

        public override void read()
        {
            this.TimeRemaining = this.readUD();
            this.Value = this.readT();
            this.Round = (int)this.readC();
            this.Ping = (int)this.readC();
            this.Hack = (int)this.readC();
            this.Latency = (int)this.readH();
        }

        public override void run()
        {
            try
            {
                Account player = this._client._player;
                this.PlayerOnline();
                if (player == null)
                    return;
                PointBlank.Game.Data.Model.Room room = player._room;
                if (room == null)
                    return;
                bool isBotMode = room.isBotMode();
                PointBlank.Core.Models.Room.Slot slot = room.getSlot(player._slotId);
                if (slot == null || slot.state != SlotState.BATTLE)
                    return;
                if ((double)this.Value != 1.0 || this.Hack != 0)
                {

                        player.SendPacket((SendPacket)new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                        player.Close(1000, true);
                    
                    Logger.LogHack("[Value: " + this.Value.ToString() + " HackType: " + this.Hack.ToString() + " (" + ((HackType)this.Hack).ToString() + ")] Player: " + player.player_name + " Id: " + player.player_id.ToString() + " Login: " + player.login);
                }
                room._timeRoom = this.TimeRemaining;
                this.SyncPlayerPings(player, room, slot, isBotMode);
                if (this.TimeRemaining != 0U && this.TimeRemaining <= 2147483648U || room.swapRound || !this.CompareRounds(room, this.Round) || room._state != RoomState.Battle)
                    return;
                this.EndRound(room, isBotMode);
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_BATTLE_TIMERSYNC_REQ: " + ex.ToString());
            }
        }

        private void PlayerOnline()
        {
            if ((DateTime.Now - this.LastTimeSync).TotalSeconds < 60.0)
                return;
            this.LastTimeSync = DateTime.Now;
            foreach (Channel channel in ChannelsXml._channels)
                ComDiv.updateDB("channels", "online", (object)channel._players.Count, "channel_id", (object)channel._id);
        }

        private void SyncPlayerPings(Account p, PointBlank.Game.Data.Model.Room room, PointBlank.Core.Models.Room.Slot slot, bool isBotMode)
        {
            if (isBotMode)
                return;
            slot.latency = this.Latency;
            slot.ping = 5;
            if (slot.latency >= GameConfig.maxBattleLatency)
                ++slot.failLatencyTimes;
            else
                slot.failLatencyTimes = 0;
            if (p.DebugPing && (DateTime.Now - p.LastPingDebug).TotalSeconds >= 5.0)
            {
                p.LastPingDebug = DateTime.Now;
                p.SendPacket((SendPacket)new PROTOCOL_LOBBY_CHATTING_ACK("Server", 0U, 5, false, this.Latency.ToString() + "ms (" + this.Ping.ToString() + " bar)"));
            }
            if (slot.failLatencyTimes >= GameConfig.maxRepeatLatency)
            {
                Logger.error("Player: '" + p.player_name + "' (Id: " + slot._playerId.ToString() + ") kicked due to high latency. (" + slot.latency.ToString() + "/" + GameConfig.maxBattleLatency.ToString() + "ms)");
                this._client.Close(500);
            }
            else
            {
                if ((DateTime.Now - room.LastPingSync).TotalSeconds < 7.0)
                    return;
                byte[] Pings = new byte[16];
                for (int index = 0; index < 16; ++index)
                    Pings[index] = (byte)room._slots[index].ping;
                using (PROTOCOL_BATTLE_SENDPING_ACK battleSendpingAck = new PROTOCOL_BATTLE_SENDPING_ACK(Pings))
                    room.SendPacketToPlayers((SendPacket)battleSendpingAck, SlotState.BATTLE, 0);
                room.LastPingSync = DateTime.Now;
            }
        }

        private bool CompareRounds(PointBlank.Game.Data.Model.Room room, int externValue) => room.rounds == externValue;

        private void EndRound(PointBlank.Game.Data.Model.Room room, bool isBotMode)
        {
            try
            {
                room.swapRound = true;
                if (room.room_type == RoomType.Boss || room.room_type == RoomType.CrossCounter)
                {
                    if (room.rounds == 1)
                    {
                        room.rounds = 2;
                        foreach (PointBlank.Core.Models.Room.Slot slot in room._slots)
                        {
                            if (slot.state == SlotState.BATTLE)
                            {
                                slot.killsOnLife = 0;
                                slot.lastKillState = 0;
                                slot.repeatLastState = false;
                            }
                        }
                        List<int> dinossaurs = AllUtils.getDinossaurs(room, true, -2);
                        using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK missionRoundEndAck = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(room, 2, RoundEndType.TimeOut))
                        {
                            using (PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK roundPreStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(room, dinossaurs, isBotMode))
                                room.SendPacketToPlayers((SendPacket)missionRoundEndAck, (SendPacket)roundPreStartAck, SlotState.BATTLE, 0);
                        }
                        room.round.Start(5250, (TimerCallback)(callbackState =>
                        {
                            if (room._state == RoomState.Battle)
                            {
                                room.BattleStart = DateTime.Now.AddSeconds(5.0);
                                using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK missionRoundStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(room))
                                    room.SendPacketToPlayers((SendPacket)missionRoundStartAck, SlotState.BATTLE, 0);
                            }
                            room.swapRound = false;
                            lock (callbackState)
                                room.round.Timer = (Timer)null;
                        }));
                    }
                    else
                    {
                        if (room.rounds != 2)
                            return;
                        AllUtils.EndBattle(room, isBotMode);
                    }
                }
                else if (room.thisModeHaveRounds())
                {
                    int winner = 1;
                    if (room.room_type != RoomType.Destroy)
                        ++room.blue_rounds;
                    else if (room.Bar1 > room.Bar2)
                    {
                        ++room.red_rounds;
                        winner = 0;
                    }
                    else if (room.Bar1 < room.Bar2)
                        ++room.blue_rounds;
                    else
                        winner = 2;
                    AllUtils.BattleEndRound(room, winner, RoundEndType.TimeOut);
                }
                else
                {
                    List<Account> allPlayers = room.getAllPlayers(SlotState.READY, 1);
                    if (allPlayers.Count != 0)
                    {
                        TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room);
                        room.CalculateResult(winnerTeam, isBotMode);
                        using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK missionRoundEndAck = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(room, winnerTeam, RoundEndType.TimeOut))
                        {
                            ushort MissionFlag;
                            ushort SlotFlag;
                            AllUtils.getBattleResult(room, out MissionFlag, out SlotFlag);
                            byte[] completeBytes = missionRoundEndAck.GetCompleteBytes(nameof(PROTOCOL_BATTLE_TIMERSYNC_REQ));
                            foreach (Account p in allPlayers)
                            {
                                if (room._slots[p._slotId].state == SlotState.BATTLE)
                                    p.SendCompletePacket(completeBytes);
                                p.SendPacket((SendPacket)new PROTOCOL_BATTLE_ENDBATTLE_ACK(p, winnerTeam, SlotFlag, MissionFlag, isBotMode));
                            }
                        }
                    }
                    AllUtils.resetBattleInfo(room);
                }
            }
            catch (Exception ex)
            {
                if (room != null)
                    Logger.error("PROTOCOL_BATTLE_TIMERSYNC_REQ: RoomId: " + room._roomId.ToString() + " ChannelId: " + room._channelId.ToString() + " RoomType: " + room.room_type.ToString());
                Logger.error(ex.ToString());
            }
        }
    }
}
