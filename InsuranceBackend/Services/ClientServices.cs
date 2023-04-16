using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
            var res = _context.Clients.FirstOrDefault(c=>c.ClientId==clientID);
            return res ?? throw new Exception();
        }

        public Client GetClientById(int userId)
        {
            Client client = _context.Clients.FirstOrDefault(c=>c.UserId == userId) ?? throw new Exception();
            return client;
        }
        public Client GetClientByName(string userName)
        {
            return _context.Clients.FirstOrDefault(c=>c.ClientName==userName)?? throw new DataMisalignedException(nameof(userName));
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
            try
            {
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");
                //_context.Clients.Add(client);
                //_context.SaveChanges();
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");

                var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;");
                con.Open();
                var cmd = new SqlCommand("INSERT INTO Clients(userID,clientName,gender,dob,address,profilePic,phoneNum,email,status) VALUES('" + client.UserId + "','" + client.ClientName + "','" + client.Gender + "','Dob','Address','" + client.ProfilePic + "','" + client.PhoneNum + "','" + client.Email + "',0)", con);
                cmd.ExecuteNonQuery();
                con.Close();
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");
            }
            catch (Exception)
            {
                throw new Exception(null);
            }
            //var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            //con.Open();
            //var cmd = new SqlCommand("INSERT INTO Clients(userID,clientName,gender,dob,address,profilePic,phoneNum,email,status) VALUES('" + client.UserId + "','" + client.ClientName + "','Male','Dob','Address','" + client.ProfilePic + "','" + client.PhoneNum + "','" + client.Email + "',0)", con);
            //cmd.ExecuteNonQuery();
            return GetClientByName(client.ClientName);
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
            _context.SaveChanges();
        }

        public void AddClientDeath(ClientDeath clientDeath)
        {
            ValidateClientDeath(clientDeath);
            _context.ClientDeaths.Add(clientDeath);
            _context.SaveChanges();    
        }

        public void AddPolicyMaturity(Maturity maturity)
        {   
            ValidateMaturity(maturity);
            _context.Maturities.Add(maturity);
            _context.SaveChanges();  
        }

        public void AddPolicyPremium(Premium premium)
        {
            ValidatePremium(premium);
            _context.Premia.Add(premium);
            _context.SaveChanges();
        }

        public void AddNominee(Nominee nominee)
        {
            ValidateNominee(nominee);
            var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            con.Open();
            var cmd = new SqlCommand("INSERT INTO Nominees(clientID,nomineeName,relation,address,phoneNum) VALUES('" + nominee.ClientId + "','" + nominee.NomineeName + "','" + nominee.Relation + "','"+nominee.Address+"','" + nominee.PhoneNum + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();
            //_context.Nominees.Add(nominee);
            //_context.SaveChanges();
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
            clientpolicies = _context.ClientPolicies.Include(c => c.ClientId == clientID).ToList();
            return clientpolicies;
        }

        public IEnumerable<Nominee> ViewClientNominees(int clientID)
        {
            ValidateClient(clientID);
            using (var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                var cmd = new SqlCommand($"SELECT * FROM Nominees WHERE ClientId={clientID}", con);
                var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                var nominees = dt.AsEnumerable().Select(row =>
                    new Nominee
                    {
                        NomineeId = row.Field<int>("NomineeId"),
                        ClientId = row.Field<int>("ClientId"),
                        NomineeName = row.Field<string>("NomineeName"),
                        Relation = row.Field<string>("Relation"),
                        Address = row.Field<string>("Address"),
                        PhoneNum = row.Field <string> ("PhoneNum")
                    }).ToList();
                return nominees;
            }
           
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

        //private bool ValidateClient(int clientID)
        //{
        //    return _context.Clients.FirstOrDefault(c => c.ClientId == clientID) != null; 
        //}

        private void ValidateClientPolicy(ClientPolicy clientPolicy)
        {
            if(clientPolicy == null)
                throw new ArgumentNullException(nameof(clientPolicy));
            else if(!ValidateAgent(clientPolicy.AgentId))
                throw new ArgumentNullException(null, nameof(clientPolicy.AgentId));
            else if(!ValidateClient(clientPolicy.ClientId))
                throw new ArgumentNullException(null, nameof(clientPolicy.ClientId));
        }
        
        private void ValidateNominee(Nominee nominee)
        {
            if (nominee == null)
                throw new ArgumentNullException(nameof(nominee));
            else if (!ValidateClient(nominee.ClientId))
                throw new ArgumentException(null, nameof(nominee));
        }

        private bool ValidateClient(int clientId)
        {
            var res = _context.Clients.FirstOrDefault(c=>c.ClientId == clientId);
            if (res == null)
                return false;
            else return true;
        }
    }
}
