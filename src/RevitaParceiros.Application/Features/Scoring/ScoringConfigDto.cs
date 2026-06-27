namespace RevitaParceiros.Application.Features.Scoring;

public class ScoringConfigDto
{
    public decimal PurchaseAmountPerPoint { get; set; }
    public int PointsGenerated { get; set; }
    public int PointsForRedemption { get; set; }
    public decimal RedemptionValue { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
