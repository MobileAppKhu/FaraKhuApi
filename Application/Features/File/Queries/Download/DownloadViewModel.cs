using System.IO;
using Application.DTOs.FileEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Application.Features.File.Queries.Download
{
    public class DownloadViewModel 
    {
        public DownloadDto DownloadDto { get; set; }
    }
}