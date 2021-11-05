using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;
using Expression = NHibernate.Criterion.Expression;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class GetFilesQueryHandler : QueryHandlerBase<GetFilesQuery, PagedResult<FileDto>>
    {
        public GetFilesQueryHandler(ISession session, SecurityInfo securityInfo, IMapper mapper,
            IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService) : base(session, securityInfo, mapper, dateTimeProvider,fileService)
        {
        }

        protected override async Task<PagedResult<FileDto>> Execute(ISession session, GetFilesQuery param)
        {
            ApplicationUser createdBy = null!;
            File file = null!;
            IQueryOver<File, File> query = session.QueryOver<File>(()=>file)
                .JoinAlias(x=>x.CreatedBy,()=> createdBy,JoinType.LeftOuterJoin)
                .Where(x =>!x.IsDeleted);

          
            if (!string.IsNullOrEmpty(param.SearchText))
            {
                var filenameCrit = Restrictions.Like(Projections.Property<File>(x => x.FileName), param.SearchText, MatchMode.Anywhere);

                var orCrit = Expression.Disjunction()
                    .Add(filenameCrit);
                query = query.Where(orCrit);
            }
            if (param.ObjectId.HasValue)
            {
                query = query.Where(x => x.ObjectId == param.ObjectId);
            }
            query = ApplySorting(param, query);

            return await ToPagedResults<File,FileDto>(query, param).ConfigureAwait(false);
        }


        

        private static IQueryOver<File, File> ApplySorting(GetFilesQuery param, IQueryOver<File, File> query)
        {
            IQueryOverOrderBuilder<File, File> orderQuery;
            switch (param.SortBy)
            {
                case FilesSortColumn.FileName:
                    orderQuery = query.OrderBy(x => x.FileName);
                    break;
                case FilesSortColumn.Length:
                    orderQuery = query.OrderBy(x => x.Length);
                    break;
                case FilesSortColumn.CreatedDateTime:
                    orderQuery = query.OrderBy(x => x.CreatedDateTime);
                    break;
                default:
                    orderQuery = query.OrderBy(x => x.FileName);
                    break;
            }

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
    }
}