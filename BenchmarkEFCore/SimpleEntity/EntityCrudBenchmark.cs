using BenchmarkDotNet.Attributes;
#if NET5_0
using EFCore5;
#endif
#if NET6_0
using EFCore6;
#endif
using System;
using System.Linq;

namespace BenchmarkEFCore
{
    //[SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 50)]
    [MedianColumn]
    [Config(typeof(EntityCrud))]
    public class EntityCrudBenchmark
    {
        private int _runCount5 = 0;
        private int _runCountReadId5 = 0;
        private int _runCountReadName5 = 0;
        private int _runCountUpdate5 = 0;
        private int _runCountDelete5 = 0;

        private int _runCount6 = 0;
        private int _runCountReadId6 = 0;
        private int _runCountReadName6 = 0;
        private int _runCountUpdate6 = 0;
        private int _runCountDelete6 = 0;

        [Benchmark]
        public void Create()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCount5++;
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCount6++;
#endif
                var newEntity = new Entity()
                {
#if NET5_0
                    Name = $"Name-{_runCount5}",
#endif
#if NET6_0
                    Name = $"Name-{_runCount6}",
#endif
                    Description = "Description",
                    Date = DateTime.UtcNow,
                    Property1 = 1,
                    Property2 = .1,
                    Property3 = new decimal(1),
                    IsActive = true
                };

                context.Entities.Add(newEntity);
                context.SaveChanges();
            }
        }

        [Benchmark]
        public void Read()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
#endif
                var book = context.Entities.FirstOrDefault();
            }
        }

        [Benchmark]
        public void ReadById()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCountReadId5++;
                var book = context.Entities.Where(e => e.Id == _runCountReadId5).FirstOrDefault();
            }
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountReadId6++;
                var book = context.Entities.Where(e => e.Id == _runCountReadId6).FirstOrDefault();
            }
#endif
        }

        [Benchmark]
        public void ReadByName()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCountReadId5++;
                var book = context.Entities.Where(e => e.Name == $"Name-{_runCountReadName5}").FirstOrDefault();
            }
#endif

#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountReadId6++;
                var book = context.Entities.Where(e => e.Name == $"Name-{_runCountReadName6}").FirstOrDefault();
            }
#endif
        }

        [Benchmark]
        public void Update()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCountUpdate5++;
                var entityToUpdate = context.Entities.Where(e => e.Id == _runCountUpdate5).FirstOrDefault();
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountUpdate6++;
                var entityToUpdate = context.Entities.Where(e => e.Id == _runCountUpdate6).FirstOrDefault();
#endif

                entityToUpdate.Name = "Name-Updated";
                entityToUpdate.Description = "Description-Updated";
                entityToUpdate.Date = DateTime.UtcNow;
                entityToUpdate.Property1 = 2;
                entityToUpdate.Property2 = .2;
                entityToUpdate.Property3 = new decimal(2);
                entityToUpdate.IsActive = false;

                context.SaveChanges();
            }
        }

        [Benchmark]
        public void Delete()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCountDelete5++;
                var entityToDelete = context.Entities.Where(e => e.Id == _runCountDelete5).FirstOrDefault();
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountDelete6++;
                var entityToDelete = context.Entities.Where(e => e.Id == _runCountDelete6).FirstOrDefault();
#endif

                context.Entities.Remove(entityToDelete);
                context.SaveChanges();
            }
        }
    }
}
