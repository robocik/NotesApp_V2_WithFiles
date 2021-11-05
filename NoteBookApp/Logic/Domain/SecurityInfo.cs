namespace NoteBookApp.Logic.Domain
{
    public class SecurityInfo
    {
        private ApplicationUser? _user;

        public SecurityInfo(ApplicationUser? user)
        {
            _user = user;
        }

        public ApplicationUser User
        {
            get => _user!;
            set => _user = value;
        }
    }
}