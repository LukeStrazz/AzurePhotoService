using System.ComponentModel.DataAnnotations;
using System;

namespace APSApp.Models
{
    public class ImageUpload
    {
        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ImageUrl))
                {
                    Console.WriteLine("ImageUrl is null or white space");
                    return false;
                }

                Uri uriResult;
                bool isUrl = Uri.TryCreate(ImageUrl, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!isUrl)
                {
                    Console.WriteLine("Invalid URL: " + ImageUrl);
                    return false;
                }

                Console.WriteLine("URL is valid");
                return true;
            }
        }
    }
}
