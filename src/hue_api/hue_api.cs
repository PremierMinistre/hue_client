using System;
using System.Net;
//using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace _client_hue
{
  public class hue_api
  {
    /* Variables */
    private WebClient webclient_id;
    private string bridge_address;
    /* Functions */
    public hue_api(string bridge_ip)
    {
      /* Serialisation fonctionnelle
      string jsonString;
      var myhueanswer = new _client_hue.hue_answer();
      myhueanswer.error.address = "tata";
      myhueanswer.error.description = "toto";
      myhueanswer.error.type = 546;
      jsonString = JsonSerializer.Serialize(myhueanswer);
      hue_trace.DebugPrint(jsonString);
      */

      string command, bridge_ret;
      hue_answer myhueanswer = new hue_answer();
      this.bridge_address = String.Concat("http://", bridge_ip);
      command = String.Concat(this.bridge_address, "/api/newdeveloper");
      this.webclient_id = new WebClient();
      bridge_ret = this.webclient_id.DownloadString(command);
      bridge_ret = bridge_ret.Trim('[');
      bridge_ret = bridge_ret.Trim(']');
      hue_trace.DebugPrint(bridge_ret);
      myhueanswer = JsonSerializer.Deserialize<hue_answer>(bridge_ret);
      hue_trace.DebugPrint("myhueanswer.error.address = {0}", myhueanswer.error.address);
      hue_trace.DebugPrint("myhueanswer.error.description = {0}", myhueanswer.error.description);
      hue_trace.DebugPrint("myhueanswer.error.type = {0}", myhueanswer.error.type.ToString());

      //hue_trace.DebugPrint(command);
      //var ser = new DataContractJsonSerializer(typeof(_client_hue.hue_answer));
      //string ret;
      //byte[] ret;
      
      //ret = this.webclient_id.DownloadData(command);

      /*hue_error.type = 1;
      hue_error.address = "/";
      hue_error.description = "unauthorized user";*/
      
      //stream1.Position = 0;
      //stream1.Write(ret);
      //stream1.Position = 0;
      //var my_hue_answer = (_client_hue.hue_answer)ser.ReadObject(stream1);
      
      //var my_hue_answer = ser.WriteObjectContent(myhueanswer);
      /* var sr = new StreamReader(stream1);
      Console.Write("JSON form of Person object: ");
      Console.WriteLine(sr.ReadToEnd()); */
      //hue_trace.DebugPrint("type:{0}, address:{1}, description:{2}", my_hue_answer.error.type.ToString(), my_hue_answer.error.address, my_hue_answer.error.description);
    }
  }
}

