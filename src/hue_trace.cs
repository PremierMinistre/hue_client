#define DEBUG
using System;

namespace _client_hue
{
  class hue_trace
  {
    /* Debug methods */
    public static void DebugPrint(string param)
    {
#if (DEBUG)
      Console.WriteLine("{0}", param);
#endif
    }
    public static void DebugPrint(string format, string param)
    {
#if (DEBUG)
      Console.WriteLine(format, param);
#endif
    }
    public static void DebugPrint(string format, string param1, string param2)
    {
#if (DEBUG)
      Console.WriteLine(format, param1, param2);
#endif
    }
    public static void DebugPrint(string format, string param1, string param2, string param3)
    {
#if (DEBUG)
      Console.WriteLine(format, param1, param2, param3);
#endif
    }
    /* Operational methods */
    public static void Print(string param)
    {
      Console.WriteLine("{0}", param);
    }
    public static void Print(string format, string param)
    {
      Console.WriteLine(format, param);
    }
    public static void Print(string format, string param1, string param2)
    {
      Console.WriteLine(format, param1, param2);
    }
    public static void Print(string format, string param1, string param2, string param3)
    {
      Console.WriteLine(format, param1, param2, param3);
    }
  }
}

