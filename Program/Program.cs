using ArcInstance;
using System.Diagnostics;
Instance arc = new();
#if !DEBUG
try
{
#endif

Stopwatch s = Stopwatch.StartNew();

arc.Run();

#if !DEBUG
}
catch (Exception e)
{
	Console.WriteLine(e);
}
Console.WriteLine("Press any key to exit");
Console.ReadKey();
#endif

Console.WriteLine((((double)s.ElapsedMilliseconds)/1000).ToString("0.000"));
return 0;