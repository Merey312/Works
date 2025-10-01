using System;
using System.Collections.Generic;

namespace UserManagement
{
    
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public User(string name, string email, string role)
        {
            Name = name;
            Email = email;
            Role = role;
        }

        public override string ToString()
        {
            return $"{Name} ({Email}) - {Role}";
        }
    }

    
    public class UserManager
    {
        private List<User> _users = new List<User>();

        public void AddUser(User user)
        {
            _users.Add(user);
            Console.WriteLine($"User {user.Name} added.");
        }

        public void RemoveUser(string email)
        {
            var user = FindUser(email);
            if (user != null)
            {
                _users.Remove(user);
                Console.WriteLine($"User {user.Name} removed.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public void UpdateUser(string email, string newName, string newRole)
        {
            var user = FindUser(email);
            if (user != null)
            {
                user.Name = newName;
                user.Role = newRole;
                Console.WriteLine($"User {email} updated.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

    
        private User FindUser(string email)
        {
            return _users.Find(u => u.Email == email);
        }

        public void PrintAllUsers()
        {
            Console.WriteLine("=== Users ===");
            foreach (var user in _users)
            {
                Console.WriteLine(user);
            }
        }
    }

   
    class Program
    {
        static void Main(string[] args)
        {
            UserManager manager = new UserManager();

            
            manager.AddUser(new User("Merey", "mereidosaibek@gmail.com", "Admin"));
            manager.AddUser(new User("Altynay", "altynay@gmail.com", "User"));

            manager.PrintAllUsers();

        
            manager.UpdateUser("altynay@gmail.com", "Altynay1", "Admin");
            manager.PrintAllUsers();

        
            manager.RemoveUser("mereidosaibek@gmail.com");
            manager.PrintAllUsers();
        }
    }
}
