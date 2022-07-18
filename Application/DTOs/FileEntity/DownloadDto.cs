using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.FileEntity;

public class DownloadDto : IMapFrom<Domain.BaseModels.FileEntity>
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string ContentType { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.BaseModels.FileEntity, DownloadDto>();
    }
}