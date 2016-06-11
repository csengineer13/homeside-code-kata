using System;
using Bogus;

namespace CodeKata.Domain.Models.Seed
{
    public class UserSeed
    {
        public static Faker<User> Seed(CodeKataContext context)
        {
            Randomizer.Seed = new Random(3897234);

            var testUsers = new Faker<User>()
                .RuleFor(u => u.EmployeeId, f => f.Random.Replace("######"))
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName, "gohomeside.com"))
                .RuleFor(u => u.Phone, f => f.Random.Replace("##########"))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine("User Created! Id={0}", u.Id);
                });

            return testUsers;
        }
    }
}