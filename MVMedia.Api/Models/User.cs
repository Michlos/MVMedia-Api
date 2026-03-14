namespace MVMedia.Api.Models;

public class User
{

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Login { get; private set; }
    //public string Password { get; set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }

    public int CompanyId { get; set; }
    //public  virtual Company Company { get; set; }

    public User(int id, string name, string login, string email)
    {
        Id = id;
        Name = name;
        Login = login;
        Email = email;
        IsAdmin = false;
    }

    public User(string name, string login, string email)
    {
        Name = name;
        Login = login;
        Email = email;
    }

    public void SetPassword(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void SetAdmin(bool isAdmin)
    {
        IsAdmin = isAdmin;
    }

    //public void SetPassword(string password)
    //{
    //    //using (var hmac = new System.Security.Cryptography.HMACSHA512())
    //    //{
    //    //    PasswordSalt = hmac.Key;
    //    //    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    //    //}
    //}

}
