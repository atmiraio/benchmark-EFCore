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
    [Config(typeof(EntityBulkCrud))]
    public class EntityBulkCrudBenchmark
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
                var newSimplentity = new SimpleEntity()
                {
#if NET5_0
                    Name = $"Name-{_runCount5}",
#endif
#if NET6_0
                    Name = $"Name-{_runCount6}",
#endif
                    Description = "Description",
                };

                context.SimpleEntities.Add(newSimplentity);
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
                var simpleEntity = context.SimpleEntities.FirstOrDefault();
            }
        }

        [Benchmark]
        public void ReadById()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCountReadId5++;
                var simpleEntity = context.SimpleEntities.Where(e => e.Id == _runCountReadId5).FirstOrDefault();
            }
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountReadId6++;
                var simpleEntity = context.SimpleEntities.Where(e => e.Id == _runCountReadId6).FirstOrDefault();
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
                var simpleEntity = context.SimpleEntities.Where(e => e.Name == $"Name-{_runCountReadName5}").FirstOrDefault();
            }
#endif

#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountReadId6++;
                var simpleEntity = context.SimpleEntities.Where(e => e.Name == $"Name-{_runCountReadName6}").FirstOrDefault();
            }
#endif
        }

        [Benchmark]
        public void ReadAll()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
#endif

#if NET6_0
            using (var context = new EFCore6DbContext())
            {
#endif
                var list = context.SimpleEntities.ToList();
            }
        }

        [Benchmark]
        public void Update()
        {
#if NET5_0
            using (var context = new EFCore5DbContext())
            {
                _runCountUpdate5++;
                var entityToUpdate = context.SimpleEntities.Where(e => e.Id == _runCountUpdate5).FirstOrDefault();
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountUpdate6++;
                var entityToUpdate = context.SimpleEntities.Where(e => e.Id == _runCountUpdate6).FirstOrDefault();
#endif

                entityToUpdate.Name = "Name-Updated";
                entityToUpdate.Description = "Description-Updated";

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
                var entityToDelete = context.SimpleEntities.Where(e => e.Id == _runCountDelete5).FirstOrDefault();
#endif
#if NET6_0
            using (var context = new EFCore6DbContext())
            {
                _runCountDelete6++;
                var entityToDelete = context.SimpleEntities.Where(e => e.Id == _runCountDelete6).FirstOrDefault();
#endif

                context.SimpleEntities.Remove(entityToDelete);
                context.SaveChanges();
            }
        }
    }
}
