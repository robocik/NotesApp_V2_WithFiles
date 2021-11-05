namespace NoteBookApp.Shared
{
    public class MyProfileDto
    {
        public MyProfileDto()
        {

        }
        public MyProfileDto(string id, string email)
        {
            Email = email;
            Id = id;
            DisplayName = email;
        }

        public string DisplayName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Id { get; set; } = null!;
    }
}
