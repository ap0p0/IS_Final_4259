using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using midTerm.Data;
using midTerm.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midTerm.Services.Tests.Internal
{
    public abstract class SqlLiteContext
         : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly MidTermDbContext DbContext;


        protected SqlLiteContext(bool withData = false)
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            DbContext = new MidTermDbContext(CreateOptions());
            _connection.Open();
            DbContext.Database.EnsureCreated();
            if (withData)
                SeedData(DbContext);
        }
        private DbContextOptions<MidTermDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<MidTermDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseSqlite(_connection)
                .Options;
        }

        private void SeedData(MidTermDbContext dbContext)
        {


            var options = new List<Option>
            {
                new Option
                {
                    Id = 1,
                    Order = 1,
                    QuestionId = 1,
                    Text = "prva opcija"

                },
                new Option
                {
                    Id = 2,
                    Order = 2,
                    QuestionId = 1,
                    Text = "vtora opcija"
                },
                new Option
                {
                    Id = 3,
                    Order = 3,
                    QuestionId = 1,
                    Text = "treta opcija"
                },
                 new Option
                {
                    Id = 4,
                    Order = 4,
                    QuestionId = 1,
                    Text = "cetvrta opcija"
                }
            };

            var questions = new List<Question> {

                new Question
                {
                    Id = 1,
                    Text = "Prvo Prashanje",
                    Description = "Deskripcija za prvo prashanje"

                }

            };
            DbContext.AddRange(options);
            DbContext.AddRange(questions);
            DbContext.SaveChanges();
            //var answers = new List<Answers>
            //{
            //    new Answers
            //    {
            //        Id = 1,
            //        UserId =1,
            //        OptionId = 1,
            //    },
            //      new Answers
            //    {
            //        Id = 2,
            //        UserId =2,
            //        OptionId = 2
            //    },
            //        new Answers
            //    {
            //        Id = 3,
            //        UserId =3,
            //        OptionId = 3
            //    },
            //           new Answers
            //    {
            //        Id = 4,
            //        UserId =4,
            //        OptionId = 4
            //    }

            //};

            //var surveyusers = new List<SurveyUser>
            //{
            //    new SurveyUser
            //    {
            //        Id=1,
            //        FirstName ="Andrej",
            //        LastName = "Popovski",
            //        Country = "MKD",
            //        Gender = 0,
            //        DoB = new DateTime(2000, 06, 21)
            //    },
            //     new SurveyUser
            //    {
            //        Id=2,
            //        FirstName ="Vtor",
            //        LastName = "Korisnik",
            //        Country = "MKD",
            //        Gender = 0,
            //        DoB = new DateTime(1990, 06, 21)
            //    },
            //      new SurveyUser
            //    {
            //        Id=3,
            //        FirstName ="Tret",
            //        LastName = "Korisnik",
            //        Country = "MKD",
            //        Gender = 0,
            //        DoB = new DateTime(2000, 02, 11)
            //    },
            //       new SurveyUser
            //    {
            //        Id=4,
            //        FirstName ="Cetvrt",
            //        LastName = "Korisnik",
            //        Country = "MKD",
            //        Gender = 0,
            //        DoB = new DateTime(1992, 06, 21)
            //    }
            //};

        }




        public void Dispose()
        {
            _connection.Close();
            _connection?.Dispose();
            DbContext?.Dispose();
        }
    }
}
