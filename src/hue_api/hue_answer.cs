/** Réponses renvoyées par le pont Philips Hue */
namespace _client_hue
{
  public class error
  {
    public int type {get; set;}
    public string address {get; set;}
    public string description {get; set;}
  }

  /* Root answer class */
  public class hue_answer
  {
    public _client_hue.error error {get; set;}
  }
}

