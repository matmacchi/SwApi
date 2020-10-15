using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System.Runtime.InteropServices;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

public class SwApiManager : IHostedService
{


    [DllImport("ole32.dll")]
    private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

    private const string SW_PATH = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";

    private ISldWorks app;

    private ModelDoc2 activeModel;


    public Task StartAsync(CancellationToken stoppingToken)
    {
        Startup();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {

        return Task.CompletedTask;
    }



    public void Startup()
	{
        try
        {
			app = StartSwApp(SW_PATH);
            System.Diagnostics.Debug.WriteLine(app.RevisionNumber());

        } catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Failed to connect to SOLIDWORKS instance:" + ex.Message );
        }
	}

    public bool isAwake()
    {
        if(app.RevisionNumber() != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool CheckMissingFiles(string path)
    {
        ModelDoc2 doc = this.OpenSwAssembly(path);
        var dependancies = doc.GetDependencies(1,1);

        Debugger.Break();


        return true;
    }


    public bool GenerateAssembly()
    {


        return true;
    }

    public EquationMgr getEquationMgr()
    {
        EquationMgr swEqMgr = default(EquationMgr);

        swEqMgr = activeModel.GetEquationMgr();


        return swEqMgr;
        
    }

    public ModelDoc2 OpenSwAssembly(string path)
    {

        ModelDoc2 swModel = default(ModelDoc2);
        DocumentSpecification swDocSpecification = default(DocumentSpecification);
        string[] componentsArray = new string[1];
        object[] components = null;
        string name = null;
        int errors = 0;
        int warnings = 0;

        //Set the specifications
        swDocSpecification = (DocumentSpecification)app.GetOpenDocSpec(path);

        componentsArray[0] = "food bowl-1@bowl and chute";
        components = (object[])componentsArray;

        swDocSpecification.ComponentList = components;
        swDocSpecification.Selective = true;
        name = swDocSpecification.FileName;

        swDocSpecification.DocumentType = (int)swDocumentTypes_e.swDocASSEMBLY;
        swDocSpecification.DisplayState = "Default_Display State-1";
        swDocSpecification.UseLightWeightDefault = false;
        swDocSpecification.LightWeight = true;
        swDocSpecification.Silent = true;
        swDocSpecification.IgnoreHiddenComponents = true;

        //Open the assembly document as per the specifications
        swModel = (ModelDoc2)app.OpenDoc7(swDocSpecification);
        errors = swDocSpecification.Error;
        warnings = swDocSpecification.Warning;

        Debugger.Break();

        if(errors > 0)
        {
            throw new Exception("Error while opening Solidworks assembly");
        }


        if(warnings > 0)
        {
            System.Diagnostics.Debug.WriteLine("Warning while opening Soliworks assembly");
        }

        activeModel = swModel;

        return swModel;
    }

	private static ISldWorks StartSwApp(string appPath, int timeoutSec = 10)
    {
		var timeout = TimeSpan.FromSeconds(timeoutSec);

		var startTime = DateTime.Now;

		var prc = Process.Start(appPath);
		ISldWorks app = null;

		while (app == null)
        {
			if(DateTime.Now - startTime > timeout)
            {
				throw new TimeoutException();
            }

			app = GetSwAppFromProcess(prc.Id);
        }

		return app;
    }

    private static ISldWorks GetSwAppFromProcess(int processId)
    {


        var monikerName = "SolidWorks_PID_" + processId.ToString();

        IBindCtx context = null;
        IRunningObjectTable rot = null;
        IEnumMoniker monikers = null;

        try
        {
            CreateBindCtx(0, out context);

            context.GetRunningObjectTable(out rot);
            rot.EnumRunning(out monikers);

            var moniker = new IMoniker[1];

            while (monikers.Next(1, moniker, IntPtr.Zero) == 0)
            {
                var curMoniker = moniker.First();

                string name = null;

                if (curMoniker != null)
                {
                    try
                    {
                        curMoniker.GetDisplayName(context, null, out name);
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }

                if (string.Equals(monikerName,
                    name, StringComparison.CurrentCultureIgnoreCase))
                {
                    object app;
                    rot.GetObject(curMoniker, out app);
                    return app as ISldWorks;
                }
            }
        }
        finally
        {
            if (monikers != null)
            {
                Marshal.ReleaseComObject(monikers);
            }

            if (rot != null)
            {
                Marshal.ReleaseComObject(rot);
            }

            if (context != null)
            {
                Marshal.ReleaseComObject(context);
            }
        }

        return null;
    }

}
