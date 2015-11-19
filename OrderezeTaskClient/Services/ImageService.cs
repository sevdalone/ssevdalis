using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OrderezeTask;
using OrderezeTaskClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrderezeTaskClient.Services
{
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
                _container = blobClient.GetContainerReference("blobcontainer");
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
            if (image == null)
            {
                throw new ArgumentException("Image cannot be null");
            }

            // Create the db entry
            using (OrdezeTaskEntities context = new OrdezeTaskEntities())
            {
                context.Images.Add(image);
                context.SaveChanges();
            }

            // Create the blob with contents from a local file.
            using (var fileStream = File.OpenRead(image.ImagePath))
            {
                _container.GetBlockBlobReference(image.Id.ToString()).UploadFromStream(fileStream);
            }
            return image.Id;
        }

        void IImagesService.DeleteImage(int id)
        {
            Image image;
            // Get the db entry
            using (OrdezeTaskEntities context = new OrdezeTaskEntities())
            {
                image = context.Images.Find(id);

                CloudBlockBlob blob = _container.GetBlockBlobReference(id.ToString());
                blob.Delete();

                context.Images.Remove(image);
                context.SaveChanges();
            }
        }

        List<Image> IImagesService.GetImages()
        {
            string uri = _container.Uri.ToString();
            List<Image> images = new List<Image>();

            using (OrdezeTaskEntities context = new OrdezeTaskEntities())
            {
                List<string> paths = _container.ListBlobs().Select(x => x.Uri.ToString()).ToList();
                images = context.Images.ToList();
                images.ForEach(item => item.ImagePath = string.Format("{0}/{1}", uri, item.Id));
            }
            return images;
        }
    }
}