using System;
using System.Net;
using System.Collections.Generic;
using _hue_common;

namespace _hue_api
{
  public class hue_api
  {
    /* Variables */
    internal WebClient webclient_id;
    internal string bridge_address;
    internal string user_token { get; set; }
    internal resources.resources resources { get; set; }

    private List<hue_answer> _last_answer;
    ///<summary>
    /// This is the last error occured.
    ///</summary>
    public List<hue_answer> last_answer
    {
      get => _last_answer;
    }
    /* Functions */
    internal void F_HUE_API_set_last_answer(List<hue_answer> new_answer)
    {
      _last_answer = new_answer;
    }
    internal List<hue_answer> F_HUE_API_send_post_cmd(string url, string command)
    {
      string bridge_command, bridge_ret;

      bridge_command = String.Concat(this.bridge_address, url);

      bridge_ret = this.webclient_id.UploadString(bridge_command, command);
      hue_trace.DebugPrint(bridge_ret);

      F_HUE_API_set_last_answer(hue_answer.F_HUE_ANS_json_convert(bridge_ret));

      return this.last_answer;
    }

    internal List<hue_answer> F_HUE_API_send_put_cmd(string url, string command)
    {
      string bridge_command, bridge_ret;

      bridge_command = String.Concat(this.bridge_address, url);

      bridge_ret = this.webclient_id.UploadString(bridge_command,
                                                  WebRequestMethods.Http.Put,
                                                  command);
      hue_trace.DebugPrint(bridge_ret);

      F_HUE_API_set_last_answer(hue_answer.F_HUE_ANS_json_convert(bridge_ret));

      return this.last_answer;
    }

    internal String F_HUE_API_send_get(string url)
    {
      string bridge_command, bridge_ret;
      hue_answer myhueanswer = new hue_answer();

      bridge_command = String.Concat(this.bridge_address, url);

      bridge_ret = this.webclient_id.DownloadString(bridge_command);
      bridge_ret = bridge_ret.Trim('[');
      bridge_ret = bridge_ret.Trim(']');
      hue_trace.DebugPrint(bridge_ret);

      return bridge_ret;
    }

    public bool F_HUE_API_get_lights()
    {
      bool success = false;

      this.resources.lights = hue_query.F_HUE_QUERY_get_lights(this);

      if (this.resources.lights != null)
      {
        success = true;
      }

      return success;
    }
    public bool F_HUE_API_set_light_state(uint light_id_IN)
    {
      return hue_query.F_HUE_QUERY_set_light_state(this, light_id_IN);
    }
    /* Constructors */
    ///<summary>
    /// Class constructor with the bridge IP address (format "XXX.XXX.XXX.XXX") as parameter
    ///</summary>
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
      /* Attributes init */
      this.bridge_address = String.Concat("http://", bridge_ip);
      this.webclient_id = new WebClient();
      this.resources = new resources.resources();
    }
  }
}

