namespace NoteBookApp.Shared
{
    public class CreateNoteParam
    {
        public CreateNoteParam(string content)
        {
            Content = content;
        }

        public string Content { get; set; }
    }
}