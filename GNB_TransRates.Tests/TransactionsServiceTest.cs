using FluentAssertions;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Repositories;
using GNB_TransRates.DL.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GNB_TransTransactions.Tests
{
    public class TransactionsServiceTest
    {

        private Mock<IBaseRepository<Transactions>> Repository { get; }

        private IBaseService<Transactions> Service { get; }


        public TransactionsServiceTest()
        {
            var entity = new List<Transactions>
            {
                new Transactions
                {
                    Id = 1,
                    Sku = "A1234",
                    Currency = "USD",
                    amount = 2
                },
                new Transactions
                {
                    Id = 2,
                    Sku = "A4321",
                    Currency = "EUR",
                    amount = (decimal)3.5
                }
            };

            Repository = new Mock<IBaseRepository<Transactions>>();

            Repository.Setup(x => x.GetAll())
                .ReturnsAsync(entity);

            Repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => Task.Run(() => entity.Find(s => s.Id == id)));


            Repository.Setup(x => x.Where(It.IsAny<Expression<Func<Transactions, bool>>>()))
                .Returns((Expression<Func<Transactions, bool>> exp) => entity.AsQueryable().Where(exp));


            Repository.Setup(x => x.Insert(It.IsAny<Transactions>()))
                .Callback((Transactions label) => entity.Add(label));

            Repository.Setup(x => x.Update(It.IsAny<Transactions>()))
                .Callback((Transactions label) => entity[entity.FindIndex(x => x.Id == label.Id)] = label);

            Repository.Setup(x => x.Delete(It.IsAny<Transactions>()))
            .Callback((Transactions label) => entity.RemoveAt(entity.FindIndex(x => x.Id == label.Id)));


            Service = new BaseService<Transactions>(Repository.Object);
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
            l.Currency.Should().Be("USD");
            l.Sku.Should().Be("A1234");
        }

        [Fact]
        public void Can_Insert_Entity()
        {
            // Arrange
            var entity = new Transactions
            {
                Id = 3,
                Sku = "A4321",
                Currency = "EUR",
                amount = (decimal)3.5
            };

            // Act
            Service.AddOrUpdate(entity);


            // Assert
            Repository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            Repository.Verify(x => x.Insert(It.IsAny<Transactions>()), Times.Once);
            var entities = Service.GetAsync().Result;
            entities.Should().NotBeNull();
            entities.Should().HaveCount(3);
        }

    }
}