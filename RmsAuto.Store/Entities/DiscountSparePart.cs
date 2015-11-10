using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Entities
{
	public partial class DiscountSparePart
	{
		/// <summary>
		/// Отображаемый минимальный срок поставки
		/// </summary>
		public int DisplayDeliveryDaysMin
		{
			get { return DeliveryDaysMin > 0 ? DeliveryDaysMin : 1; }
		}

		/// <summary>
		/// Отображаемый максимальный срок поставки
		/// </summary>
		public int DisplayDeliveryDaysMax
		{
			get { return DeliveryDaysMax > 0 ? DeliveryDaysMax : 1; }
		}

		public int DefaultOrderQty
		{
			get { return MinOrderQty ?? 1; }
		}

		public decimal GetFinalSalePrice(RmsAuto.Acctg.ClientGroup clientGroup, decimal personalMarkup)
		{
			var clientGroupPrice = InitialPrice * CorrectionFactor * CorrectionFactor39 * GetCustomFactor(clientGroup) + _PriceConstantTerm.GetValueOrDefault();
			var finalPrice = clientGroupPrice * (100 + personalMarkup) / 100;
			return Math.Round(finalPrice, 2); //Math.Round(x, 2) - since coefs have 4-digits accuracy
		}

		public decimal GetCustomFactor(RmsAuto.Acctg.ClientGroup clientGroup)
		{
            switch (clientGroup)
			{
                case RmsAuto.Acctg.ClientGroup.Group1: return CustomFactor1;
                case RmsAuto.Acctg.ClientGroup.Group2: return CustomFactor2;
                case RmsAuto.Acctg.ClientGroup.Group3: return CustomFactor3;
                case RmsAuto.Acctg.ClientGroup.Group4: return CustomFactor4;
                case RmsAuto.Acctg.ClientGroup.Group5: return CustomFactor5;
                case RmsAuto.Acctg.ClientGroup.Group6: return CustomFactor6;
                case RmsAuto.Acctg.ClientGroup.Group7: return CustomFactor7;
				case RmsAuto.Acctg.ClientGroup.Group8: return CustomFactor8;
                case RmsAuto.Acctg.ClientGroup.Group9: return CustomFactor9;
                case RmsAuto.Acctg.ClientGroup.Group10: return CustomFactor10;
                case RmsAuto.Acctg.ClientGroup.Group11: return CustomFactor11;
                case RmsAuto.Acctg.ClientGroup.Group12: return CustomFactor12;
                case RmsAuto.Acctg.ClientGroup.Group13: return CustomFactor13;
                case RmsAuto.Acctg.ClientGroup.Group14: return CustomFactor14;
                case RmsAuto.Acctg.ClientGroup.Group15: return CustomFactor15;
                case RmsAuto.Acctg.ClientGroup.Group16: return CustomFactor16;
                case RmsAuto.Acctg.ClientGroup.Group17: return CustomFactor17;
                case RmsAuto.Acctg.ClientGroup.Group18: return CustomFactor18;
                case RmsAuto.Acctg.ClientGroup.Group19: return CustomFactor19;
                case RmsAuto.Acctg.ClientGroup.Group20: return CustomFactor20;
                case RmsAuto.Acctg.ClientGroup.Group21: return CustomFactor21;
                case RmsAuto.Acctg.ClientGroup.Group22: return CustomFactor22;
                case RmsAuto.Acctg.ClientGroup.Group23: return CustomFactor23;
                case RmsAuto.Acctg.ClientGroup.Group24: return CustomFactor24;
                case RmsAuto.Acctg.ClientGroup.Group25: return CustomFactor25;
                case RmsAuto.Acctg.ClientGroup.Group26: return CustomFactor26;
                case RmsAuto.Acctg.ClientGroup.Group27: return CustomFactor27;
                case RmsAuto.Acctg.ClientGroup.Group28: return CustomFactor28;
                case RmsAuto.Acctg.ClientGroup.Group29: return CustomFactor29;
                case RmsAuto.Acctg.ClientGroup.Group30: return CustomFactor30;
                case RmsAuto.Acctg.ClientGroup.Group31: return CustomFactor31;
                case RmsAuto.Acctg.ClientGroup.Group32: return CustomFactor32;
                case RmsAuto.Acctg.ClientGroup.Group33: return CustomFactor33;
                case RmsAuto.Acctg.ClientGroup.Group34: return CustomFactor34;
                case RmsAuto.Acctg.ClientGroup.Group35: return CustomFactor35;
                case RmsAuto.Acctg.ClientGroup.Group36: return CustomFactor36;
                case RmsAuto.Acctg.ClientGroup.Group37: return CustomFactor37;
                case RmsAuto.Acctg.ClientGroup.Group38: return CustomFactor38;
                case RmsAuto.Acctg.ClientGroup.Group39: return CustomFactor39;
                case RmsAuto.Acctg.ClientGroup.Group40: return CustomFactor40;
                case RmsAuto.Acctg.ClientGroup.Group41: return CustomFactor41;
                case RmsAuto.Acctg.ClientGroup.Group42: return CustomFactor42;
                case RmsAuto.Acctg.ClientGroup.Group43: return CustomFactor43;
                case RmsAuto.Acctg.ClientGroup.Group44: return CustomFactor44;
                case RmsAuto.Acctg.ClientGroup.Group45: return CustomFactor45;
                case RmsAuto.Acctg.ClientGroup.Group46: return CustomFactor46;
                case RmsAuto.Acctg.ClientGroup.Group47: return CustomFactor47;
                case RmsAuto.Acctg.ClientGroup.Group48: return CustomFactor48;
                case RmsAuto.Acctg.ClientGroup.Group49: return CustomFactor49;
                case RmsAuto.Acctg.ClientGroup.Group50: return CustomFactor50;
                case RmsAuto.Acctg.ClientGroup.Group51: return CustomFactor51;
                case RmsAuto.Acctg.ClientGroup.Group52: return CustomFactor52;
                case RmsAuto.Acctg.ClientGroup.Group53: return CustomFactor53;
                case RmsAuto.Acctg.ClientGroup.Group54: return CustomFactor54;
                case RmsAuto.Acctg.ClientGroup.Group55: return CustomFactor55;
                case RmsAuto.Acctg.ClientGroup.Group56: return CustomFactor56;
                case RmsAuto.Acctg.ClientGroup.Group57: return CustomFactor57;
                case RmsAuto.Acctg.ClientGroup.Group58: return CustomFactor58;
                case RmsAuto.Acctg.ClientGroup.Group59: return CustomFactor59;
                case RmsAuto.Acctg.ClientGroup.Group60: return CustomFactor60;
                case RmsAuto.Acctg.ClientGroup.Group61: return CustomFactor61;
                case RmsAuto.Acctg.ClientGroup.Group62: return CustomFactor62;
                case RmsAuto.Acctg.ClientGroup.Group63: return CustomFactor63;
                case RmsAuto.Acctg.ClientGroup.Group64: return CustomFactor64;
                case RmsAuto.Acctg.ClientGroup.Group65: return CustomFactor65;
                case RmsAuto.Acctg.ClientGroup.Group66: return CustomFactor66;
                case RmsAuto.Acctg.ClientGroup.Group67: return CustomFactor67;
                case RmsAuto.Acctg.ClientGroup.Group68: return CustomFactor68;
                case RmsAuto.Acctg.ClientGroup.Group69: return CustomFactor69;
                case RmsAuto.Acctg.ClientGroup.Group70: return CustomFactor70;
                case RmsAuto.Acctg.ClientGroup.Group71: return CustomFactor71;
                case RmsAuto.Acctg.ClientGroup.Group72: return CustomFactor72;
                case RmsAuto.Acctg.ClientGroup.Group73: return CustomFactor73;
                case RmsAuto.Acctg.ClientGroup.Group74: return CustomFactor74;
                case RmsAuto.Acctg.ClientGroup.Group75: return CustomFactor75;
                case RmsAuto.Acctg.ClientGroup.Group76: return CustomFactor76;
                case RmsAuto.Acctg.ClientGroup.Group77: return CustomFactor77;
                case RmsAuto.Acctg.ClientGroup.Group78: return CustomFactor78;
                case RmsAuto.Acctg.ClientGroup.Group79: return CustomFactor79;
                case RmsAuto.Acctg.ClientGroup.Group80: return CustomFactor80;
                case RmsAuto.Acctg.ClientGroup.Group81: return CustomFactor81;
                case RmsAuto.Acctg.ClientGroup.Group82: return CustomFactor82;
                case RmsAuto.Acctg.ClientGroup.Group83: return CustomFactor83;
                case RmsAuto.Acctg.ClientGroup.Group84: return CustomFactor84;
                case RmsAuto.Acctg.ClientGroup.Group85: return CustomFactor85;
                case RmsAuto.Acctg.ClientGroup.Group86: return CustomFactor86;
                case RmsAuto.Acctg.ClientGroup.Group87: return CustomFactor87;
                case RmsAuto.Acctg.ClientGroup.Group88: return CustomFactor88;
                case RmsAuto.Acctg.ClientGroup.Group89: return CustomFactor89;
                case RmsAuto.Acctg.ClientGroup.Group90: return CustomFactor90;
                case RmsAuto.Acctg.ClientGroup.Group91: return CustomFactor91;
                case RmsAuto.Acctg.ClientGroup.Group92: return CustomFactor92;
                case RmsAuto.Acctg.ClientGroup.Group93: return CustomFactor93;
                case RmsAuto.Acctg.ClientGroup.Group94: return CustomFactor94;
                case RmsAuto.Acctg.ClientGroup.Group95: return CustomFactor95;
                case RmsAuto.Acctg.ClientGroup.Group96: return CustomFactor96;
                case RmsAuto.Acctg.ClientGroup.Group97: return CustomFactor97;
                case RmsAuto.Acctg.ClientGroup.Group98: return CustomFactor98;
                case RmsAuto.Acctg.ClientGroup.Group99: return CustomFactor99;
                case RmsAuto.Acctg.ClientGroup.Group100: return CustomFactor100;
				default: throw new IndexOutOfRangeException();
			}
		}
	}
}
