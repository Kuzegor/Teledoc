using Teledock.Data;
using Teledock.Interface;
using Teledock.Models;

namespace Teledock.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;
        public ClientRepository(DataContext context)
        {
            _context = context;
        }
        public bool ClientExists(int clientId)
        {
            return _context.Clients.Any(c => c.Id == clientId);
        }

        public bool CreateClient(Client client)
        {
            _context.Add(client);
            return Save();
        }
        public bool CreateClient(Client client, int[] constitutorIds)
        {
            _context.Add(client);
            if (constitutorIds != null)
            {
                foreach (var id in constitutorIds)
                {
                    var constitutor = _context.Constitutors.Where(c => c.Id == id).FirstOrDefault();
                    ClientConstitutor clientConstitutor = new ClientConstitutor
                    {
                        ClientId = client.Id,
                        ConstitutorId = id,
                        Client = client,
                        Constitutor = constitutor
                    };
                    _context.Add(clientConstitutor);

                }
            }
            return Save();
        }

        public bool DeleteClient(Client client)
        {
            _context.Remove(client);
            return Save();
        }

        public Client GetClient(int clientId)
        {
            return _context.Clients.Where(c => c.Id == clientId).FirstOrDefault();
        }

        public ICollection<Client> GetClients()
        {
            return _context.Clients.ToList();
        }

        public ICollection<Constitutor> GetConstitutorsByClient(int clientId)
        {
            return _context.ClientConstitutors.Where(c => c.ClientId == clientId).Select(c => c.Constitutor).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateClient(Client client)
        {
            _context.Update(client);
            return Save();
        }
    }
}
