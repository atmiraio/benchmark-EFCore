using BenchmarkDotNet.Attributes;
#if NET5_0
using EFCore5;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endif
#if NET6_0
using EFCore6;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endif
using System.Linq;

namespace BenchmarkEFCore
{
    [MedianColumn]
    [MemoryDiagnoser]
    [Config(typeof(SimpleEntityCrud))]
    public class SimpleEntityCrudBenchmark
    {
#if NET5_0
        private IDbContextFactory<EFCore5DbContext> _poolingFactory;

        private int _runCount5 = 0;
        private int _runCountReadId5 = 0;
        private int _runCountReadName5 = 0;
#endif
#if NET6_0
        private IDbContextFactory<EFCore6DbContext> _poolingFactory;

        private int _runCount6 = 0;
        private int _runCountReadId6 = 0;
        private int _runCountReadName6 = 0;
#endif

        [GlobalSetup]
        public void Setup()
        {
#if NET5_0
            var services5 = new ServiceCollection();

            services5.AddPooledDbContextFactory<EFCore5DbContext>(options => options
                .UseSqlServer(@"Server=.\SQLEXPRESS;Database=EFCore5;Trusted_Connection=True;")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            var serviceProvider = services5.BuildServiceProvider();
            _poolingFactory = serviceProvider.GetRequiredService<IDbContextFactory<EFCore5DbContext>>();
#endif
#if NET6_0
            var services6 = new ServiceCollection();

            services6.AddPooledDbContextFactory<EFCore6DbContext>(options => options
                .UseSqlServer(@"Server=.\SQLEXPRESS;Database=EFCore6;Trusted_Connection=True;")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableThreadSafetyChecks(false));
            var serviceProvider = services6.BuildServiceProvider();
            _poolingFactory = serviceProvider.GetRequiredService<IDbContextFactory<EFCore6DbContext>>();
#endif
        }

        [Benchmark]
        public void Create()
        {
            using var context = _poolingFactory.CreateDbContext();
#if NET5_0
            _runCount5++;
#endif
#if NET6_0

            _runCount6++;
#endif
            var newEntity = new SimpleEntity()
            {
#if NET5_0
                Name = $"Name-{_runCount5}",
#endif
#if NET6_0
                Name = $"Name-{_runCount6}",
#endif
                Description = "Description"
            };

            context.SimpleEntities.Add(newEntity);
            context.SaveChanges();

        }

        [Benchmark]
        public void Read()
        {
            using var context = _poolingFactory.CreateDbContext();
            var entity = context.SimpleEntities.FirstOrDefault();
        }

        [Benchmark]
        public void ReadById()
        {
            using var context = _poolingFactory.CreateDbContext();
#if NET5_0
            _runCountReadId5++;
            var entity = context.SimpleEntities.Where(e => e.Id == _runCountReadId5).FirstOrDefault();
#endif
#if NET6_0
            _runCountReadId6++;
            var entity = context.Entities.Where(e => e.Id == _runCountReadId6).FirstOrDefault();
#endif
        }

        [Benchmark]
        public void ReadByName()
        {
            using var context = _poolingFactory.CreateDbContext();
#if NET5_0
            _runCountReadName5++;
            var entity = context.SimpleEntities.Where(e => e.Name == $"Name-{_runCountReadName5}").FirstOrDefault();
#endif

#if NET6_0
            _runCountReadName6++;
            var entity = context.Entities.Where(e => e.Name == $"Name-{_runCountReadName6}").FirstOrDefault();
#endif
        }

        [Benchmark]
        public void ReadAll()
        {
            using var context = _poolingFactory.CreateDbContext();
            var list = context.SimpleEntities.ToList();
        }

        [Benchmark]
        public void Update()
        {
            using var context = _poolingFactory.CreateDbContext();
#if NET5_0
            var entityToUpdate = context.SimpleEntities.FirstOrDefault();
#endif
#if NET6_0
            var entityToUpdate = context.SimpleEntities.FirstOrDefault();
#endif
            if (entityToUpdate != null)
            {
                entityToUpdate.Name = "Name-Updated";
                entityToUpdate.Description = "Description-Updated";

                context.SaveChanges();
            }
        }

        [Benchmark]
        public void Delete()
        {
            using var context = _poolingFactory.CreateDbContext();
#if NET5_0
            var entityToDelete = context.SimpleEntities.FirstOrDefault();
#endif
#if NET6_0

            var entityToDelete = context.SimpleEntities.FirstOrDefault();
#endif
            if (entityToDelete != null)
            {
                context.SimpleEntities.Remove(entityToDelete);
                context.SaveChanges();
            }
        }
    }
}
