using System;
using Xunit;
using CatCardGateway.Helpers;

namespace CatCardGatewayUnitTest
{
    public class CatCardsGatewayTests
    {
        private readonly LocalStorage local = new LocalStorage() {
            PhotosPath = @"E:\Temp\Photos"
        };

        private readonly CatCardAPI api = 
            new CatCardAPI(new Uri("https://idmsapi.fso.arizona.edu"), "**************************")
        {
            ExistsPath = "photos/exists",
            PersonsPath = "persons",
            PhotosPath = "photos"
        };


        [Fact]
        public void LocalPhotoIsNewer()
        {
            local.SavePhoto(api.GetPhoto("ggracia"), "ggracia.jpg");
            var localPhoto = local.GetLocalPhotoInfo("ggracia.jpg");
            var remotePerson = api.GetPersonDetails("ggracia");

            Assert.True(remotePerson.PhotoDate < localPhoto.LastWriteTime);
        }

        [Fact]
        public void GetRemotePerson()
        {
            var person = api.GetPersonDetails("ggracia");
            Assert.NotNull(person.NetId = "ggracia");
        }

        [Fact]
        public void GetLocalPhoto()
        {
            var pic = api.GetPhoto("ggracia");
            local.SavePhoto(pic, "ggracia.jpg");
            var img = local.GetPhoto("ggracia.jpg");
            Assert.NotNull(img);
        }

        [Fact]
        public void GetRemotePhoto()
        {
            var picture = api.GetPhoto("22054916");
            Assert.NotEmpty(picture);
        }

        [Fact]
        public void SavePhoto()
        {
            var pic = api.GetPhoto("ggracia");
            local.SavePhoto(pic, "ggracia.jpg");
            Assert.True(local.PhotoExists("ggracia.jpg"));
        }

        [Fact]
        public void PhotoExistsRemotely()
        {
            Assert.True((!api.PhotoExists("99999999")) && api.PhotoExists("22054916"));
        }

        [Fact]
        public void PhotoExistsLocaly()
        {
            Assert.False(local.PhotoExists("WWW3434.jpg"));
        }
    }
}
