using ArcInstance;

Instance arc = new Instance();
#if !DEBUG
try
{
#endif

arc.test();

#if !DEBUG
}
catch (Exception e)
{
    Console.WriteLine(e);
}
#endif
Console.WriteLine("Press any key to exit");
Console.ReadKey();