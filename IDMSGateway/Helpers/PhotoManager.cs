using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CatCardGateway.Helpers
{
    public class PhotoManager
    {
        public string ApiKey { get { return this.ApiKey; } set {

                var apiURL = _config.GetSection("CustomSettings").GetSection("CatCardApiURL").Value;

                api = new CatCardAPI(new Uri(apiURL), value)
                {
                    ExistsPath = _config["CustomSettings:ApiExistsPath"],
                    PersonsPath = _config["CustomSettings:ApiPersonsPath"],
                    PhotosPath = _config["CustomSettings:ApiPhotosPath"]
                };
            }
        }

        private IConfiguration _config;
        private LocalStorage local;
        private CatCardAPI api;
        private int photosUpdateDaysLimit;
        

        public PhotoManager(IConfiguration config)
        {
            _config = config;

            photosUpdateDaysLimit = int.Parse(config["CustomSettings:PhotoUpdateDaysLimit"]);

            local = new LocalStorage()
            {
                PhotosPath = config["CustomSettings:PhotosPath"]
            };
        }

        public string GetPhoto(string id)
        {
            var PhotoName = $"{id}.jpg";
            var photoIsOld = false;
            var newerPhotoRemotely = false;
            var photoExistsRemotely = false;

            var photoExistsLocaly = local.PhotoExists(PhotoName);

            // Photo does not exists locally
            if (!photoExistsLocaly) photoExistsRemotely = api.PhotoExists(id);

            // If the photo does not exists locally and does not exists remotely
            if (!photoExistsLocaly && !photoExistsRemotely) return null;

            //If photo exists remotelly it means it does not exists locally
            if (!photoExistsLocaly && photoExistsRemotely) return DownloadAndReturnPicture(id);

            //Photo exists locally
            var photoInfo = local.GetLocalPhotoInfo(PhotoName);
            photoIsOld = photoInfo.LastWriteTime <= DateTime.Now.AddDays(photosUpdateDaysLimit * (-1));

            //Photo is not older than specified
            if (!photoIsOld)
                return Path.Combine(local.PhotosPath, PhotoName);
            
            //Photo is older than specified
            newerPhotoRemotely = api.GetPersonDetails(id).PhotoDate > photoInfo.LastWriteTime;

            //The remote picture is older
            if (!newerPhotoRemotely)
                return Path.Combine(local.PhotosPath, PhotoName);

            // The remote picture is newer and it exists remotely
            return  DownloadAndReturnPicture(id);

        }

        private string DownloadAndReturnPicture(string id)
        {
            var PhotoName = $"{id}.jpg";
            var photo = api.GetPhoto(id);
            local.SavePhoto(photo, PhotoName);
            return Path.Combine(local.PhotosPath, PhotoName);
        }
    }
}
