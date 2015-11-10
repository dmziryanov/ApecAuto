using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using RmsAuto.Store.Configuration;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Entities
{
    public partial class OrderLineStatusChange
    {
		/// <summary>
		/// Показывать на странице "Требуется реакция клиента"
		/// </summary>
        //public bool ShowOnClientReactionPendingPage
        //{
        //    get
        //    {
        //        var requiresClientReaction = false;
        //        var settings = OrderLineStatusConfiguration.Current.OrderLineStatusSettings[ this.OrderLineStatus ];
        //        if( settings != null )
        //            requiresClientReaction = settings.RequiresClientReaction;
        //        //this.OrderLineStatus
        //        //OrderLineStatuses.RequiresClientReaction.
        //        //
        //        return IsLast && requiresClientReaction;
        //    }
        //}

		/// <summary>
		/// Требуется ли реакция клиента на статус
		/// </summary>
        public bool ClientReactionPending
        {
            get 
            {
                // deas 28.04.2011 task3981
                // включение реакции клиента через сайт
                //StoreDataContext DC = new StoreDataContext();
                //var curSt = DC.OrderLineStatuses.FirstOrDefault( t => t.OrderLineStatusID == OrderLineStatus );
                //if ( curSt != null )
                //{
                //    return curSt.RequiresClientReaction;
                //}
                //else
                //{
                //    return false;
                //}

                // но сделано только для одного статуса, т.к. не известно поведение при других статусах
                return OrderLineStatus == 250;

				//Временно заблокирован функционал реакции на статусы
				//return false;
				
                //if( ShowOnClientReactionPendingPage )
                //{
                //    if( OrderLineStatus == OrderLineStatus.ShipmentDelay )
                //        return false; 
                //    else if( OrderLineStatus == OrderLineStatus.PriceAdjustment )
                //        return false;
                //    else if( OrderLineStatus == OrderLineStatus.PartNumberTransition )
                //        return OrderLine.StrictlyThisNumber && !ClientReaction.HasValue;
                //    else
                //        return !ClientReaction.HasValue;
                //}
                //else
                //{
                //    return false;
                //}
            }
        }

        public bool IsLast
        {
            get { return OrderLine.LastStatusChange.OrderLineStatusChangeID == OrderLineStatusChangeID; }
        }
    }
}
