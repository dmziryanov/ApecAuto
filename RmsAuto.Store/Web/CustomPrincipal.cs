using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    public class CustomPrincipal : GenericPrincipal
    {
        private int _userId;
        private string _acctgId;
        private SecurityRole _role;
        private string _internalFranchName;
        
        public CustomPrincipal(
            IIdentity identity,
            string[] roles,
            int userId,
            string acctgId,
            SecurityRole role, string internalFranchName)
            : base (identity, roles)
        {
            if (string.IsNullOrEmpty(acctgId))
                throw new ArgumentException("Accounting system ID cannot be empty", "acctgId");
            _userId = userId;
            _acctgId = acctgId;
            _role = role;
            _internalFranchName = internalFranchName;
        }

        public int UserId
        {
            get { return _userId; }
        }

        public string AcctgID
        {
            get { return _acctgId; }
        }

        public SecurityRole Role
        {
            get { return _role; }
        }

        public string InternalFranchName
        {
            get { return _internalFranchName; }
        }
    }
}
