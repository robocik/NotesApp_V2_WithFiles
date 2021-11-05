using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Type;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Notes
{
    public class GetNotesQueryHandler : QueryHandlerBase<GetNotesQuery, PagedResult<NoteDto>>
    {

        protected override Task<PagedResult<NoteDto>> Execute(ISession session, GetNotesQuery param)
        {
            IQueryOver<Note, Note> query = session.QueryOver<Note>().Where(x=>x.User== _securityInfo.User);

            query = ApplySorting(param, query);

            return ToPagedResults<Note, NoteDto>(query, param);
        }

        private static IQueryOver<Note, Note> ApplySorting(GetNotesQuery param, IQueryOver<Note, Note> query)
        {
            IQueryOverOrderBuilder<Note, Note> orderQuery;
            switch (param.SortBy)
            {
                case NoteSort.Content:
                    orderQuery = query.OrderBy(x => x.Content);
                    break;
                case NoteSort.CreatedDateTime:
                    orderQuery = query.OrderBy(x => x.CreatedDateTime);
                    break;
                default:
                    orderQuery = query.OrderBy(x => x.Id);
                    break;
            }

            query = AppSortDirection(param, orderQuery);

            return query;
        }



        private static IQueryOver<Note, Note> AppSortDirection(GetNotesQuery param, IQueryOverOrderBuilder<Note, Note> orderQuery)
        {
            IQueryOver<Note, Note> query;
            if (param.SortAsc)
            {
                query = orderQuery.Asc;
            }
            else
            {
                query = orderQuery.Desc;
            }

            return query;
        }

        public GetNotesQueryHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService) : base(session, securityInfo, mapper, dateTimeProvider, fileService)
        {
        }

    }
}