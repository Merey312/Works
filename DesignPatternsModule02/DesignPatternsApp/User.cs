using System;

namespace DesignPatternsApp
{
    public class User
    {
        public string Name { get; set; }    
        public string Email { get; set; }

        public void SaveToDatabase()
        {
            Console.WriteLine($"Saving {Name} ({Email}) to DB...");
        }
    }
}

