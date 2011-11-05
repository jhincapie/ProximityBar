//
//***************************************************************************
//This file is part of the ProximityBar, developed by:
//     Juan David Hincapie-Ramos <jhincapie@gmail.com>
//
//ProximityBar is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//ProximityBar is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public License
//along with ProximityBar.  If not, see <http://www.gnu.org/licenses/>.
//***************************************************************************
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProximityBarTestApp.Properties;
using ProximityBar;
using CAF.ContextService;
using CAF.ContextAdapter;

namespace ProximityBarTestApp
{

  internal class Program
  {

    private ProximityBarCS proximityBarCS;

    public void Configure()
    {
      ProximityReader reader = new ProximityReader(Settings.Default.COMPort);
      ProximityCM proximityCM = new ProximityCM(reader);
      proximityBarCS = new ProximityBarCS(proximityCM);

      proximityCM.OnNotifyContextServices += proximityBarCS.UpdateMonitorReading;

      ContextMonitorContainer.AddMonitor(proximityCM);
      ContextServiceContainer.AddContextService(proximityBarCS);
    }

    public void StartUp()
    {
      ContextServiceContainer.StartServices();
      ContextMonitorContainer.StartMonitors();
    }

    public void RunTest()
    {
      Console.WriteLine("Getting Reader Version...");
      String rVersion = proximityBarCS.GetReaderVersion();
      Console.WriteLine("Reader Version: {0}", rVersion);

      try
      {
        Console.WriteLine("Setting Reader Mode to Simple...");
        proximityBarCS.SetModeSimple();
        Console.WriteLine("Mode Simple Activated");
      }
      catch (Exception e) 
      {
        Console.WriteLine("Exception Setting Reader Mode to Simple", e.Message);
      }

      Console.WriteLine("Reading Proximity {0} Times in Mode Simple...", Settings.Default.SimpleReadings);
      Console.WriteLine("Time Between Readings: {0} milliseconds", Settings.Default.BreakMS);
      proximityBarCS.ReadPromixity(Settings.Default.SimpleReadings, Settings.Default.BreakMS);

      try
      {
        Console.WriteLine("Setting Reader Mode to Complex...");
        proximityBarCS.SetModeComplex();
        Console.WriteLine("Mode Complex Activated");
      }
      catch (Exception e)
      {
        Console.WriteLine("Exception Setting Reader Mode to Complex", e.Message);
      }

      Console.WriteLine("Reading Proximity {0} Times in Mode Complex...", Settings.Default.SimpleReadings);
      Console.WriteLine("Time Between Readings: {0} milliseconds", Settings.Default.BreakMS);
      proximityBarCS.ReadPromixity(Settings.Default.ComplexReadings, Settings.Default.BreakMS);
    }

    static void Main(string[] args)
    {
      log4net.Config.XmlConfigurator.Configure();

      Program program = new Program();
      program.Configure();
      program.StartUp();
      program.RunTest();
    }

  }

}
