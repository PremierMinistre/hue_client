using System;
using _hue_api;
using _hue_common;

namespace _client_hue
{
  class Program
  {


    static String bridge_ip;
    static String user_token;
    public static bool F_HUE_CLT_load_config(string config_file_path = "./config")
    {
      bool success = true;
      System.Xml.XmlDocument file_config = new System.Xml.XmlDocument();
      System.Xml.XmlNode config_node;

      try
      {
        file_config.Load(config_file_path);
        config_node = file_config.DocumentElement.SelectSingleNode("user_token");
        user_token = config_node.InnerText;
        config_node = file_config.DocumentElement.SelectSingleNode("bridge_ip");
        bridge_ip = config_node.InnerText;
      }
      catch
      {
        success = false;
      }

      return success;
    }
    static void Main(string[] args)
    {
      int i;

      F_HUE_CLT_load_config();

      hue_trace.DebugPrint("Bridge IP address = {0}", bridge_ip);
      hue_trace.DebugPrint("User Token = {0}", user_token);


      hue_api hue_api_id = new hue_api(bridge_ip);
      hue_api_id.user_token = user_token;
      /* Add a new user Baptou */
      /*hue_query.F_HUE_QUERY_create_user(hue_api_id, "Baptou").print();*/

      hue_api_id.F_HUE_API_update_lights();

      for (i = 0; i < hue_api_id.resources.lights.Length; i++)
      {
        hue_api_id.resources.lights[i].Print(true);
      }
    }
  }
}
