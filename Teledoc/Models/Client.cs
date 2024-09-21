using Teledock.Enums;

namespace Teledock.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? INN { get; set; }
        public string? ClientName { get; set; }
        public ClientType ClientType { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateOfUpdate { get; set; }
        public ICollection<ClientConstitutor>? ClientConstitutors { get; set; }
    }
}
