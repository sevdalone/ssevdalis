using OrderezeTask;
using OrderezeTaskClient.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace OrderezeTaskClient.Controllers
{
    public class HomeController : Controller
    {
        IImagesService _imageService;

        public HomeController(IImagesService imageService)
        {
            _imageService = imageService;
        }

        public ActionResult Index()
        {
            return View(new ImageListViewModel() { UploadedImages = _imageService.GetImages() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ImageViewModel imageToBeUploaded)
        {
            if (ModelState.IsValid)
            {
                Image image = new Image();
                image.Description = imageToBeUploaded.Description;
                image.ImagePath = imageToBeUploaded.ImageUpload.FileName;
                image.Name = imageToBeUploaded.Name;
                _imageService.AddNewImage(image);
                return RedirectToAction("Index");
            }
            return View(new ImageListViewModel() { UploadedImages = _imageService.GetImages(), ImageToBeUploaded = imageToBeUploaded });
        }


        [HttpPost]
        public JsonResult DeleteImage(int? id)
        {
            bool deleted = false;
            try
            {
                if (id.HasValue)
                {
                    _imageService.DeleteImage(id.Value);
                    deleted = true;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                deleted = false;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            
            return deleted
                ? new JsonResult() { Data = string.Format("Image with id: {0} deleted succesfully", id) }
                : new JsonResult() { Data = "Image could not be deleted" };
        }
    }
}