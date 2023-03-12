using ConsoleApp1;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        /*/
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start(); // */
        /*/
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}.{4:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds, ts.Microseconds);
        Console.WriteLine("RunTime " + elapsedTime); // */
    }
}
