using System;

namespace Domain.BaseModels;

public interface IBaseEntity
{
    public DateTime CreatedDate { get; set; }
        
    public DateTime LastModifiedDate { get; set; }

    public bool IsDeleted { get; set; }
}