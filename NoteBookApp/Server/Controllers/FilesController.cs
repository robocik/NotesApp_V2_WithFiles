using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoteBookApp.Logic.Handlers.Files;
using NoteBookApp.Logic.Handlers.Files.DeleteFile;
using NoteBookApp.Server.Infrastructure;
using NoteBookApp.Shared;

namespace NoteBookApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController:Controller
    {
        public FilesController(IMapper mapper, INotifierMediatorService mediator)
        {
            Mapper = mapper;
            Mediator = mediator;
        }

        private IMapper Mapper { get; }
        private INotifierMediatorService Mediator { get; }

        [HttpPost("uploadAvatarFull")]
        public async Task<IActionResult> UploadAvatarFull([FromForm] UploadAvatarParameter uploadParam)
        {
            var fileName = ContentDispositionHeaderValue.Parse(uploadParam.File!.ContentDisposition).FileName!.Trim('"');
            var stream = uploadParam.File.OpenReadStream();
            var command = new UploadAvatarFullCommand(fileName, uploadParam.File!.Length, stream);
            var id = await Mediator.Send(command);
            return Created("file", id);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromBody] FileMetaData? uploadParam)
        {
            if (uploadParam == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = Mapper.Map<UploadFileCommand>(uploadParam);
            var fileAccessToken = await Mediator.Send(command);
            return Created("file", fileAccessToken);
        }

        [HttpGet()]
        public async Task<IActionResult> GetFiles([FromQuery] GetFilesParams param)
        {
            var query = Mapper.Map<GetFilesQuery>(param);
            var retValue = await Mediator.Send(query);
            return Ok(retValue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(Guid? id)
        {
            if (id == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new DeleteFileCommand(id.Value);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpGet("fileDirectUrl/{id}")]
        public async Task<IActionResult> GetFileDirectUrl(Guid id)
        {
            var query = new GetFileDirectUrlQuery(id);
            var retValue = await Mediator.Send(query);
            return Ok(retValue);
        }

        [HttpDelete("deleteAvatar")]
        public async Task<IActionResult> DeleteAvatar()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new DeleteAvatarCommand();
            await Mediator.Send(command);
            return Ok();
        }
    }
    

    public class UploadAvatarParameter
    {
        [FromForm]
        [Required]
        public IFormFile? File { get; set; }

        [FromJson]
        [Required]
        public UploadFileParam? Meta { get; set; }
    }
}