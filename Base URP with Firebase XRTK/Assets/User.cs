public class User
{
    public string email;
    public string username;
    public int creationDate;

    public User()
    {
    }

    public User(string email, string username, int creationDate)
    {
        this.email = email;
        this.username = username;
        this.creationDate = creationDate;
    }
}