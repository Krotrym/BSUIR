using Microsoft.EntityFrameworkCore;

class Program
{
    async static Task Main(string[] args)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            User user1 = new User { Name = "Tom", Age = 33 };
            User user2 = new User { Name = "Alice", Age = 26 };

            db.Users.Add(user1);
            db.Users.Add(user2);
            db.SaveChanges();

            var users = db.Users.ToList();
            Console.WriteLine("Список пользователей:");
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }
    }
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=helloapp.db");
        optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
    }
}
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}