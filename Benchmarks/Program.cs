using BenchmarkDotNet.Running;
using Benchmarks;

Console.WriteLine("Hello, World!");
BenchmarkRunner.Run<ArrayCreationVsArrayPool>();