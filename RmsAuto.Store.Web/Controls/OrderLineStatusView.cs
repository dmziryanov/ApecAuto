using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Configuration;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Controls
{
	public class OrderLineStatusView : WebControl, INamingContainer
	{
		public event EventHandler<ChangeReactionEventArgs> ChangeReaction;

		public OrderLineStatusChange OrderLineStatusChange
		{
			set
			{
				if( value != null )
				{
					OrderLineId = value.OrderLineID;
					Status = value.OrderLineStatus;
					ClientReaction = value.ClientReaction;
					ClientReactionTime = value.ClientReactionTime;
					ClientReactionPending = value.ClientReactionPending;
				}
			}
		}

		public int OrderLineId
		{
			get { return Convert.ToInt32( ViewState[ "__id" ] ); }
			set { ViewState[ "__id" ] = value; }
		}

		public byte Status
		{
			get
			{
				object s = ViewState[ "__status" ];
                return s != null ? (byte)s : OrderLineStatusUtil.StatusByte("New");
			}
			set { ViewState[ "__status" ] = (byte)value; }
		}

		public byte? ClientReaction
		{
			get { object r = ViewState[ "__reaction" ]; return r != null ? Convert.ToByte( r ) : (byte?)null; }
			set { ViewState[ "__reaction" ] = value; }
		}

		public DateTime? ClientReactionTime
		{
			get { object r = ViewState[ "__reactionTime" ]; return r != null ? Convert.ToDateTime( r ) : (DateTime?)null; }
			set { ViewState[ "__reactionTime" ] = value; }
		}

		public bool ClientReactionPending
		{
			get { return Convert.ToBoolean( ViewState[ "__crPending" ] ); }
			set { ViewState[ "__crPending" ] = value; }
		}

		public string DisplayAccept
		{
			get { return Convert.ToString( ViewState[ "__dispAccept" ] ); }
			set { ViewState[ "__dispAccept" ] = value; }
		}

		public string DisplayDecline
		{
			get { return Convert.ToString( ViewState[ "__dispDecline" ] ); }
			set { ViewState[ "__dispDecline" ] = value; }
		}

		[TemplateContainer( typeof( OrderLineStatusNameContainer ) )]
		public ITemplate StatusNameTemplate { get; set; }

		[TemplateContainer( typeof( ReactionInfoContainer ) )]
		public ITemplate ReactionInfoTemplate { get; set; }

		public ITemplate ReactionCommandsTemplate { get; set; }

		protected override void CreateChildControls()
		{
			Controls.Clear();
			if( StatusNameTemplate != null )
			{
			    String dispName = OrderLineStatusUtil.DisplayName(Status);
					
				var statusNameContainer = new OrderLineStatusNameContainer(
					OrderLineId,
					dispName );
				StatusNameTemplate.InstantiateIn( statusNameContainer );
				Controls.Add( statusNameContainer );
				statusNameContainer.DataBind();
			}
			if( ReactionInfoTemplate != null && ClientReaction.HasValue )
			{
				var reaction = (OrderLineChangeReaction)ClientReaction.Value == OrderLineChangeReaction.Accept ?
					( !string.IsNullOrEmpty( DisplayAccept ) ? DisplayAccept : OrderLineChangeReaction.Accept.ToTextOrName() ) :
					( !string.IsNullOrEmpty( DisplayDecline ) ? DisplayDecline : OrderLineChangeReaction.Decline.ToTextOrName() );
				var reactionInfoContainer = new ReactionInfoContainer( reaction, ClientReactionTime.Value );
				ReactionInfoTemplate.InstantiateIn( reactionInfoContainer );
				Controls.Add( reactionInfoContainer );
				reactionInfoContainer.DataBind();
			}

			//Показываем кнопки только для клиента
			if( ReactionCommandsTemplate != null && ClientReactionPending && SiteContext.Current.User.Role == SecurityRole.Client )
			{
				var container = new Control();
				ReactionCommandsTemplate.InstantiateIn( container );
				Controls.Add( container );
				container.DataBind();
			}
		}

		public override void DataBind()
		{
			base.DataBind();
			CreateChildControls();
			this.ChildControlsCreated = true;
		}

		public override void RenderControl( HtmlTextWriter writer )
		{
			this.RenderChildren( writer );
		}

		protected override bool OnBubbleEvent( object source, EventArgs args )
		{
			var ceArgs = args as CommandEventArgs;
			if( ceArgs != null && ceArgs.CommandName == "Reaction" )
			{
				OnChangeReaction(
					new ChangeReactionEventArgs(
						OrderLineId,
						(OrderLineChangeReaction)Enum.Parse( typeof( OrderLineChangeReaction ), (string)ceArgs.CommandArgument ) ) );
				return true;
			}

			return false;
		}

		protected virtual void OnChangeReaction( ChangeReactionEventArgs args )
		{
			if( ChangeReaction != null )
				ChangeReaction( this, args );
		}
	}

	public class OrderLineStatusNameContainer : Control, INamingContainer
	{
		private int _orderLineId;
		private string _statusName;

		public OrderLineStatusNameContainer( int orderLineId, string statusName )
		{
			_orderLineId = orderLineId;
			_statusName = statusName;
		}

		public string StatusName
		{
			get { return _statusName; }
		}

		public int OrderLineId
		{
			get { return _orderLineId; }
		}
	}

	public class ReactionInfoContainer : Control, INamingContainer
	{
		private string _reaction;
		private DateTime _reactionTime;

		public ReactionInfoContainer( string reaction, DateTime reactionTime )
		{
			_reaction = reaction;
			_reactionTime = reactionTime;
		}

		public string Reaction
		{
			get { return _reaction; }
		}

		public DateTime ReactionTime
		{
			get { return _reactionTime; }
		}
	}

	public class ChangeReactionEventArgs : EventArgs
	{
		public readonly int OrderLineId;
		public readonly OrderLineChangeReaction Reaction;

		public ChangeReactionEventArgs( int orderLineId, OrderLineChangeReaction reaction )
		{
			OrderLineId = orderLineId;
			Reaction = reaction;
		}
	}
}
