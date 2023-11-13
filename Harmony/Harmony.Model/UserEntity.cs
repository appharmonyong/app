using Harmony.Persistence.Domain.Base;

namespace Harmony.Persistence.Domain
{
    public class UserEntity: BaseEntity
    {
      
        public string FirstName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string? UserName { get; set; }
        public string Email { get; set; } = string.Empty!;
        public string? Phone { get; set; }
        public string Hash { get; set; } = string.Empty!;
        public byte[] Salt { get; set; }
        public DateTime? BirthDay { get; set; }


        // TODO: Crear objeto para las direcciones, para que pueda tener mas de una. 
    }
}