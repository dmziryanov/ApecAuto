using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    [Serializable()]
    public class HandyClientSet
    {
        public static HandyClientSet Load(int managerId)
        {
			var set = new HandyClientSet() { _managerId = managerId };

			var manager = AcctgRefCatalog.RmsEmployees[ SiteContext.Current.User.AcctgID ];

            foreach (var entry in ManagerBO.GetManagerHandySet(managerId))
            {
                try
                {
                    var profile = ClientProfile.Load(entry.ClientID);
                    set._innerClients.Add(
                        entry.ClientID,
                        new ClientData()
                        {
                            Profile = profile,
                            Status = ClientBO.GetClientStatus(profile.ClientId),
							Cart = new ShoppingCart( managerId, entry.ClientID, profile.ClientGroup, profile.PersonalMarkup )
                        });
                    if (string.IsNullOrEmpty(set._defaultClientId) && entry.IsDefault)
                        set._defaultClientId = entry.ClientID;
                }
                catch (AcctgException ex)
                {
					//TODO: добавлять в список клиентов клиента с пометкой "профиль не подгружен из-за ошибки"
					//set._innerClients.Add
					//    (
					//    entry.ClientID,
					//    null );
					if( ex.ErrorCode == AcctgError.ClientNotFound )
						ManagerBO.RemoveClientFromHandySet( entry.ManagerID, entry.ClientID );
                }
            }
            return set;
        }

		private int _managerId;
        private string _defaultClientId;
        private Dictionary<string, ClientData> _innerClients = new Dictionary<string, ClientData>();

		public int Count
		{
			get { return _innerClients.Count; }
		}
        public IEnumerable<ClientData> Clients
        {
            get { return _innerClients.Values; }
        }

        public ClientData DefaultClient
        {
            get { return !string.IsNullOrEmpty(_defaultClientId) ? _innerClients[_defaultClientId] : null; }
        }

        public ClientData this[string clientId]
        {
            get { return _innerClients[clientId]; }
        }

        public bool Contains(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");
            return _innerClients.ContainsKey(clientId);
        }

        public void AddClient(string clientId)
        {
            AddClient(clientId, false);
        }

        public void AddClient(string clientId, bool setDefault)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");
            AddClientInternal(ClientProfile.Load(clientId), setDefault);
        }

        public void AddClient(ClientProfile profile, bool setDefault)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");
            AddClientInternal(profile, setDefault);
        }

        private void AddClientInternal(ClientProfile profile, bool setDefault)
        {
            var cd = new ClientData()
            {
                Profile = profile,
                Status = ClientBO.GetClientStatus(profile.ClientId),
				Cart = new ShoppingCart( _managerId, profile.ClientId, profile.ClientGroup, profile.PersonalMarkup )
            };

            ManagerBO.AddClientToHandySet(_managerId, profile.ClientId, setDefault);
            _innerClients.Add(profile.ClientId, cd);
            if (setDefault)
                _defaultClientId = profile.ClientId;
        }

		public void RefreshClientInfo( string clientId )
		{
			ClientData cd = _innerClients[ clientId ];

			var profile = ClientProfile.Load( clientId );
			cd.Profile = profile;
			cd.Status = ClientBO.GetClientStatus( profile.ClientId );
		}

		public void SetDefaultClient( string clientId )
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");
            if (!_innerClients.ContainsKey(clientId))
                throw new BLException("Клиент отсутствует в оперативном список");

            ManagerBO.SetDefaultClientState(_managerId, clientId);

			_defaultClientId = clientId;
        }

        public bool IsClientDefault(string clientId)
        {
            return _defaultClientId == clientId;
        }

        public void RemoveClient(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId cannot be empty", "clientId");
            if (!_innerClients.ContainsKey(clientId))
                throw new BLException("Клиент отсутствует в оперативном список");

            ManagerBO.RemoveClientFromHandySet(_managerId, clientId);
            _innerClients.Remove(clientId);
            if (_defaultClientId == clientId)
                _defaultClientId = null;
        }

        public void Clear()
        {
            ManagerBO.ClearHandyClientSet(_managerId);
            _innerClients.Clear();
            _defaultClientId = null;
        }
	}
}
