namespace RevitaParceiros.API.AuthContext
{
    public class UserContext
    {
        public Guid UserId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? ClientId { get; set; }
    }
}
