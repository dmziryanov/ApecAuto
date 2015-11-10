using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Acctg.Entities
{
    class MessageSerializer
    {
        private const string _RequestXmlRoot = "Request";
        private const string _ResponseXmlRoot = "Response";
        private const string _EnvelopeElementName = "Command";
        private const string _FaultElementName = "Error";

        private ServiceMethodInfo _method;

        public MessageSerializer(ServiceMethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            _method = method;
        }

        public string SerializeRequest(string username, string password, Envelope argsEnvelope)
        {
            if (argsEnvelope == null)
                throw new ArgumentNullException("argsEnvelope");
            argsEnvelope.Action = _method.ServiceAction;
			//#region ### for test, delete this ###

			//using (var ms = new MemoryStream())
			//using (var xmlTextWriter = new XmlTextWriter( ms, Encoding.UTF8 ))
			//{
			//    xmlTextWriter.Formatting = Formatting.Indented;

			//    xmlTextWriter.WriteStartDocument();
			//    xmlTextWriter.WriteStartElement( _RequestXmlRoot );

			//    xmlTextWriter.WriteStartElement( "AuthHeader" );
			//    xmlTextWriter.WriteElementString( "Username", username );
			//    xmlTextWriter.WriteElementString( "Password", password );
			//    xmlTextWriter.WriteEndElement();

			//    var ns = new XmlSerializerNamespaces();
			//    ns.Add( string.Empty, string.Empty );

			//    _method.ArgsSerializer.Serialize( xmlTextWriter, argsEnvelope, ns );

			//    xmlTextWriter.WriteEndElement();
			//    xmlTextWriter.WriteEndDocument();

			//    return Encoding.UTF8.GetString( ((MemoryStream)xmlTextWriter.BaseStream).ToArray() );
			//}

			//#endregion ### for test, delete this ###
            using (var output = new CustomStringWriter(Encoding.UTF8))
            using (var writer = XmlWriter.Create(output))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(_RequestXmlRoot);

                writer.WriteStartElement("AuthHeader");
                writer.WriteElementString("Username", username);
                writer.WriteElementString("Password", password);
                writer.WriteEndElement();

                var xsn = new XmlSerializerNamespaces();
                xsn.Add(string.Empty, string.Empty);

                _method.ArgsSerializer.Serialize(writer, argsEnvelope, xsn);                
                
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();

                return output.ToString();
            }
        }

        public object ReadResultObject(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException("xml content cannot be empty", "xml");
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                reader.MoveToContent();
                reader.Read();
                if (reader.Name == _EnvelopeElementName)
                {
                    return _method.ResultsSerializer.Deserialize(reader);
                }
                else if (reader.Name == _FaultElementName)
                {
                    var errorCode = (AcctgError)reader.ReadElementContentAsInt();
                    var message = Enum.IsDefined(typeof(AcctgError), errorCode) ?
                        errorCode.ToTextOrName() : "Undefined error code";
                    throw new AcctgException(message) { ErrorCode = errorCode };
                }
                else
                {
                    throw new AcctgException("Unexpected service response: '" + xml + "'"); 
                }
            }
        }
    }
}
