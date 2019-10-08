using CatCardGateway.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CatCardGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        IConfiguration _iconfiguration;

        public PhotoController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            return new EmptyResult();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string id, string ApiKey)
        {
            var photoManager = new PhotoManager(_iconfiguration) {
                 ApiKey = ApiKey
            };

            var photoPath = photoManager.GetPhoto(id);

            if(string.IsNullOrEmpty(photoPath)) return new EmptyResult();

            return PhysicalFile(photoPath, "image/jpeg");
        }

    }
}
