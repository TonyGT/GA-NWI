//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GAWetlands {
    using ESRI.ArcGIS.Framework;
    using ESRI.ArcGIS.ArcMapUI;
    using System;
    using System.Collections.Generic;
    using ESRI.ArcGIS.Desktop.AddIns;
    
    
    /// <summary>
    /// A class for looking up declarative information in the associated configuration xml file (.esriaddinx).
    /// </summary>
    internal class ThisAddIn {
        
        internal static string Name {
            get {
                return "GeorgiaWetlandTool";
            }
        }
        
        internal static string AddInID {
            get {
                return "{a6e2253a-7b44-4f92-a477-ee55801e908f}";
            }
        }
        
        internal static string Company {
            get {
                return "CGIS, Georgia Tech";
            }
        }
        
        internal static string Version {
            get {
                return "1.0";
            }
        }
        
        internal static string Description {
            get {
                return "Type in a description for this Add-in.";
            }
        }
        
        internal static string Author {
            get {
                return "gtg221h";
            }
        }
        
        internal static string Date {
            get {
                return "5/24/2013";
            }
        }
        
        /// <summary>
        /// A class for looking up Add-in id strings declared in the associated configuration xml file (.esriaddinx).
        /// </summary>
        internal class IDs {
            
            /// <summary>
            /// Returns 'BtnSymbolize_System', the id declared for Add-in Button class 'BtnSymbolize_System'
            /// </summary>
            internal static string BtnSymbolize_System {
                get {
                    return "BtnSymbolize_System";
                }
            }
            
            /// <summary>
            /// Returns 'BtnSymbolize_Class', the id declared for Add-in Button class 'BtnSymbolize_Class'
            /// </summary>
            internal static string BtnSymbolize_Class {
                get {
                    return "BtnSymbolize_Class";
                }
            }
            
            /// <summary>
            /// Returns 'BtnSymbolize_Regime', the id declared for Add-in Button class 'BtnSymbolize_Regime'
            /// </summary>
            internal static string BtnSymbolize_Regime {
                get {
                    return "BtnSymbolize_Regime";
                }
            }
            
            /// <summary>
            /// Returns 'BtnSymbolize_Special', the id declared for Add-in Button class 'BtnSymbolize_Special'
            /// </summary>
            internal static string BtnSymbolize_Special {
                get {
                    return "BtnSymbolize_Special";
                }
            }
            
            /// <summary>
            /// Returns 'BtnSymbolize_Chemistry', the id declared for Add-in Button class 'BtnSymbolize_Chemistry'
            /// </summary>
            internal static string BtnSymbolize_Chemistry {
                get {
                    return "BtnSymbolize_Chemistry";
                }
            }
            
            /// <summary>
            /// Returns 'BtnSymbolize_Soil', the id declared for Add-in Button class 'BtnSymbolize_Soil'
            /// </summary>
            internal static string BtnSymbolize_Soil {
                get {
                    return "BtnSymbolize_Soil";
                }
            }
            
            /// <summary>
            /// Returns 'BtnQuery_All', the id declared for Add-in Button class 'BtnQueryAll'
            /// </summary>
            internal static string BtnQueryAll {
                get {
                    return "BtnQuery_All";
                }
            }
            
            /// <summary>
            /// Returns 'BtnClipByPolygon', the id declared for Add-in Tool class 'ClipNWIByPolygon'
            /// </summary>
            internal static string ClipNWIByPolygon {
                get {
                    return "BtnClipByPolygon";
                }
            }
            
            /// <summary>
            /// Returns 'BtnClipByRadius', the id declared for Add-in Tool class 'ClipNWIByRadius'
            /// </summary>
            internal static string ClipNWIByRadius {
                get {
                    return "BtnClipByRadius";
                }
            }
        }
    }
    
internal static class ArcMap
{
  private static IApplication s_app = null;
  private static IDocumentEvents_Event s_docEvent;

  public static IApplication Application
  {
    get
    {
      if (s_app == null)
        s_app = Internal.AddInStartupObject.GetHook<IMxApplication>() as IApplication;

      return s_app;
    }
  }

  public static IMxDocument Document
  {
    get
    {
      if (Application != null)
        return Application.Document as IMxDocument;

      return null;
    }
  }
  public static IMxApplication ThisApplication
  {
    get { return Application as IMxApplication; }
  }
  public static IDockableWindowManager DockableWindowManager
  {
    get { return Application as IDockableWindowManager; }
  }
  public static IDocumentEvents_Event Events
  {
    get
    {
      s_docEvent = Document as IDocumentEvents_Event;
      return s_docEvent;
    }
  }
}

namespace Internal
{
  [StartupObjectAttribute()]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public sealed partial class AddInStartupObject : AddInEntryPoint
  {
    private static AddInStartupObject _sAddInHostManager;
    private List<object> m_addinHooks = null;

    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    public AddInStartupObject()
    {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool Initialize(object hook)
    {
      bool createSingleton = _sAddInHostManager == null;
      if (createSingleton)
      {
        _sAddInHostManager = this;
        m_addinHooks = new List<object>();
        m_addinHooks.Add(hook);
      }
      else if (!_sAddInHostManager.m_addinHooks.Contains(hook))
        _sAddInHostManager.m_addinHooks.Add(hook);

      return createSingleton;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Shutdown()
    {
      _sAddInHostManager = null;
      m_addinHooks = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal static T GetHook<T>() where T : class
    {
      if (_sAddInHostManager != null)
      {
        foreach (object o in _sAddInHostManager.m_addinHooks)
        {
          if (o is T)
            return o as T;
        }
      }

      return null;
    }

    // Expose this instance of Add-in class externally
    public static AddInStartupObject GetThis()
    {
      return _sAddInHostManager;
    }
  }
}
}
