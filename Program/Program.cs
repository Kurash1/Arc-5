using ArcInstance;

Instance arc = new Instance();
#if !DEBUG
try
{
#endif

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