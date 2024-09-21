using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Gift;
using PointBlank.Core.Sql;

namespace PointBlank.Core.Managers
{
    // Token: 0x02000079 RID: 121
    public static class TicketManager
    {
        // Token: 0x0600019E RID: 414 RVA: 0x0000BC04 File Offset: 0x00009E04
        public static List<TicketModel> GetTickets()
        {
            List<TicketModel> Tickets = new List<TicketModel>();
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM tickets";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        TicketModel Ticket = new TicketModel((TicketType)data.GetInt32(0), data.GetString(1));
                        bool flag = Ticket.Type.HasFlag(TicketType.ITEM);
                        if (flag)
                        {
                            Ticket.ItemId = data.GetInt32(2);
                            Ticket.ItemName = data.GetString(3);
                            Ticket.Count = data.GetInt32(4);
                            Ticket.Equip = data.GetInt32(5);
                        }
                        bool flag2 = Ticket.Type.HasFlag(TicketType.MONEY);
                        if (flag2)
                        {
                            Ticket.Point = data.GetInt32(6);
                            Ticket.Cash = data.GetInt32(7);
                            Ticket.Tag = data.GetInt32(8);
                        }

                        Ticket.MaxUse = data.GetInt32(9);

                        Tickets.Add(Ticket);
                    }

                   

                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
            return Tickets;
        }

        public static bool TicketUsedLog(string couponCode, long userId)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT COUNT(*) FROM ticket_log WHERE ticket = @couponCode AND player_id = @userId";
                    command.Parameters.AddWithValue("@couponCode", couponCode);
                    command.Parameters.AddWithValue("@userId", userId);
                    long count = (long)command.ExecuteScalar();
                        if (count > 0)
                        {
                            result = true;
                        }

                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
            return result;
        }

        public static void TicketSaveLog(string couponCode, long userId)
        {
           
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                     // Insert a new record in the ticket_log table
                     command.CommandText = "INSERT INTO ticket_log (ticket, player_id, date) VALUES (@couponCode, @userId, @usedDate)";
                     command.Parameters.AddWithValue("@couponCode", couponCode);
                     command.Parameters.AddWithValue("@userId", userId);
                     command.Parameters.AddWithValue("@usedDate", DateTime.Now);
                     command.ExecuteNonQuery();

                    

                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
           
        }

        public static bool TicketCanUse(string couponCode, int countTicket)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    connection.Open();
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        // Check if the ticket has been used the maximum number of times
                        command.CommandText = "SELECT COUNT(*) FROM ticket_log WHERE ticket = @cc1";
                        command.Parameters.AddWithValue("@cc1", couponCode);
                        long usesCount = (long)command.ExecuteScalar();


                        if (usesCount >= countTicket)
                        {
                            result = true;
                        }
                        
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
            return result;
        }


    }
}