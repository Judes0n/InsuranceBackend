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

    
    }
}
