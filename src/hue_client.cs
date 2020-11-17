using System;

namespace _client_hue
{
  class Program
  {
    static void Main(string[] args)
    {
      String bridge_ip = "192.168.1.99";

      hue_trace.DebugPrint("Bridge IP address = {0}", bridge_ip);

      hue_api hue_api_id = new hue_api(bridge_ip);
    }
  }
}
