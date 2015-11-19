using OrderezeTask;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using System.IO;
using OrderezeClient.Models;

namespace OrderezeClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ImageListViewModel());
        }
    }

    public class ImageService : IImagesService
    {
        CloudBlobContainer _container;

        public ImageService()
        {
            CloudStorageAccount storageAccount;
            // Retrieve storage account from connection string.
            if (CloudStorageAccount.TryParse(RoleEnvironment.GetConfigurationSettingValue("ImageStorageAccountConnection"), out storageAccount))
            {
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container.
                _container = blobClient.GetContainerReference("blobContainer");
                if (_container.CreateIfNotExists())
                {
                    _container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }
            }
            else
            {
                throw new ApplicationException("Could not find ImageStorageAccountConnection setting");
            }
        }

        int IImagesService.AddNewImage(Image image)
        {
            // Create or overwrite the blob with contents from a local file.
            using (var fileStream = File.OpenRead(image.ImagePath))
            {
                _container.GetBlockBlobReference(image.Name).UploadFromStream(fileStream);
            }
            return 0;
        }

        void IImagesService.DeleteImage(int id)
        {
            throw new NotImplementedException();
        }

        List<Image> IImagesService.GetImages()
        {
            List<IListBlobItem> blobs = _container.ListBlobs().ToList();
            foreach (var blob in blobs)
            {
                string path = blob.Uri.ToString();
            }

            return new List<Image>();
        }
    }
}