using Microsoft.EntityFrameworkCore;
using System;

namespace EvolutionOfCreatures.Logic.Tests.TestTools
{
    /// <summary> for to use "using" </summary>
    public class MsSqlDbContextWrapper<TDbContext> : IDisposable
                          where TDbContext : DbContext
    {
        public TDbContext Instance { get; set; }

        public void Dispose() => Instance.DisposeAndDeleteMsSqlDb();
    }
}
