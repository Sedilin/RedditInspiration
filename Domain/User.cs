using System.Text.Json.Serialization;

namespace Shared;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Type { get; set; }
}