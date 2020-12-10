using _hue_common;
using System.Text.Json;

/* Réponses renvoyées par le pont Philips Hue */
namespace _hue_api
{
  ///<summary>
  /// A Hue bridge error
  ///</summary>
  public class error
  {
    public int type {get; set;}
    public string address {get; set;}
    public string description {get; set;}
  }
  public class success
  {
    public string username {get; set;}
  }
  /* Root answer class */
  public class hue_answer
  {
    /* Attributes */
    public error error {get; set;}
    public success success {get; set;}

    /* Functions */
    internal static hue_answer F_HUE_ANS_json_convert(string answer_serialized)
    {
      return JsonSerializer.Deserialize<hue_answer>(answer_serialized);
    }
    public void print()
    {
      if (this.error != null)
      {
        hue_trace.DebugPrint("myhueanswer.error.address = {0}", this.error.address);
        hue_trace.DebugPrint("myhueanswer.error.description = {0}", this.error.description);
        hue_trace.DebugPrint("myhueanswer.error.type = {0}", this.error.type.ToString());
      }
      if (this.success != null)
      {
        if (this.success.username != null)
        {
          hue_trace.DebugPrint("myhueanswer.success.username = {0}", this.success.username);
        }
      }
    }
  }
}

