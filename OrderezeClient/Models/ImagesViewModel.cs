using OrderezeTask;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OrderezeClient.Models
{
    public class ImageListViewModel
    {
        public ImageListViewModel()
        {
            UploadedImages = new List<ImageViewModel>();
        }

        public List<ImageViewModel> UploadedImages { get; set; }

        public ImageViewModel ImageToBeUploaded { get; set; }
    }

    public class ImageViewModel
    {
        /// <summary>
        /// The Id of the entity
        /// </summary>
        public int Id { get; set; }

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
    }
}