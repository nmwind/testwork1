namespace TestWork.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? MiddleName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User(Guid id, string firstName, string lastName, string? middleName, string email, string passwordHash, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static User Create(string firstName, string lastName, string? middleName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName)) 
            throw new ArgumentException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName)) 
            throw new ArgumentException("Last name is required");
        if (string.IsNullOrWhiteSpace(email)) 
            throw new ArgumentException("Email is required");

        DateTime created = DateTime.UtcNow;
        return new User(Guid.NewGuid(), firstName, lastName, middleName, email, passwordHash, created, created);
    }

    public void Update(string firstName, string lastName, string? middleName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) 
            throw new ArgumentException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName)) 
            throw new ArgumentException("Last name is required");

        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) 
            throw new ArgumentException("Password is required");

        PasswordHash = password;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public bool ValidatePassword(string password)
    {
        return PasswordHash == password;
    }
}
