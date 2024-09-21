// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Model.Room
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Rank;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Gift;
using PointBlank.Core.Models.Map;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Core.Progress;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Sync;
using PointBlank.Game.Data.Sync.Server;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace PointBlank.Game.Data.Model
{
    public class Room
    {
        public Slot[] _slots = new Slot[16];
        public int _channelType;
        public int rounds = 1;
        public int TRex = -1;
        public int blue_rounds;
        public int blue_dino;
        public int red_rounds;
        public int red_dino;
        public int Bar1;
        public int Bar2;
        public int _ping = 5;
        public int _redKills;
        public int _redDeaths;
        public int _redAssists;
        public int _blueKills;
        public int _blueDeaths;
        public int _blueAssists;
        public int spawnsCount;
        public int rule;
        public int killtime;
        public int _roomId;
        public int _channelId;
        public int _leader;
        public byte Limit;
        public byte WatchRuleFlag;
        public byte aiCount = 1;
        public byte IngameAiLevel;
        public byte aiLevel;
        public byte aiType;
        public byte stage;
        public short BalanceType;
        public readonly int[] TIMES = new int[11]
        {
      3,
      3,
      3,
      5,
      7,
      5,
      10,
      15,
      20,
      25,
      30
        };
        public readonly int[] KILLS = new int[9]
        {
      15,
      30,
      50,
      60,
      80,
      100,
      120,
      140,
      160
        };
        public readonly int[] ROUNDS = new int[6]
        {
      1,
      2,
      3,
      5,
      7,
      9
        };
        public readonly int[] RED_TEAM = new int[8]
        {
      0,
      2,
      4,
      6,
      8,
      10,
      12,
      14
        };
        public readonly int[] BLUE_TEAM = new int[8]
        {
      1,
      3,
      5,
      7,
      9,
      11,
      13,
      15
        };
        public byte[] HitParts = new byte[35];
        public byte[] DefaultParts = new byte[35]
        {
       0,
       1,
       2,
       3,
       4,
       5,
       6,
       7,
       8,
       9,
       10,
       11,
       12,
       13,
       14,
       15,
       16,
       17,
       18,
       19,
       20,
       21,
       22,
       23,
       24,
       25,
       26,
       27,
       28,
       29,
       30,
       31,
       32,
       33,
       34
        };
        public uint _timeRoom;
        public DateTime StartDate;
        public uint UniqueRoomId;
        public uint Seed;
        public long StartTick;
        public string name;
        public string password;
        public string _mapName;
        public Core.Models.Room.VoteKick votekick;
        public MapIdEnum mapId;
        public RoomType room_type;
        public RoomState _state;
        public GameRuleFlag RuleFlag;
        public RoomStageFlag Flag;
        public RoomWeaponsFlag weaponsFlag;
        public bool C4_actived;
        public bool swapRound;
        public bool changingSlots;
        public bool blockedClan;
        public bool ShotgunMode;
        public bool SniperMode;
        public BattleServer UdpServer;
        public DateTime BattleStart;
        public DateTime LastPingSync = DateTime.Now;
        public TimerState bomb = new TimerState();
        public TimerState countdown = new TimerState();
        public TimerState round = new TimerState();
        public TimerState vote = new TimerState();
        public SafeList<long> kickedPlayers = new SafeList<long>();
        public SafeList<long> requestHost = new SafeList<long>();
        public bool GameRuleActive;
        public bool ShotgunActive;
        public bool BarrettActive;
        public bool MaskActive;
        public List<GameRule> GameRules = new List<GameRule>();
        public byte[] SlotRewards;
        public int[] ItensRewards;

        public Room(int roomId, Channel ch)
        {
            _roomId = roomId;
            for (int slotIdx = 0; slotIdx < _slots.Length; ++slotIdx)
                _slots[slotIdx] = new Slot(slotIdx);
            _channelId = ch._id;
            _channelType = ch._type;
            SetUniqueId();
        }

        public bool thisModeHaveCD()
        {
            RoomType roomType = room_type;
            switch (roomType)
            {
                case RoomType.Bomb:
                case RoomType.Annihilation:
                case RoomType.Boss:
                case RoomType.CrossCounter:
                case RoomType.Convoy:
                    return true;
                default:
                    return roomType == RoomType.Ace;
            }
        }

        public bool thisModeHaveRounds()
        {
            RoomType roomType = room_type;
            switch (roomType)
            {
                case RoomType.Bomb:
                case RoomType.Destroy:
                case RoomType.Annihilation:
                case RoomType.Defense:
                case RoomType.Convoy:
                    return true;
                default:
                    return roomType == RoomType.Ace;
            }
        }

        public int getFlag()
        {
            int num = 0;
            if (Flag.HasFlag(RoomStageFlag.RANDOM_MAP))
                num += 2;
            if (Flag.HasFlag(RoomStageFlag.PASSWORD) || password.Length > 0)
                num += 4;
            if (BalanceType == 1)
                num += 32;
            if (Limit > 0 && _state > RoomState.Ready)
                num += 128;
            Flag = (RoomStageFlag)num;
            return num;
        }

        public void LoadHitParts()
        {
            int next = new Random().Next(34);
            byte[] array = DefaultParts.OrderBy(x => x <= next).ToArray();
            Logger.warning("Idx: " + next.ToString() + "/ Hits: " + BitConverter.ToString(array));
            HitParts = array;
            byte[] numArray = new byte[35];
            for (int index = 0; index < 35; ++index)
            {
                byte hitPart = HitParts[index];
                numArray[(index + 8) % 35] = hitPart;
            }
            Logger.warning("Array: " + BitConverter.ToString(numArray));
        }

        private void SetUniqueId() => UniqueRoomId = (uint)((GameConfig.serverId & byte.MaxValue) << 20 | (_channelId & byte.MaxValue) << 12 | _roomId & 4095);

        public void SetSeed() => Seed = (uint)((RoomType)((int)(mapId & (MapIdEnum)255) << 20 | (rule & byte.MaxValue) << 12) | room_type & (RoomType)4095);

        public void SetBotLevel()
        {
            if (!isBotMode())
                return;
            IngameAiLevel = aiLevel;
            for (int index = 0; index < 16; ++index)
                _slots[index].aiLevel = IngameAiLevel;
        }

        public bool isBotMode() => stage == 2 || stage == 4 || stage == 6;

        private void SetSpecialStage()
        {
            if (room_type == RoomType.Defense)
            {
                if (mapId != MapIdEnum.BlackPanther)
                    return;
                Bar1 = 6000;
                Bar2 = 9000;
            }
            else
            {
                if (room_type != RoomType.Destroy)
                    return;
                if (mapId == MapIdEnum.Hospital)
                {
                    Bar1 = 12000;
                    Bar2 = 12000;
                }
                else
                {
                    if (mapId != MapIdEnum.BreakDown)
                        return;
                    Bar1 = 6000;
                    Bar2 = 6000;
                }
            }
        }

        public int getInBattleTime()
        {
            int num = 0;
            if (BattleStart != new DateTime() && (_state == RoomState.Battle || _state == RoomState.PreBattle))
            {
                num = (int)(DateTime.Now - BattleStart).TotalSeconds;
                if (num < 0)
                    num = 0;
            }
            return num;
        }

        public int getInBattleTimeLeft()
        {
            int inBattleTime = getInBattleTime();
            return getTimeByMask() * 60 - inBattleTime;
        }

        public Channel getChannel() => ChannelsXml.getChannel(_channelId);

        public bool getChannel(out Channel ch)
        {
            ch = ChannelsXml.getChannel(_channelId);
            return ch != null;
        }

        public bool getSlot(int slotIdx, out Slot slot)
        {
            slot = null;
            lock (_slots)
            {
                if (slotIdx >= 0 && slotIdx <= 15)
                    slot = _slots[slotIdx];
                return slot != null;
            }
        }

        public Slot getSlot(int slotIdx)
        {
            lock (_slots)
                return slotIdx >= 0 && slotIdx <= 15 ? _slots[slotIdx] : null;
        }

        public void StartCounter(int type, Account player, Slot slot)
        {
            EventErrorEnum error = EventErrorEnum.SUCCESS;
            int period;
            switch (type)
            {
                case 0:
                    error = EventErrorEnum.BATTLE_FIRST_MAINLOAD;
                    period = 90000;
                    break;
                case 1:
                    error = EventErrorEnum.BATTLE_FIRST_HOLE;
                    period = 30000;
                    break;
                default:
                    return;
            }
            slot.timing.Start(period, callbackState =>
            {
                BaseCounter(error, player, slot);
                lock (callbackState)
                    slot?.StopTiming();
            });
        }

        private void BaseCounter(EventErrorEnum error, Account player, Slot slot)
        {
            player.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(error));
            player.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(player, 0));
            slot.state = SlotState.NORMAL;
            AllUtils.BattleEndPlayersCount(this, isBotMode());
            updateSlotsInfo();
        }

        public void StartBomb()
        {
            try
            {
                bomb.Start(42000, callbackState =>
                {
                    if (this != null && C4_actived)
                    {
                        ++red_rounds;
                        C4_actived = false;
                        AllUtils.BattleEndRound(this, 0, RoundEndType.BombFire);
                    }
                    lock (callbackState)
                        bomb.Timer = null;
                });
            }
            catch (Exception ex)
            {
                Logger.warning("StartBomb: " + ex.ToString());
            }
        }

        public void StartVote()
        {
            try
            {
                if (votekick == null)
                    return;
                vote.Start(20000, callbackState =>
                {
                    AllUtils.votekickResult(this);
                    lock (callbackState)
                        vote.Timer = null;
                });
            }
            catch (Exception ex)
            {
                Logger.warning("StartVote: " + ex.ToString());
                if (vote.Timer != null)
                    vote.Timer = null;
                votekick = null;
            }
        }

        public void RoundRestart()
        {
            try
            {
                StopBomb();
                foreach (Slot slot in _slots)
                {
                    if (slot._playerId > 0L && slot.state == SlotState.BATTLE)
                    {
                        if (!slot._deathState.HasFlag(DeadEnum.UseChat))
                            slot._deathState |= DeadEnum.UseChat;
                        if (slot.espectador)
                            slot.espectador = false;
                        if (slot.killsOnLife >= 3 && room_type == RoomType.Annihilation)
                            ++slot.objects;
                        slot.killsOnLife = 0;
                        slot.lastKillState = 0;
                        slot.repeatLastState = false;
                        slot.damageBar1 = 0;
                        slot.damageBar2 = 0;
                    }
                }
                round.Start(8000, callbackState =>
                {
                    foreach (Slot slot in _slots)
                    {
                        if (slot._playerId > 0L)
                        {
                            if (!slot._deathState.HasFlag(DeadEnum.UseChat))
                                slot._deathState |= DeadEnum.UseChat;
                            if (slot.espectador)
                                slot.espectador = false;
                        }
                    }
                    StopBomb();
                    DateTime now = DateTime.Now;
                    if (_state == RoomState.Battle)
                        BattleStart = room_type == RoomType.Boss || room_type == RoomType.CrossCounter ? now.AddSeconds(5.0) : now;
                    using (PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK roundPreStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(this))
                    {
                        using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK missionRoundStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(this))
                            SendPacketToPlayers(roundPreStartAck, missionRoundStartAck, SlotState.BATTLE, 0);
                    }
                    StopBomb();
                    swapRound = false;
                    lock (callbackState)
                        round.Timer = null;
                });
            }
            catch (Exception ex)
            {
                Logger.warning("[Room.RoundRestart] " + ex.ToString());
            }
        }

        public void StopBomb()
        {
            if (!C4_actived)
                return;
            C4_actived = false;
            if (bomb == null)
                return;
            bomb.Timer = null;
        }

        public void StartBattle(bool updateInfo)
        {
            Monitor.Enter(_slots);
            _state = RoomState.Loading;
            requestHost.Clear();
            UdpServer = BattleServerXml.GetRandomServer();
            StartTick = DateTime.Now.Ticks;
            StartDate = DateTime.Now;
            SetBotLevel();
            AllUtils.CheckClanMatchRestrict(this);
            using (PROTOCOL_BATTLE_START_GAME_ACK battleStartGameAck = new PROTOCOL_BATTLE_START_GAME_ACK(this))
            {
                byte[] completeBytes = battleStartGameAck.GetCompleteBytes("Room.StartBattle");
                foreach (Account allPlayer in getAllPlayers(SlotState.READY, 0))
                {
                    Slot slot = getSlot(allPlayer._slotId);
                    if (slot != null)
                    {
                        slot.withHost = true;
                        slot.state = SlotState.LOAD;
                        slot.SetMissionsClone(allPlayer._mission);
                        allPlayer.SendCompletePacket(completeBytes);
                    }
                }
            }
            if (updateInfo)
                updateSlotsInfo();
            updateRoomInfo();
            Monitor.Exit(_slots);
        }

        public void StartCountDown()
        {
            using (PROTOCOL_BATTLE_START_COUNTDOWN_ACK startCountdownAck = new PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum.Start))
                SendPacketToPlayers(startCountdownAck);
            countdown.Start(5250, callbackState =>
            {
                try
                {
                    if (_slots[_leader].state == SlotState.READY)
                    {
                        if (_state == RoomState.CountDown)
                            StartBattle(true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.warning("[Room.StartCountDown] " + ex.ToString());
                }
                lock (callbackState)
                    countdown.Timer = null;
            });
        }

        public void StopCountDown(CountDownEnum motive, bool refreshRoom = true)
        {
            _state = RoomState.Ready;
            if (refreshRoom)
                updateRoomInfo();
            countdown.Timer = null;
            using (PROTOCOL_BATTLE_START_COUNTDOWN_ACK startCountdownAck = new PROTOCOL_BATTLE_START_COUNTDOWN_ACK(motive))
                SendPacketToPlayers(startCountdownAck);
        }

        public void StopCountDown(int slotId)
        {
            if (_state != RoomState.CountDown)
                return;
            if (slotId == _leader)
            {
                StopCountDown(CountDownEnum.StopByHost);
            }
            else
            {
                if (getPlayingPlayers(_leader % 2 == 0 ? 1 : 0, SlotState.READY, 0) != 0)
                    return;
                changeSlotState(_leader, SlotState.NORMAL, false);
                StopCountDown(CountDownEnum.StopByPlayer);
            }
        }

        public void CalculateResult()
        {
            lock (_slots)
                BaseResultGame(AllUtils.GetWinnerTeam(this), isBotMode());
        }

        public void CalculateResult(TeamResultType resultType)
        {
            lock (_slots)
                BaseResultGame(resultType, isBotMode());
        }

        public void CalculateResult(TeamResultType resultType, bool isBotMode)
        {
            lock (_slots)
                BaseResultGame(resultType, isBotMode);
        }

        public void CalculateResultFreeForAll(int SlotWin)
        {
            lock (_slots)
                BaseResultGameFreeForAll(SlotWin);
        }

        private void BaseResultGame(TeamResultType winnerTeam, bool isBotMode)
        {
            ServerConfig config = GameManager.Config;
            EventUpModel runningEvent1 = EventRankUpSyncer.getRunningEvent();
            EventMapModel runningEvent2 = EventMapSyncer.getRunningEvent();
            bool flag = EventMapSyncer.EventIsValid(runningEvent2, (int)mapId, (int)room_type);
            PlayTimeModel runningEvent3 = EventPlayTimeSyncer.getRunningEvent();
            DateTime now = DateTime.Now;
            if (config == null)
            {
                Logger.error("Server Config Null. RoomResult canceled.");
            }
            else
            {
                Random rnd = new Random();
                int EndPlayers = getPlayingPlayers(2, SlotState.BATTLE, 0);
                double RequiredRoomTime = (now - StartDate).TotalSeconds / 2;
                Dictionary<byte, int> EndBattleRewards = new Dictionary<byte, int>();

                foreach (Slot slot in _slots)
                {
                    Account player;
                    if (!slot.check && slot.state == SlotState.BATTLE && getPlayerBySlot(slot, out player))
                    {
                        double timePlayed = slot.inBattleTime(now); //tempo decorido da partida do slot
                        DBQuery query1 = new DBQuery();
                        DBQuery query2 = new DBQuery();
                        DBQuery RKDQuery = new DBQuery();
                        slot.check = true;
                        double num1 = slot.inBattleTime(now);
                        int gp = player._gp;
                        int exp = player._exp;
                        int money = player._money;


                        if (!isBotMode)
                        {
                            if (config.missions)
                            {
                                AllUtils.endMatchMission(this, player, slot, winnerTeam);
                                if (slot.MissionsCompleted)
                                {
                                    player._mission = slot.Missions;
                                    MissionManager.getInstance().updateCurrentMissionList(player.player_id, player._mission);
                                }
                                AllUtils.GenerateMissionAwards(player, query1);
                            }

                            //Criterios pra entrar no sorteio
                            //4 players ou +
                            // Jogado 240 segundos
                            // 5 sorteados  é fixo, não mude
                            if (GameConfig.RewardPerBattle && EndPlayers >= 2 && timePlayed > 120 && EndBattleRewards.Count < 5)
                            {
                                int sorted = rnd.Next(0, 15);
                                int RandomDraw = rnd.Next(1, 8);
                               

                                //Aumenta a chance se jogou 50% da partida (2 em 16)
                                //caso jogou menos é 1 em 16
                               if (RequiredRoomTime > timePlayed ? sorted == slot._id : (sorted == slot._id || (slot._id == 15 ? sorted == slot._id - 1 : sorted == slot._id + 1)))
                                {
                                    try
                                    {

                                        int Result;
                                        if (RandomDraw == 2)
                                        {
                                            Result = 1800666; // Token Dourado
                                        }
                                        else if (RandomDraw == 4)
                                        {
                                           
                                            Result = 1800667; // Token preto
                                        }
                                        else if (RandomDraw == 6)
                                        {
                                            Result = 1800668; //Token Vermelho
                                        }
                                        else
                                        {
                                            Result = 2900001; // tag
                                        }

                                        BattleBoxInfo bbx = rnd.Next(0, 15) == slot._id ? BattleBoxManager.Sort() : null;
                                        int item = bbx != null ? bbx.ID : Result;

                                        EndBattleRewards.Add((byte)slot._id, item);

                                        if (item == 2900001)
                                            query1.AddQuery("tag", ++player._tag);
                                        else if (bbx == null)
                                            player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, new ItemsModel(Result, 3, "Ticket", 1, 1)), false);
                                        else
                                            player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, bbx!.ToItemModel()));
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.error($"[Room.BaseResultGame][EndBattleRewards][{slot._id}]: {ex.Message}");
                                    }
                               }
                           }
                            int num2 = slot.allKills != 0 || slot.allDeaths != 0 ? (int)num1 : (int)(num1 / 3.0);
                            if (room_type == RoomType.Bomb || room_type == RoomType.Annihilation)
                            {
                                slot.exp = (int)(slot.Score + num2 / 10.5 + slot.allDeaths * 7.2 + slot.objects * 20);
                                slot.gp = (int)(slot.Score + num2 / 3.0 + slot.allDeaths * 2.2 + slot.objects * 20);
                                slot.money = (int)(slot.Score / 0.1 + num2 / 70.5 + slot.allDeaths * 10.5 + slot.objects * 30);
                            }
                            else
                            {
                                slot.exp = (int)(slot.Score + num2 / 10.5 + slot.allDeaths * 8.8 + slot.objects * 20);
                                slot.gp = (int)(slot.Score + num2 / 3.0 + slot.allDeaths * 1.8 + slot.objects * 20);
                                slot.money = (int)(slot.Score / 0.1 + num2 / 70.5 + slot.allDeaths * 10.1 + slot.objects * 30);
                            }
                            bool WonTheMatch = (TeamResultType)slot._team == winnerTeam;
                            if (rule != 80 && rule != 32)
                            {
                                player._statistic.headshots_count += slot.headshots;
                                player._statistic.kills_count += slot.allKills;
                                player._statistic.totalkills_count += slot.allKills;
                                player._statistic.deaths_count += slot.allDeaths;
                                player._statistic.assist += slot.allAssists;
                                AddKDInfosToQuery(slot, player._statistic, query1);
                                AllUtils.updateMatchCount(WonTheMatch, player, (int)winnerTeam, query1);
                                if (player.Daily != null)
                                {
                                    player.Daily.Kills += slot.allKills;
                                    player.Daily.Deaths += slot.allDeaths;
                                    player.Daily.Headshots += slot.headshots;
                                    AddDailyToQuery(slot, player.Daily, query2);
                                    AllUtils.UpdateDailyRecord(WonTheMatch, player, (int)winnerTeam, query2);
                                }
                            }
                            if (WonTheMatch)
                            {
                                slot.gp += AllUtils.percentage(slot.gp, 15);
                                slot.exp += AllUtils.percentage(slot.exp, 20);
                            }
                            if (slot.earnedXP > 0)
                                slot.exp += slot.earnedXP * 5;
                            slot.exp = slot.exp > GameConfig.maxBattleXP ? GameConfig.maxBattleXP : slot.exp;
                            slot.gp = slot.gp > GameConfig.maxBattleGP ? GameConfig.maxBattleGP : slot.gp;
                            slot.money = slot.money > GameConfig.maxBattleMY ? GameConfig.maxBattleMY : slot.money;
                            if (slot.exp < 0 || slot.gp < 0 || slot.money < 0)
                            {
                                slot.exp = 2;
                                slot.gp = 2;
                                slot.money = 2;
                            }
                            int num3 = 0;
                            int num4 = 0;
                            //int num5 = 0;
                            //int num6 = 0;
                            int num7 = 0;
                            int num8 = 0;
                            if (runningEvent1 != null | flag)
                            {
                                if (runningEvent1 != null)
                                {
                                    num7 += runningEvent1._percentXp;
                                    num8 += runningEvent1._percentGp;
                                }
                                if (flag)
                                {
                                    num7 += runningEvent2._percentXp;
                                    num8 += runningEvent2._percentGp;
                                }
                                if (!slot.bonusFlags.HasFlag(ResultIcon.Event))
                                    slot.bonusFlags |= ResultIcon.Event;
                            }
                            PlayerBonus bonus = player._bonus;
                            if (bonus != null && bonus.bonuses > 0)
                            {
                                if ((bonus.bonuses & 8) == 8)
                                    num3 += 100;
                                if ((bonus.bonuses & 128) == 128)
                                    num4 += 100;
                                if ((bonus.bonuses & 4) == 4)
                                    num3 += 50;
                                if ((bonus.bonuses & 64) == 64)
                                    num4 += 50;
                                if ((bonus.bonuses & 2) == 2)
                                    num3 += 30;
                                if ((bonus.bonuses & 32) == 32)
                                    num4 += 30;
                                if ((bonus.bonuses & 1) == 1)
                                    num3 += 10;
                                if ((bonus.bonuses & 16) == 16)
                                    num4 += 10;
                                if (!slot.bonusFlags.HasFlag(ResultIcon.Item))
                                    slot.bonusFlags |= ResultIcon.Item;
                                slot.BonusItemExp += num3;
                                slot.BonusItemPoint += num4;
                            }
                            if (player.pc_cafe == 1 || player.pc_cafe == 2 || player.pc_cafe == 3 || player.pc_cafe == 4 || player.pc_cafe == 5 || player.pc_cafe == 6 && !isBotMode)
                            {
                                int ExpCafe = 0;
                                int Goldcafe = 0;
                                int Cashcafe = 0;

                                if (player.pc_cafe == 1) //VIP BASIC
                                {
                                    ExpCafe = 1250;
                                    Goldcafe = 1250;
                                    Cashcafe = 500;
                                }
                                else if (player.pc_cafe == 2)  // VIP PLUS
                                {
                                    ExpCafe = 2500;
                                    Goldcafe = 2500;
                                    Cashcafe = 1000;
                                }
                                else if (player.pc_cafe == 3) // VIP MASTER
                                {
                                    ExpCafe = 3750;
                                    Goldcafe = 3750;
                                    Cashcafe = 1500;
                                }
                                else if (player.pc_cafe == 4) // VIP COMBAT
                                {
                                    ExpCafe = 6000;
                                    Goldcafe = 6000;
                                    Cashcafe = 2000;
                                }
                                else if (player.pc_cafe == 5) // VIP EXTREME
                                {
                                    ExpCafe = 8250;
                                    Goldcafe = 8250;
                                    Cashcafe = 2500;
                                }
                                else if (player.pc_cafe == 6) // VIP BOOSTER
                                {
                                    ExpCafe = 500;
                                    Goldcafe = 500;
                                    Cashcafe = 250;
                                }


                                if (player.pc_cafe == 1 && !slot.bonusFlags.HasFlag(ResultIcon.Pc))
                                    slot.bonusFlags |= ResultIcon.Pc;
                                else if (player.pc_cafe == 2 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                    slot.bonusFlags |= ResultIcon.PcPlus;
                                else if (player.pc_cafe == 3 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                    slot.bonusFlags |= ResultIcon.PcPlus;
                                else if (player.pc_cafe == 4 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                    slot.bonusFlags |= ResultIcon.PcPlus;
                                else if (player.pc_cafe == 5 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                    slot.bonusFlags |= ResultIcon.PcPlus;
                                else if (player.pc_cafe == 6 && !slot.bonusFlags.HasFlag(ResultIcon.Pc))
                                    slot.bonusFlags |= ResultIcon.Pc;

                                slot.BonusCafeExp += ExpCafe;
                                slot.BonusCafePoint += Goldcafe;
                                slot.money += Cashcafe;

                            }
                            int percent1 = num7 + slot.BonusCafeExp + slot.BonusItemExp;
                            int percent2 = num8 + slot.BonusCafePoint + slot.BonusItemPoint;
                            slot.BonusEventExp = AllUtils.percentage(slot.exp, percent1);
                            slot.BonusEventPoint = AllUtils.percentage(slot.gp, percent2);



                            EventWeaponExp(player, slot);

                            if (name.ToLower().Contains("@ranked") && room_type == RoomType.Bomb && GameManager.Config.RankedEnable)
                                {
                                    int expranked = -1;

                                    if (WonTheMatch) // Vencedor
                                    {
                                        expranked = (int)(0.5 * num2 + slot.objects * player.pc_cafe);
                                    }
                                    else // Perdedor
                                    {
                                        expranked = (int)(0.1 * num2 + slot.objects * player.pc_cafe);
                                    }

                                    if (player.Ranked != null)
                                    {
                                        player.Ranked.Exp += expranked;
                                        player.Ranked.Playtime += (int)timePlayed;
                                        RKDQuery.AddQuery("exp", player.Ranked.Exp);
                                        RKDQuery.AddQuery("playtime", player.Ranked.Playtime);

                                        player.Ranked.Kills += slot.allKills;
                                        player.Ranked.Deaths += slot.allDeaths;
                                        player.Ranked.Headshots += slot.headshots;

                                        AddRankedToQuery(slot, player.Ranked, RKDQuery);
                                        AllUtils.UpdateRanked(WonTheMatch, player, RKDQuery);

                                        RankedModel Ranked = RankedXml.getRanked(player.Ranked.Rank);
                                       
                                        if (Ranked != null && player.Ranked.Exp >= Ranked._onNextLevel  && player.Ranked.Rank <= 20)
                                        {
                                            List<ItemsModel> RankedAward = RankedXml.getRankedAwards(player.Ranked.Rank);
                                            if (RankedAward.Count > 0)
                                            {
                                                for (int index = 0; index < RankedAward.Count; ++index)
                                                {
                                                    ItemsModel itemsModel = RankedAward[index];
                                                    if (itemsModel._id != 0)
                                                        player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                                }
                                            }
                                            player._gp += Ranked._onGPUp;
                                            player._money += Ranked._onCHUp;
                                            player._tag += Ranked._onTGUp;

                                        player.tourneyLevel = ++player.Ranked.Rank;

                                        RKDQuery.AddQuery("rank", player.tourneyLevel);
                                        query1.AddQuery("ranked_point", player.tourneyLevel);

                                        Message msg = new Message(3)
                                        {
                                            sender_name = "[Ranked] Combat Global",
                                            sender_id = 0,
                                            text = $"Parabéns! Você subiu para a patente {Functions.NameRanked(player.tourneyLevel)} no modo Ranked. Continue evoluindo e conquistando!",
                                            state = 1
                                        };

                                        player.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                                        MessageManager.CreateMessage(player.player_id, msg);

                                    }

                                        ComDiv.updateDB("player_ranked", "player_id", player.player_id, RKDQuery.GetTables(), RKDQuery.GetValues());

                                    }
                                }
                            


                            player._gp += slot.gp + slot.BonusEventPoint;
                            player._exp += slot.exp + slot.BonusEventExp;

                            if (player.Daily != null)
                            {
                                player.Daily.Point += slot.gp + slot.BonusEventPoint;
                                player.Daily.Exp += slot.exp + slot.BonusEventExp;
                                player.Daily.Playtime += (int)timePlayed;
                                player.Daily.Money += slot.money;


                                query2.AddQuery("point", player.Daily.Point);
                                query2.AddQuery("exp", player.Daily.Exp);
                                query2.AddQuery("playtime", player.Daily.Playtime);
                                query2.AddQuery("money", player.Daily.Money);
                            }

                            if (GameConfig.winCashPerBattle)
                                player._money += slot.money;
                            RankModel rank = RankXml.getRank(player._rank);
                            //RankXml.getRank(61);
                            if (rank != null && player._exp >= rank._onNextLevel + rank._onAllExp && player._rank <= 50)
                            {
                                List<ItemsModel> awards = RankXml.getAwards(player._rank);
                                if (awards.Count > 0)
                                {
                                    for (int index = 0; index < awards.Count; ++index)
                                    {
                                        ItemsModel itemsModel = awards[index];
                                        if (itemsModel._id != 0)
                                            player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                    }
                                }
                                player._gp += rank._onGPUp;
                                player.LastRankUpDate = uint.Parse(now.ToString("yyMMddHHmm"));
                                player.SendPacket(new PROTOCOL_BASE_RANK_UP_ACK(++player._rank, rank._onNextLevel));
                                query1.AddQuery("last_rankup_date", (long)player.LastRankUpDate);
                                query1.AddQuery("rank", player._rank);
                            }
                            if (runningEvent3 != null)
                            AllUtils.PlayTimeEvent((long)num1, player, runningEvent3, isBotMode);
                            AllUtils.DiscountPlayerItems(slot, player);

                            if(player.LastFreeBonus.Date != DateTime.Now.Date ) 
                            {

                                player.LastFreeBonus = DateTime.Now;
                                if (ComDiv.updateDB("players", "\"LastFreeBonus\"", player.LastFreeBonus, "player_id", player.player_id))
                                {
                                    int bonusBase = new Random().Next(500, 3500);

                                    int bonusEvent = new Random().Next(500, 1500);

                                    int bonusVIP = -1;

                                    int bonusEV = -1;

                                    string message = $"Você recebeu seu bônus diário de {bonusBase:n0} Cash";

                                    switch (player.pc_cafe)
                                    {
                                        case 6: bonusVIP = (int)(bonusBase * 0.1); break; // BOOSTER
                                        case 5: bonusVIP = (int)(bonusBase * 2); break; // EXTREME
                                        case 4: bonusVIP = (int)(bonusBase * 1); break; // COMBAT
                                        case 3: bonusVIP = (int)(bonusBase * 0.8); break; // MASTER
                                        case 2: bonusVIP = (int)(bonusBase * 0.6); break; // PLUS
                                        case 1: bonusVIP = (int)(bonusBase * 0.4); break; // BASIC
                                    }

                                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                                    {
                                        bonusEV = (int)(bonusEvent * 0.1);
                                    }

                                    if (bonusEV >= 0)
                                    {
                                        message += $"\nBônus EVE:+{bonusEV:n0}";
                                        player._money += bonusEV;
                                    }

                                    if (bonusVIP >= 0)
                                    {
                                        message += $"\nBônus VIP:+{bonusVIP:n0}";
                                        player._money += bonusVIP;
                                    }

                                    if (bonusEV >= 0 || bonusVIP >= 0)
                                    {
                                        bonusEV = bonusEV == -1 ? 0 : bonusEV;
                                        bonusVIP = bonusVIP == -1 ? 0 : bonusVIP;
                                        message += $"\nTotal: {(bonusBase + bonusVIP + bonusEV):n0}";
                                    }
                                    else
                                    {
                                        message += $"\nTotal: {bonusBase}";
                                    }

                                    // Send  Message
                                    player._money += bonusBase;

                                    Message msg = new Message(3)
                                    {
                                        sender_name = "[Free Bonus] Combat Global",
                                        sender_id = 0,
                                        text = message,
                                        state = 1
                                    };

                                    player.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                                    MessageManager.CreateMessage(player.player_id, msg);

                                }


                            }

                                if (gp != player._gp)
                                query1.AddQuery("gp", player._gp);
                            if (exp != player._exp)
                                query1.AddQuery("exp", player._exp);
                            if (money != player._money)
                                query1.AddQuery("money", player._money);
                            ComDiv.updateDB("players", "player_id", player.player_id, query1.GetTables(), query1.GetValues());
                            ComDiv.updateDB("player_dailyrecord", "player_id", player.player_id, query2.GetTables(), query2.GetValues());
                            if (GameConfig.winCashPerBattle && GameConfig.showCashReceiveWarn)
                                player.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Translation.GetLabel("CashReceived", slot.money)));
                           
                        }
                        else
                        {
                            player._gp += slot.gp = 100;
                            player._exp += slot.exp  = 100;
                            query1.AddQuery("gp", player._gp);
                            query1.AddQuery("exp", player._exp);
                            ComDiv.updateDB("players", "player_id", player.player_id, query1.GetTables(), query1.GetValues());
                            ComDiv.updateDB("player_dailyrecord", "player_id", player.player_id, query2.GetTables(), query2.GetValues());
                        }
                    }

                    
                }

                //ajuste dos premios
                if (!isBotMode && GameConfig.RewardPerBattle && SlotRewards is null)
                {
                    SlotRewards = EndBattleRewards.Keys.ToArray();
                    ItensRewards = EndBattleRewards.Values.ToArray();
                    if (SlotRewards.Length < 5)
                    {
                        Array.Resize(ref SlotRewards, 5);
                        Array.Resize(ref ItensRewards, 5);

                        int add = 5 - EndBattleRewards.Count;
                        for (int i = 4 - (add - 1); i <= 4; i++)
                        {
                            SlotRewards[i] = 255;
                            ItensRewards[i] = -1;
                        }
                    }
                }



                updateSlotsInfo();
                CalculateClanMatchResult((int)winnerTeam);

            }
        }

        private void BaseResultGameFreeForAll(int winner)
        {
            ServerConfig config = GameManager.Config;
            EventUpModel runningEvent1 = EventRankUpSyncer.getRunningEvent();
            EventMapModel runningEvent2 = EventMapSyncer.getRunningEvent();
            bool flag = EventMapSyncer.EventIsValid(runningEvent2, (int)mapId, (int)room_type);
            PlayTimeModel runningEvent3 = EventPlayTimeSyncer.getRunningEvent();
            DateTime now = DateTime.Now;
            int[] numArray = new int[16];
            int SlotWin = 0;
            if (config == null)
            {
                Logger.error("Server Config Null. RoomResult canceled.");
            }
            else
            {

                Random rnd = new Random();
                int EndPlayers = getPlayingPlayers(2, SlotState.BATTLE, 0);
                double RequiredRoomTime = (now - StartDate).TotalSeconds / 2;
                Dictionary<byte, int> EndBattleRewards = new Dictionary<byte, int>();

                for (int index1 = 0; index1 < 16; ++index1)
                {
                    Slot slot = _slots[index1];
                    numArray[index1] = slot._playerId == 0L ? 0 : slot.allKills;
                    if (numArray[index1] > numArray[SlotWin])
                        SlotWin = index1;
                    Account player;
                    if (!slot.check && slot.state == SlotState.BATTLE && getPlayerBySlot(slot, out player))
                    {
                        double timePlayed = slot.inBattleTime(now); //tempo decorido da partida do slot
                        DBQuery query1 = new DBQuery();
                        DBQuery query2 = new DBQuery();
                        slot.check = true;
                        double num1 = slot.inBattleTime(now);
                        int gp = player._gp;
                        int exp = player._exp;
                        int money = player._money;
                        if (config.missions)
                        {
                            AllUtils.endMatchMission(this, player, slot, winner == SlotWin ? (_slots[SlotWin]._team == 0 ? TeamResultType.TeamRedWin : TeamResultType.TeamBlueWin) : TeamResultType.TeamDraw);
                            if (slot.MissionsCompleted)
                            {
                                player._mission = slot.Missions;
                                MissionManager.getInstance().updateCurrentMissionList(player.player_id, player._mission);
                            }
                            AllUtils.GenerateMissionAwards(player, query1);
                        }

                        //Criterios pra entrar no sorteio
                        //4 players ou +
                        // Jogado 240 segundos
                        // 5 sorteados  é fixo, não mude
                        if (GameConfig.RewardPerBattle && EndPlayers >= 4 && timePlayed > 240 && EndBattleRewards.Count < 5)
                        {
                            int sorted = rnd.Next(0, 15);
                            int RandomDraw = rnd.Next(1, 8);
                            //Aumenta a chance se jogou 50% da partida (2 em 16)
                            //caso jogou menos é 1 em 16
                            if (RequiredRoomTime > timePlayed ? sorted == slot._id : (sorted == slot._id || (slot._id == 15 ? sorted == slot._id - 1 : sorted == slot._id + 1)))
                            {
                                try
                                {
                                    int Result;
                                    if (RandomDraw == 2)
                                    {
                                        Result = 1800666; // Token Dourado
                                    }
                                    else if (RandomDraw == 4)
                                    {
                                        Result = 1800667; // Token preto
                                    }
                                    else if (RandomDraw == 6)
                                    {
                                        Result = 1800668; //Token Vermelho
                                    }
                                    else
                                    {
                                        Result = 2900001; // tag
                                    }

                                    BattleBoxInfo bbx = rnd.Next(0, 15) == slot._id ? BattleBoxManager.Sort() : null;
                                    int item = bbx != null ? bbx.ID : Result;

                                    EndBattleRewards.Add((byte)slot._id, item);

                                    if (item == 2900001)
                                        query1.AddQuery("tag", ++player._tag);
                                    else if (bbx == null)
                                        player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, new ItemsModel(Result, 3, "Ticket", 1, 1)), false);
                                    else
                                        player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, bbx!.ToItemModel()));
                                }

                                catch (Exception ex)
                                {
                                    Logger.error($"[Room.BaseResultGameFreeForAll][EndBattleRewards][{slot._id}]: {ex.Message}");
                                }
                            }

                        }


                        int num2 = slot.allKills != 0 || slot.allDeaths != 0 ? (int)num1 : (int)(num1 / 3.0);
                        slot.exp = (int)(slot.Score + num2 / 20.5 + slot.allDeaths * 10.8 + slot.objects * 20);
                        slot.gp = (int)(slot.Score + num2 / 3.0 + slot.allDeaths * 1.8 + slot.objects * 20);
                        slot.money = (int)(slot.Score / 0.1 + num2 / 70.5 + slot.allDeaths * 10.5 + slot.objects * 30);
                        if (rule != 80 && rule != 32)
                        {
                            player._statistic.headshots_count += slot.headshots;
                            player._statistic.kills_count += slot.allKills;
                            player._statistic.totalkills_count += slot.allKills;
                            player._statistic.deaths_count += slot.allDeaths;
                            player._statistic.assist += slot.allAssists;
                            AddKDInfosToQuery(slot, player._statistic, query1);
                            AllUtils.updateMatchCountFreeForAll(this, player, SlotWin, query1);
                            if (player.Daily != null)
                            {
                                player.Daily.Kills += slot.allKills;
                                player.Daily.Deaths += slot.allDeaths;
                                player.Daily.Headshots += slot.headshots;
                                AddDailyToQuery(slot, player.Daily, query2);
                                AllUtils.UpdateMatchDailyRecordFreeForAll(this, player, SlotWin, query2);
                            }
                        }
                        if (winner == SlotWin)
                        {
                            slot.gp += AllUtils.percentage(slot.gp, 15);
                            slot.exp += AllUtils.percentage(slot.exp, 20);
                        }
                        if (slot.earnedXP > 0)
                            slot.exp += slot.earnedXP * 5;
                        slot.exp = slot.exp > GameConfig.maxBattleXP ? GameConfig.maxBattleXP : slot.exp;
                        slot.gp = slot.gp > GameConfig.maxBattleGP ? GameConfig.maxBattleGP : slot.gp;
                        slot.money = slot.money > GameConfig.maxBattleMY ? GameConfig.maxBattleMY : slot.money;
                        if (slot.exp < 0 || slot.gp < 0 || slot.money < 0)
                        {
                            slot.exp = 2;
                            slot.gp = 2;
                            slot.money = 2;
                        }
                        int num3 = 0;
                        int num4 = 0;
                        //int num5 = 0;
                        //int num6 = 0;
                        int total1 = 0;
                        int total2 = 0;
                        if (runningEvent1 != null | flag)
                        {
                            if (runningEvent1 != null)
                            {
                                total1 += runningEvent1._percentXp;
                                total2 += runningEvent1._percentGp;
                            }
                            if (flag)
                            {
                                total1 += runningEvent2._percentXp;
                                total2 += runningEvent2._percentGp;
                            }
                            if (!slot.bonusFlags.HasFlag(ResultIcon.Event))
                                slot.bonusFlags |= ResultIcon.Event;
                            slot.BonusEventExp += AllUtils.percentage(total1, 100);
                            slot.BonusEventPoint += AllUtils.percentage(total2, 100);
                        }
                        PlayerBonus bonus = player._bonus;
                        if (bonus != null && bonus.bonuses > 0)
                        {
                            if ((bonus.bonuses & 8) == 8)
                                num3 += 100;
                            if ((bonus.bonuses & 128) == 128)
                                num4 += 100;
                            if ((bonus.bonuses & 4) == 4)
                                num3 += 50;
                            if ((bonus.bonuses & 64) == 64)
                                num4 += 50;
                            if ((bonus.bonuses & 2) == 2)
                                num3 += 30;
                            if ((bonus.bonuses & 32) == 32)
                                num4 += 30;
                            if ((bonus.bonuses & 1) == 1)
                                num3 += 10;
                            if ((bonus.bonuses & 16) == 16)
                                num4 += 10;
                            if (!slot.bonusFlags.HasFlag(ResultIcon.Item))
                                slot.bonusFlags |= ResultIcon.Item;
                            slot.BonusItemExp += num3;
                            slot.BonusItemPoint += num4;
                        }
                        if (player.pc_cafe == 1 || player.pc_cafe == 2 || player.pc_cafe == 3 || player.pc_cafe == 4 || player.pc_cafe == 5 || player.pc_cafe == 6)
                        {
                            int ExpCafe = 0;
                            int Goldcafe = 0;
                            int Cashcafe = 0;


                            if (player.pc_cafe == 1) //VIP BASIC
                            {
                                ExpCafe = 1250;
                                Goldcafe = 1250;
                                Cashcafe = 500;
                            }
                            else if (player.pc_cafe == 2)  // VIP PLUS
                            {
                                ExpCafe = 2500;
                                Goldcafe = 2500;
                                Cashcafe = 1000;
                            }
                            else if (player.pc_cafe == 3) // VIP MASTER
                            {
                                ExpCafe = 3750;
                                Goldcafe = 3750;
                                Cashcafe = 1500;
                            }
                            else if (player.pc_cafe == 4) // VIP COMBAT
                            {
                                ExpCafe = 6000;
                                Goldcafe = 6000;
                                Cashcafe = 2000;
                            }
                            else if (player.pc_cafe == 5) // VIP EXTREME
                            {
                                ExpCafe = 8250;
                                Goldcafe = 8250;
                                Cashcafe = 2500;
                            }
                            else if (player.pc_cafe == 6) // VIP BOOSTER
                            {
                                ExpCafe = 500;
                                Goldcafe = 500;
                                Cashcafe = 250;
                            }


                            if (player.pc_cafe == 1 && !slot.bonusFlags.HasFlag(ResultIcon.Pc))
                                slot.bonusFlags |= ResultIcon.Pc;
                            else if (player.pc_cafe == 2 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                slot.bonusFlags |= ResultIcon.PcPlus;
                            else if (player.pc_cafe == 3 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                slot.bonusFlags |= ResultIcon.PcPlus;
                            else if (player.pc_cafe == 4 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                slot.bonusFlags |= ResultIcon.PcPlus;
                            else if (player.pc_cafe == 5 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                                slot.bonusFlags |= ResultIcon.PcPlus;
                            else if (player.pc_cafe == 6 && !slot.bonusFlags.HasFlag(ResultIcon.Pc))
                                slot.bonusFlags |= ResultIcon.Pc;

                            slot.BonusCafeExp += ExpCafe;
                            slot.BonusCafePoint += Goldcafe;
                            slot.money += Cashcafe;

                        }
                        int percent1 = total1 + slot.BonusCafeExp + slot.BonusItemExp;
                        int percent2 = total2 + slot.BonusCafePoint + slot.BonusItemPoint;
                        slot.BonusEventExp = AllUtils.percentage(slot.exp, percent1);
                        slot.BonusEventPoint = AllUtils.percentage(slot.gp, percent2);

                        EventWeaponExp(player, slot);

                        player._gp += slot.gp + slot.BonusEventPoint;
                        player._exp += slot.exp + slot.BonusEventExp;
                        if (player.Daily != null)
                        {
                            player.Daily.Point += slot.gp + slot.BonusEventPoint;
                            player.Daily.Exp += slot.exp + slot.BonusEventExp;
                            player.Daily.Playtime += (int)timePlayed;
                            player.Daily.Money += slot.money;


                            query2.AddQuery("point", player.Daily.Point);
                            query2.AddQuery("exp", player.Daily.Exp);
                            query2.AddQuery("playtime", player.Daily.Playtime);
                            query2.AddQuery("money", player.Daily.Money);
                        }
                        if (GameConfig.winCashPerBattle)
                            player._money += slot.money;
                        RankModel rank1 = RankXml.getRank(player._rank);
                        RankModel rank2 = RankXml.getRank(61);
                        if (rank1 != null && player._exp >= rank1._onNextLevel + rank1._onAllExp && player._rank <= 50)
                        {
                            List<ItemsModel> awards = RankXml.getAwards(player._rank);
                            if (awards.Count > 0)
                            {
                                for (int index2 = 0; index2 < awards.Count; ++index2)
                                {
                                    ItemsModel itemsModel = awards[index2];
                                    if (itemsModel._id != 0)
                                        player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                }
                            }
                            player._gp += rank1._onGPUp;
                            player.LastRankUpDate = uint.Parse(now.ToString("yyMMddHHmm"));
                            player.SendPacket(new PROTOCOL_BASE_RANK_UP_ACK(++player._rank, rank1._onNextLevel));
                            query1.AddQuery("last_rankup_date", (long)player.LastRankUpDate);
                            query1.AddQuery("rank", player._rank);
                        }
                        else if (rank1 != null && player._exp >= rank2._onAllExp && player._rank == 51)
                        {
                            List<ItemsModel> awards = RankXml.getAwards(player._rank);
                            if (awards.Count > 0)
                            {
                                for (int index3 = 0; index3 < awards.Count; ++index3)
                                {
                                    ItemsModel itemsModel = awards[index3];
                                    if (itemsModel._id != 0)
                                        player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                }
                            }
                            player._gp += rank1._onGPUp;
                            player.LastRankUpDate = uint.Parse(now.ToString("yyMMddHHmm"));
                            player.SendPacket(new PROTOCOL_BASE_RANK_UP_ACK(61, rank1._onNextLevel));
                            query1.AddQuery("last_rankup_date", (long)player.LastRankUpDate);
                            query1.AddQuery("rank", 61);
                        }
                        else if (rank1 != null && player._exp >= rank1._onNextLevel + rank1._onAllExp && player._rank <= 110 && player._rank >= 61)
                        {
                            List<ItemsModel> awards = RankXml.getAwards(player._rank);
                            if (awards.Count > 0)
                            {
                                for (int index4 = 0; index4 < awards.Count; ++index4)
                                {
                                    ItemsModel itemsModel = awards[index4];
                                    if (itemsModel._id != 0)
                                        player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, itemsModel));
                                }
                            }
                            player._gp += rank1._onGPUp;
                            player.LastRankUpDate = uint.Parse(now.ToString("yyMMddHHmm"));
                            player.SendPacket(new PROTOCOL_BASE_RANK_UP_ACK(++player._rank, rank1._onNextLevel));
                            query1.AddQuery("last_rankup_date", (long)player.LastRankUpDate);
                            query1.AddQuery("rank", player._rank);
                        }
                        if (runningEvent3 != null)
                            AllUtils.PlayTimeEvent((long)num1, player, runningEvent3, false);
                        AllUtils.DiscountPlayerItems(slot, player);

                        if (player.LastFreeBonus.Date != DateTime.Now.Date)
                        {

                            player.LastFreeBonus = DateTime.Now;
                            if (ComDiv.updateDB("players", "\"LastFreeBonus\"", player.LastFreeBonus, "player_id", player.player_id))
                            {
                                int bonusBase = new Random().Next(500, 3500);

                                int bonusEvent = new Random().Next(500, 1500);

                                int bonusVIP = -1;

                                int bonusEV = -1;

                                string message = $"Você recebeu seu bônus diário de {bonusBase:n0} Cash";

                                switch (player.pc_cafe)
                                {
                                    case 6: bonusVIP = (int)(bonusBase * 0.1); break; // BOOSTER
                                    case 5: bonusVIP = (int)(bonusBase * 2); break; // EXTREME
                                    case 4: bonusVIP = (int)(bonusBase * 1); break; // COMBAT
                                    case 3: bonusVIP = (int)(bonusBase * 0.8); break; // MASTER
                                    case 2: bonusVIP = (int)(bonusBase * 0.6); break; // PLUS
                                    case 1: bonusVIP = (int)(bonusBase * 0.4); break; // BASIC
                                }

                                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    bonusEV = (int)(bonusEvent * 0.1);
                                }

                                if (bonusEV >= 0)
                                {
                                    message += $"\nBônus EVE:+{bonusEV:n0}";
                                    player._money += bonusEV;
                                }

                                if (bonusVIP >= 0)
                                {
                                    message += $"\nBônus VIP:+{bonusVIP:n0}";
                                    player._money += bonusVIP;
                                }

                                if (bonusEV >= 0 || bonusVIP >= 0)
                                {
                                    bonusEV = bonusEV == -1 ? 0 : bonusEV;
                                    bonusVIP = bonusVIP == -1 ? 0 : bonusVIP;
                                    message += $"\nTotal: {(bonusBase + bonusVIP + bonusEV):n0}";
                                }
                                else
                                {
                                    message += $"\nTotal: {bonusBase}";
                                }

                                // Send  Message
                                player._money += bonusBase;

                                Message msg = new Message(3)
                                {
                                    sender_name = "[Free Bonus] Combat Global!",
                                    sender_id = 0,
                                    text = message,
                                    state = 1
                                };

                                player.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                                MessageManager.CreateMessage(player.player_id, msg);

                            }


                        }

                        if (gp != player._gp)
                            query1.AddQuery("gp", player._gp);
                        if (exp != player._exp)
                            query1.AddQuery("exp", player._exp);
                        if (money != player._money)
                            query1.AddQuery("money", player._money);
                        ComDiv.updateDB("players", "player_id", player.player_id, query1.GetTables(), query1.GetValues());
                        ComDiv.updateDB("player_dailyrecord", "player_id", player.player_id, query2.GetTables(), query2.GetValues());
                        if (GameConfig.winCashPerBattle && GameConfig.showCashReceiveWarn)
                            player.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Translation.GetLabel("CashReceived", slot.money)));

                    }
                }

                                
                //ajuste dos premios
                if (GameConfig.RewardPerBattle && SlotRewards is null)
                {
                    SlotRewards = EndBattleRewards.Keys.ToArray();
                    ItensRewards = EndBattleRewards.Values.ToArray();
                    if (SlotRewards.Length < 5)
                    {
                        Array.Resize(ref SlotRewards, 5);
                        Array.Resize(ref ItensRewards, 5);

                        int add = 5 - EndBattleRewards.Count;
                        for (int i = 4 - (add - 1); i <= 4; i++)
                        {
                            SlotRewards[i] = 255;
                            ItensRewards[i] = -1;
                        }
                    }
                }

                updateSlotsInfo();
            }

        }

        public void EventWeaponExp(Account player, Slot slot)
        {
            //Event EXP Weapon
            foreach (int armaUsada in slot.armas_usadas)
            {
                WeaponExp ExpWeapon = WeaponExpXml.GetWeaponExp(armaUsada);
                if (ExpWeapon != null && armaUsada == ExpWeapon._Weapon)
                {
                    slot.BonusEventExp += ExpWeapon._exp;
                    if (!slot.bonusFlags.HasFlag(ResultIcon.Event))
                        slot.bonusFlags |= ResultIcon.Event;
                    Logger.console(player.player_name + " |-> Weapon " + ExpWeapon._Weapon + " Exp " + ExpWeapon._exp);
                }
            }

        }

        private void AddKDInfosToQuery(Slot slot, PlayerStats stats, DBQuery query)
        {
            if (slot.allKills > 0)
            {
                query.AddQuery("kills_count", stats.kills_count);
                query.AddQuery("totalkills_count", stats.totalkills_count);
            }
            if (slot.allAssists > 0)
                query.AddQuery("assist", stats.assist);
            if (slot.allDeaths > 0)
                query.AddQuery("deaths_count", stats.deaths_count);
            if (slot.headshots <= 0)
                return;
            query.AddQuery("headshots_count", stats.headshots_count);
        }

        private void AddDailyToQuery(Slot slot, PlayerDailyRecord Daily, DBQuery query)
        {
            if (Daily.Kills > 0)
                query.AddQuery("kills", Daily.Kills);
            if (Daily.Deaths > 0)
                query.AddQuery("deaths", Daily.Deaths);
            if (Daily.Headshots <= 0)
                return;
            query.AddQuery("headshots", Daily.Headshots);
        }
        private void AddRankedToQuery(Slot slot, PlayerRanked Rakd, DBQuery query)
        {
            if (Rakd.Kills > 0)
                query.AddQuery("kills", Rakd.Kills);
            if (Rakd.Deaths > 0)
                query.AddQuery("deaths", Rakd.Deaths);
            if (Rakd.Headshots <= 0)
                return;
            query.AddQuery("headshots", Rakd.Headshots);
        }

        private void CalculateClanMatchResult(int winnerTeam)
        {
            if (_channelType != 4 || blockedClan)
                return;
            SortedList<int, Core.Models.Account.Clan.Clan> sortedList = new SortedList<int, Core.Models.Account.Clan.Clan>();
            foreach (Slot slot in _slots)
            {
                Account player;
                if (slot.state == SlotState.BATTLE && getPlayerBySlot(slot, out player))
                {
                    Core.Models.Account.Clan.Clan clan = ClanManager.getClan(player.clanId);
                    if (clan._id != 0)
                    {
                        bool WonTheMatch = slot._team == winnerTeam;
                        clan._exp += slot.exp;
                        clan.BestPlayers.SetBestExp(slot);
                        clan.BestPlayers.SetBestKills(slot);
                        clan.BestPlayers.SetBestHeadshot(slot);
                        clan.BestPlayers.SetBestWins(player._statistic, slot, WonTheMatch);
                        clan.BestPlayers.SetBestParticipation(player._statistic, slot);
                        if (!sortedList.ContainsKey(player.clanId))
                        {
                            sortedList.Add(player.clanId, clan);
                            if (winnerTeam != 2)
                            {
                                CalculateSpecialCM(clan, winnerTeam, slot._team);
                                if (WonTheMatch)
                                    ++clan.vitorias;
                                else
                                    ++clan.derrotas;
                            }
                            PlayerManager.updateClanBattles(clan._id, ++clan.partidas, clan.vitorias, clan.derrotas);
                        }
                    }
                }
            }
            foreach (Core.Models.Account.Clan.Clan clan in (IEnumerable<Core.Models.Account.Clan.Clan>)sortedList.Values)
            {
                PlayerManager.updateClanExp(clan._id, clan._exp);
                PlayerManager.updateClanPoints(clan._id, clan._pontos);
                PlayerManager.updateBestPlayers(clan);
                RankModel rank = ClanRankXml.getRank(clan._rank);
                if (rank != null && clan._exp >= rank._onNextLevel + rank._onAllExp)
                    PlayerManager.updateClanRank(clan._id, ++clan._rank);
            }
        }

        private void CalculateSpecialCM(Core.Models.Account.Clan.Clan clan, int winnerTeam, int teamIdx)
        {
            if (winnerTeam == 2)
                return;
            if (winnerTeam == teamIdx)
            {
                float num = 25f + (room_type != RoomType.DeathMatch ? (teamIdx == 0 ? red_rounds : blue_rounds) : (teamIdx == 0 ? _redKills : _blueKills) / 20);
                clan._pontos += num;
            }
            else
            {
                if (clan._pontos == 0.0)
                    return;
                float num = 40f - (room_type != RoomType.DeathMatch ? (teamIdx == 0 ? red_rounds : blue_rounds) : (teamIdx == 0 ? _redKills : _blueKills) / 20);
                clan._pontos -= num;
            }
        }

        public bool isStartingMatch() => _state > RoomState.Ready;

        public bool isPreparing() => _state >= RoomState.Loading;

        public void updateRoomInfo()
        {
            SetSeed();
            using (PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK changeRoominfoAck = new PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(this))
                SendPacketToPlayers(changeRoominfoAck);
        }

        public void initSlotCount(int count, bool Change = false)
        {
            MapMatch mapMatch = MapModel.Matchs.Find(x => (MapIdEnum)x.Id == mapId && MapModel.getRule(x.Mode).Rule == rule);
            if (mapMatch != null)
                count = mapMatch.Limit;
            if (room_type == RoomType.Tutorial)
                count = 1;
            if (isBotMode())
                count = 8;
            if (count <= 0)
                count = 1;
            for (int index = 0; index < _slots.Length; ++index)
            {
                if (index >= count)
                    _slots[index].state = SlotState.CLOSE;
            }
            if (!Change)
                return;
            updateSlotsInfo();
        }

        public int getSlotCount()
        {
            lock (_slots)
            {
                int num = 0;
                for (int index = 0; index < _slots.Length; ++index)
                {
                    if (_slots[index].state != SlotState.CLOSE)
                        ++num;
                }
                return num;
            }
        }

        public void SwitchNewSlot(
          List<SlotChange> slots,
          Account p,
          Slot old,
          int teamIdx,
          bool Mode)
        {
            if (Mode)
            {
                Slot slot = _slots[teamIdx];
                if (slot._playerId != 0L || slot.state != SlotState.EMPTY)
                    return;
                slot.state = SlotState.NORMAL;
                slot._playerId = p.player_id;
                slot._equip = p._equip;
                old.state = SlotState.EMPTY;
                old._playerId = 0L;
                old._equip = null;
                if (p._slotId == _leader)
                    _leader = teamIdx;
                p._slotId = teamIdx;
                slots.Add(new SlotChange()
                {
                    oldSlot = old,
                    newSlot = slot
                });
            }
            else
            {
                for (int index = 0; index < GetTeamArray(teamIdx).Length; ++index)
                {
                    int team = GetTeamArray(teamIdx)[index];
                    Slot slot = _slots[team];
                    if (slot._playerId == 0L && slot.state == SlotState.EMPTY)
                    {
                        slot.state = SlotState.NORMAL;
                        slot._playerId = p.player_id;
                        slot._equip = p._equip;
                        old.state = SlotState.EMPTY;
                        old._playerId = 0L;
                        old._equip = null;
                        if (p._slotId == _leader)
                            _leader = team;
                        p._slotId = team;
                        slots.Add(new SlotChange()
                        {
                            oldSlot = old,
                            newSlot = slot
                        });
                        break;
                    }
                }
            }
        }

        public void SwitchSlots(
          List<SlotChange> slots,
          int newSlotId,
          int oldSlotId,
          bool changeReady)
        {
            Slot slot1 = _slots[newSlotId];
            Slot slot2 = _slots[oldSlotId];
            if (changeReady)
            {
                if (slot1.state == SlotState.READY)
                    slot1.state = SlotState.NORMAL;
                if (slot2.state == SlotState.READY)
                    slot2.state = SlotState.NORMAL;
            }
            slot1.SetSlotId(oldSlotId);
            slot2.SetSlotId(newSlotId);
            _slots[newSlotId] = slot2;
            _slots[oldSlotId] = slot1;
            slots.Add(new SlotChange()
            {
                oldSlot = slot1,
                newSlot = slot2
            });
        }

        public void changeSlotState(int slotId, SlotState state, bool sendInfo) => changeSlotState(getSlot(slotId), state, sendInfo);

        public void changeSlotState(Slot slot, SlotState state, bool sendInfo)
        {
            if (slot == null || slot.state == state)
                return;
            slot.state = state;
            if (state == SlotState.EMPTY || state == SlotState.CLOSE)
            {
                AllUtils.ResetSlotInfo(this, slot, false);
                slot._playerId = 0L;
            }
            if (!sendInfo)
                return;
            updateSlotsInfo();
        }

        public Account getPlayerBySlot(Slot slot)
        {
            try
            {
                long playerId = slot._playerId;
                return playerId > 0L ? AccountManager.getAccount(playerId, true) : null;
            }
            catch
            {
                return null;
            }
        }

        public Account getPlayerBySlot(int slotId)
        {
            try
            {
                long playerId = _slots[slotId]._playerId;
                return playerId > 0L ? AccountManager.getAccount(playerId, true) : null;
            }
            catch
            {
                return null;
            }
        }

        public bool getPlayerBySlot(int slotId, out Account player)
        {
            try
            {
                long playerId = _slots[slotId]._playerId;
                player = playerId > 0L ? AccountManager.getAccount(playerId, true) : null;
                return player != null;
            }
            catch
            {
                player = null;
                return false;
            }
        }

        public bool getPlayerBySlot(Slot slot, out Account player)
        {
            try
            {
                long playerId = slot._playerId;
                player = playerId > 0L ? AccountManager.getAccount(playerId, true) : null;
                return player != null;
            }
            catch
            {
                player = null;
                return false;
            }
        }

        public int getTimeByMask() => TIMES[killtime >> 4];

        public int getRoundsByMask() => ROUNDS[killtime & 15];

        public int getKillsByMask() => KILLS[killtime & 15];

        public void updateSlotsInfo()
        {
            using (PROTOCOL_ROOM_GET_SLOTINFO_ACK roomGetSlotinfoAck = new PROTOCOL_ROOM_GET_SLOTINFO_ACK(this))
                SendPacketToPlayers(roomGetSlotinfoAck);
        }

        public bool getLeader(out Account p)
        {
            p = null;
            if (getAllPlayers().Count <= 0)
                return false;
            if (_leader == -1)
                setNewLeader(-1, 0, -1, false);
            if (_leader >= 0)
                p = AccountManager.getAccount(_slots[_leader]._playerId, true);
            return p != null;
        }

        public Account getLeader()
        {
            if (getAllPlayers().Count <= 0)
                return null;
            if (_leader == -1)
                setNewLeader(-1, 0, -1, false);
            return _leader != -1 ? AccountManager.getAccount(_slots[_leader]._playerId, true) : null;
        }

        public void setNewLeader(int leader, int state, int oldLeader, bool updateInfo)
        {
            Monitor.Enter(_slots);
            if (leader == -1)
            {
                for (int index = 0; index < 16; ++index)
                {
                    Slot slot = _slots[index];
                    if (index != oldLeader && slot._playerId > 0L && slot.state > (SlotState)state)
                    {
                        _leader = index;
                        break;
                    }
                }
            }
            else
                _leader = leader;
            if (_leader != -1)
            {
                Slot slot = _slots[_leader];
                if (slot.state == SlotState.READY)
                    slot.state = SlotState.NORMAL;
                if (updateInfo)
                    updateSlotsInfo();
            }
            Monitor.Exit(_slots);
        }

        public void SendPacketToPlayers(SendPacket packet)
        {
            List<Account> allPlayers = getAllPlayers();
            if (allPlayers.Count == 0)
                return;
            byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket)");
            foreach (Account account in allPlayers)
                account.SendCompletePacket(completeBytes);
        }

        public void SendPacketToPlayers(SendPacket packet, long player_id)
        {
            List<Account> allPlayers = getAllPlayers(player_id);
            if (allPlayers.Count == 0)
                return;
            byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,long)");
            foreach (Account account in allPlayers)
                account.SendCompletePacket(completeBytes);
        }

        public void SendPacketToPlayers(SendPacket packet, SlotState state, int type)
        {
            List<Account> allPlayers = getAllPlayers(state, type);
            if (allPlayers.Count == 0)
                return;
            byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int)");
            for (int index = 0; index < allPlayers.Count; ++index)
                allPlayers[index].SendCompletePacket(completeBytes);
        }

        public void SendPacketToPlayers(
          SendPacket packet,
          SendPacket packet2,
          SlotState state,
          int type)
        {
            List<Account> allPlayers = getAllPlayers(state, type);
            if (allPlayers.Count == 0)
                return;
            byte[] completeBytes1 = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-1");
            byte[] completeBytes2 = packet2.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-2");
            foreach (Account account in allPlayers)
            {
                account.SendCompletePacket(completeBytes1);
                account.SendCompletePacket(completeBytes2);
            }
        }

        public void SendPacketToPlayers(SendPacket packet, SlotState state, int type, int exception)
        {
            List<Account> allPlayers = getAllPlayers(state, type, exception);
            if (allPlayers.Count == 0)
                return;
            byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int)");
            foreach (Account account in allPlayers)
                account.SendCompletePacket(completeBytes);
        }

        public void SendPacketToPlayers(
          SendPacket packet,
          SlotState state,
          int type,
          int exception,
          int exception2)
        {
            List<Account> allPlayers = getAllPlayers(state, type, exception, exception2);
            if (allPlayers.Count == 0)
                return;
            byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int,int)");
            foreach (Account account in allPlayers)
                account.SendCompletePacket(completeBytes);
        }

        public void RemovePlayer(Account player, bool WarnAllPlayers, int quitMotive = 0)
        {
            Slot slot;
            if (player == null || !getSlot(player._slotId, out slot))
                return;
            BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
        }

        public void RemovePlayer(Account player, Slot slot, bool WarnAllPlayers, int quitMotive = 0)
        {
            if (player == null || slot == null)
                return;
            BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
        }

        private void BaseRemovePlayer(Account player, Slot slot, bool WarnAllPlayers, int quitMotive)
        {
            Monitor.Enter(_slots);
            bool flag = false;
            bool host = false;
            if (player != null && slot != null)
            {
                if (slot.state >= SlotState.LOAD)
                {
                    if (_leader == slot._id)
                    {
                        int leader = _leader;
                        int state = 1;
                        if (_state == RoomState.Battle)
                            state = 14;
                        else if (_state >= RoomState.Loading)
                            state = 9;
                        if (getAllPlayers(slot._id).Count >= 1)
                            setNewLeader(-1, state, _leader, false);
                        if (getPlayingPlayers(2, SlotState.READY, 1) >= 2)
                        {
                            using (PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK leaveP2PserverAck = new PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK(this))
                                SendPacketToPlayers(leaveP2PserverAck, SlotState.RENDEZVOUS, 1, slot._id);
                        }
                        host = true;
                    }
                    using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK battleGiveupbattleAck = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(player, quitMotive))
                        SendPacketToPlayers(battleGiveupbattleAck, SlotState.READY, 1, !WarnAllPlayers ? slot._id : -1);
                    BattleLeaveSync.SendUDPPlayerLeave(this, slot._id);
                    slot.ResetSlot();
                    if (votekick != null)
                        votekick.TotalArray[slot._id] = false;
                }
                slot._playerId = 0L;
                slot._equip = null;
                slot.state = SlotState.EMPTY;
                if (_state == RoomState.CountDown)
                {
                    if (slot._id == _leader)
                    {
                        _state = RoomState.Ready;
                        flag = true;
                        countdown.Timer = null;
                        using (PROTOCOL_BATTLE_START_COUNTDOWN_ACK startCountdownAck = new PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum.StopByHost))
                            SendPacketToPlayers(startCountdownAck);
                    }
                    else if (getPlayingPlayers(slot._team, SlotState.READY, 0) == 0)
                    {
                        if (slot._id != _leader)
                            changeSlotState(_leader, SlotState.NORMAL, false);
                        StopCountDown(CountDownEnum.StopByPlayer, false);
                        flag = true;
                    }
                }
                else if (isPreparing())
                {
                    AllUtils.BattleEndPlayersCount(this, isBotMode());
                    if (_state == RoomState.Battle)
                        AllUtils.BattleEndRoundPlayersCount(this);
                }
                CheckToEndWaitingBattle(host);
                requestHost.Remove(player.player_id);
                if (vote.Timer != null && votekick != null && votekick.victimIdx == player._slotId && quitMotive != 2)
                {
                    vote.Timer = null;
                    votekick = null;
                    using (PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK kickvoteCancelAck = new PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK())
                        SendPacketToPlayers(kickvoteCancelAck, SlotState.BATTLE, 0);
                }
                Match match = player._match;
                if (match != null && player.matchSlot >= 0)
                {
                    match._slots[player.matchSlot].state = SlotMatchState.Normal;
                    using (PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK registMercenaryAck = new PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(match))
                        match.SendPacketToPlayers(registMercenaryAck);
                }
                player._room = null;
                player._slotId = -1;
                player._status.updateRoom(byte.MaxValue);
                AllUtils.syncPlayerToClanMembers(player);
                AllUtils.syncPlayerToFriends(player, false);
                player.updateCacheInfo();
            }
            updateSlotsInfo();
            if (flag)
                updateRoomInfo();
            Monitor.Exit(_slots);
        }

        public int addPlayer(Account p)
        {
            lock (_slots)
            {
                for (int index = 0; index < 16; ++index)
                {
                    Slot slot = _slots[index];
                    if (slot._playerId == 0L && slot.state == SlotState.EMPTY)
                    {
                        slot._playerId = p.player_id;
                        slot.state = SlotState.NORMAL;
                        p._room = this;
                        p._slotId = index;
                        slot._equip = p._equip;
                        p._status.updateRoom((byte)_roomId);
                        AllUtils.syncPlayerToClanMembers(p);
                        AllUtils.syncPlayerToFriends(p, false);
                        p.updateCacheInfo();
                        return index;
                    }
                }
            }
            return -1;
        }

        public int addPlayer(Account p, int teamIdx)
        {
            int[] teamArray = GetTeamArray(teamIdx);
            lock (_slots)
            {
                for (int index1 = 0; index1 < teamArray.Length; ++index1)
                {
                    int index2 = teamArray[index1];
                    Slot slot = _slots[index2];
                    if (slot._playerId == 0L && slot.state == SlotState.EMPTY)
                    {
                        slot._playerId = p.player_id;
                        slot.state = SlotState.NORMAL;
                        p._room = this;
                        p._slotId = index2;
                        slot._equip = p._equip;
                        p._status.updateRoom((byte)_roomId);
                        AllUtils.syncPlayerToClanMembers(p);
                        AllUtils.syncPlayerToFriends(p, false);
                        p.updateCacheInfo();
                        return index2;
                    }
                }
            }
            return -1;
        }

        public int[] GetTeamArray(int index) => index != 0 ? BLUE_TEAM : RED_TEAM;

        public List<Account> getAllPlayers(
          SlotState state,
          int type)
        {
            List<Account> accountList = new List<Account>();
            lock (_slots)
            {
                for (int index = 0; index < _slots.Length; ++index)
                {
                    Slot slot = _slots[index];
                    long playerId = slot._playerId;
                    if (playerId > 0L && (type == 0 && slot.state == state || type == 1 && slot.state > state))
                    {
                        Account account = AccountManager.getAccount(playerId, true);
                        if (account != null && account._slotId != -1)
                            accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public List<Account> getAllPlayers(
          SlotState state,
          int type,
          int exception)
        {
            List<Account> accountList = new List<Account>();
            lock (_slots)
            {
                for (int index = 0; index < _slots.Length; ++index)
                {
                    Slot slot = _slots[index];
                    long playerId = slot._playerId;
                    if (playerId > 0L && index != exception && (type == 0 && slot.state == state || type == 1 && slot.state > state))
                    {
                        Account account = AccountManager.getAccount(playerId, true);
                        if (account != null && account._slotId != -1)
                            accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public List<Account> getAllPlayers(
          SlotState state,
          int type,
          int exception,
          int exception2)
        {
            List<Account> accountList = new List<Account>();
            lock (_slots)
            {
                for (int index = 0; index < _slots.Length; ++index)
                {
                    Slot slot = _slots[index];
                    long playerId = slot._playerId;
                    if (playerId > 0L && index != exception && index != exception2 && (type == 0 && slot.state == state || type == 1 && slot.state > state))
                    {
                        Account account = AccountManager.getAccount(playerId, true);
                        if (account != null && account._slotId != -1)
                            accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public List<Account> getAllPlayers(int exception)
        {
            List<Account> accountList = new List<Account>();
            lock (_slots)
            {
                for (int index = 0; index < _slots.Length; ++index)
                {
                    long playerId = _slots[index]._playerId;
                    if (playerId > 0L && index != exception)
                    {
                        Account account = AccountManager.getAccount(playerId, true);
                        if (account != null && account._slotId != -1)
                            accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public List<Account> getAllPlayers(long exception)
        {
            List<Account> accountList = new List<Account>();
            lock (_slots)
            {
                for (int index = 0; index < _slots.Length; ++index)
                {
                    long playerId = _slots[index]._playerId;
                    if (playerId > 0L && playerId != exception)
                    {
                        Account account = AccountManager.getAccount(playerId, true);
                        if (account != null && account._slotId != -1)
                            accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public List<Account> getAllPlayers()
        {
            List<Account> accountList = new List<Account>();
            lock (_slots)
            {
                for (int index = 0; index < _slots.Length; ++index)
                {
                    long playerId = _slots[index]._playerId;
                    if (playerId > 0L)
                    {
                        Account account = AccountManager.getAccount(playerId, true);
                        if (account != null && account._slotId != -1)
                            accountList.Add(account);
                    }
                }
            }
            return accountList;
        }

        public int getPlayingPlayers(int team, bool inBattle)
        {
            int num = 0;
            lock (_slots)
            {
                foreach (Slot slot in _slots)
                {
                    if (slot._playerId > 0L && (slot._team == team || team == 2) && (inBattle && slot.state == SlotState.BATTLE_LOAD && !slot.espectador || !inBattle && slot.state >= SlotState.LOAD))
                        ++num;
                }
            }
            return num;
        }

        public int getPlayingPlayers(int team, SlotState state, int type)
        {
            int num = 0;
            lock (_slots)
            {
                foreach (Slot slot in _slots)
                {
                    if (slot._playerId > 0L && (type == 0 && slot.state == state || type == 1 && slot.state > state) && (team == 2 || slot._team == team))
                        ++num;
                }
            }
            return num;
        }

        public int getPlayingPlayers(int team, SlotState state, int type, int exception)
        {
            int num = 0;
            lock (_slots)
            {
                for (int index = 0; index < 16; ++index)
                {
                    Slot slot = _slots[index];
                    if (index != exception && slot._playerId > 0L && (type == 0 && slot.state == state || type == 1 && slot.state > state) && (team == 2 || slot._team == team))
                        ++num;
                }
            }
            return num;
        }

        public void getPlayingPlayers(bool inBattle, out int RedPlayers, out int BluePlayers)
        {
            RedPlayers = 0;
            BluePlayers = 0;
            lock (_slots)
            {
                foreach (Slot slot in _slots)
                {
                    if (slot._playerId > 0L && (inBattle && slot.state == SlotState.BATTLE && !slot.espectador || !inBattle && slot.state >= SlotState.RENDEZVOUS))
                    {
                        if (slot._team == 0)
                            ++RedPlayers;
                        else
                            ++BluePlayers;
                    }
                }
            }
        }

        public void getPlayingPlayers(
          bool inBattle,
          out int RedPlayers,
          out int BluePlayers,
          out int RedDeaths,
          out int BlueDeaths)
        {
            RedPlayers = 0;
            BluePlayers = 0;
            RedDeaths = 0;
            BlueDeaths = 0;
            lock (_slots)
            {
                foreach (Slot slot in _slots)
                {
                    if (slot._deathState.HasFlag(DeadEnum.Dead))
                    {
                        if (slot._team == 0)
                            ++RedDeaths;
                        else
                            ++BlueDeaths;
                    }
                    if (slot._playerId > 0L && (inBattle && slot.state == SlotState.BATTLE && !slot.espectador || !inBattle && slot.state >= SlotState.RENDEZVOUS))
                    {
                        if (slot._team == 0)
                            ++RedPlayers;
                        else
                            ++BluePlayers;
                    }
                }
            }
        }

        public void CheckToEndWaitingBattle(bool host)
        {
            if (_state != RoomState.CountDown && _state != RoomState.Loading && _state != RoomState.Rendezvous || !host && _slots[_leader].state != SlotState.BATTLE_READY)
                return;
            AllUtils.EndBattleNoPoints(this);
        }

        public void SpawnReadyPlayers()
        {
            lock (_slots)
                BaseSpawnReadyPlayers(isBotMode());
        }

        public void SpawnReadyPlayers(bool isBotMode)
        {
            lock (_slots)
                BaseSpawnReadyPlayers(isBotMode);
        }

        private void BaseSpawnReadyPlayers(bool isBotMode)
        {
            DateTime now = DateTime.Now;
            foreach (Slot slot in _slots)
            {
                if (slot.state == SlotState.BATTLE_READY && slot.isPlaying == 0 && slot._playerId > 0L)
                {
                    slot.isPlaying = 1;
                    slot.startTime = now;
                    slot.state = SlotState.BATTLE;
                    if (_state == RoomState.Battle && (room_type == RoomType.Bomb || room_type == RoomType.Annihilation || room_type == RoomType.Convoy))
                        slot.espectador = true;
                }
            }
            updateSlotsInfo();
            List<int> dinossaurs = AllUtils.getDinossaurs(this, false, -1);
            if (_state == RoomState.PreBattle)
            {
                BattleStart = room_type == RoomType.Bomb || room_type == RoomType.CrossCounter ? now.AddMinutes(5.0) : now;
                SetSpecialStage();
            }
            bool flag = false;
            using (PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK roundPreStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(this, dinossaurs, isBotMode))
            {
                using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK missionRoundStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(this))
                {
                    using (PROTOCOL_BATTLE_RECORD_ACK protocolBattleRecordAck = new PROTOCOL_BATTLE_RECORD_ACK(this))
                    {
                        byte[] completeBytes1 = roundPreStartAck.GetCompleteBytes("Room.BaseSpawnReadyPlayers-1");
                        byte[] completeBytes2 = missionRoundStartAck.GetCompleteBytes("Room.BaseSpawnReadyPlayers-2");
                        byte[] completeBytes3 = protocolBattleRecordAck.GetCompleteBytes("Room.BaseSpawnReadyPlayers-3");
                        foreach (Slot slot in _slots)
                        {
                            Account player;
                            if (slot.state == SlotState.BATTLE && slot.isPlaying == 1 && getPlayerBySlot(slot, out player))
                            {
                                slot.isPlaying = 2;
                                if (_state == RoomState.PreBattle)
                                {
                                    using (PROTOCOL_BATTLE_STARTBATTLE_ACK battleStartbattleAck = new PROTOCOL_BATTLE_STARTBATTLE_ACK(slot, player, dinossaurs, isBotMode, true))
                                        SendPacketToPlayers(battleStartbattleAck, SlotState.READY, 1);
                                    player.SendCompletePacket(completeBytes1);
                                    if (room_type == RoomType.Boss || room_type == RoomType.CrossCounter)
                                        flag = true;
                                    else
                                        player.SendCompletePacket(completeBytes2);
                                }
                                else if (_state == RoomState.Battle)
                                {
                                    using (PROTOCOL_BATTLE_STARTBATTLE_ACK battleStartbattleAck = new PROTOCOL_BATTLE_STARTBATTLE_ACK(slot, player, dinossaurs, isBotMode, false))
                                        SendPacketToPlayers(battleStartbattleAck, SlotState.READY, 1);
                                    if (room_type == RoomType.Bomb || room_type == RoomType.Annihilation || room_type == RoomType.Convoy)
                                        GameSync.SendUDPPlayerSync(this, slot, 0, 1);
                                    else
                                        player.SendCompletePacket(completeBytes1);
                                    player.SendCompletePacket(completeBytes2);
                                    player.SendCompletePacket(completeBytes3);
                                }
                            }
                        }
                    }
                }
            }
            if (_state == RoomState.PreBattle)
            {
                _state = RoomState.Battle;
                updateRoomInfo();
            }
            if (!flag)
                return;
            StartDinoRound();
        }

        private void StartDinoRound() => round.Start(5250, callbackState =>
        {
            if (_state == RoomState.Battle)
            {
                using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK missionRoundStartAck = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(this))
                    SendPacketToPlayers(missionRoundStartAck, SlotState.BATTLE, 0);
                swapRound = false;
            }
            lock (callbackState)
                round.Timer = null;
        });
    }
}
