﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

using System.Collections;
using System.ComponentModel;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GAWetlands
{
    public class BtnSymbolize_Base : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        #region"Get Index Number from Layer Name"
        // ArcGIS Snippet Title:
        // Get Index Number from Layer Name
        // 
        // Long Description:
        // Get the index number for the specified layer name.
        // 
        // Add the following references to the project:
        // ESRI.ArcGIS.Carto
        // 
        // Intended ArcGIS Products for this snippet:
        // ArcGIS Desktop (ArcEditor, ArcInfo, ArcView)
        // ArcGIS Engine
        // ArcGIS Server
        // 
        // Applicable ArcGIS Product Versions:
        // 9.2
        // 9.3
        // 9.3.1
        // 10.0
        // 
        // Required ArcGIS Extensions:
        // (NONE)
        // 
        // Notes:
        // This snippet is intended to be inserted at the base level of a Class.
        // It is not intended to be nested within an existing Method.
        // 

        ///<summary>Get the index number for the specified layer name.</summary>
        /// 
        ///<param name="activeView">An IActiveView interface</param>
        ///<param name="layerName">A System.String that is the layer name in the active view. Example: "states"</param>
        ///  
        ///<returns>A System.Int32 representing a layer number</returns>
        ///  
        ///<remarks>Return values of 0 and greater are valid layers. A return value of -1 means the layer name was not found.</remarks>
        public static System.Int32 GetIndexNumberFromLayerName(ESRI.ArcGIS.Carto.IActiveView activeView, System.String layerName)
        {
            if (activeView == null || layerName == null)
            {
                return -1;
            }
            ESRI.ArcGIS.Carto.IMap map = activeView.FocusMap;

            // Get the number of layers
            int numberOfLayers = map.LayerCount;

            // Loop through the layers and get the correct layer index
            for (System.Int32 i = 0; i < numberOfLayers; i++)
            {
            //    return 0;
            }

            return 0;
        }
        #endregion

        private class DoSymbolizeLayerClass
        {
            public System.Collections.Generic.HashSet<string> hsToRemove = new System.Collections.Generic.HashSet<string>();
            public string symbType = "";
            public bool IsGPBusy = false;
        }

        private static DoSymbolizeLayerClass dslc = new DoSymbolizeLayerClass();
        private static IUniqueValueRenderer urvl = null;
        private static IFeatureLayer srcLayer = null;

        private static Geoprocessor gp = new Geoprocessor();

        public static void doSymbolizeLayer(IActiveView activeView, int i, string symbType) {
            if (dslc.IsGPBusy)
            {
                System.Windows.Forms.MessageBox.Show("Symbolization in progress. Please wait until complete before restarting.");
                return;
            }

            ESRI.ArcGIS.Carto.IMap map = activeView.FocusMap;
            ESRI.ArcGIS.Carto.ILayerFile layerFile = new ESRI.ArcGIS.Carto.LayerFileClass();
            
            System.Console.WriteLine((System.IO.Directory.GetCurrentDirectory()));

            if (ArcMap.Document.SelectedLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("Select a layer before continuing.");
                return;
            }

            dslc = new DoSymbolizeLayerClass();

            try
            {
                IFeatureLayer ifl = (IFeatureLayer)ArcMap.Document.SelectedLayer;
                srcLayer = ifl;

                IFeatureWorkspace iw = (IFeatureWorkspace)((IDataset)ifl.FeatureClass).Workspace;

                ESRI.ArcGIS.AnalysisTools.Statistics stats = new ESRI.ArcGIS.AnalysisTools.Statistics();
                stats.in_table = "NWI_POLY";
                stats.out_table = "In_memory\\SymbolStats" + "_" + symbType;
                stats.case_field = symbType;

                stats.statistics_fields = "\"" + symbType + "\" " + "COUNT";

                gp.AddOutputsToMap = true;
                gp.OverwriteOutput = true;
                gp.ToolExecuted += new EventHandler<ToolExecutedEventArgs>(gp_ToolExecuted);
                gp.ProgressChanged += new EventHandler<ESRI.ArcGIS.Geoprocessor.ProgressChangedEventArgs>(gp_ProgressChanged);

                dslc.symbType = symbType;
                dslc.hsToRemove = new HashSet<string>();

                string geomTypeName = "Polygon";

                if (ifl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    geomTypeName = "Polyline";

                var asmPath = GetAssemblyPath();

                layerFile.Open(asmPath + "/Symbology/" + symbType + "_" + geomTypeName + ".lyr");
                IGeoFeatureLayer igfl = (IGeoFeatureLayer)layerFile.Layer;

                urvl = (IUniqueValueRenderer)igfl.Renderer;

                for (int index = 0; index < urvl.ValueCount; index++)
                {
                    dslc.hsToRemove.Add(urvl.Value[index]);
                }

                ITable tbl = null;
                try
                {
                    IGPUtilities igpu = new GPUtilitiesClass();
                    //IArray tbl = (IArray)igpu.GetGxObjects((string) igpr.ReturnValue);
                    tbl = igpu.FindMapTable("SymbolStats" + "_" + dslc.symbType);
                }
                catch(Exception eee) {
                }

                if (tbl == null)
                    gp.ExecuteAsync((IGPProcess)stats);
                else
                {
                    ProcessResult();
                }
                /*ISQLSyntax sql = (ISQLSyntax)iw;

                string prefix = sql.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_DelimitedIdentifierPrefix);
                string suffix = sql.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_DelimitedIdentifierSuffix);
                //ITable tbl = (ITable)((IFeatureLayer)igfl).FeatureClass;

                IQueryFilter iqf = new QueryFilterClass();
                iqf.WhereClause = "1=1";
                ICursor csr = (ICursor) ifl.Search(iqf, true);

                dslc.symbType = symbType;
                dslc.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(dslc_RunWorkerCompleted);
                dslc.RunWorkerAsync(csr);*/
#if false

                dslc.hsToRemove.Clear();

                for (int index = 0; index < dslc.urvl.ValueCount; index++)
                {
                    dslc.hsToRemove.Add(dslc.urvl.Value[index]);
                }

                dslc.csr = csr;
                dslc.symbType = symbType;

                IFeature feature = null;
                int pos = csr.Fields.FindField(symbType);

                if (pos == -1) //
                    return;

                while ((feature = csr.NextFeature()) != null)
                {
                    dslc.hsToRemove.Remove((string)feature.get_Value(pos));

                    stepProgressor.Step();

                    if (stepProgressor.Position == stepProgressor.MaxRange)
                    {
                        stepProgressor.Position = stepProgressor.MinRange;
                    }

                    if (dslc.hsToRemove.Count == 0)
                    {
                        break;
                    }
                }

                IUniqueValueRenderer urvl = dslc.urvl;

                foreach (string h in dslc.hsToRemove)
                {
                    urvl.RemoveValue(h);
                }


                IGeoFeatureLayer igfl_lyr = (IGeoFeatureLayer)ifl;
                igfl_lyr.Renderer = (IFeatureRenderer)urvl;

                progressDialog2.HideDialog();

                ArcMap.Document.ActiveView.ContentsChanged();
                ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
#endif
#if false
                IQueryDef2 iqd = (IQueryDef2) ws.CreateQueryDef();
                iqd.PrefixClause = "DISTINCT";
                iqd.WhereClause = "1=1";
                iqd.SubFields = ifl.FeatureClass.OIDFieldName + "," + ifl.FeatureClass.ShapeFieldName + "," + symbType;
                iqd.Tables = ifl.Name;

                {
                    IDataStatistics ids = new DataStatisticsClass();
                    ids.Field = symbType;
                    ids.Cursor = csr;

                    IEnumerator iem = null;

                    try
                    {
                        iem = ids.UniqueValues;
                    }
                    catch
                    {
                    }

                    if (ids.UniqueValueCount > 0)
                    {
                        while (iem.MoveNext())
                        {
                            object val = iem.Current;

                            if (val != null && ((string)val).Trim() != "")
                                hs.Add(val.ToString());
                        }
                    }

                    IRow rw = null;

                    int index = csr.Fields.FindField(symbType);
                    while ((rw = csr.NextRow()) != null)
                    {
                        rw.get_Value(index);
                    }

                    if (hs.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("No unique values other than null or blank found. No changes will be made to the symbology");
                        return;
                    }

                    for (int j = 0; j < urvl.ValueCount; j++)
                    {
                        string a = urvl.get_Value(j);
                        if (a == null) continue;

                        if (!hs.Contains(a))
                            urvl.RemoveValue(a);
                    }
                }
#endif

                //ArcMap.Document.CurrentContentsView.Refresh(null);
                //ArcMap.Document.ActiveView.Refresh();
            }
            catch (System.Exception err)
            {
                object severity = null;
                string s = gp.GetMessages(ref severity);
            }
            finally
            {
                if(layerFile != null) layerFile.Close();
            }
        }

        static void gp_ProgressChanged(object sender, ESRI.ArcGIS.Geoprocessor.ProgressChangedEventArgs e)
        {
            IStepProgressor isp = ArcMap.Application.StatusBar.ProgressBar;
            ArcMap.Application.StatusBar.ProgressBar.Position = Convert.ToInt32((e.ProgressPercentage * (isp.MaxRange - isp.MinRange) + isp.MinRange));
        }

        static void gp_ToolExecuted(object sender, ToolExecutedEventArgs e)
        {
            try {
                IGeoProcessorResult igpr = e.GPResult;

                if (igpr.Status == esriJobStatus.esriJobSucceeded)
                {
                    ProcessResult();
                }

                if (igpr.Status == esriJobStatus.esriJobFailed || igpr.Status == esriJobStatus.esriJobSucceeded ||
                    igpr.Status == esriJobStatus.esriJobTimedOut)
                {
                    dslc.IsGPBusy = false;
                }
            }
            catch(Exception err) {
            }

            finally {
            }
        }

        private static void ProcessResult()
        {
            IGPUtilities igpu = new GPUtilitiesClass();
            //IArray tbl = (IArray)igpu.GetGxObjects((string) igpr.ReturnValue);
            ITable tbl = igpu.FindMapTable("SymbolStats" + "_" + dslc.symbType);

            int symbPos = tbl.FindField(dslc.symbType);
            //int countPos = tbl.FindField("Count_" + dslc.symbType);

            IRow rw = null;
            IQueryFilter iqf = new QueryFilterClass();
            iqf.WhereClause = "1=1";

            ICursor csr = tbl.Search(iqf, true);

            int oldCount = dslc.hsToRemove.Count;

            while ((rw = csr.NextRow()) != null)
            {
                dslc.hsToRemove.Remove((string)rw.get_Value(symbPos));
            }

            if (dslc.hsToRemove.Count == oldCount)
            {
                System.Windows.Forms.MessageBox.Show("No unique values other than null or blank found. No changes will be made to the symbology");
                return;
            }

            for (int j = 0; j < urvl.ValueCount; j++)
            {
                string a = urvl.get_Value(j);
                if (a == null) continue;

                if (dslc.hsToRemove.Contains(a))
                    urvl.RemoveValue(a);
            }

            IGeoFeatureLayer igd_dest = (IGeoFeatureLayer)srcLayer;
            igd_dest.Renderer = (IFeatureRenderer)urvl;

            ArcMap.Document.CurrentContentsView.Deactivate();
            ArcMap.Document.ContentsView[0].Activate(ArcMap.Application.hWnd, ArcMap.Document);

            ArcMap.Document.ActiveView.ContentsChanged();
            ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private static string GetAssemblyPath()
        {
            var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            var uriBuilder = new UriBuilder(codeBase);
            var asmPath = Uri.UnescapeDataString(uriBuilder.Path);
            asmPath = System.IO.Path.GetDirectoryName(asmPath);
            return asmPath;
        }

        protected void doSymbolize(string type)
        {
             IActiveView iav = ArcMap.Document.ActiveView;
             doSymbolizeLayer(iav, 0, type);
        }

    }

    public class BtnSymbolize_System : BtnSymbolize_Base
    {
        protected override void OnClick()
        {
            doSymbolize("System");
        }
    }

    public class BtnSymbolize_Class : BtnSymbolize_Base
    {
        protected override void OnClick()
        {
            doSymbolize("Class1");
        }
    }

    public class BtnSymbolize_Regime : BtnSymbolize_Base
    {
        protected override void OnClick()
        {
            doSymbolize("Water1");
        }
    }

    public class BtnSymbolize_Special : BtnSymbolize_Base
    {
        protected override void OnClick()
        {
            doSymbolize("Special1");
        }
    }

    public class BtnSymbolize_Chemistry : BtnSymbolize_Base
    {
        protected override void OnClick()
        {
            doSymbolize("Chem");
        }
    }

    public class BtnSymbolize_Soil : BtnSymbolize_Base
    {
        protected override void OnClick()
        {
            doSymbolize("Soil");
        }
    }

    public class BtnQueryAll : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            QueryForm qf = new QueryForm();
            qf.Show();
            base.OnClick();
        }
    }
}