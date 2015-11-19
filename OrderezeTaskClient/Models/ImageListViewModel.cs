using OrderezeTask;
using System.Collections.Generic;

namespace OrderezeTaskClient.Models
{
    public class ImageListViewModel
    {
        public ImageListViewModel()
        {
            UploadedImages = new List<Image>();
        }

        public List<Image> UploadedImages { get; set; }

        public ImageViewModel ImageToBeUploaded { get; set; }
    }
}