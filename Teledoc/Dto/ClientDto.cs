using Teledock.Enums;

namespace Teledoc.Dto
{
    public class ClientDto
    {
        public string? INN { get; set; }
        public string? ClientName { get; set; }
        public ClientType ClientType { get; set; }
    }
}
