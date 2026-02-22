namespace FitMatch.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }


    public string? Info { get; set; }
    public string? ProfileUrl { get; set; }
    public required string Email { get; set; }
}