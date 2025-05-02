public class Account
{
    public string Username { get; set; }
    public string Password { get; set; }


    // Parameterless constructor for deserialization or initialization.

    public Account() { }

    public Account(string username, string password)
    {
        Username = username;
        Password = password;
    }
}