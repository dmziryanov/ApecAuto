using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace RmsAuto.Store.Cms.Mail
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
	class MailAttachmentAttribute : Attribute
	{

		public string Name
		{
			get { return _name; }
		}

		public byte[] Data
		{
			get
			{
				if( _data == null )
				{
					lock( typeof( MailTemplateAttribute ) )
					{
						byte[] data;
						using( Stream stm = typeof( MailTemplateAttribute ).Assembly.GetManifestResourceStream( _resourceName ) )
						{
							stm.Seek( 0, SeekOrigin.End );
							data = new byte[ stm.Position ];
							stm.Seek( 0, SeekOrigin.Begin );
							for( int pos = 0 ; pos < data.Length ; ++pos )
								pos += stm.Read( data, pos, data.Length - pos );
						}
						_data = data;
					}
				}
				return _data;
			}
		}
		byte[] _data;

		string _name;
		string _resourceName;

		public MailAttachmentAttribute( string name, string resourceName )
		{
			_name = name;
			_resourceName = resourceName;
		}

	}
}
