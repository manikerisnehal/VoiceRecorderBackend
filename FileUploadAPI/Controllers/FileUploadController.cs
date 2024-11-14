using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace FileUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string[] _allowedExtensions = { "ogg", "mp3", "wav" };

        private readonly string _uploadFolder = @"E:\Uploads";

        public FileUploadController()
        {
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile audio)
        {
            Console.WriteLine("request reached");
            if (audio == null || audio.Length == 0)
            {
                return BadRequest(new { error = "No file part" });
            }

            if (string.IsNullOrEmpty(audio.FileName))
            {
                return BadRequest(new { error = "No selected file" });
            }

            var fileExtension = Path.GetExtension(audio.FileName).ToLower().Substring(1); 
            if (!_allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { error = "Invalid file format" });
            }

            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(); 
            var filename = $"recording_{timestamp}.{fileExtension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), _uploadFolder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audio.CopyToAsync(stream);
            }

            return Ok(new { message = "File successfully uploaded", filename = filename });
        }
    }
}
