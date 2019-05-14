namespace DataLoaderQueryHandlerWrapper.Tests
{
    using System;
    using System.Threading.Tasks;

    using CQRS.Light.Core.Queries;
    
    using DataLoaderQueryHandlerWrapper;

    using GraphQL.DataLoader;

    using Moq;

    using Xunit;

    public class DataLoaderQueryHandlerWrapperTests
    {
        public DataLoaderQueryHandlerWrapperTests()
        {
            queryHandler = new Mock<IQueryHandler<IQuery, string>>();
            var dataLoaderContextAccessorMock = new Mock<IDataLoaderContextAccessor>();

            target = new DataLoaderQueryHandlerWrapper<IQuery, string>(
                loaderKey,
                queryHandler.Object,
                dataLoaderContextAccessorMock.Object);

            dataLoaderContext = new DataLoaderContext();
            dataLoaderContextAccessorMock.Setup(e => e.Context).Returns(dataLoaderContext);
        }
        private readonly DataLoaderQueryHandlerWrapper<IQuery, string> target;
        private readonly Mock<IQueryHandler<IQuery, string>> queryHandler;
        private readonly string loaderKey = Guid.NewGuid().ToString();

        private readonly DataLoaderContext dataLoaderContext;

        [Fact]
        public async Task AskAsyncTest()
        {
            var query = new Mock<IQuery>();

            var data = Guid.NewGuid().ToString();

            queryHandler
                .Setup(e => e.AskAsync(query.Object))
                .ReturnsAsync(data);

            var task = target.AskAsync(query.Object);

            await dataLoaderContext.DispatchAllAsync();
            var result = await task;

            Assert.Equal(data, result);
        }
    }
}
