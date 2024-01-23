namespace IdiomasLIB.Models;

public class Client
{
    private string? user;

    public string? User { get => user; set => user = value == "model" ? "Bard IA" : "Usuário"; }
    public string? Response { get; set; }

    public Client() { }

    public Client(string user, string response)
    {
        User = user;
        Response = response;
    }

    public override string ToString() => string.Format("[{0}]: \"{1}\"\n", user, Response);
}
