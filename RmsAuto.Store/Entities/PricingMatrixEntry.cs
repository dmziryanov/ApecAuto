using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Data;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(PricingMatrixEntryMetadata))]
    public partial class PricingMatrixEntry
    {
        partial void OnPartNumberChanged()
        {
            if (_PartNumber != null) _PartNumber = _PartNumber.ToUpper();
        }

        partial void OnRgCodeSpecChanged()
        {
            if (_RgCodeSpec != null) _RgCodeSpec = _RgCodeSpec.ToUpper();
        }

        partial void OnValidate(ChangeAction action)
        {
            if (action == ChangeAction.Insert || action == ChangeAction.Update)
            {
                if (!string.IsNullOrEmpty(RgCodeSpec) && !string.IsNullOrEmpty(PartNumber))
                    throw new ValidationException("может быть задан либо 'RG код', либо 'номер'");
                if ((!string.IsNullOrEmpty(RgCodeSpec) || !string.IsNullOrEmpty(PartNumber)) &&
                    string.IsNullOrEmpty(Manufacturer))
                    throw new ValidationException("для указания 'RG кода' или 'номера' необходимо выбрать производителя");

                using (var dc = new DCFactory<StoreDataContext>())
                {
                    if (dc.DataContext.PricingMatrixEntries.SingleOrDefault(
                        e => e.SupplierID == SupplierID &&
                            e.Manufacturer == Manufacturer &&
                            e.PartNumber == PartNumber &&
                            e.RgCode == RgCodeSpec) != null)
                        throw new ValidationException("ценовые коэффициенты уже заданы для данной комбинации 'поставщик-производитель-RG-номер'");
                }
            }
        }
    }

    [DisplayName("Ценовые коэффициенты")]
    public partial class PricingMatrixEntryMetadata
    {
        [DisplayName("Поставщик")]
        [Required(ErrorMessage = "не выбран поставщик")]
        [UIHint("Custom/AcctgRef", null, "BindingOptions", "Suppliers;SupplierId;SupplierName")] 
        public object SupplierID { get; set; }

        [DisplayName("Производитель")]
        public object Manufacturer { get; set; }

        [DisplayName("RG код")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,10}$", 
            ErrorMessage = "RG код не должен содержать пробелов и спец символов и иметь не более 10 символов в длину")]
        public object RgCodeSpec { get; set; }

        [DisplayName("Оригинальный номер запчасти")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,50}$",
            ErrorMessage = "оригинальный номер не должен содержать пробелов и спец символов")]
        public object PartNumber { get; set; }

        [DisplayName("С1 (скидка поставщика)")]
        //раньше коэффициенты были в процентах и использовался [UIHint("Custom/DecimalCorrection")]
        public object CorrectionFactor { get; set; }

        [DisplayName("K39 (розничная наценка)")]
        public object CorrectionFactor39 { get; set; }

        [DisplayName("Ск1 (персональная скидка 1)")]
        public object CustomFactor1 { get; set; }

        [DisplayName("Ск2 (персональная скидка 2)")]
        public object CustomFactor2 { get; set; }

        [DisplayName("Ск3 (персональная скидка 3)")]
        public object CustomFactor3 { get; set; }

        [DisplayName("Ск4 (персональная скидка 4)")]
        public object CustomFactor4 { get; set; }

        [DisplayName("Ск5 (персональная скидка 5)")]
        public object CustomFactor5 { get; set; }

        [DisplayName("Ск6 (персональная скидка 6)")]
        public object CustomFactor6 { get; set; }

        [DisplayName("Ск7 (персональная скидка 7)")]
        public object CustomFactor7 { get; set; }

        [DisplayName("Ск8 (персональная скидка 8)")]
        public object CustomFactor8 { get; set; }

        [DisplayName("Ск9 (персональная скидка 9)")]
        public object CustomFactor9 { get; set; }

        [DisplayName("Ск10 (персональная скидка 10)")]
        public object CustomFactor10 { get; set; }

        [DisplayName("Ск11 (персональная скидка 11)")]
        public object CustomFactor11 { get; set; }

        [DisplayName("Ск12 (персональная скидка 12)")]
        public object CustomFactor12 { get; set; }

        [DisplayName("Ск13 (персональная скидка 13)")]
        public object CustomFactor13 { get; set; }

        [DisplayName("Ск14 (персональная скидка 14)")]
        public object CustomFactor14 { get; set; }

        [DisplayName("Ск15 (персональная скидка 15)")]
        public object CustomFactor15 { get; set; }

		[DisplayName("Ск16 (персональная скидка 16)")]
		public object CustomFactor16 { get; set; }

		[DisplayName("Ск17 (персональная скидка 17)")]
		public object CustomFactor17 { get; set; }

		[DisplayName("Ск18 (персональная скидка 18)")]
		public object CustomFactor18 { get; set; }

		[DisplayName("Ск19 (персональная скидка 19)")]
		public object CustomFactor19 { get; set; }

		[DisplayName("Ск20 (персональная скидка 20)")]
		public object CustomFactor20 { get; set; }

		[DisplayName("Ск21 (персональная скидка 21)")]
		public object CustomFactor21 { get; set; }

		[DisplayName("Ск22 (персональная скидка 22)")]
		public object CustomFactor22 { get; set; }

		[DisplayName("Ск23 (персональная скидка 23)")]
		public object CustomFactor23 { get; set; }

		[DisplayName("Ск24 (персональная скидка 24)")]
		public object CustomFactor24 { get; set; }

		[DisplayName("Ск25 (персональная скидка 25)")]
		public object CustomFactor25 { get; set; }

        [DisplayName("Ск26 (персональная скидка 26)")]
        public object CustomFactor26 { get; set; }

        [DisplayName("Ск27 (персональная скидка 27)")]
        public object CustomFactor27 { get; set; }

        [DisplayName("Ск28 (персональная скидка 28)")]
        public object CustomFactor28 { get; set; }

        [DisplayName("Ск29 (персональная скидка 29)")]
        public object CustomFactor29 { get; set; }

        [DisplayName("Ск30 (персональная скидка 30)")]
        public object CustomFactor30 { get; set; }

        [DisplayName("Ск31 (персональная скидка 31)")]
        public object CustomFactor31 { get; set; }

        [DisplayName("Ск32 (персональная скидка 32)")]
        public object CustomFactor32 { get; set; }

        [DisplayName("Ск33 (персональная скидка 33)")]
        public object CustomFactor33 { get; set; }

        [DisplayName("Ск34 (персональная скидка 34)")]
        public object CustomFactor34 { get; set; }

        [DisplayName("Ск35 (персональная скидка 35)")]
        public object CustomFactor35 { get; set; }

        [DisplayName("Ск36 (персональная скидка 36)")]
        public object CustomFactor36 { get; set; }

        [DisplayName("Ск37 (персональная скидка 37)")]
        public object CustomFactor37 { get; set; }

        [DisplayName("Ск38 (персональная скидка 38)")]
        public object CustomFactor38 { get; set; }

        [DisplayName("Ск39 (персональная скидка 39)")]
        public object CustomFactor39 { get; set; }

        [DisplayName("Ск40 (персональная скидка 40)")]
        public object CustomFactor40 { get; set; }

        [DisplayName("Ск41 (персональная скидка 41)")]
        public object CustomFactor41 { get; set; }

        [DisplayName("Ск42 (персональная скидка 42)")]
        public object CustomFactor42 { get; set; }

        [DisplayName("Ск43 (персональная скидка 43)")]
        public object CustomFactor43 { get; set; }

        [DisplayName("Ск44 (персональная скидка 44)")]
        public object CustomFactor44 { get; set; }

        [DisplayName("Ск45 (персональная скидка 45)")]
        public object CustomFactor45 { get; set; }

        [DisplayName("Ск46 (персональная скидка 46)")]
        public object CustomFactor46 { get; set; }

        [DisplayName("Ск47 (персональная скидка 47)")]
        public object CustomFactor47 { get; set; }

        [DisplayName("Ск48 (персональная скидка 48)")]
        public object CustomFactor48 { get; set; }

        [DisplayName("Ск49 (персональная скидка 49)")]
        public object CustomFactor49 { get; set; }

        [DisplayName("Ск50 (персональная скидка 50)")]
        public object CustomFactor50 { get; set; }

        [DisplayName("Ск51 (персональная скидка 51)")]
        public object CustomFactor51 { get; set; }

        [DisplayName("Ск52 (персональная скидка 52)")]
        public object CustomFactor52 { get; set; }

        [DisplayName("Ск53 (персональная скидка 53)")]
        public object CustomFactor53 { get; set; }

        [DisplayName("Ск54 (персональная скидка 54)")]
        public object CustomFactor54 { get; set; }

        [DisplayName("Ск55 (персональная скидка 55)")]
        public object CustomFactor55 { get; set; }

        [DisplayName("Ск56 (персональная скидка 56)")]
        public object CustomFactor56 { get; set; }

        [DisplayName("Ск57 (персональная скидка 57)")]
        public object CustomFactor57 { get; set; }

        [DisplayName("Ск58 (персональная скидка 58)")]
        public object CustomFactor58 { get; set; }

        [DisplayName("Ск59 (персональная скидка 59)")]
        public object CustomFactor59 { get; set; }

        [DisplayName("Ск60 (персональная скидка 60)")]
        public object CustomFactor60 { get; set; }

        [DisplayName("Ск61 (персональная скидка 61)")]
        public object CustomFactor61 { get; set; }

        [DisplayName("Ск62 (персональная скидка 62)")]
        public object CustomFactor62 { get; set; }

        [DisplayName("Ск63 (персональная скидка 63)")]
        public object CustomFactor63 { get; set; }

        [DisplayName("Ск64 (персональная скидка 64)")]
        public object CustomFactor64 { get; set; }

        [DisplayName("Ск65 (персональная скидка 65)")]
        public object CustomFactor65 { get; set; }

        [DisplayName("Ск66 (персональная скидка 66)")]
        public object CustomFactor66 { get; set; }

        [DisplayName("Ск67 (персональная скидка 67)")]
        public object CustomFactor67 { get; set; }

        [DisplayName("Ск68 (персональная скидка 68)")]
        public object CustomFactor68 { get; set; }

        [DisplayName("Ск69 (персональная скидка 69)")]
        public object CustomFactor69 { get; set; }

        [DisplayName("Ск70 (персональная скидка 70)")]
        public object CustomFactor70 { get; set; }

        [DisplayName("Ск71 (персональная скидка 71)")]
        public object CustomFactor71 { get; set; }

        [DisplayName("Ск72 (персональная скидка 72)")]
        public object CustomFactor72 { get; set; }

        [DisplayName("Ск73 (персональная скидка 73)")]
        public object CustomFactor73 { get; set; }

        [DisplayName("Ск74 (персональная скидка 74)")]
        public object CustomFactor74 { get; set; }

        [DisplayName("Ск75 (персональная скидка 75)")]
        public object CustomFactor75 { get; set; }

        [DisplayName("Ск76 (персональная скидка 76)")]
        public object CustomFactor76 { get; set; }

        [DisplayName("Ск77 (персональная скидка 77)")]
        public object CustomFactor77 { get; set; }

        [DisplayName("Ск78 (персональная скидка 78)")]
        public object CustomFactor78 { get; set; }

        [DisplayName("Ск79 (персональная скидка 79)")]
        public object CustomFactor79 { get; set; }

        [DisplayName("Ск80 (персональная скидка 80)")]
        public object CustomFactor80 { get; set; }

        [DisplayName("Ск81 (персональная скидка 81)")]
        public object CustomFactor81 { get; set; }

        [DisplayName("Ск82 (персональная скидка 82)")]
        public object CustomFactor82 { get; set; }

        [DisplayName("Ск83 (персональная скидка 83)")]
        public object CustomFactor83 { get; set; }

        [DisplayName("Ск84 (персональная скидка 84)")]
        public object CustomFactor84 { get; set; }

        [DisplayName("Ск85 (персональная скидка 85)")]
        public object CustomFactor85 { get; set; }

        [DisplayName("Ск86 (персональная скидка 86)")]
        public object CustomFactor86 { get; set; }

        [DisplayName("Ск87 (персональная скидка 87)")]
        public object CustomFactor87 { get; set; }

        [DisplayName("Ск88 (персональная скидка 88)")]
        public object CustomFactor88 { get; set; }

        [DisplayName("Ск89 (персональная скидка 89)")]
        public object CustomFactor89 { get; set; }

        [DisplayName("Ск90 (персональная скидка 90)")]
        public object CustomFactor90 { get; set; }

        [DisplayName("Ск91 (персональная скидка 91)")]
        public object CustomFactor91 { get; set; }

        [DisplayName("Ск92 (персональная скидка 92)")]
        public object CustomFactor92 { get; set; }

        [DisplayName("Ск93 (персональная скидка 93)")]
        public object CustomFactor93 { get; set; }

        [DisplayName("Ск94 (персональная скидка 94)")]
        public object CustomFactor94 { get; set; }

        [DisplayName("Ск95 (персональная скидка 95)")]
        public object CustomFactor95 { get; set; }

        [DisplayName("Ск96 (персональная скидка 96)")]
        public object CustomFactor96 { get; set; }

        [DisplayName("Ск97 (персональная скидка 97)")]
        public object CustomFactor97 { get; set; }

        [DisplayName("Ск98 (персональная скидка 98)")]
        public object CustomFactor98 { get; set; }

        [DisplayName("Ск99 (персональная скидка 99)")]
        public object CustomFactor99 { get; set; }

        [DisplayName("Ск100 (персональная скидка 100)")]
        public object CustomFactor100 { get; set; }

        // Прячем колонки для статистики
        [ScaffoldColumn(false)]
        public object BeforeTime { get; set; }

        [ScaffoldColumn(false)]
        public object OnTime { get; set; }

        [ScaffoldColumn(false)]
        public object Delay { get; set; }

        [ScaffoldColumn(false)]
        public object NonDelivery { get; set; }
    }
}
