using FluentAssertions;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Repositories;
using GNB_TransRates.DL.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GNB_TransRates.Tests
{
    public class RatesServiceTest
    {

        private Mock<IBaseRepository<Rates>> Repository { get; }

        private IBaseService<Rates> Service { get; }


        public RatesServiceTest()
        {
            var entity = new List<Rates>
            {
                new Rates
                {
                    Id = 1,
                    FromCurr = "EUR",
                    ToCurr = "USD",
                    Amount = 2
                },
                new Rates
                {
                    Id = 2,
                    FromCurr = "USD",
                    ToCurr = "EUR",
                    Amount = (decimal)0.5
                }
            };

            Repository = new Mock<IBaseRepository<Rates>>();

            Repository.Setup(x => x.GetAll())
                .ReturnsAsync(entity);

            Repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => Task.Run(() => entity.Find(s => s.Id == id)));

            Repository.Setup(x => x.Insert(It.IsAny<Rates>()))
                .Callback((Rates label) => entity.Add(label));

            Service = new BaseService<Rates>(Repository.Object);
        }

        [Fact]
        public void Can_Get_All()
        {
            // Act
            var entities = Service.GetAsync().Result;
            // Assert
            Repository.Verify(x => x.GetAll(), Times.Once);            
            entities.Should().NotBeNull();
            entities.Should().HaveCount(2);

        }

        [Fact]
        public void Can_Get_Single()
        {
            // Arrange
            var testId = 1;

            // Act
            var l = Service.GetById(testId).Result;

            // Assert
            Repository.Verify(x => x.GetById(testId), Times.Once);
            l.FromCurr.Should().Be("EUR");
            l.ToCurr.Should().Be("USD");
        }

        [Fact]
        public void Can_Insert_Entity()
        {
            // Arrange
            var entity = new Rates
            {
                Id = 3,
                FromCurr = "EUR",
                ToCurr = "CAD",
                Amount = (decimal)0.75
            };

            // Act
            Service.AddOrUpdate(entity);


            // Assert
            Repository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            Repository.Verify(x => x.Insert(It.IsAny<Rates>()), Times.Once);

            var entities = Service.GetAsync().Result;
            entities.Should().NotBeNull();
            entities.Should().HaveCount(3);
        }

    }
}