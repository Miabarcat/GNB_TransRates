using AutoMapper;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Models;
using GNB_TransRates.DL.Repositories;
using GNB_TransRates.DL.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GNB_TransRates.Test.Services
{
    public class RatesServiceTest : IClassFixture<TestFixture<Program>>
    {
        private Mock<IBaseRepository<Rates>> Repository { get; }

        private IRatesService Service { get; }


        public RatesServiceTest(TestFixture<Program> fixture)
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


            Repository.Setup(x => x.Where(It.IsAny<Expression<Func<Rates, bool>>>()))
                .Returns((Expression<Func<Rates, bool>> exp) => entity.AsQueryable().Where(exp));


            Repository.Setup(x => x.Insert(It.IsAny<Rates>()))
                .Callback((Rates label) => entity.Add(label));

            Repository.Setup(x => x.Update(It.IsAny<Rates>()))
                .Callback((Rates label) => entity[entity.FindIndex(x => x.Id == label.Id)] = label);

            Repository.Setup(x => x.Delete(It.IsAny<Rates>()))
            .Callback((Rates label) => entity.RemoveAt(entity.FindIndex(x => x.Id == label.Id)));

            var mapper = (IMapper)fixture.Server.Host.Services.GetService(typeof(IMapper));
            var baseService = new BaseService<Rates>(Repository.Object);


            Service = new RatesService(baseService, mapper);
        }

        [Fact]
        public void Can_Get_All()
        {
            // Act
            var entities = Service.GetAsync().Result;
            // Assert
            Repository.Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(1, entities.Count());
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
            Assert.Equal("EUR", l.From);
        }

        [Fact]
        public void Can_Insert_Entity()
        {
            // Arrange
            var entity = new RatesResponseModel
            {
                From = "EUR",
                To = "CAD",
                Rate = 3
            };

            // Act
            Service.AddOrUpdate(entity);

            // Assert
            Repository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            Repository.Verify(x => x.Insert(It.IsAny<Rates>()), Times.Once);
            var entities = Service.GetAsync().Result;
            Assert.Equal(2, entities.Count());
        }

    }
}
