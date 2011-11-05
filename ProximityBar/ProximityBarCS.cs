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
using CAF.ContextService;
using System.Threading;

namespace ProximityBar
{

  public class ProximityBarCS : ContextService
  {

    private ProximityCM proximityCM;

    public ProximityBarCS(ProximityCM pCM)
    {
      proximityCM = pCM;
    }

    protected override void CustomUpdateMonitorReading(object sender, CAF.ContextAdapter.NotifyContextMonitorListenersEventArgs e)
    {
      Console.WriteLine("New Proximity Reading: {0}", e.NewObject);

      Thread.Sleep(breakMS);

      if (--numberOfReadings > 0)
        proximityCM.RequestRead();
    }

    public void SetModeSimple()
    {
      proximityCM.Reader.SetMode('S');
    }

    public void SetModeComplex()
    {
      proximityCM.Reader.SetMode('C');
    }

    int numberOfReadings = 0;
    int breakMS = 0;
    public void ReadPromixity(int times, int breakMilliseconds)
    {
      breakMS = breakMilliseconds;
      numberOfReadings = times;
      if (numberOfReadings > 0)
        proximityCM.RequestRead();
    }

    public String GetReaderVersion()
    {
      return proximityCM.Reader.GetVersion();
    }

  }

}
