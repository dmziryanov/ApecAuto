using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Data;
using RmsAuto.Store.Cms.Mail;
using RmsAuto.Store.Cms.Mail.Messages;
using System.Net.Mail;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.BL
{
    public static class ManagerBO
    {
        public static List<EmployeeInfo> GetManagerList(string notSelectedString)
        {
            var l = new List<EmployeeInfo>();
            l.Add(new EmployeeInfo() { EmployeeId = "0", FullName = notSelectedString });
            l.AddRange(AcctgRefCatalog.RmsEmployees.Items);
            return l;
        }
        
        public static void AddClientToHandySet(int managerId, string clientId, bool setDefault)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");

            ManagerDac.AddHandySetEntry(new HandyClientSetEntry
            {
                ManagerID = managerId,
                ClientID = clientId,
                IsDefault = setDefault
            });
        }

		public static void SetDefaultClientState( int managerId, string clientId )
		{
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");
            ManagerDac.SetHandySetEntryAsDefault(managerId, clientId);
		}
        
        public static HandyClientSetEntry[] GetManagerHandySet(int managerId)
        {
            return ManagerDac.GetHandySetEntries(managerId);
        }

        public static void RemoveClientFromHandySet(int managerId, string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");
            ManagerDac.DeleteHandySetEntry(managerId, clientId);
        }

        public static void ClearHandyClientSet(int managerId)
        {
            ManagerDac.DeleteAllHandySetEntries(managerId);
        }

        public static void SendVinRequestAnswer(int vinRequestId, string clientEmail, string clientName)
        {
            if (string.IsNullOrEmpty(clientEmail))
                throw new ArgumentException("Email cannot be empty", "clientEmail");
            if (string.IsNullOrEmpty(clientName))
                throw new ArgumentException("Client name cannot be empty", "clientName");

            var request = VinRequestsDac.GetRequest(vinRequestId);
            if (request.Proceeded)
                throw new BLException("Ответ на запрос по VIN уже отправлен");


            var alert = new VinRequestAnswerAlert(vinRequestId);
            var requestItems = VinRequestsDac.GetRequestItems(vinRequestId);

            alert.Items.AddRange(requestItems.Select(i => new AnswerLineItem()
            {
                Name = i.Name,
                Qty = i.Quantity,
                Description = i.Description,
                PartNumber = i.PartNumber,
                DeliveryPeriod = i.DeliveryDays,
                ManagerComment = i.ManagerComment,
                Manufacturer = i.Manufacturer,
                PartNumberOriginal = i.PartNumberOriginal,
                PricePerUnit = i.PricePerUnit.Value,
                SearchUrl = UrlManager.MakeAbsoluteUrl(UrlManager.GetSearchManufacturersUrl(i.PartNumberOriginal, true))
            }));

            alert.TotalPrice = requestItems.Sum(i => i.PricePerUnit.Value * i.Quantity);

            MailEngine.SendMail(new MailAddress(clientEmail, clientName), alert);
            VinRequestsDac.SetRequestProceeded(request.Id, DateTime.Now);
        }
		
	}
}
