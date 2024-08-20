using System;
using KnowledgeHubPortal.Infrastructure.Services;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the password for the admin user:");
        string password = Console.ReadLine();

        PasswordHasher.CreatePasswordHash(password, out string passwordHash, out string passwordSalt);

        Console.WriteLine($"PasswordHash: {passwordHash}");
        Console.WriteLine($"PasswordSalt: {passwordSalt}");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}