using FizzWare.NBuilder;
using WebApiPaging.Models;

namespace WebApiPaging.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApiPaging.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApiPaging.Models.ApplicationDbContext context)
        {
            // If no customers exist, let's create some dummy data
            if (!context.Customers.Any())
            {
                var customers = Builder<Customer>.CreateListOfSize(220)
                    .All()
                    .With(c => c.FirstName = Faker.Name.First())
                    .With(c => c.LastName = Faker.Name.Last())
                    .Build()
                    .ToArray();

                context.Customers.AddOrUpdate(c => c.Id,
                    customers);
            }

            // If no tweets exist, add them
            if (!context.Tweets.Any())
            {
                var createdDate = DateTime.Now.AddDays(-2);

                var tweets = Builder<Tweet>.CreateListOfSize(1000)
                    .All()
                    .With(tweet =>
                    {
                        createdDate = createdDate.AddSeconds(1);
                        tweet.CreatedDate = createdDate;

                        return tweet;
                    })
                    .With(tweet => tweet.Text = Faker.Lorem.Sentence())
                    .Build()
                    .ToArray();

                context.Tweets.AddOrUpdate(t => t.Id, tweets);
            }
        }
    }
}
