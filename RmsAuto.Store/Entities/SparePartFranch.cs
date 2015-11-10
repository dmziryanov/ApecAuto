using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Common.Linq;
using System.Configuration;
using System.Globalization;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Entities
{
	[Serializable]
	public class SparePartFranch
	{
        // TODO Пустые setter-ы у ряда свойств выставлены по причине того, что иначе код следующего вида:
		// context.ExecuteQuery<SparePart>( query, ... выпадает с ошибкой, требуя сеттера у свойств у которых его нет
		// необходимо подумать на предмет можно ли обойти данную проблему и как

        private System.Nullable<decimal> _PriceConstantTerm;

		#region === properties ===
		public string Manufacturer { get; set; }
		public string PartNumber { get; set; }
		public string InternalPartNumber { get; set; }
		public string PartName { get; set; }
		public string PartDescription { get; set; }
		public int SupplierID { get; set; }
		public int DeliveryDaysMin { get; set; }
		public int DeliveryDaysMax { get; set; }
		public decimal InitialPrice { get; set; }
		public string RgCode { get; set; }
		public decimal? WeightPhysical { get; set; }
		public decimal? WeightVolume { get; set; }
		public int? QtyInStock { get; set; }
		public int? MinOrderQty { get; set; }
		public decimal? PriceConstantTerm { get; set; }
		public DateTime PriceDate { get; set; }
		public int? SparePartGroupID { get; set; }
		public int PricingMatrixEntryID { get; set; }
		public decimal CorrectionFactor { get; set; }
		public decimal CorrectionFactor39 { get; set; }
		public decimal CustomFactor1 { get; set; }
		public decimal CustomFactor2 { get; set; }
		public decimal CustomFactor3 { get; set; }
		public decimal CustomFactor4 { get; set; }
		public decimal CustomFactor5 { get; set; }
		public decimal CustomFactor6 { get; set; }
		public decimal CustomFactor7 { get; set; }
		public decimal CustomFactor8 { get; set; }
		public decimal CustomFactor9 { get; set; }
		public decimal CustomFactor10 { get; set; }
		public decimal CustomFactor11 { get; set; }
		public decimal CustomFactor12 { get; set; }
		public decimal CustomFactor13 { get; set; }
		public decimal CustomFactor14 { get; set; }
		public decimal CustomFactor15 { get; set; }
		public decimal CustomFactor16 { get; set; }
		public decimal CustomFactor17 { get; set; }
		public decimal CustomFactor18 { get; set; }
		public decimal CustomFactor19 { get; set; }
		public decimal CustomFactor20 { get; set; }
		public decimal CustomFactor21 { get; set; }
		public decimal CustomFactor22 { get; set; }
		public decimal CustomFactor23 { get; set; }
		public decimal CustomFactor24 { get; set; }
		public decimal CustomFactor25 { get; set; }
        public decimal CustomFactor26 { get; set; }
        public decimal CustomFactor27 { get; set; }
        public decimal CustomFactor28 { get; set; }
        public decimal CustomFactor29 { get; set; }
        public decimal CustomFactor30 { get; set; }
        public decimal CustomFactor31 { get; set; }
        public decimal CustomFactor32 { get; set; }
        public decimal CustomFactor33 { get; set; }
        public decimal CustomFactor34 { get; set; }
        public decimal CustomFactor35 { get; set; }
        public decimal CustomFactor36 { get; set; }
        public decimal CustomFactor37 { get; set; }
        public decimal CustomFactor38 { get; set; }
        public decimal CustomFactor39 { get; set; }
        public decimal CustomFactor40 { get; set; }
        public decimal CustomFactor41 { get; set; }
        public decimal CustomFactor42 { get; set; }
        public decimal CustomFactor43 { get; set; }
        public decimal CustomFactor44 { get; set; }
        public decimal CustomFactor45 { get; set; }
        public decimal CustomFactor46 { get; set; }
        public decimal CustomFactor47 { get; set; }
        public decimal CustomFactor48 { get; set; }
        public decimal CustomFactor49 { get; set; }
        public decimal CustomFactor50 { get; set; }
        public decimal CustomFactor51 { get; set; }
        public decimal CustomFactor52 { get; set; }
        public decimal CustomFactor53 { get; set; }
        public decimal CustomFactor54 { get; set; }
        public decimal CustomFactor55 { get; set; }
        public decimal CustomFactor56 { get; set; }
        public decimal CustomFactor57 { get; set; }
        public decimal CustomFactor58 { get; set; }
        public decimal CustomFactor59 { get; set; }
        public decimal CustomFactor60 { get; set; }
        public decimal CustomFactor61 { get; set; }
        public decimal CustomFactor62 { get; set; }
        public decimal CustomFactor63 { get; set; }
        public decimal CustomFactor64 { get; set; }
        public decimal CustomFactor65 { get; set; }
        public decimal CustomFactor66 { get; set; }
        public decimal CustomFactor67 { get; set; }
        public decimal CustomFactor68 { get; set; }
        public decimal CustomFactor69 { get; set; }
        public decimal CustomFactor70 { get; set; }
        public decimal CustomFactor71 { get; set; }
        public decimal CustomFactor72 { get; set; }
        public decimal CustomFactor73 { get; set; }
        public decimal CustomFactor74 { get; set; }
        public decimal CustomFactor75 { get; set; }
        public decimal CustomFactor76 { get; set; }
        public decimal CustomFactor77 { get; set; }
        public decimal CustomFactor78 { get; set; }
        public decimal CustomFactor79 { get; set; }
        public decimal CustomFactor80 { get; set; }
        public decimal CustomFactor81 { get; set; }
        public decimal CustomFactor82 { get; set; }
        public decimal CustomFactor83 { get; set; }
        public decimal CustomFactor84 { get; set; }
        public decimal CustomFactor85 { get; set; }
        public decimal CustomFactor86 { get; set; }
        public decimal CustomFactor87 { get; set; }
        public decimal CustomFactor88 { get; set; }
        public decimal CustomFactor89 { get; set; }
        public decimal CustomFactor90 { get; set; }
        public decimal CustomFactor91 { get; set; }
        public decimal CustomFactor92 { get; set; }
        public decimal CustomFactor93 { get; set; }
        public decimal CustomFactor94 { get; set; }
        public decimal CustomFactor95 { get; set; }
        public decimal CustomFactor96 { get; set; }
        public decimal CustomFactor97 { get; set; }
        public decimal CustomFactor98 { get; set; }
        public decimal CustomFactor99 { get; set; }
        public decimal CustomFactor100 { get; set; }
		public int? BeforeTime { get; set; }
		public int? OnTime { get; set; }
		public int? Delay { get; set; }
		public int? NonDelivery { get; set; }
		public int SizeID { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Розница Базовый"
		/// </summary>
		public decimal AdditionalCF1 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Розница -3%"
		/// </summary>
		public decimal AdditionalCF2 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Розница -5%"
		/// </summary>
		public decimal AdditionalCF3 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Розница -7%"
		/// </summary>
		public decimal AdditionalCF4 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Розница Специальный"
		/// </summary>
		public decimal AdditionalCF5 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Опт ТН1"
		/// </summary>
		public decimal AdditionalCF6 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Опт ТН2"
		/// </summary>
		public decimal AdditionalCF7 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Опт ТН3"
		/// </summary>
		public decimal AdditionalCF8 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Опт ТН4"
		/// </summary>
		public decimal AdditionalCF9 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Опт ТН5"
		/// </summary>
		public decimal AdditionalCF10 { get; set; }
		/// <summary>
		/// Добавочный коэффициент "Прайс Опт ТН6"
		/// </summary>
		public decimal AdditionalCF11 { get; set; }
		#endregion

		#region === calculated properties ===
		/// <summary>
		/// Отображаемый минимальный срок поставки
		/// </summary>
		public int DisplayDeliveryDaysMin
		{
			get { return DeliveryDaysMin > 0 ? DeliveryDaysMin : 1; }
			set { }
		}

		/// <summary>
		/// Отображаемый максимальный срок поставки
		/// </summary>
		public int DisplayDeliveryDaysMax
		{
			get { return DeliveryDaysMax > 0 ? DeliveryDaysMax : 1; }
			set { }
		}

		/// <summary>
		/// Закупочная цена (по которой франч закупает товар у нас)
		/// </summary>
		public decimal SupplierPriceWithMarkup
		{
			get
			{
				if (SiteContext.Current.InternalFranchName == "rmsauto")
                {
                    return InitialPrice * CorrectionFactor + _PriceConstantTerm.GetValueOrDefault();
                }
                else
                {
                    RmsAuto.Acctg.ClientGroup clientGroup;
                    //Для франчайзи (и для лайтов) номер колонки берем из справочника франчей
                    clientGroup = (RmsAuto.Acctg.ClientGroup)AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].ClientGroup;

                    var clientGroupPrice = InitialPrice * CorrectionFactor * CorrectionFactor39 * GetCustomFactor(clientGroup) + PriceConstantTerm.GetValueOrDefault();
                    return Math.Round(clientGroupPrice, 2);
                }
			}
			set { }
		}

		public int DefaultOrderQty
		{
			get { return MinOrderQty ?? 1; }
			set { }
		}
		#endregion

		#region === helpers ===
		public decimal GetFinalSalePrice(RmsAuto.Acctg.ClientGroup clientGroup, decimal personalMarkup )
		{
			GlobalSetting setting = null;
			decimal additionalPriceFactor = 1.0m;
			if (SizeID == 1) /* значит товар - габарит */
			{
				setting = GlobalDBSettings.GlobalSettings.Current.GetSettingByName("PriceFactors.Gabarit");
			}
			else if (SizeID == 2) /* значит товар - шина */
			{
				setting = GlobalDBSettings.GlobalSettings.Current.GetSettingByName("PriceFactors.Tires");
			}
			if (setting != null)
			{
				if (!Decimal.TryParse(
					setting.Value,
					NumberStyles.AllowDecimalPoint,
					new NumberFormatInfo() { NumberDecimalSeparator = "." },
					out additionalPriceFactor)) { additionalPriceFactor = 1.0m; }
			}

            decimal clientGroupPrice = 0;
            if (SiteContext.Current.InternalFranchName == "rmsauto") //для Москвы цена считается по-старому
            {
                clientGroupPrice = InitialPrice * CorrectionFactor * CorrectionFactor39 * GetCustomFactor(clientGroup)
                    /** GetAdditionalCustomFactor(clientGroup)*/ * additionalPriceFactor + PriceConstantTerm.GetValueOrDefault();
            }
            else
            {
                //считаем цену для франча
                //номер колонки для расчета базовой цены для франча берем из справочника
                RmsAuto.Acctg.ClientGroup cgf = (RmsAuto.Acctg.ClientGroup)AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].ClientGroup;
                var clientGroupPrice_Franch = InitialPrice * CorrectionFactor * CorrectionFactor39 * GetCustomFactor(cgf)
                    * GetAdditionalCustomFactor(clientGroup) * additionalPriceFactor + PriceConstantTerm.GetValueOrDefault();
                //считаем цену для РМС
                var clientGroupPrice_RMS = InitialPrice * CorrectionFactor * CorrectionFactor39 * GetCustomFactor(clientGroup)
                    * additionalPriceFactor + PriceConstantTerm.GetValueOrDefault();
                //берем максимальную (исключая таким образом занижение франчами цен относительно наших)
                clientGroupPrice = Math.Max(clientGroupPrice_Franch, clientGroupPrice_RMS);
            }
            var finalPrice = clientGroupPrice * (100 + personalMarkup) / 100;
            return Math.Round(finalPrice, 2); //Math.Round(x, 2) - since coefs have 4-digits accuracy
		}

        public decimal GetCustomFactor(RmsAuto.Acctg.ClientGroup clientGroup)
		{
			switch( clientGroup )
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

		public decimal GetAdditionalCustomFactor(RmsAuto.Acctg.ClientGroup clientGroup)
		{
			//Если это не франч, то этот параметр не учитывается
            if (SiteContext.Current.InternalFranchName == "rmsauto")
                return 1.0m;

            switch (clientGroup)
			{
                case RmsAuto.Acctg.ClientGroup.Group1:  return AdditionalCF1;
                case RmsAuto.Acctg.ClientGroup.Group2:  return AdditionalCF2;
                case RmsAuto.Acctg.ClientGroup.Group3:  return AdditionalCF3;
                case RmsAuto.Acctg.ClientGroup.Group4:  return AdditionalCF4;
                case RmsAuto.Acctg.ClientGroup.Group5:  return AdditionalCF5;
                case RmsAuto.Acctg.ClientGroup.Group20: return AdditionalCF6;
                case RmsAuto.Acctg.ClientGroup.Group21: return AdditionalCF7;
                case RmsAuto.Acctg.ClientGroup.Group8:  return AdditionalCF8;
                case RmsAuto.Acctg.ClientGroup.Group10: return AdditionalCF9;
                case RmsAuto.Acctg.ClientGroup.Group13: return AdditionalCF10;
                case RmsAuto.Acctg.ClientGroup.Group15: return AdditionalCF11;
				default: throw new IndexOutOfRangeException();
			}
		}

		public RmsAuto.Common.Data.PartKey PartKey
		{
			get
			{
				return new RmsAuto.Common.Data.PartKey( this.Manufacturer, this.PartNumber );
			}
			set { }
		}
		#endregion

	}
}
