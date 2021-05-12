using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using midTerm.Controllers;
using midTerm.Models.Models.Question;
using midTerm.Services.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Controller.Test
{
    public class QuestionsControllerShould
    {

        private readonly Mock<IQuestionService> _mockService;
        private readonly QuestionsController _controller;

        public QuestionsControllerShould()
        {
            _mockService = new Mock<IQuestionService>();
            _controller = new QuestionsController(_mockService.Object);
        }

        [Fact]
        public async Task ReturnExtendedQuestionByIdWhenHasData()
        {
            int expectedId = 1;
            var question = new Faker<QuestionModelExtended>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(question.Find(x => x.Id == expectedId));
            var result = await _controller.GetById(expectedId);

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<QuestionModelExtended>().Subject.Id.Should().Be(expectedId);
        }

        [Fact]
        public async Task ReturnQuestionWhenHasData()
        {
            int expectedId = 10;
            var question = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(10);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(question);
            var result = await _controller.Get();

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<List<QuestionModelBase>>().Subject.Count().Should().Be(expectedId);
        }


        [Fact]
        public async Task ReturnEmptyListWhenNoData()
        {

            var question = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(0);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(question);
            var result = await _controller.Get();

            result.Should().BeOfType<NoContentResult>();



        }

        [Fact]
        public async Task ReturnNoContentWhenHasNoData()
        {
            int expectedId = 1;
            var question = new Faker<QuestionModelExtended>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(question.Find(x => x.Id == expectedId));
            var result = await _controller.GetById(expectedId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnBadRequestOnCreateWhenModelNotValid()
        {
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var question = new Faker<QuestionCreateModel>()
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(expected);
            var result = await _controller.Post(question);

            result.Should().BeOfType<BadRequestResult>();


        }


        [Fact]
        public async Task ReturnBadRequestOnUpdateWhenModelNotValid()
        {
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(expected);
            var result = await _controller.Put(question.Id, question);

            result.Should().BeOfType<BadRequestResult>();


        }

        [Fact]
        public async Task ReturnQuestionOnUpdateWhenCorrectModel()
        {
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(expected);
            var result = await _controller.Put(question.Id, question);

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);
        }

        [Fact]
        public async Task ReturnOkWhenDeletedData()
        {
            int id = 1;
            bool expected = true;
            _mockService.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(expected);
            var result = await _controller.Delete(id);

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);

        }

        [Fact]
        public async Task ReturnOkFalseWhenNoDataToDelete()
        {
            int id = 1;
            bool expected = false;
            _mockService.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(expected);
            var result = await _controller.Delete(id);

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);

        }
    }
}

