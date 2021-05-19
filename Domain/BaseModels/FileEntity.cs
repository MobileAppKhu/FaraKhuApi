using System;
using Domain.Enum;
using Domain.Models;

namespace Domain.BaseModels
{
    public class FileEntity : BaseEntity
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string ContentType { set; get; }
        public long Size { set; get; }
        public FileType Type { get; set; }

    }
}