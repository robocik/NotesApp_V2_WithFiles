using System;
using MediatR;

namespace NoteBookApp.Logic.Handlers.Files
{
    
    public class GetFileDirectUrlQuery : IRequest<string>
    {
        public GetFileDirectUrlQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}