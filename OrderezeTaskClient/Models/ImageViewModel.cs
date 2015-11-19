using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderezeTaskClient.Models
{
    public class ImageViewModel : IValidatableObject    
    {
        /// <summary>
        /// The Id of the entity
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The name of the image. It can be different than actual file name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the image
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The path the actual image is stored (normally the blob storage reference)
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validImageTypes = new string[]
           {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
           };

            List<ValidationResult> errors = new List<ValidationResult>();
            if (ImageUpload == null || ImageUpload.ContentLength == 0)
            {
                errors.Add(new ValidationResult("This field is required.", new List<string> { "ImageUpload" }));
            }
            else if (!validImageTypes.Contains(ImageUpload.ContentType))
            {
                errors.Add(new ValidationResult("Please choose either a GIF, JPG or PNG image.", new List<string> { "ImageUpload" }));
            }
            return errors;
        }
    }
}