using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Core.Progress;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Server;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PointBlank.Game.Data.Chat
{
    internal class NewCommands
    {

        public static string BanNew(string valor)
        {
            try
            {
                
                long player_id = long.Parse(valor);
                Account arc = AccountManager.getAccount(player_id, 0);
                if (arc != null && !arc.IsGM())
                {
                    bool bannedwins = ComDiv.updateDB("players", "access_level", -1, "player_id", arc.player_id);
                    if (bannedwins)
                    {
                        if (arc._isOnline)
                        {
                            arc.access = AccessLevel.Banned;
                            arc.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                            arc.Close(1000, true);
                        }
                        MsgToAll(arc.player_name + " Banido.");
                        
                    }
                   
                    return arc.player_name + " banido(a) com sucesso ";
                }
                else
                  return arc.player_name + " [não foi banido(a) por ser um GM]";

            }
            catch 
            {
                return "Erro ao processar banimento.";
            }
        }
        public static string uBanNew(string valor)
        {
            try
            {
                long player_id = long.Parse(valor);
                Account arc = AccountManager.getAccount(player_id, 0);
                if (arc != null)
                {
                    bool bannedwins = ComDiv.updateDB("players", "access_level", 0, "player_id", arc.player_id);
                    if (bannedwins)
                    {

                        return arc.player_name + " foi desbanido(a)";

                    }
                    else
                        return "Erro ao desbanir o jogador.";
                }
                else
                    return "A conta banida não existe.";

            }
            catch
            {
                return "Erro ao processar desbanimento.";
            }
        }


        public static string SendCashVip(string pReceive, Account pSend)
        {
            try
            {
                string[] strArray = pReceive.Substring(pReceive.IndexOf(" ") + 1).Split(' ');
                long UidReceive = Convert.ToInt64(strArray[0]);
                int Value = Convert.ToInt32(strArray[1]);

                return VipSendCash(pSend, UidReceive, Value);
            }
            catch 
            { return "Erro ao enviar o cash"; }
        }
        private static string  VipSendCash(Account PlayerS, long UidReceive, int valueSend)
        {
            if (InGame(PlayerS))
                return Translation.GetLabel("InGameBlock");


            Account PlayerR = AccountManager.getAccount(UidReceive, 0);
            if (PlayerR == null || PlayerS == null)
            {
                return "Não foi possível encontrar o jogador.";
            }
            else if (valueSend == 0)
            {
                return "Envie um valor acima de 0!";
            }
            else if(PlayerS._money < valueSend)
            {
                return "Você não tem cash suficiente!";
            }
            else if (PlayerS.pc_cafe >= 0 && PlayerS.pc_cafe <= 1)
            {
                return "Seu vip não pode utilizar essa função, entre em contato com o GM.";
            }
            else if (PlayerS.pc_cafe == 6)
            {
                return "Seu vip não pode utilizar essa função, entre em contato com o GM.";
            }
            else 
            {

                if (PlayerManager.updateAccountCashing(PlayerS.player_id, PlayerS._gp - 0, PlayerS._money - valueSend))
                {

                    PlayerS._money -= valueSend;
                    PlayerS.SendPacket((SendPacket)new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, PlayerS._gp, PlayerS._money, PlayerS._tag), false);

                    if (PlayerManager.updateAccountCash(PlayerR.player_id, PlayerR._money + valueSend))
                    {
                        PlayerR._money += valueSend;
                        PlayerR.SendPacket((SendPacket)new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, PlayerR._gp, PlayerR._money, PlayerR._tag), false);
                        SendItemInfo.LoadGoldCash(PlayerR);
                        PlayerR.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK($"Gift Cash", PlayerR.getSessionId(), 0, true, $"{PlayerS.player_name} enviou {valueSend} de cash"));
                    }
                }
                    return $"Cash enviado com sucesso ->  [{PlayerR.player_name}]";
            }

                
        }

        // Gold
        public static string SendGoldVip(string pReceive, Account pSend)
        {
            try
            {
                string[] strArray = pReceive.Substring(pReceive.IndexOf(" ") + 1).Split(' ');
                long UidReceive = Convert.ToInt64(strArray[0]);
                int Value = Convert.ToInt32(strArray[1]);

                return VipGoldCash(pSend, UidReceive, Value);
            }
            catch
            { return "Erro ao enviar o gold"; }
        }
        private static string VipGoldCash(Account PlayerS, long UidReceive, int valueSend)
        {
            if (InGame(PlayerS))
                return Translation.GetLabel("InGameBlock");


            Account PlayerR = AccountManager.getAccount(UidReceive, 0);
            if (PlayerR == null || PlayerS == null)
            {
                return "Não foi possível encontrar o jogador.";
            }
            else if (valueSend == 0)
            {
                return "Envie um valor acima de 0!";
            }
            else if (PlayerS._gp < valueSend)
            {
                return "Você não tem cash suficiente!";
            }
            else if (PlayerS.pc_cafe >= 0 && PlayerS.pc_cafe <= 1)
            {
                return "Seu vip não pode utilizar essa função, entre em contato com o GM.";
            }
            else if (PlayerS.pc_cafe == 6)
            {
                return "Seu vip não pode utilizar essa função, entre em contato com o GM.";
            }
            else
            {

                if (PlayerManager.updateAccountCashing(PlayerS.player_id, PlayerS._gp - valueSend, PlayerS._money - 0))
                {

                    PlayerS._gp -= valueSend;
                    PlayerS.SendPacket((SendPacket)new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, PlayerS._gp, PlayerS._money, PlayerS._tag), false);

                    if (PlayerManager.updateAccountGold(PlayerR.player_id, PlayerR._gp + valueSend))
                    {
                        PlayerR._gp += valueSend;
                        PlayerR.SendPacket((SendPacket)new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, PlayerR._gp, PlayerR._money, PlayerR._tag), false);
                        SendItemInfo.LoadGoldCash(PlayerR);
                        PlayerR.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK($"Gift Gold", PlayerR.getSessionId(), 0, true, $"{PlayerS.player_name} enviou {valueSend} de Gold"));
                    }
                }
                return $"Gold enviado com sucesso ->  [{PlayerR.player_name}]";
            }


        }
        // Gold End




        public static string RankFakeVip(Account jogador, Room room) {
           // int num = int.Parse(str.Substring(9));
            return VipRankFake(jogador, room); 
        }
        private static string VipRankFake(Account player, Room room)
        {
            if (InGame(player))
                return Translation.GetLabel("InGameBlock");
            try
            {

                int num = 0;

                if (player._bonus.fakeRank == 55)
                {

                    if (player.pc_cafe == 1)
                    {
                        num = 57;
                    }
                    else if (player.pc_cafe == 2)
                    {
                        num = 58;
                    }
                    else if (player.pc_cafe == 3)
                    {
                        num = 59;
                    }
                    else if (player.pc_cafe == 4)
                    {
                        num = 60;
                    }
                    else if (player.pc_cafe == 5)
                    {
                        num = 56;
                    }
                    else if (player.pc_cafe == 6)
                    {
                        num = 52;
                    }
                }
                else
                {
                    num = 55;
                }
                if (ComDiv.updateDB("player_bonus", "fakerank", (object)num, "player_id", (object)player.player_id))
                    {
                        player._bonus.fakeRank = num;
                        player.SendPacket((SendPacket)new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, player));
                        room?.updateSlotsInfo();
                    }

                if (num == 55)
                    {
                        return "[Desativado] Patente VIP";

                    }
                    else
                    {
                        return "[Ativado] Patente VIP ";
                    }
               

            }catch
            {
                return "Houve uma falha ao salvar o Fake Rank.";
            }
        }


        public static string ViewUser(string Nick)
        {
            try
            {

                Account account = AccountManager.getAccount(Nick, 1, 0);
                if (account != null)
                {
                    return $"PlayerID: {account.player_id} Nickname: {account.player_name}";
                }
                else
                    return "Usuário não existe.";
            }
            catch
            {
                return "Error";
            }
        }

        private static void MsgToAll(string text)
        {
           

            string texto = $"{text}";
            using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(texto))
            {
                GameManager.SendPacketToAllClients(packet);
            }
        }


        public static void ServerNotice(string text)
        {


            string texto = $"{text}";
            using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(texto))
            {
                GameManager.SendPacketToAllClients(packet);
            }
        }

        private static bool InGame(Account player)
        {
            PointBlank.Game.Data.Model.Room room = player._room;
            Slot slot;
            return room != null && room.getSlot(player._slotId, out slot) && slot.state >= SlotState.READY;
        }
    }
}
