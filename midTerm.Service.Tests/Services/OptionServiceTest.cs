using AutoMapper;
using FluentAssertions;
using midTerm.Models.Models.Option;
using midTerm.Models.Profiles;
using midTerm.Services.Services;
using midTerm.Services.Tests.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;



namespace midTerm.Services.Tests
{
    public class OptionServiceTest : SqlLiteContext

    {
        private readonly IMapper _mapper;
        private readonly OptionService _service;
        public OptionServiceTest()
            : base(withData: true)
        {
            if (_mapper == null)
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(OptionProfile));
                }).CreateMapper();
                _mapper = mapper;
            }

            _service = new OptionService(DbContext, _mapper);

        }

        [Fact]
        public async Task GetOptionById()
        {
            var expected = 1;
            var result = await _service.GetById(expected);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OptionModelExtended>();
            result.Id.Should().Be(expected);
        }


        [Fact]
        public async Task GetOptions()
        {
            var expected = 4;
            var result = await _service.Get();
            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(expected);
            result.Should().BeAssignableTo<IEnumerable<OptionBaseModel>>();

        }

        [Fact]
        public async Task GetOptionByQuestionId()
        {
            var result = await _service.GetByQuestionId(1);

            result.Should().BeAssignableTo<IEnumerable<OptionModelExtended>>();
            result.Should().NotBeNull().And.NotBeEmpty();

        }
        [Fact]
        public async Task InsertNewOption()
        {
            var option = new OptionCreateModel
            {
                Text = "New Option",
                Order = 6,
                QuestionId = 1
            };
            var result = await _service.Insert(option);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OptionBaseModel>();
            result.Id.Should().NotBe(0);
        }

        [Fact]
         public async Task UpdateOption()
        {
            var option = new OptionUpdateModel
            {
                Id = 1,
                Text = "is it Updated?",
                Order = 5,
                QuestionId = 1

            };
            var result = await _service.Update(option);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OptionBaseModel>();
            result.Id.Should().Be(option.Id);
            result.Order.Should().Be(option.Order);
            result.QuestionId.Should().Be(option.QuestionId);
        }

        [Fact]
        public async Task DeleteOption()
        {
            
            var expected = 1;
            var result = await _service.Delete(expected);
            var option = await _service.GetById(expected);

            result.Should().Be(true);
            option.Should().BeNull();

        }

    }
}

