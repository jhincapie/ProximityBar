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
using CAF.ContextAdapter;

namespace ProximityBar
{
  public class ProximityCM : CAF.ContextAdapter.ContextMonitor
  {

    public ProximityReader Reader { get; set; }

    public ProximityCM(ProximityReader r)
    {
      Reader = r;
      updateType = CAF.ContextAdapter.ContextAdapterUpdateType.OnRequest;
      updateInterval = 500;
    }

    protected override void CustomStart()
    {
      if (Reader == null)
        throw new ArgumentException("No reader specified");
      if (!Reader.TestConnection())
        throw new ArgumentException("No connection to reader");
    }

    protected override void CustomRun()
    {
      NotifyContextServices(this, new NotifyContextMonitorListenersEventArgs(Reader.GetType(), Reader.ActualProximity));
    }

    public void RequestRead()
    {
      CustomRun();
    }

  }

}
