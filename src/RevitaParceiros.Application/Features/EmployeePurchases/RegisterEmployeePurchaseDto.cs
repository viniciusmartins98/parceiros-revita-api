namespace RevitaParceiros.Application.Features.EmployeePurchases
{
    public class RegisterEmployeePurchaseDto
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
