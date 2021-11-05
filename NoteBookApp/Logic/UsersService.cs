using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.AspNetCore.Identity;
using NoteBookApp.Logic.Domain;

namespace NoteBookApp.Logic
{
    public class UsersService : UserOnlyStore<ApplicationUser>
    {
        private readonly ISession _session;

        public UsersService(ISession session, IdentityErrorDescriber describer) : base(session, describer)
        {
            _session = session;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.CreateAsync(user, cancellationToken).ConfigureAwait(false);
        }
    }
}