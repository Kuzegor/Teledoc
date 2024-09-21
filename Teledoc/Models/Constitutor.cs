using Teledock.Enums;

namespace Teledock.Models
{
    public class Constitutor
    {
        public int Id { get; set; }
        public string? INN { get; set; }
        public string? ConstitutorName { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateOfUpdate { get; set; }
        public ICollection<ClientConstitutor>? ConstitutorClients { get; set; }

    }
}
