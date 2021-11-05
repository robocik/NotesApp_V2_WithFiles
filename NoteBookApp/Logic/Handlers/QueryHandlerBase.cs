using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers
{
    public abstract class QueryHandlerBase<T, TReturn> : IRequestHandler<T, TReturn> where T : IRequest<TReturn>
    {
        private readonly ISession _session;
        protected readonly IDateTimeProvider _dateTimeProvider;
        protected readonly IMapper _mapper;
        protected readonly SecurityInfo _securityInfo;
        protected IFileSystemProvider FileSystemProvider { get; }
        protected QueryHandlerBase(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileSystemProvider)
        {
            _session = session;
            _securityInfo = securityInfo;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            FileSystemProvider = fileSystemProvider;
        }
        
        protected abstract Task<TReturn> Execute(ISession session, T param);

        public async Task<TReturn> Handle(T request, CancellationToken cancellationToken = default)
        {
            
            using (var trans = _session.BeginTransaction())
            {
                var retValue = await Execute(_session, request).ConfigureAwait(false);
                if (!trans.WasRolledBack)
                {
                    await trans.CommitAsync(cancellationToken).ConfigureAwait(false);
                }

                return retValue;
            }
        }

        protected async Task<PagedResult<DTO>> ToPagedResults<Model, DTO>(IQueryOver<Model, Model> query, PartialRetrievingInfo retrievingInfo, Action<IList<Model>, DTO[]>? afterMap = null)
        {
            var rowCountQuery = query.ToRowCountQuery();
            int count;
            IList<Model> queryResult = null!;
            if (retrievingInfo.PageSize > PartialRetrievingInfo.AllElementsPageSize)
            {
                count = await rowCountQuery.SingleOrDefaultAsync<int>().ConfigureAwait(false);
                var pagedResults = query.Take(retrievingInfo.PageSize).Skip(retrievingInfo.PageIndex * retrievingInfo.PageSize);
                queryResult = await pagedResults.ListAsync().ConfigureAwait(false);
            }
            else
            {
                queryResult = await query.ListAsync().ConfigureAwait(false);
                count = queryResult.Count;
            }

            var list = _mapper.Map<DTO[]>(queryResult);
            afterMap?.Invoke(queryResult, list);
            var res = new PagedResult<DTO>(list, count, retrievingInfo.PageIndex, retrievingInfo.PageSize);
            return res;
        }
    }
}