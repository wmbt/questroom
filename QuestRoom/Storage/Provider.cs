﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Storage
{
    public class Provider
    {
        public const string EngCulturePostfix = "Eng";
        private const string IsCorrectQuery = "select count(*) from Schedule where QuestId = @QuestId and BeginTime = @BeginTime";
        private const string IsFreeQuery = "select count(*) from Schedule t " +
                                           "where t.QuestId = @QuestId and t.BeginTime = cast(@DateTime as time) " +
                                           "and not exists(select * from Bookings b " +
                                           "where b.QuestId = @QuestId and b.Date = @DateTime and b.Status in (0, 1))";

        private static readonly string ConnectionString;

        static Provider()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        }

        public User GetUser(string login, string password)
        {
            const string query = "select * from Users u where lower(u.Login) = lower(@Login) and u.Password = @Password";
            var item = GetItems(query,
                new[] {new SqlParameter("@Login", login), new SqlParameter("@Password", password)}, x => new User(x));
            
            return item.SingleOrDefault();
        }

        public Quest[] GetQuests()
        {
            const string query = "SELECT Id, Name__ as Name, Complexity, Duration, Persons, " +
                                 "Description__ as Description, Visible, Active, Note from Quests ORDER BY Id";

            var items = GetItems(query, x => new Quest(x));
            
            return items.ToArray();
        }

        public Quest GetQuest(int questId)
        {
            const string query = "SELECT Id, Name__ as Name, Complexity, Duration, Persons, " +
                             "Description__ as Description, Visible, Active, Note from Quests where Id = @Id";

            var item = GetItems(query, new SqlParameter("@Id", questId), x => new Quest(x)).Single();

            return item;
        }

        public Period[] GetPeriods(int questId, DateTime questDate)
        {
            const string query = "select s.*, " +
                                        "(SELECT CAST( " +
                                           "CASE WHEN EXISTS(SELECT * FROM Bookings b " +
                                                "where cast(b.Date as date) = @QuestDate " +
                                                "and b.Status in (0, 1) and cast(b.Date as time) = s.BeginTime) " +
                                        "THEN 1 ELSE 0 END AS BIT)) Booked " +
                                    "from schedule s where QuestId = @QuestId order by BeginTime";
            
            var items = GetItems(query, new[] {new SqlParameter("@QuestId", questId), new SqlParameter("@QuestDate", questDate)}, x => new Period(x));

            return items.ToArray();
        }

        public Cost[] GetCosts()
        {
            const string query = "select * from Cost order by Id";
            var items = GetItems(query, x => new Cost(x));

            return items.ToArray();
        }

        public ProcessBookingStatus CheckBooking(int questId, DateTime bookingDateTime)
        {
            var count1 = (int)GetScalar(IsCorrectQuery,
                new[]
                {new SqlParameter("@QuestId", questId), new SqlParameter("@BeginTime", bookingDateTime.TimeOfDay)});

            if (count1 == 0) 
                return ProcessBookingStatus.NotExist;

            var count2 = (int)GetScalar(IsFreeQuery,
                new[] { new SqlParameter("@QuestId", questId), new SqlParameter("@DateTime", bookingDateTime) });

            return count2 > 0 ? ProcessBookingStatus.Free : ProcessBookingStatus.Booked;
        }

        public ProcessBookingStatus SaveBooking(int questId,
                                                DateTime bookingDateTime, 
                                                int costId,
                                                string name, 
                                                string phone, 
                                                string email, 
                                                string comments)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var trn = conn.BeginTransaction(IsolationLevel.Serializable);
                var isCorrectCommand = new SqlCommand(IsCorrectQuery, conn, trn);
                isCorrectCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@QuestId", questId),
                    new SqlParameter("@BeginTime", bookingDateTime.TimeOfDay)
                });

                var existsTimeCount = (int) isCorrectCommand.ExecuteScalar();
                if (existsTimeCount == 0)
                    return ProcessBookingStatus.NotExist;

                var isTimeFreeCommand = new SqlCommand(IsFreeQuery, conn, trn);
                isTimeFreeCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@QuestId", questId),
                    new SqlParameter("@DateTime", bookingDateTime)
                });

                var freeTimeCount = (int) isTimeFreeCommand.ExecuteScalar();
                if (freeTimeCount == 0)
                    return ProcessBookingStatus.Booked;

                const string query =
                    "insert into Bookings(QuestId, Date, CostId, PlayerName, Email, Phone, Comment) values(@QuestId, @Date, @CostId, @PlayerName, @Email, @Phone, @Comment)";
                var addBookingCommand = new SqlCommand(query, conn, trn);
                addBookingCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@QuestId", questId),
                    new SqlParameter("@Date", bookingDateTime),
                    new SqlParameter("@CostId", costId),
                    new SqlParameter("@PlayerName", name),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@Comment", comments)
                });
                addBookingCommand.ExecuteNonQuery();
                trn.Commit();
                return ProcessBookingStatus.Complete;
            }
        }

        public FeedbackMessage[] GetFeedbackMessages()
        {
            const string query = "select f.Id, q.Id as QuestId, q.Name__ as QuestName, f.Created, " +
                                 "(select top(1) b.PlayerName from Bookings b where lower(b.Email) = lower(f.Email)) PlayerName, " +
                                 "f.Text " +
                                 "from [Feedback] f, Quests q " +
                                 "where f.QuestId = q.Id and f.Status = 1 " +
                                 "order by f.Created desc";

            var items = GetItems(query, x => new FeedbackMessage(x));
            return items.ToArray();
        }

        public bool AddFeedbackMessage(int questId, string email, string text)
        {
            const string isUserPlayingQuery =
                "select count(*) from Bookings  where lower(Email) = lower(@Email) and Status = 1 and Date < getdate()";

            var userCompleteGamesCount = (int)GetScalar(isUserPlayingQuery, new SqlParameter("@Email", email));
            if (userCompleteGamesCount == 0)
                return false;
            const string addMessageQuery = "INSERT INTO Feedback(QuestId, Email, Text) values(@QuestId, @Email, @Text)";
            ExecuteNonQuery(addMessageQuery, new[]
            {
                new SqlParameter("@QuestId", questId),
                new SqlParameter("@Email", email),
                new SqlParameter("@Text", text)
            });
            return true;
        }

        private IEnumerable<T> GetItems<T>(string commandText, Func<IDataRecord, T> itemFactory)
        {
            return GetItems(commandText, (IEnumerable<SqlParameter>)null, itemFactory);
        }

        private IEnumerable<T> GetItems<T>(string commandText, SqlParameter parameter, Func<IDataRecord, T> itemFactory)
        {
            return GetItems(commandText, new[] { parameter }, itemFactory);
        }

        private static IEnumerable<T> GetItems<T>(string commandText, IEnumerable<SqlParameter> parameters, Func<IDataRecord, T> itemFactory)
        {
            commandText = WritePostfix(commandText);

            var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(commandText, conn);
            
            try
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters.ToArray());

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        yield return itemFactory(rdr);
                }
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

        }

        private static object GetScalar(string commandText, SqlParameter parameter)
        {
            return GetScalar(commandText, new[] {parameter});
        }

        private static object GetScalar(string commandText, IEnumerable<SqlParameter> parameters = (IEnumerable<SqlParameter>)null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(commandText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters.ToArray());
                conn.Open();
                var result = command.ExecuteScalar();
                return result;
            }
        }

        private static int ExecuteNonQuery(string commandText, SqlParameter parameter)
        {
            return ExecuteNonQuery(commandText, new[] {parameter});
        }

        private static int ExecuteNonQuery(string commandText, IEnumerable<SqlParameter> parameters = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(commandText, conn))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters.ToArray());
                conn.Open();
                var result = command.ExecuteNonQuery();
                return result;
            }
        }

        private static string WritePostfix(string query)
        {
            var threadCulture = Thread.CurrentThread.CurrentCulture;
            var postfix = threadCulture.Name == "en-US" ? EngCulturePostfix : string.Empty;
            return query.Replace("__", postfix);
        }
    }
}