using Humanizer;

namespace Models;

public class ProductModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsChecked { get; set; }
    public string? Type { get; set; }

    public decimal GetTotalPrice() => Price.HasValue ? Price.Value * Stock : 0m;

    public string? GetHumanizedName() =>  Name?.Humanize();

    public int GetTotalDiffTime()
    {
        if (UpdatedAt.HasValue)
        {
            var diff = DateTime.UtcNow - UpdatedAt.Value;
            return (int)diff.TotalDays;
        }
        return 0;
    }

    public int GetDaysLeft()
    {
        if (ExpirationDate is null) return 0;

        var today = DateTime.UtcNow.Date;
        int daysLeft = ((DateTime)ExpirationDate - today).Days;

        return daysLeft;
    }
}