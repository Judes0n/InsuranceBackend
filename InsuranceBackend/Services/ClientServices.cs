using InsuranceBackend.Enum;
using InsuranceBackend.Models;

namespace InsuranceBackend.Services
{
    public class ClientServices
    {   
        InsuranceDbContext _context;

        public ClientServices()
        {
            _context = new InsuranceDbContext();
        }
        public Client GetClient(int clientID) 
        {
            var res = _context.Clients.Find(clientID);
            return res == null ? throw new Exception() : res;
        }
        public IEnumerable<Client> GetAllClient()
        { 
            return _context.Clients.ToList(); 
        }

        public void DeleteClient(int clientID) 
        {   
            var client = GetClient(clientID);
            _context.Clients.Remove(client);
        }

        public Client UpdateClient(int clientID,Client client)
        {
            if (GetClient(clientID) == null) 
            { 
                throw new Exception(); 
            }
            _context.Clients.Update(client);
            _context.SaveChanges();
            return GetClient(clientID);
        }
  
        public Client AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges(true);
            return GetClient(client.ClientId);
        }
        //approvals
        public void ApproveClient(int _clientID)
        {
            var dbclient = GetClient(_clientID);
            dbclient.Status = StatusEnum.Active;
            UpdateClient(_clientID, dbclient);
        }
        public void RejectClient(int _clientID)
        {
            var dbclient = GetClient(_clientID);
            dbclient.Status = StatusEnum.Inactive;
            UpdateClient(_clientID, dbclient);
        }
        public void BlockClient(int _clientID)
        {
            var dbclient = GetClient(_clientID);
            dbclient.Status = StatusEnum.Blocked;
            UpdateClient(_clientID, dbclient);
        }
        //ClientPolicies
        public IEnumerable<ClientPolicy> GetClientPolicies(int _clientID)
        {
           IEnumerable<ClientPolicy> clientpolicies = new List<ClientPolicy>();
           foreach (var _clientpolicy in _context.ClientPolicies)
                if(_clientpolicy.ClientId == _clientID)
                {
                    clientpolicies.Append(_clientpolicy);
                }
           return clientpolicies;
        }

    }
}
