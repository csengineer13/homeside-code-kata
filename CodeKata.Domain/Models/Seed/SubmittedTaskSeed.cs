using System;
using System.Linq;
using Bogus;

namespace CodeKata.Domain.Models.Seed
{
    public class SubmittedTaskSeed
    {
        public static void Seed(CodeKataContext context, Faker<User> testUsers)
        {
            Randomizer.Seed = new Random(3897234);

            // CONFIG
            var numSubmittedTasksToGen = 500;
            
            var recentDay = DateTime.Now.AddDays(-2);

            // SUBMITTEDTASK RULES
            var testSubmittedTasks = new Faker<SubmittedTask>()
                .RuleFor(u => u.Name, f => f.Lorem.Sentence(3))
                .RuleFor(u => u.Description, f => f.Lorem.Sentence())
                .RuleFor(u => u.Type, f => f.PickRandom<TaskType>())
                .RuleFor(u => u.Status, f => f.PickRandom<TaskStatus>())
                .RuleFor(u => u.SubmitDateTime, f => f.Date.Recent(2))
                .RuleFor(u => u.SubmittedBy, f => testUsers.Generate())
                .RuleFor(u => u.QueuedDateTime, f => f.Date.Between(recentDay, recentDay).AddMinutes(55))
                .RuleFor(u => u.StartDateTime, f => f.Date.Between(recentDay.AddMinutes(55), recentDay).AddMinutes(120))
                .RuleFor(u => u.EndDateTime, f => f.Date.Between(recentDay.AddMinutes(120), recentDay.AddMinutes(600)))
                .RuleFor(u => u.LastUpdatedDateTime, (f, u) => u.EndDateTime)
                .RuleFor(u => u.LastUpdatedBy, (f, u) => u.SubmittedBy)
                .FinishWith((f, u) =>
                {
                    Console.WriteLine("Submitted Task Created! Id={0}", u.Id);
                });

            var cachedUser = testUsers.Generate();
            while (numSubmittedTasksToGen > 0)
            {
                var genNewUser = new Randomizer().Number(0, 10) > 3;
                var newTask = testSubmittedTasks.Generate();

                // > 1 job per user
                cachedUser = genNewUser ? newTask.SubmittedBy : cachedUser;
                newTask.SubmittedBy = cachedUser;
                newTask.LastUpdatedBy = cachedUser;

                // todo: check statuses match timestamps (queued should not have a finishedDateTime)

                context.SubmittedTasks.Add(newTask);
                numSubmittedTasksToGen--;
            }

        }
    }
}