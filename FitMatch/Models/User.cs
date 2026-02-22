namespace FitMatch.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? Info { get; set; }
    public string? ProfileUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}