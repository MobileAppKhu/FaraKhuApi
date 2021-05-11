using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Application.Features.File.Queries.Download
{
    public class DownloadViewModel 
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
    }
}