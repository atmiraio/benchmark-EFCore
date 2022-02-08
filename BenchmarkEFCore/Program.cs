using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
#if NET5_0
using EFCore5;
using Microsoft.EntityFrameworkCore;
#endif
#if NET6_0
using EFCore6;
using Microsoft.EntityFrameworkCore;
#endif

namespace BenchmarkEFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleEntityCrudBenchmark>();
        }
    }

    public class SimpleEntityCrud : ManualConfig
    {
        public SimpleEntityCrud()
        {
            //Add Exporters
            //AddExporter(CsvMeasurementsExporter.Default, RPlotExporter.Default, MarkdownExporter.Default);
            AddJob(Job.Default.WithRuntime(CoreRuntime.Core50).WithId("net5.0")); //WithWarmupCount(10).WithLaunchCount(1).WithIterationCount(10000)
            AddJob(Job.Default.WithRuntime(CoreRuntime.Core60).WithId("net6.0")); //WithWarmupCount(10).WithLaunchCount(1).WithIterationCount(10000)
        }
    }
}