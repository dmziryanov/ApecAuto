using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Data;
using RmsAuto.Store.BL;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {

        /// <summary>
        /// Телефон контактного лица
        /// </summary>
        public string ContactPhone { get; set; }

        partial void OnValidate(ChangeAction action)
        {
			if( action == ChangeAction.Insert || action == ChangeAction.Update )
            {
                string errorMessage = null;
                using (var dc = new DCFactory<StoreDataContext>())
                {
                    if (dc.DataContext.Users.Any(u => (u.Username == Username && u.UserID != UserID) && !string.IsNullOrEmpty(Username)))
                        errorMessage = "логин уже используется";
                   
                    if (Role == SecurityRole.Manager)
                    {
                        if (dc.DataContext.Users.Any(u => 
                            u.AcctgID == AcctgID && 
                            u.Role == SecurityRole.Manager
							&& u.UserID != UserID ) )
                            errorMessage = "логин уже создан для данного сотрудника";
                    }
                }
                if (errorMessage != null)
                    throw new ValidationException(errorMessage);
            }

        }
    }

    
    [DisplayName("Логины операторов")]
    public partial class UserMetadata
    {
        [ScaffoldColumn(true)]
        [DisplayName("Сотрудник РМС-Авто")]
        [Required(ErrorMessage="не выбран сотрудник РМС-Авто")]
        [UIHint("Custom/AcctgRef", null, "BindingOptions","Employees;EmployeeId;FullName")] 
        public object AcctgID { get; set; }
      
        [ScaffoldColumn(true)]
        [DisplayName("Логин")]
        [RegularExpression(@"\w{1,20}$", ErrorMessage = "Логин должен содержать только буквы и цифры и иметь 20 символов в длину")]
        public object Username { get; set; }

        [DisplayName("Пароль")]
        [UIHint("Password")]
        [ScaffoldColumn(true)]
        [RegularExpression(@".{1,10}$", ErrorMessage = "Длина пароля не должна превышать 10 символов")]
        public object Password { get; set; }
                
        [ScaffoldColumn(true)]
        [DisplayName("Роль")]
        [UIHint("Enumeration")]
        [StaticFilter(SecurityRole.Manager)]
        public object Role { get; set; }
     
        [ScaffoldColumn(false)]
        public object CreationTime { get; set; }
    }
     
}
