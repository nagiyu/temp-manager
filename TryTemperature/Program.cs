using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        try
        {
            var cpuCounter = new PerformanceCounter("Thermal Zone Information", "Temperature", @"\_TZ.THRM");

            var temperature = cpuCounter.NextValue(); // 次の値を取得します

            // 温度はセルシウス温度単位で提供される
            Console.WriteLine("CPU温度: {0} ℃", temperature);
        }
        catch (Exception e)
        {
            Console.WriteLine("エラーが発生しました: " + e.Message);
        }

        //foreach (var category in PerformanceCounterCategory.GetCategories())
        //{
        //    Console.WriteLine(category.CategoryName);
        //}

        //foreach (var counter in new PerformanceCounterCategory("Thermal Zone Information").GetCounters())
        //{
        //    Console.WriteLine(counter.CounterName);
        //}
    }
}
