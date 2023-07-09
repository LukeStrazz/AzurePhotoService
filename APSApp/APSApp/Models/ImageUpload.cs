using System;
using System.ComponentModel.DataAnnotations;

namespace APSApp.Models
{
    public class ImageUpload
    {
        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public bool IsValidUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ImageUrl))
                {
                    return false;
                }

                Uri uriResult;
                bool isUrl = Uri.TryCreate(ImageUrl, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                return isUrl;
            }
        }
    }
}
