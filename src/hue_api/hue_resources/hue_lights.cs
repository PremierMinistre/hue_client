using _hue_common;

namespace _hue_api.resources.lights
{
  ///<summary>
  /// State of a light
  ///</summary>
  public class state
  {
    ///<summary>
    /// On/Off state of the light. On=true, Off=false
    ///</summary>
    public bool on { get; set; }
    private uint _bri = 0;
    ///<summary>
    /// Brightness of the light. This is a scale from the minimum brightness the
    /// light is capable of, 1, to the maximum capable brightness, 254.
    ///</summary>
    public uint bri
    {
      get => _bri;
      set
      {
        if (value > 254)
        {
          _bri = 254;
        }
        else if (value == 0)
        {
          _bri = 1;
        }
        else
        {
          _bri = value;
        }
      }
    }
    ///<summary>
    /// Hue of the light. This is a wrapping value between 0 and 65535.
    ///  Note, that hue/sat values are hardware dependent which means that
    ///  programming two devices with the same value does not garantuee that 
    ///  they will be the same color. Programming 0 and 65535 would mean that
    ///  the light will resemble the color red, 21845 for green and 43690 for blue.
    ///</summary>
    public System.UInt16 hue { get; set; }

    private uint _sat = 0;
    ///<summary>
    /// Saturation of the light. 254 is the most saturated (colored) and 0 is the least saturated (white).
    ///</summary>
    public uint sat
    {
      get => _sat;
      set
      {
        if (value > 254)
        {
          _sat = 254;
        }
        else
        {
          _sat = value;
        }
      }
    }

    ///<summary>
    /// The x and y coordinates of a color in CIE color space.
    ///</summary>
    public float[] xy { get; set; }

    ///<summary>
    /// The Mired Color temperature of the light.
    ///</summary>
    public System.UInt16 ct { get; set; }

    ///<summary>
    /// The alert effect, which is a temporary change to the bulb’s state. This can take one of the following values:
    /// <list type="bullet">
    ///   <item>
    ///     <description>“none” – The light is not performing an alert effect.</description>
    ///   </item>
    ///   <item>
    ///     <description>“select” – The light is performing one breathe cycle.</description>
    ///   </item>
    ///   <item>
    ///     <description>“lselect” – The light is performing breathe cycles for 15 seconds or until an "alert":
    ///       "none" command is received.Note that this contains the last alert sent to the light and not its
    ///       current state. i.e. After the breathe cycle has finished the bridge does not reset the alert to “none“.
    ///     </description>
    ///   </item>
    /// </list>
    ///</summary>
    public string alert { get; set; }

    ///<summary>
    /// The dynamic effect of the light, can either be “none” or “colorloop”.If set to colorloop, the light will
    /// cycle through all hues using the current brightness and saturation settings.
    ///</summary>
    public string effect { get; set; }

    ///<summary>
    /// Indicates the color mode in which the light is working, this is the last command type it received.
    /// Values are :
    /// <list type="bullet">
    ///   <item>
    ///     <description>“hs” for Hue and Saturation</description>
    ///   </item>
    ///   <item>
    ///     <description>“xy” for XY</description>
    ///   </item>
    ///   <item>
    ///     <description>“ct” for Color Temperature</description>
    ///   </item>
    /// </list>
    /// This parameter is only present when the light supports at least one of the values.
    ///</summary>
    public string colormode { get; set; }

    ///<summary>
    /// Indicates if a light can be reached by the bridge.
    ///</summary>
    public bool reachable { get; set; }

    /* Constructors */
    ///<summary>
    /// state constructor.
    ///</summary>
    public state()
    {
      this.xy = new float[] { 0, 0 };
    }
  }

  ///<summary>
  /// A light object
  ///</summary>
  public class light
  {
    ///<summary>
    /// Details the state of the light, see the state table below for more details.
    ///</summary>
    public state state { get; set; }

    ///<summary>
    /// A fixed name describing the type of light e.g. “Extended color light”.
    ///</summary>
    public string type { get; set; }

    ///<summary>
    /// A unique, editable name given to the light.
    ///</summary>
    public string name { get; set; }

    ///<summary>
    /// The hardware model of the light.
    ///</summary>
    public string modelid { get; set; }

    ///<summary>
    /// Unique id of the device. The MAC address of the device with
    /// a unique endpoint id in the form: AA:BB:CC:DD:EE:FF:00:11-XX
    ///</summary>
    public string uniqueid { get; set; }

    ///<summary>
    /// The manufacturer name.
    ///</summary>
    public string manufacturername { get; set; }

    ///<summary>
    /// Unique ID of the luminaire the light is a part of
    /// in the format: AA:BB:CC:DD-XX-YY.  AA:BB:, … represents
    /// the hex of the luminaireid, XX the lightsource position
    /// (incremental but may contain gaps) and YY the lightpoint
    /// position (index of light in luminaire group).  A gap in
    /// the lightpoint position indicates an incomplete luminaire
    /// (light search required to discover missing light points in this case).
    ///</summary>
    public string luminaireuniqueid { get; set; }

    ///<summary>
    /// An identifier for the software version running on the light.
    ///</summary>
    public string swversion { get; set; }

    /* Functions */
    ///<summary>
    /// Print info. Set verbose to 'true' to print all light information.
    ///</summary>
    public void Print(bool verbose = false)
    {
      hue_trace.Print("Light info");
      hue_trace.Print("\tname = {0}", this.name);
      hue_trace.Print("\ton = {0}", this.state.on.ToString());
      if(verbose)
      {
        hue_trace.Print("\tcolormode = {0}", this.state.colormode);
        hue_trace.Print("\tct = {0}", this.state.ct.ToString());
        hue_trace.Print("\tsat = {0}", this.state.sat.ToString());
        hue_trace.Print("\talert = {0}", this.state.alert);
        hue_trace.Print("\treachable = {0}", this.state.reachable.ToString());
      }
    }
    /* Constructors */
    ///<summary>
    /// Class constructor without parameter
    ///</summary>
    public light()
    {
      this.state = new state();
    }
  }
}