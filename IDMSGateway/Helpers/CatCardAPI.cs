using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CatCardGateway.Models;
using Newtonsoft.Json;

namespace CatCardGateway.Helpers
{
    public class CatCardAPI
    {
        private readonly Uri ApiUrl;
        private readonly string ApiKey;
        
        public string PhotosPath { get; set; }
        public string PersonsPath { get; set; }
        public string ExistsPath { get; set; }

        public CatCardAPI(Uri Url, String AccessKey)
        {
            this.ApiKey = AccessKey;
            this.ApiUrl = Url;
        }

        /// <summary>
        /// Verifies if the photo exists on the Cat Card Office
        /// </summary>
        /// <param name="id">The id of the person</param>
        /// <returns>a boolean value representing if the photo exists or not in the remote server</returns>
        public bool PhotoExists(string id)
        {
            var request = (HttpWebRequest)WebRequest.Create(GetPhotosUrl(ExistsPath, id));
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(lxResponse.GetResponseStream()))
                {
                    var js = new JsonSerializer();
                    var objText = reader.ReadToEnd();
                    return bool.Parse(objText);
                }
            }
            
        }

        /// <summary>
        /// Returns a person object containing all person info in the Cat Card Office
        /// </summary>
        /// <param name="id">The id of the person</param>
        /// <returns>A Person object</returns>
        public Person GetPersonDetails(string id)
        {
            var request = (HttpWebRequest)WebRequest.Create(GetPersonsUrl(PersonsPath));
            request.Method = "PUT";

            var sb = new StringBuilder();
            var sw = new StringWriter();

            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                jw.WriteStartObject();
                jw.WritePropertyName("ApiKey");
                jw.WriteValue(ApiKey);
                jw.WritePropertyName("Id");
                jw.WriteValue(id);
                jw.WritePropertyName("IncludePhoto");
                jw.WriteValue("false");
                jw.WritePropertyName("IncludeSignature");
                jw.WriteValue("false");
                jw.WriteEndObject();
            }

            var data = sw.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/atom+xml;type=entry";
            request.ContentLength = byteArray.Length;
            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(lxResponse.GetResponseStream()))
                {
                    var js = new JsonSerializer();
                    var objText = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Person>(objText);
                }
            }
        }

        /// <summary>
        /// Builds a URL to hit the Person API
        /// </summary>
        /// <param name="path">the API path</param>
        /// <returns></returns>
        private Uri GetPersonsUrl(string path)
        {
            UriBuilder url = new UriBuilder();
            url.Host = ApiUrl.Host;
            url.Scheme = ApiUrl.Scheme;
            url.Port = ApiUrl.Port;
            url.Path = $"{path}";
            return url.Uri;
        }

        /// <summary>
        /// Builds a URL to hit the Photos API
        /// </summary>
        /// <param name="path">The path on the CatCard Office to hit</param>
        /// <param name="id">The ID</param>
        /// <returns></returns>
        private Uri GetPhotosUrl(string path, string id)
        {
            UriBuilder url = new UriBuilder();
            url.Host = ApiUrl.Host;
            url.Scheme = ApiUrl.Scheme;
            url.Port = ApiUrl.Port;
            url.Path = $"{ApiKey}/{path}/{id}";
            return url.Uri;
        }

        /// <summary>
        /// Gets the picture from the Cat Card office
        /// </summary>
        /// <param name="id">The ID of the person to get the picture for, it can be EmplID, NetID or ISO number</param>
        /// <returns>A byte array containing the picture</returns>
        public byte[] GetPhoto(string id)
        {
            var request = (HttpWebRequest)WebRequest.Create(GetPhotosUrl(PhotosPath, id));
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    return reader.ReadBytes(1 * 1024 * 1024 * 10);
                }
            }
        }



    }
}
