using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Xabe.FFmpeg;

namespace VideoTranscriberApp.BlazorUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public VideosController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(VideoUploadRequest request, CancellationToken cancellationToken)
        {
            // wwwroot/uploads
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

            // Create upload directory if it doesn't exist
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);
           
            var videoFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Video.FileName)}";

            // wwwroot/uploads/cdcasda2131.mp4
            var videoFilePath = Path.Combine(uploadsPath, videoFileName);

            await using var stream = new FileStream(videoFilePath, FileMode.Create);

            await request.Video.CopyToAsync(stream, cancellationToken);

            string audioFilePath = Path.ChangeExtension(videoFilePath, "mp3");

            FFmpeg.SetExecutablesPath("C:\\Users\\burak\\Downloads\\ffmpeg-master-latest-win64-gpl\\ffmpeg-master-latest-win64-gpl\\bin");

            var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoFilePath, audioFilePath);

            await conversion.Start(cancellationToken);

            Console.WriteLine("Conversion Completed");

            Console.WriteLine(audioFilePath);

            return Ok();
        }
    }

    public class VideoUploadRequest
    {
        public IFormFile Video { get; set; }
        public string[] Languages { get; set; }
    }
}