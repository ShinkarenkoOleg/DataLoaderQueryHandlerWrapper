namespace DataLoaderQueryHandlerWrapper
{
    using System.Threading.Tasks;

    using CQRS.Light.Core.Queries;

    using GraphQL.DataLoader;

    public class DataLoaderQueryHandlerWrapper<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery
    {
        private readonly IDataLoaderContextAccessor accessor;
        private readonly IQueryHandler<TQuery, TResult> innerHandler;
        private readonly string loaderKey;

        public DataLoaderQueryHandlerWrapper(
            string loaderKey,
            IQueryHandler<TQuery, TResult> innerHandler,
            IDataLoaderContextAccessor accessor)
        {
            this.loaderKey = loaderKey;
            this.innerHandler = innerHandler;
            this.accessor = accessor;
        }

        public Task<TResult> AskAsync(TQuery query)
        {
            return accessor.Context.GetOrAddLoader(loaderKey, _ => innerHandler.AskAsync(query)).LoadAsync();
        }
    }
}
