using BenchmarkDotNet.Running;
using Benchmarks;

Console.WriteLine("Hello, World!");

// var isssit = "hello".AsSpan().IsPalindrome();
// Console.WriteLine(isssit);
BenchmarkRunner.Run<Palindrome>();