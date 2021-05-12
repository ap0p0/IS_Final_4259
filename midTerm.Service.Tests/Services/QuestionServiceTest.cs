using AutoMapper;
using FluentAssertions;
using midTerm.Models.Models.Question;
using midTerm.Models.Profiles;
using midTerm.Services.Services;
using midTerm.Services.Tests.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Services.Tests.Service
{
   public class QuestionServiceTest
         : SqlLiteContext
    {
        private readonly IMapper _mapper;
        private readonly QuestionService _service;

        public QuestionServiceTest()
             : base(withData: true)
        {
            if (_mapper == null)
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(QuestionProfile));
                }).CreateMapper();
                _mapper = mapper;
            }

            _service = new QuestionService(DbContext, _mapper);

        }

        [Fact]
        public async Task GetQuestionById()
        {
            var expected = 1;
            var result = await _service.GetById(expected);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelExtended>();
            result.Id.Should().Be(expected);
        }

        [Fact]
        public async Task GetQuestions()
        {
            var expected = 1;

            var result = await _service.Get();

            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(expected);
            result.Should().BeAssignableTo<IEnumerable<QuestionModelBase>>();

        }

        [Fact]
        public async Task GetQuestionsFull()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.GetFull();

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(expected);
            result.Should().BeAssignableTo<IEnumerable<QuestionModelExtended>>();

        }

        [Fact]
        public async Task InsertNewQuestion()
        {
            var question = new QuestionCreateModel
            {
                Text = "New Question",
                Description = "This is a test for a new question"
            };
            var result = await _service.Insert(question);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelBase>();
            result.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task UpdateQuestion()
        {
            var question = new QuestionUpdateModel
            {
                Id = 1,
                Text = "Updating Questions",
                Description = "If you can see this, then the question is updated"
            };
            var result = await _service.Update(question);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelBase>();
            result.Id.Should().Be(question.Id);
            result.Text.Should().Be(question.Text);
            result.Description.Should().Be(question.Description);
        }
        [Fact]
        public async Task DeleteQuestion()
        {
            // Arrange
            var expected = 1;
            // Act
            var result = await _service.Delete(expected);
            var player = await _service.GetById(expected);


            // Assert
            result.Should().Be(true);
            player.Should().BeNull();

        }
    }
}
