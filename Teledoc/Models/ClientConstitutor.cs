namespace Teledock.Models
{
    public class ClientConstitutor
    {
        public int ClientId { get; set; }
        public int ConstitutorId { get; set; }
        public Client? Client { get; set; }
        public Constitutor? Constitutor { get; set; }
    }
}
