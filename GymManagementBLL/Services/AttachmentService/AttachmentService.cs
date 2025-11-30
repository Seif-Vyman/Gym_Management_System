using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly string[] allowedExtensions = {"jpg", "jpeg", "png"};
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5 MB

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public bool Delete(string folderName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(fileName)) return false;
                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
                var filePath = Path.Combine(folderPath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to delete file from floder = {folderName} : {ex}");
                return false;
            }
        }

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName) || file is null || file.Length == 0) return null;
                if (file.Length > maxFileSize) return null;
                var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                if (!allowedExtensions.Contains(extension)) return null;
                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var uniqueFileName = $"{Guid.NewGuid().ToString()}.{extension}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using FileStream? fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                return uniqueFileName;
            }
            catch (Exception ex){
                Console.WriteLine($"failed to upload file to floder = {folderName} : {ex}");
                return null;
            }

        }
    }
}
