using Teledock.Models;

namespace Teledock.Interface
{
    public interface IClientRepository
    {
        ICollection<Client> GetClients();
        Client GetClient(int clientId);
        ICollection<Constitutor> GetConstitutorsByClient(int clientId);
        bool ClientExists(int clientId);
        bool CreateClient(Client client);
        bool CreateClient(Client client, int[] constitutorIds);
        bool UpdateClient(Client client);
        bool DeleteClient(Client client);
        bool Save();
    }
}
