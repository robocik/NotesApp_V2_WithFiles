using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteBookApp.Logic.Handlers.Notes;
using NoteBookApp.Server.Infrastructure;
using NoteBookApp.Shared;

namespace NoteBookApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController: Controller
    {
        public NotesController(IMapper mapper, INotifierMediatorService mediator)
        {
            Mapper = mapper;
            Mediator = mediator;
        }

        private IMapper Mapper { get; }
        private INotifierMediatorService Mediator { get; }
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] GetNotesParam param)
        {
            var query = Mapper.Map<GetNotesQuery>(param);
            var retValue = await Mediator.Send(query).ConfigureAwait(false);
            return Ok(retValue);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteDetails(Guid id)
        {
            var query = new GetNoteDetailsQuery(id);
            var retValue = await Mediator.Send(query).ConfigureAwait(false);
            return Ok(retValue);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteParam? param)
        {
            if (param == null)
                return BadRequest();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = Mapper.Map<CreateNoteCommand>(param);

            await Mediator.Send(command).ConfigureAwait(false);
            return Created("note", null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteParam? param)
        {
            if (param == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = Mapper.Map<UpdateNoteCommand>(param);

            await Mediator.Send(command).ConfigureAwait(false);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid? id)
        {
            if (id == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new DeleteNoteCommand(id.Value);
            await Mediator.Send(command).ConfigureAwait(false);
            return Ok();
        }
    }
}