using Teledock.Models;

namespace Teledock.Interface
{
    public interface IConstitutorRepository
    {
        ICollection<Constitutor> GetConstitutors();
        Constitutor GetConstitutor(int constitutorId);
        ICollection<Client> GetClientsByConstitutor(int constitutorId);
        bool ConstitutorExists(int constitutorId);
        bool CreateConstitutor(Constitutor constitutor);
        bool CreateConstitutor(Constitutor constitutor, int[] clientIds);
        bool UpdateConstitutor(Constitutor constitutor);
        bool DeleteConstitutor(Constitutor constitutor);
        bool Save();
    }
}
