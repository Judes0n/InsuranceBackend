using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InsuranceBackend.Services
{
    public class ClientServices
    {
        readonly InsuranceDbContext _context;

        public ClientServices()
        {
            _context = new InsuranceDbContext();
        }
        public Client GetClient(int clientID) 
        {
            var res = _context.Clients.Find(clientID);
            return res ?? throw new Exception();
        }

        public Client GetClient(string userName)
        {
            return _context.Clients.Find(userName)?? throw new DataMisalignedException(nameof(userName));
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
        public void ChangeClientStatus(int _clientID,ActorStatusEnum e)
        {
            var dbclient = GetClient(_clientID);
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbclient.Status = e;
            UpdateClient(_clientID, dbclient);
        }
        //ClientPolicies && ClientDeath
        public void AddClientPolicy(ClientPolicy clientPolicy)
        {
            ValidateClientPolicy(clientPolicy);
            _context.ClientPolicies.Add(clientPolicy);
            _context.SaveChangesAsync();
        }

        public void AddClientDeath(ClientDeath clientDeath)
        {
            ValidateClientDeath(clientDeath);
            _context.ClientDeaths.Add(clientDeath);
            _context.SaveChangesAsync();    
        }

        public void AddPolicyMaturity(Maturity maturity)
        {   
            ValidateMaturity(maturity);
            _context.Maturities.Add(maturity);
            _context.SaveChangesAsync();  
        }

        public void AddPolicyPremium(Premium premium)
        {
            ValidatePremium(premium);
            _context.Premia.Add(premium);
            _context.SaveChangesAsync();
        }

        public void AddNominee(Nominee nominee)
        {
            _context.Nominees.Add(nominee);
            _context.SaveChangesAsync();
        }

        //Views
        public IEnumerable<Maturity> ViewMaturities(int clientID)
        {        
            IEnumerable<Maturity> maturities=new List<Maturity>();
            maturities = _context.Maturities.Include(m => m.ClientPolicyId).Where(m => m.ClientPolicy.ClientId == clientID).ToList();
            return maturities;
        }

        public IEnumerable<Premium> ViewPremia(int clientID)
        {
            IEnumerable<Premium> premia=new List<Premium>();
            premia = _context.Premia.Include(m => m.ClientPolicyId).Where(m => m.ClientPolicy.ClientId == clientID).ToList();
            return premia;
        }

        public List<ClientDeath> ViewClientDeath(int clientID)
        {
            return _context.ClientDeaths.Include(c => c.ClientPolicyId).Where(c => c.ClientPolicy.ClientId == clientID).ToList();
        }

        public IEnumerable<ClientPolicy> ViewClientPolicies(int clientID)
        {
            IEnumerable<ClientPolicy> clientpolicies = new List<ClientPolicy>();
            //foreach (var _clientpolicy in _context.ClientPolicies)
            //     if(_clientpolicy.ClientId == _clientID)
            //     {
            //         clientpolicies.Append(_clientpolicy);
            //     }
            clientpolicies = _context.ClientPolicies.ToList().Where(c => c.ClientId == clientID);
            return clientpolicies;
        }

        public IEnumerable<Nominee> ViewNominees(int clientID)
        {
           return _context.Nominees.Include(n=>n.ClientId == clientID).ToList();
        }       
        //Validations
        private void ValidatePremium(Premium premium)
        {
            if (premium == null)
                throw new ArgumentNullException(nameof(premium));
            else if(_context.Premia.FirstOrDefault(f=>f.ClientPolicyId==premium.ClientPolicyId) != null)
                throw new ArgumentException(null, nameof(premium));
        }

        private void ValidateMaturity(Maturity maturity)
        {
            if (maturity == null)
                throw new ArgumentNullException(nameof(maturity));
            else if (_context.Maturities.FirstOrDefault(f=>f.ClientPolicyId == maturity.ClientPolicyId) != null )
                throw new ArgumentException(null, nameof(maturity));
        }

        private void ValidateClientDeath(ClientDeath clientDeath)
        {
            if (clientDeath == null)
                throw new ArgumentNullException(nameof(clientDeath));
            else if (_context.ClientDeaths.FirstOrDefault(c => c.ClientPolicyId == clientDeath.ClientPolicyId) != null)
                throw new ArgumentException(null, nameof(clientDeath));
        }

        private bool ValidateAgent(int agentID)
        {
            return _context.Agents.FirstOrDefault(a => a.AgentId == agentID) != null;
        }

        private bool ValidateClient(int clientID)
        {
            return _context.Clients.FirstOrDefault(c => c.ClientId == clientID) != null; 
        }

        private void ValidateClientPolicy(ClientPolicy clientPolicy)
        {
            if(clientPolicy == null)
                throw new ArgumentNullException(nameof(clientPolicy));
            else if(!ValidateAgent(clientPolicy.AgentId))
                throw new ArgumentNullException(null, nameof(clientPolicy.AgentId));
            else if(!ValidateClient(clientPolicy.ClientId))
                throw new ArgumentNullException(null, nameof(clientPolicy.ClientId));
        }
        
    }
}
