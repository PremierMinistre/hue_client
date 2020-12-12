using System;
using System.Text.Json;
using System.Collections.Generic;

namespace _hue_api
{
  public class hue_query
  {
    /* Types */

    /* Variables */

    /* Functions */
    public static List<hue_answer> F_HUE_QUERY_create_user(hue_api hue_api_id, string user_name)
    {
      return hue_api_id.F_HUE_API_send_post_cmd("/api", String.Concat("{\"devicetype\":\"", user_name, "\"}"));
    }
    public static resources.lights.light[] F_HUE_QUERY_get_lights(hue_api hue_api_id)
    {
      int light_number = 0;
      bool all_lights_get = false;
      string bridge_ret;
      resources.lights.light[] light_list = null; //= new _hue_resources.lights.light[1];
      /* Lets get lights 1 by 1 until bridge error type = 3 */
      while (!all_lights_get)
      {
        /* Get current light status */
        bridge_ret = hue_api_id.F_HUE_API_send_get(String.Concat(
          "/api/",
          hue_api_id.user_token,
          "/lights/",
          (light_number + 1).ToString())
        );
        if (bridge_ret.Contains("error"))
        {
          /* This is an error and so we've retrieved all the lights available */
          all_lights_get = true;
        }
        else
        {
          /* We can assume this is a light */
          if (light_number == 0)
          {
            /* 1st light, need to establish the array */
            light_list = new resources.lights.light[1];
            light_list[light_number++] = JsonSerializer.Deserialize<resources.lights.light>(bridge_ret);
          }
          else
          {
            /* add the element */
            resources.lights.light[] light_list_temp = new resources.lights.light[++light_number];
            light_list.CopyTo(light_list_temp, 0);
            light_list_temp[light_number - 1] = JsonSerializer.Deserialize<resources.lights.light>(bridge_ret);
            light_list = light_list_temp;
          }
        }
      }

      return light_list;
    }

    public static bool F_HUE_QUERY_set_light_state(
      hue_api hue_api_id_IN,
      uint light_id_IN)
    {
      bool success = true;
      List<hue_answer> cmd_ret;
      string json_light_state = JsonSerializer.Serialize(hue_api_id_IN.resources.lights[light_id_IN].state);

      if (hue_api_id_IN.resources.lights[light_id_IN].type == "Color temperature light")
      {
        /* Let's remove color attribute */
        json_light_state = System.Text.RegularExpressions.Regex.Replace(
          json_light_state,
          @"[\\\""""]+xy[azAZ0-9:\\\[\]\""""]+,[azAZ0-9:\\\[\]\""""]+",
          "");
        json_light_state = System.Text.RegularExpressions.Regex.Replace(
          json_light_state,
          @"[\\\""]+colormode[a-zA-Z0-9:\\\[\]\""]+",
          "");
        json_light_state = System.Text.RegularExpressions.Regex.Replace(
          json_light_state,
          @"[\\\""]+hue[a-zA-Z0-9:\\\[\]\""]+",
          "");
        json_light_state = System.Text.RegularExpressions.Regex.Replace(
          json_light_state,
          @"[\\\""]+sat[a-zA-Z0-9:\\\[\]\""]+",
          "");
      }
      if (hue_api_id_IN.resources.lights[light_id_IN].state.effect == null)
      {
        json_light_state = System.Text.RegularExpressions.Regex.Replace(
          json_light_state,
          @"[\\\""]+effect[a-zA-Z0-9:\\\[\]\""]+",
          "");
      }
      json_light_state = System.Text.RegularExpressions.Regex.Replace(
        json_light_state,
        @"[\\\""]+reachable[a-zA-Z0-9:\\\[\]\""]+",
        "");
      json_light_state = System.Text.RegularExpressions.Regex.Replace(
        json_light_state,
        @",[,]+",
        @",");
      json_light_state = System.Text.RegularExpressions.Regex.Replace(
        json_light_state,
        @",}",
        @"}");

      cmd_ret = hue_api_id_IN.F_HUE_API_send_put_cmd(
        String.Concat(
          "/api/",
          hue_api_id_IN.user_token,
          "/lights/",
          (light_id_IN + 1).ToString(),
          "/state"),
        json_light_state);

      cmd_ret.ForEach(delegate (hue_answer answer)
      {
        if (answer.error != null)
        {
          success = false;
        }
      });

      return success;
    }
  }
}

