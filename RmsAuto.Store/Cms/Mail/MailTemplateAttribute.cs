using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace RmsAuto.Store.Cms.Mail
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
	class MailTemplateAttribute : Attribute
	{
		internal XslCompiledTransform XslTransform
		{
			get
			{
				if( _xslt == null )
				{
					lock( typeof( MailTemplateAttribute ) )
					{
						XslCompiledTransform t = new XslCompiledTransform();
						using( Stream stm = typeof( MailTemplateAttribute ).Assembly.GetManifestResourceStream( _xsltResourceName ) )
						using( XmlTextReader rd = new XmlTextReader( stm ) )
						{
							t.Load( rd );
						}
						_xslt = t;
					}
				}
				return _xslt;
			}
		}
		XslCompiledTransform _xslt;

		string _xsltResourceName;

		public MailTemplateAttribute( string xsltResourceName )
		{
			_xsltResourceName = xsltResourceName;
		}

	}
}
