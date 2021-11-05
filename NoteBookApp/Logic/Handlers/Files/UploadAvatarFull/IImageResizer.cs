using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NoteBookApp.Logic.Handlers.Files
{
    public interface IImageResizer
    {
        Task<Stream> ResizeAsync(Stream stream, int width,int height);
    }

    public class ImageResizer : IImageResizer
    {
        public async Task<Stream> ResizeAsync(Stream stream,int width, int height)
        {
            using var image = await Image.LoadAsync(stream);
            image.Mutate(x => x.Resize(width, height));
            var memory = new MemoryStream();
            await image.SaveAsPngAsync(memory);
            memory.Position = 0;
            return memory;
        }
    }
}