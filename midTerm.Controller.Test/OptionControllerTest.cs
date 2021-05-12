using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using midTerm.Controllers;
using midTerm.Models.Models.Option;
using midTerm.Services.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Controller.Test
{
    public class OptionsControllerShould
    {
        private readonly Mock<IOptionService> _mockService;
        private readonly OptionsController _controller;

        public OptionsControllerShould()
        {
            _mockService = new Mock<IOptionService>();
            _controller = new OptionsController(_mockService.Object);
        }

        [Fact]
        public async Task ReturnExtendedOptionByIdWhenHasData()
        {
            int expectedId = 1;
            var Option = new Faker<OptionModelExtended>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(Option.Find(x => x.Id == expectedId));
            var result = await _controller.GetById(expectedId);

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<OptionModelExtended>().Subject.Id.Should().Be(expectedId);
        }

        [Fact]
        public async Task ReturnOptionWhenHasData()
        {
            int expectedId = 10;
            var option = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate(10);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(option);
            var result = await _controller.Get();

            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<List<OptionBaseModel>>().Subject.Count().Should().Be(expectedId);
        }
        [Fact]
        public async Task ReturnBadRequestOnCreateWhenModelNotValid()
        {
            _controller.ModelState.AddModelError("If you see this there is an error", "There is an error... :'(");
            var option = new Faker<OptionCreateModel>()
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(expected);

            var result = await _controller.Post(option);

            result.Should().BeOfType<BadRequestResult>();


        }

        [Fact]
        public async Task ReturnConflictOnCreateWhenRepositoryError()
        {
            var option = new Faker<OptionCreateModel>()
               .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(() => null);
            var result = await _controller.Post(option);

            result.Should().BeOfType<ConflictResult>();


        }

     

        [Fact]
        public async Task ReturnBadRequestOnUpdateWhenModelNotValid()
        {
            _controller.ModelState.AddModelError("If you see this there is an error", "There is an error... :'(");
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(expected);

            var result = await _controller.Put(option.Id, option);

            result.Should().BeOfType<BadRequestResult>();


        }
        [Fact]
        public async Task ReturnNoContentOnUpdateWhenRepositoryError()
        {
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(() => null);
            var result = await _controller.Put(option.Id, option);

            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task ReturnOptionOnUpdateWhenCorrectModel()
        {
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(expected);
            var result = await _controller.Put(option.Id, option);

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
        public async Task ReturnBadRequestWhenModelNotValid()
        {
            _controller.ModelState.AddModelError("If you see this there is an error", "There is an error... :'(");

            var result = await _controller.Delete(1);

            result.Should().BeOfType<BadRequestResult>();

            var model = result as BadRequestResult;


        }
    }
}