using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum UserMaintEntryPurpose : byte
    {
		//[Text("Регистрация клиента онлайн")]
		[Text("Customer online registration")]
		NewClientRegistration = 1,
		//[Text("Создание учетной записи пользователя интернет-магазина")]
		[Text("Creating of internet-shop user account")]
		ExistingClientWebAccess = 2,
		//[Text("Создание нового пароля")]
		[Text("Create a new password")]
		PasswordRecovery = 3,
		//[Text( "Создание логина и пароля для пользователя" )]
		[Text("Creating login and password for user")]
		ActivateClient = 4
    }
}
