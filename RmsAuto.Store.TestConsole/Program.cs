using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.FranchServiceTestHost
{
	class Program
	{
		static void Main( string[] args )
		{
			//Create a URI to serve as the base address
			Uri httpUrl = new Uri( "http://localhost:8090/FranchService" );
			//Create ServiceHost
			ServiceHost host = new ServiceHost(typeof(FranchSvcs.FranchService), httpUrl);
			//Add a service endpoint
			host.AddServiceEndpoint(typeof(FranchSvcs.IFranchService), new WSHttpBinding(), "" );
			//Enable metadata exchange
			ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
			smb.HttpGetEnabled = true;
			host.Description.Behaviors.Add(smb);
			//Start the Service
			host.Open();

			Console.WriteLine( "Service is host at " + DateTime.Now.ToString() );
			Console.WriteLine( "Host is running... Press <Enter> key to stop" );
			Console.ReadLine();

			if (host.State != CommunicationState.Closed)
				host.Close();
		}
	}
}
