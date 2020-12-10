using System;
using System.Text.Json;

namespace _hue_api
{
  public class hue_query
  {
    /* Types */

    /* Variables */

    /* Functions */
    public static hue_answer F_HUE_QUERY_create_user(hue_api hue_api_id, string user_name)
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
          hue_api_id.F_HUE_API_set_last_answer(hue_answer.F_HUE_ANS_json_convert(bridge_ret));
        }
        else
        {
          /* We can assume this is a light */
          if(light_number == 0)
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
  }
}

