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
    public static void F_HUE_CLT_print_usage()
    {
      hue_trace.Print("Hue CLI Client");
      hue_trace.Print("\t-h : To show this help");
      hue_trace.Print("\tadd_user <user_name> : Add a new user into the bridge,");
      hue_trace.Print("\t\td'ont forget to press the bridge's button before");
      hue_trace.Print("\tlights <get/set> :");
      hue_trace.Print("\t\tget : List all the lights");
      hue_trace.Print("\t\tset <light_id> <param>=<value>: Set the light new state");
    }
    static void Main(string[] args)
    {

      int arg_cnt;/* This is the argument number left */
      int i;
      hue_api hue_api_id;

      /* For debugging purpose */
      //args = new string[] {"lights", "set", "3", "on=true", "lights", "set", "2", "on=true"};

      F_HUE_CLT_load_config();

      if (bridge_ip != null)
      {
        hue_trace.DebugPrint("Bridge IP address = {0}", bridge_ip);
        hue_api_id = new hue_api(bridge_ip);
      }
      else
      {
        /* TODO init empty xml */
        F_HUE_CLT_print_usage();
        return;
      }
      if (user_token != null)
      {
        hue_trace.DebugPrint("User Token = {0}", user_token);
        hue_api_id.user_token = user_token;
      }
      else
      {
        F_HUE_CLT_print_usage();
        return;
      }

      /* Let's parse the arguments list */
      arg_cnt = args.Length;
      if (arg_cnt == 0)
      {
        F_HUE_CLT_print_usage();
        return;
      }
      while (arg_cnt > 0)
      {
        switch (args[args.Length - arg_cnt])
        {
          case "add_user":
            if (arg_cnt < 1)
            {
              F_HUE_CLT_print_usage();
              arg_cnt = 0;
            }
            else
            {
              /* TODO, put the token into the xml and the good name */
              hue_query.F_HUE_QUERY_create_user(hue_api_id, "Baptou").ForEach(
                delegate (hue_answer answer)
                {
                  answer.print();
                });
              arg_cnt--;
            }
            break;

          case "lights":
            if (arg_cnt < 1)
            {
              /* Need at least 1 arg for "get" or "set" */
              F_HUE_CLT_print_usage();
              arg_cnt = 0;
            }
            else
            {
              arg_cnt--; /* Remove "lights" arg */
              if (args[args.Length - arg_cnt] == "get")
              {
                /* Get lights */
                arg_cnt--;/* Remove "get" arg in the list */

                hue_api_id.F_HUE_API_get_lights();

                for (i = 0; i < hue_api_id.resources.lights.Length; i++)
                {
                  hue_trace.Print("Light #{0}", i.ToString());
                  hue_api_id.resources.lights[i].Print(true);
                }

              }
              else if (args[args.Length - arg_cnt] == "set")
              {
                /* Set a light status */
                arg_cnt--;/* Remove "set" arg in the list */
                if (arg_cnt < 2)
                {
                  /* Need at least 2 args for "set", the light_id and the attribute couple */
                  F_HUE_CLT_print_usage();
                  arg_cnt = 0;
                }
                else
                {
                  int light_id;
                  if (int.TryParse(args[args.Length - arg_cnt], out light_id))
                  {
                    arg_cnt--;/* Remove light_id arg in the list */
                    string[] param_value_couple = args[args.Length - arg_cnt].Split("=");
                    if (param_value_couple != null)
                    {
                      /* This is okay so we get, update and put back to the bridge */
                      hue_api_id.F_HUE_API_get_lights();
                      bool bool_value;
                      uint uint_value;
                      switch (param_value_couple[0])
                      {
                        case "on":
                          if (bool.TryParse(param_value_couple[1], out bool_value))
                          {
                            hue_api_id.resources.lights[light_id].state.on = bool_value;
                          }
                          else
                          {
                            hue_trace.Print("Error, expected \"true\" or \"false\" and got {0}",param_value_couple[1]);
                            arg_cnt = 0;
                          }
                          break;
                        case "bri":
                          if (uint.TryParse(param_value_couple[1], out uint_value))
                          {
                            hue_api_id.resources.lights[light_id].state.bri = uint_value;
                          }
                          else
                          {
                            hue_trace.Print("Error, expected unsigned int and got {0}",param_value_couple[1]);
                            arg_cnt = 0;
                          }
                          break;
                        case "ct":
                          if (uint.TryParse(param_value_couple[1], out uint_value))
                          {
                            hue_api_id.resources.lights[light_id].state.ct = (ushort) uint_value;
                          }
                          else
                          {
                            hue_trace.Print("Error, expected unsigned short and got {0}",param_value_couple[1]);
                            arg_cnt = 0;
                          }
                          break;
                        case "alert":
                          hue_api_id.resources.lights[light_id].state.alert = param_value_couple[1];
                          break;
                        default:
                          F_HUE_CLT_print_usage();
                          arg_cnt = 0;
                          break;
                      }
                      if (arg_cnt != 0)
                      {
                        /* Hopefully there is no error so let's send the new state to the light */
                        arg_cnt --; /* Remove attribute couple arg in the list */
                        hue_api_id.F_HUE_API_set_light_state((uint)light_id);
                      }
                    }
                    else
                    {
                      F_HUE_CLT_print_usage();
                      arg_cnt = 0;
                    }
                  }
                  else
                  {
                    /* Param is not an int */
                    F_HUE_CLT_print_usage();
                    arg_cnt = 0;
                  }
                }
              }
              else
              {
                /* "ligts" cmd only takes "set" or "get" param */
                F_HUE_CLT_print_usage();
                arg_cnt = 0;
              }
            }
            break;

          case "-h":
          default:
            F_HUE_CLT_print_usage();
            arg_cnt = 0;
            break;
        }
        /* If all args not parsed then let's roll again */
      }

      return;
    }
  }
}
