using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace APSApp.Models;

public class ImageUpload
{
    private static readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    [Required]
    [Display(Name = "Upload Image")]
    public IFormFile ImageFile { get; set; }

    public bool IsValid
    {
        get
        {
            if (ImageFile == null) return false;

            var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext)) return false;

            if (ImageFile.Length > MaxFileSize) return false;

            return true;
        }
    }
}

