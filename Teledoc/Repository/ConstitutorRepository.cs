using Teledock.Data;
using Teledock.Interface;
using Teledock.Models;

namespace Teledock.Repository
{
    public class ConstitutorRepository : IConstitutorRepository
    {
        private readonly DataContext _context;
        public ConstitutorRepository(DataContext context)
        {
            _context = context;
        }
        public bool ConstitutorExists(int constitutorId)
        {
            return _context.Constitutors.Any(c => c.Id == constitutorId);
        }

        public bool CreateConstitutor(Constitutor constitutor)
        {
            _context.Add(constitutor);
            return Save();
        }

        public bool CreateConstitutor(Constitutor constitutor, int[] clientIds)
        {
            _context.Add(constitutor);
            if (clientIds != null)
            {
                foreach (var id in clientIds)
                {
                    var client = _context.Clients.Where(c => c.Id == id).FirstOrDefault();
                    ClientConstitutor clientConstitutor = new ClientConstitutor
                    {
                        ClientId = id,
                        ConstitutorId = constitutor.Id,
                        Client = client,
                        Constitutor = constitutor
                    };
                    _context.Add(clientConstitutor);

                }
            }
            return Save();
        }

        public bool DeleteConstitutor(Constitutor constitutor)
        {
            _context.Remove(constitutor);
            return Save();
        }

        public ICollection<Client> GetClientsByConstitutor(int constitutorId)
        {
            return _context.ClientConstitutors.Where(c => c.ConstitutorId == constitutorId).Select(c => c.Client).ToList();
        }

        public Constitutor GetConstitutor(int constitutorId)
        {
            return _context.Constitutors.Where(c => c.Id == constitutorId).FirstOrDefault();
        }

        public ICollection<Constitutor> GetConstitutors()
        {
            return _context.Constitutors.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateConstitutor(Constitutor constitutor)
        {
            _context.Update(constitutor);
            return Save();
        }
    }
}
