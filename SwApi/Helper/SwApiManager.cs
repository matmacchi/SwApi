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
using SwApi.Helper;
using System.Reflection;

public class SwApiManager : IHostedService
{


    [DllImport("ole32.dll")]
    private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

    private const string SW_PATH = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";

    private ISldWorks _app;

    private ModelDoc2 _activeModel;

    private AssemblyDoc _activeAssembly;


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
			_app = StartSwApp();
            System.Diagnostics.Debug.WriteLine(_app.RevisionNumber());

        } catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Failed to connect to SOLIDWORKS instance:" + ex.Message );
        }
	}

    public bool isAwake()
    {
        if(_app.RevisionNumber() != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool CheckMissingFiles(string path)
    {
        OpenSwAssembly(path);
        //var dependancies = doc.GetDependencies(1,1);

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

        swEqMgr = _activeModel.GetEquationMgr();


        return swEqMgr;
        
    }

    public void CloseSwAssembly()
    {
        _app.CloseAllDocuments(true);
    }

    public List<string> GetPartList(string path)
    {
        var partList = new List<string>();

        Component2 swComponent;

        OpenSwAssembly(path);

        Object[] components = (Object[])_activeAssembly.GetComponents(true);

        foreach(var component in components)
        {
            swComponent = (Component2)component;

            partList.Add(swComponent.Name);
        }

        CloseSwAssembly();


        return partList;

    }


    public List<string> GetAvailableEquation(string path)
    {
        OpenSwAssembly(path);

        var eqMng = _activeModel.GetEquationMgr();

        var availableEquation = new List<string>();

        for(int i = 0;i < eqMng.GetCount(); i++)
        {
            availableEquation.Add(eqMng.Equation[i]);
        }

        CloseSwAssembly();
        //Debugger.Break();

        return availableEquation;
    }

    public void OpenSwAssembly(string path)
    {

        ModelDoc2 swModel = default(ModelDoc2);
        DocumentSpecification swDocSpecification = default(DocumentSpecification);
        string[] componentsArray = new string[1];
        int warnings = 0;
        int status = 0;

        //Set the specifications
        swDocSpecification = (DocumentSpecification)_app.GetOpenDocSpec(path);



        swModel = _app.OpenDoc6(
            path,
            (int)swDocumentTypes_e.swDocASSEMBLY,
            (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
            "",
            ref status,
            ref warnings);



        if(status > 0)
        {
            throw new Exception("Error while opening Solidworks assembly");
        }


        if(warnings > 0)
        {
            System.Diagnostics.Debug.WriteLine("Warning while opening Soliworks assembly");
        }

        _activeModel = swModel;

        _activeAssembly = (AssemblyDoc)_activeModel;

    }

	private static ISldWorks StartSwApp(int timeoutSec = 10)
    {
        Process prc;

		var timeout = TimeSpan.FromSeconds(timeoutSec);

		var startTime = DateTime.Now;

        var prcs = Process.GetProcessesByName("SLDWORKS");

        if (prcs.Length == 0)
        {
		    prc = Process.Start(SW_PATH);
        } else
        {
            prc = prcs.First();
        }

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
