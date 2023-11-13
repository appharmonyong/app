namespace Harmony.Bussiness.ViewModel
{
    public class UserVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}