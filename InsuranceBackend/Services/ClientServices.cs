using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using InsuranceBackend.Database;

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
            var res = _context.Clients.FirstOrDefault(c => c.ClientId == clientID);
            return res ?? throw new Exception();
        }

        public Client? GetClientById(int userId)
        {
           return _context.Clients.FirstOrDefault(c => c.UserId == userId);
        }

        public Client GetClientByName(string userName)
        {
            return _context.Clients.FirstOrDefault(c => c.ClientName == userName)
                ?? throw new DataMisalignedException(nameof(userName));
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

        public Client UpdateClient(int clientID, Client client)
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
            if (GetClientById(client.UserId) != null)
            {
                throw new Exception(null);
            }
            //var con = new SqlConnection(DBConnection.ConnectionString);
            //con.Open();
            //var cmd = new SqlCommand(
            //    "INSERT INTO Clients(userID,clientName,gender,dob,address,profilePic,phoneNum,email,status) VALUES('"
            //        + client.UserId
            //        + "','"
            //        + client.ClientName
            //        + "','"
            //        + client.Gender
            //        + "','Dob','Address','"
            //        + client.ProfilePic
            //        + "','"
            //        + client.PhoneNum
            //        + "','"
            //        + client.Email
            //        + "',0)",
            //    con
            //);
            //cmd.ExecuteNonQuery();
            //con.Close();
            _context.Clients.Add(client);
            _context.SaveChanges();
            return _context.Clients.OrderBy(c => c.ClientId).Last();
        }

        public IEnumerable<ClientPolicy> GetCPolicies(int clientID)
        {
            return _context.ClientPolicies.Where(cp => cp.ClientId == clientID).ToList();
        }

        public IEnumerable<Maturity> GetMaturities(int clientID)
        {
            var mps = new List<Maturity>();
            var cps = _context.ClientPolicies.Where(cp => cp.ClientId == clientID).ToList();
            foreach (var cp in cps)
            {
                var a = _context.Maturities.FirstOrDefault(
                    m => m.ClientPolicyId == cp.ClientPolicyId
                );
                if (a != null)
                {
                    mps.Add(a);
                }
            }
            return mps;
        }

        //approvals
        public void ChangeClientStatus(int _clientID, ActorStatusEnum e)
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
        public ClientPolicy AddClientPolicy(ClientPolicy clientPolicy)
        {
            ValidateClientPolicy(clientPolicy);
            var testcp = _context.ClientPolicies.FirstOrDefault(
                cp => cp.PolicyTermId == clientPolicy.PolicyTermId
            );
            if (testcp == null)
            {
                var con = new SqlConnection(DBConnection.ConnectionString);
                con.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO ClientPolicy(clientID,policyTermID,nomineeID,startDate,expDate,counter,status,referral,agentID) VALUES('"
                        + clientPolicy.ClientId
                        + "','"
                        + clientPolicy.PolicyTermId
                        + "','"
                        + clientPolicy.NomineeId
                        + "','"
                        + clientPolicy.StartDate
                        + "','"
                        + clientPolicy.ExpDate
                        + "','"
                        + clientPolicy.Counter
                        + "','"
                        + (int)StatusEnum.Inactive
                        + "','"
                        + clientPolicy.Referral
                        + "','"
                        + clientPolicy.AgentId
                        + "')",
                    con
                );
                cmd.ExecuteNonQuery();
                clientPolicy = _context.ClientPolicies.OrderBy(cp => cp.ClientPolicyId).Last();
                return clientPolicy;
            }
            return new ClientPolicy() { PolicyTermId = 0 };
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

        public IEnumerable<PolicyTerm> GetPterms(int policyId)
        {
            return _context.PolicyTerms.Where(pt => pt.PolicyId == policyId).ToList();
        }

        public Payment MakePayment(Payment payment, int penalty)
        {
            var dbcp = _context.ClientPolicies.First(
                cp => cp.ClientPolicyId == payment.ClientPolicyId
            );
            var dbpt = _context.PolicyTerms.First(pt => pt.PolicyTermId == dbcp.PolicyTermId);
            var dbp = _context.Policies.First(p => p.PolicyId == dbpt.PolicyId);
            var dbc = _context.Companies.First(c => c.CompanyId == dbp.CompanyId);
            var dbac = _context.AgentCompanies.First(ac => ac.CompanyId == dbp.CompanyId);
            var dba = _context.Agents.First(a => a.AgentId == dbcp.AgentId);
            Premium dbpen = new();
            if (penalty != 0)
                dbpen = _context.Premia.First(pen =>pen.ClientPolicyId == payment.ClientPolicyId&& pen.Status == PenaltyStatusEnum.Pending);
            if (
                dbcp.Counter == 0
                || dbcp.Status != ClientPolicyStatusEnum.Active
                || dbp.Status != StatusEnum.Active
                || dbc.Status != ActorStatusEnum.Approved
                || dba.Status != ActorStatusEnum.Approved)
                payment.Status = PaymentStatusEnum.Unsuccessful;
            else
            {
                if (dbpen.PremiumId != 0)
                {
                    payment.Status = PaymentStatusEnum.Successfull;
                    dbpen.Status = PenaltyStatusEnum.Paid;
                    _context.Premia.Update(dbpen);
                }
                else
                {
                    payment.Status = PaymentStatusEnum.Successfull;
                    dbcp.Counter -= 1;
                    _context.ClientPolicies.Update(dbcp);
                }
            }
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return _context.Payments.OrderBy(p => p.PaymentId).Last();
        }

        public AgentCompany ValidateReferral(string referral)
        {
            var res = _context.AgentCompanies.FirstOrDefault(ac => ac.Referral == referral);
            if (res != null)
            {
                return res;
            }
            else
                return new AgentCompany();
        }

        //Views

        public IEnumerable<Policy> GetPolicies(int typeId = 0, int order = 0, int agentId = 0)
        {
            List<Policy>? policies = new();
            if (agentId != 0)
            {
                var agentpolicies = new List<Policy>();
                var comps = _context.AgentCompanies
                    .Where(ac => ac.AgentId.Equals(agentId))
                    .Select(ac => ac.CompanyId)
                    .ToArray();
                foreach (var compid in comps)
                {
                    agentpolicies.Add(
                        (Policy)
                            _context.Policies.Where(
                                p => p.CompanyId.Equals(compid) && p.Status == StatusEnum.Active
                            )
                    );
                }
                policies = agentpolicies;
            }
            if (typeId != 0)
            {
                if (agentId != 0)
                {
                    policies = null;
                    var comps = _context.AgentCompanies
                        .Where(ac => ac.AgentId.Equals(agentId))
                        .Select(ac => ac.CompanyId)
                        .ToArray();
                    foreach (var compid in comps)
                    {
                        policies.Add(
                            (Policy)
                                _context.Policies.Where(
                                    p =>
                                        p.CompanyId.Equals(compid)
                                        && p.Status == StatusEnum.Active
                                        && p.PolicytypeId == typeId
                                )
                        );
                    }
                }
                else
                    policies = _context.Policies
                        .Where(p => p.PolicytypeId == typeId && p.Status == StatusEnum.Active)
                        .ToList();
            }
            if (order != 0)
            {
                policies.OrderByDescending(p => p.PolicyId);
            }
            if (policies.Count == 0)
            {
                policies = _context.Policies.Where(p => p.Status == StatusEnum.Active).ToList();
            }
            return policies;
        }

        public ClientPolicy GetClientPolicy(int clientpolicyId)
        {
            return _context.ClientPolicies.FirstOrDefault(
                cp => cp.ClientPolicyId == clientpolicyId
            );
        }

        public PolicyTerm GetPolicyTerm(int policytermId)
        {
            return _context.PolicyTerms.FirstOrDefault(pt => pt.PolicyTermId == policytermId);
        }

        public IEnumerable<PolicyType> GetTypes()
        {
            return _context.PolicyTypes.ToList();
        }

        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies.ToList();
        }

        //Nominees
        public void AddNominee(Nominee nominee)
        {
            ValidateNominee(nominee);
            var con = new SqlConnection(DBConnection.ConnectionString);
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Nominees(clientID,nomineeName,relation,address,phoneNum) VALUES('"
                    + nominee.ClientId
                    + "','"
                    + nominee.NomineeName
                    + "','"
                    + nominee.Relation
                    + "','"
                    + nominee.Address
                    + "','"
                    + nominee.PhoneNum
                    + "')",
                con
            );
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //Views
        public IEnumerable<Maturity> ViewMaturities(int clientID)
        {
            var maturities = new List<Maturity>();
            var dbpolicies = _context.ClientPolicies.Where(p => p.ClientId == clientID).ToList();
            foreach (var cpolicy in dbpolicies)
            {
                var dbm = _context.Maturities.FirstOrDefault(
                    m => m.ClientPolicyId == cpolicy.ClientPolicyId
                );
                if (dbm != null)
                    maturities.Add(dbm);
            }
            return maturities;
        }

        public IEnumerable<Premium> ViewPenalties(int clientpolicyId)
        {
            return _context.Premia.Where(p => p.ClientPolicyId == clientpolicyId).ToList();
        }

        public List<ClientDeath> ViewClientDeath(int clientID)
        {
            return _context.ClientDeaths
                .Include(c => c.ClientPolicyId)
                .Where(c => c.ClientPolicy.ClientId == clientID)
                .ToList();
        }

        public IEnumerable<Nominee> ViewClientNominees(int clientID)
        {
            ValidateClient(clientID);
            using (var con = new SqlConnection(DBConnection.ConnectionString))
            {
                var cmd = new SqlCommand($"SELECT * FROM Nominees WHERE ClientId={clientID}", con);
                var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                var nominees = dt.AsEnumerable()
                    .Select(
                        row =>
                            new Nominee
                            {
                                NomineeId = row.Field<int>("NomineeId"),
                                ClientId = row.Field<int>("ClientId"),
                                NomineeName = row.Field<string>("NomineeName"),
                                Relation = row.Field<string>("Relation"),
                                Address = row.Field<string>("Address"),
                                PhoneNum = row.Field<string>("PhoneNum")
                            }
                    )
                    .ToList();
                return nominees;
            }
        }

        //Validations
        private void ValidatePremium(Premium premium)
        {
            if (premium == null)
                throw new ArgumentNullException(nameof(premium));
            else if (
                _context.Premia.FirstOrDefault(f => f.ClientPolicyId == premium.ClientPolicyId)
                != null
            )
                throw new ArgumentException(null, nameof(premium));
        }

        private void ValidateMaturity(Maturity maturity)
        {
            if (maturity == null)
                throw new ArgumentNullException(nameof(maturity));
            else if (
                _context.Maturities.FirstOrDefault(f => f.ClientPolicyId == maturity.ClientPolicyId)
                != null
            )
                throw new ArgumentException(null, nameof(maturity));
        }

        private void ValidateClientDeath(ClientDeath clientDeath)
        {
            if (clientDeath == null)
                throw new ArgumentNullException(nameof(clientDeath));
            else if (
                _context.ClientDeaths.FirstOrDefault(
                    c => c.ClientPolicyId == clientDeath.ClientPolicyId
                ) != null
            )
                throw new ArgumentException(null, nameof(clientDeath));
        }

        private bool ValidateAgent(int agentID)
        {
            return _context.Agents.FirstOrDefault(a => a.AgentId == agentID) != null;
        }

        private void ValidateClientPolicy(ClientPolicy clientPolicy)
        {
            if (clientPolicy == null)
                throw new ArgumentNullException(nameof(clientPolicy));
            else if (!ValidateAgent(clientPolicy.AgentId))
                throw new ArgumentNullException(null, nameof(clientPolicy.AgentId));
            else if (!ValidateClient(clientPolicy.ClientId))
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
            var res = _context.Clients.FirstOrDefault(c => c.ClientId == clientId);
            if (res == null)
                return false;
            else
                return true;
        }
    }
}
