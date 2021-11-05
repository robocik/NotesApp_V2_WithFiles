namespace NoteBookApp.Logic.Domain
{
    public record FileIdentifier
    {
        public FileIdentifier(string container, string file)
        {
            Container = container;
            File = file;
        }

        public string File { get; }

        public string Container { get; }

    }
}