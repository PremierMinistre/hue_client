/* A Hue resource object */
namespace _hue_api.resources
{
  ///<summary>
  /// A resource object
  ///</summary>
  public class resources
  {
    ///<summary>
    /// A list of lights.
    ///</summary>
    public lights.light[] lights { get; set; }

    /*TODO add these types
      public class group { }
      public class config { }
      public class schedule { }
      public class scene { }
      public class sensor { }
      public class rule { }
    */
  }
}