using System;

namespace Infrastructure.Tools.Db
{
    public interface IUniqueIdentifiableEntity
    {
        Guid Id { get; }
    }
}
