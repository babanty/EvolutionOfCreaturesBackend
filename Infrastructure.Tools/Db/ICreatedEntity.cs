using System;

namespace Infrastructure.Tools.Db
{
    public interface ICreatedEntity
    {
        DateTime CreatedAt { get; }
    }
}
