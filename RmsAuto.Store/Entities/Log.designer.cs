﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RmsAuto.Store.Entities
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="ex_rmsauto_log")]
	public partial class LogDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public LogDataContext() : 
				base(global::RmsAuto.Store.Properties.Settings.Default.ex_rmsauto_logConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SearchSparePartsLog> SearchSparePartsLogs
		{
			get
			{
				return this.GetTable<SearchSparePartsLog>();
			}
		}
		
		public System.Data.Linq.Table<SearchSparePartsWebServiceLog> SearchSparePartsWebServiceLogs
		{
			get
			{
				return this.GetTable<SearchSparePartsWebServiceLog>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SearchSparePartsLog")]
	public partial class SearchSparePartsLog
	{
		
		private System.DateTime _SearchDate;
		
		private string _PartNumber;
		
		private string _ClientIP;
		
		private string _Manufacturer;
		
		public SearchSparePartsLog()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SearchDate", DbType="DateTime NOT NULL")]
		public System.DateTime SearchDate
		{
			get
			{
				return this._SearchDate;
			}
			set
			{
				if ((this._SearchDate != value))
				{
					this._SearchDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PartNumber", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string PartNumber
		{
			get
			{
				return this._PartNumber;
			}
			set
			{
				if ((this._PartNumber != value))
				{
					this._PartNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientIP", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string ClientIP
		{
			get
			{
				return this._ClientIP;
			}
			set
			{
				if ((this._ClientIP != value))
				{
					this._ClientIP = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Manufacturer", DbType="Varchar(50)")]
		public string Manufacturer
		{
			get
			{
				return this._Manufacturer;
			}
			set
			{
				if ((this._Manufacturer != value))
				{
					this._Manufacturer = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SearchSparePartsWebServiceLog")]
	public partial class SearchSparePartsWebServiceLog
	{
		
		private System.DateTime _SearchDate;
		
		private string _PartNumber;
		
		private string _ClientIP;
		
		private string _ClientID;
		
		private string _Manufacturer;
		
		public SearchSparePartsWebServiceLog()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SearchDate", DbType="DateTime NOT NULL")]
		public System.DateTime SearchDate
		{
			get
			{
				return this._SearchDate;
			}
			set
			{
				if ((this._SearchDate != value))
				{
					this._SearchDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PartNumber", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string PartNumber
		{
			get
			{
				return this._PartNumber;
			}
			set
			{
				if ((this._PartNumber != value))
				{
					this._PartNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientIP", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string ClientIP
		{
			get
			{
				return this._ClientIP;
			}
			set
			{
				if ((this._ClientIP != value))
				{
					this._ClientIP = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientID", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string ClientID
		{
			get
			{
				return this._ClientID;
			}
			set
			{
				if ((this._ClientID != value))
				{
					this._ClientID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Manufacturer", DbType="VarChar(50)")]
		public string Manufacturer
		{
			get
			{
				return this._Manufacturer;
			}
			set
			{
				if ((this._Manufacturer != value))
				{
					this._Manufacturer = value;
				}
			}
		}
	}
}
#pragma warning restore 1591