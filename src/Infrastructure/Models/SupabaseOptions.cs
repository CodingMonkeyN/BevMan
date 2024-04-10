namespace BevMan.Infrastructure.Models;

public class SupabaseOptions
{
    public required string ProjectName { get; set; }
    public required string JwtSecret { get; set; }
}
