using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Messages
{
    public class MessageRepository : IMessageRepository
    {
        public IEnumerable<ClientMessage> Get(int clientId, int count, int pagesize)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                var List = ctx.DataContext.ExecuteQuery<ClientMessage>("select * from (select Row_Number() OVER (PARTITION BY 1 ORDER BY Time) rownum, a.*, b.ClientName, b.UserRole from UserMessages a LEFT JOIN Users  b ON a.User_From = b.AcctgID  LEFT JOIN users c ON a.User_To = c.AcctgID where c.UserID = {0}) ab where rownum > {1} and rownum < {2}", clientId.ToString(), count * pagesize, count * pagesize + pagesize).ToList();
                return List;
            }
        }

        public int GetUnreadCount(int clientId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return
                    ctx.DataContext.ExecuteQuery<int>("select Count(*) From UserMessages a LEFT JOIN Users b ON CONVERT(varchar, a.User_From) = b.AcctgID  LEFT JOIN users c ON a.User_To = c.AcctgID where (Viewed = 0  OR VIEWED IS NULL) And c.UserId = {0}",
                        clientId.ToString()).FirstOrDefault();
            }
        }

        public void InsertMessage(int clientId, int userTo, string msgText)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                ctx.DataContext.ExecuteCommand("insert into UserMessages (User_From, user_To, Text) values ({0},{1},{2})", clientId, userTo, msgText);
            }
        }

        public IEnumerable<ClientMessage> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    public interface IMessageRepository
    {
        IEnumerable<ClientMessage> Get(int clientId, int count, int pagesize);
        void InsertMessage(int clientId, int userTo, string msgText);
    }
}


