using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using System.Data.Linq;
using System.Linq.Expressions;
using RmsAuto.Common.Linq;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web
{
	public static class ReclamationTracking
	{
		private static List<ReclamationStatus> _allStatuses;

		static ReclamationTracking()
        {
            using ( var dc = new DCFactory<StoreDataContext>() )
			{
				#region === Подгружаем локализации ===
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<ReclamationStatus>(rs => rs.ReclamationStatusesLocs);
				dc.DataContext.LoadOptions = dlo;
				dc.DataContext.DeferredLoadingEnabled = false;
				#endregion
				_allStatuses = dc.DataContext.ReclamationStatus.ToList<ReclamationStatus>();
            }
        }

		public enum ReclamationType
		{
			/// <summary>
			/// Возврат
			/// </summary>
			//[Text("возврат")]
			[Text("return")]
			Reclamation = 0,
			/// <summary>
			/// Отказ
			/// </summary>
			//[Text("отказ")]
			[Text("refusal")]
			Rejection = 1
		}

		public class ReclamationFilter
		{
			public ReclamationType? ReclamationType { get; set; }
			public DateTime? ReclamationDateBegin { get; set; }
			public DateTime? ReclamationDateEnd { get; set; }
		}

		public enum ReclamationSortFields
		{
			//[Text("по дате заявки (убыв.)")]
			[Text("by reclamation date (desc.)")]
			ReclamationDateDesc,
			//[Text("по дате заявки (возр.)")]
			[Text("by reclamation date (asc.)")]
			ReclamationDateAsc,
			//[Text("по статусу")]
			[Text("by status")]
			Status
		}

		/// <summary>
		/// сохранение рекламации в БД
		/// </summary>
		/// <param name="rec">рекламация</param>
		public static int SendReclamation(Reclamation rec)
		{
			using (var dc = new DCFactory<StoreDataContext>())
			{
				//Коннекция открывается в фабрике
                //dc.DataContext.Connection.Open();
				dc.DataContext.Reclamations.InsertOnSubmit(rec);
				dc.DataContext.SubmitChanges();
				//dc.Transaction = dc.Connection.BeginTransaction();
				//try
				//{
				//    dc.SubmitChanges();

				//    ReclamationEnvelope envelope = new ReclamationEnvelope()
				//    {
				//        ContactPerson = rec.ContactPerson,
				//        ContactPhone = rec.ContactPhone,
				//        Qty = rec.Qty,
				//        ReclamationReason = rec.ReclamationReason,
				//        ReclamationDescription = rec.ReclamationDescription,
				//        Torg12Number = rec.Torg12Number,
				//        Torg12Price = rec.Torg12Price,
				//        InvoiceNumber = rec.InvoiceNumber,
				//        ClientName = rec.ClientName,
				//        ClientID = rec.ClientID,
				//        ReclamationDate = rec.ReclamationDate,
				//        OrderID = rec.OrderID,
				//        OrderDate = rec.OrderDate,
				//        EstSupplyDate = rec.EstSupplyDate,
				//        ManagerName = rec.ManagerName,
				//        Manufacturer = rec.Manufacturer,
				//        PartNumber = rec.PartNumber,
				//        PartName = rec.PartName,
				//        UnitPrice = rec.UnitPrice,
				//        UnitQty = rec.UnitQty,
				//        SupplyDate = rec.SupplyDate,
				//        SupplierID = rec.SupplierID,
				//        CurrentStatus = rec.CurrentStatus,
				//        CurrentStatusDate = rec.CurrentStatusDate,
				//        ReclamationType = (byte)rec.ReclamationType,
				//        OrderLineID = rec.OrderLineID
				//    };
				//    ServiceProxy.Default.SendReclamation( envelope );
						
				//    dc.Transaction.Commit();
				//}
				//catch(Exception ex)
				//{
				//    dc.Transaction.Rollback();
				//    Logger.WriteException( ex );
				//    throw;
				//}
				//finally
				//{
				//    if (dc.Connection.State == System.Data.ConnectionState.Open)
				//        dc.Connection.Close();
				//}
				return rec.ReclamationID;
			}
		}

		public static Reclamation[] GetReclamations( string clientId, ReclamationFilter filter, ReclamationSortFields sortField, int startIndex, int size )
		{
			using ( var dc = new DCFactory<StoreDataContext>() )
			{
				//TODO add DatatLoadOptions if required
				//DataLoadOptions options = new DataLoadOptions();
				//options.LoadWith<OrderLine>( l => l.OrderLineStatusChanges );
				//options.LoadWith<OrderLine>( l => l.Order );
				//dc.LoadOptions = options;
				var reclamations = dc.DataContext.Reclamations.Where( r => r.ClientID == clientId );
				if ( filter != null )
				{
					reclamations = ApplyReclamationFilter( reclamations, filter );
				}
				reclamations = ApplyReclamationSort( reclamations, sortField );
				return reclamations.Skip( startIndex ).Take( size ).ToArray();
			}
		}

		public static int GetReclamationsCount( string clientId, ReclamationFilter filter )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				var reclamations = dc.DataContext.Reclamations.Where( r => r.ClientID == clientId );
				if ( filter != null )
				{
					reclamations = ApplyReclamationFilter( reclamations, filter );
				}
				return reclamations.Count();
			}
		}

		public static Reclamation GetReclamation( string clientId, int reclamationId )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				return dc.DataContext.Reclamations.Single( r => r.ClientID == clientId && r.ReclamationID == reclamationId );
			}
		}

		/// <summary>
		/// Получить имя статуса по ID
		/// </summary>
		/// <param name="statusID">ID статуса</param>
		/// <returns>Имя статуса</returns>
		public static string StatusName( byte statusID )
		{
			var el =
				  from o in _allStatuses
				  where o.ReclamationStatusID == statusID
				  select o.Name;

			return (String)el.First();
		}

		/// <summary>
		/// Проверяет существует ли уже заявка на такую строку заказа
		/// </summary>
		/// <param name="orderLineID">ID строки заказа</param>
		/// <returns>результат</returns>
		//public static bool IsReclamationExist( int orderLineID )
		//{
		//    using (var dc = new StoreDataContext())
		//    {
		//        return dc.Reclamations.Where( r => r.OrderLineID == orderLineID ).Any();
		//    }
		//}

		/// <summary>
		/// Проверяет существует ли уже заявка на такую строку заказа
		/// </summary>
		/// <param name="acctgOrderLineID">ID 1С строки заказа</param>
		/// <returns>результат</returns>
		public static bool IsReclamationExist( int acctgOrderLineID )
		{
			using (var dc = new DCFactory<StoreDataContext>())
			{
				return dc.DataContext.Reclamations.Where( r => r.AcctgOrderLineID == acctgOrderLineID ).Any();
			}
		}

		#region ==== Helpers ====
		private static IQueryable<Reclamation> ApplyReclamationFilter( IQueryable<Reclamation> reclamations, ReclamationFilter filter )
		{
			if ( filter.ReclamationType.HasValue )
			{
				reclamations = reclamations.Where( r => r.ReclamationType == filter.ReclamationType.Value );
			}
			if ( filter.ReclamationDateBegin.HasValue )
			{
				reclamations = reclamations.Where( r => r.ReclamationDate >= filter.ReclamationDateBegin.Value );
			}
			if ( filter.ReclamationDateEnd.HasValue )
			{
				reclamations = reclamations.Where( r => r.ReclamationDate <= filter.ReclamationDateEnd.Value );
			}

			return reclamations;
		}

		private static IQueryable<Reclamation> ApplyReclamationSort( IQueryable<Reclamation> reclamations, ReclamationSortFields sortField )
		{
			switch ( sortField )
			{
				case ReclamationSortFields.ReclamationDateAsc: reclamations = reclamations.OrderBy( r => r.ReclamationDate ); break;
				case ReclamationSortFields.ReclamationDateDesc: reclamations = reclamations.OrderByDescending( r => r.ReclamationDate ); break;
				case ReclamationSortFields.Status: reclamations = reclamations.OrderBy( r => r.CurrentStatus ); break;
			}
			return reclamations;
		}
		#endregion
	}
}
