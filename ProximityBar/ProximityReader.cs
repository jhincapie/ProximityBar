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

using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProximityBar
{

  public class ProximityReader : IProximityReader
  {

    public const char INPUT_READ = 'R';
    public const char INPUT_MODESIMPLE = 'S';
    public const char INPUT_MODECOMPLEX = 'C';
    public const char INPUT_VERSION = 'V';

    log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProximityReader));
    Random r = new Random((int)DateTime.Now.Ticks);

    String commPort;
    SerialPort port;
    private ProximityReading actualProximity;

    public ProximityReading ActualProximity
    {
      get
      {
        IList<ProximityReading> readings = ReadProximity(CleanRead(ProximityReader.INPUT_READ));
        if (readings.Count > 0)
        {
          var orderedR = from r in readings orderby r.Distance select r;
          actualProximity = orderedR.First();
        }
        else
        {
          actualProximity = new ProximityReading();
          actualProximity.Distance = 999;
          actualProximity.Position = Position.Undefined;
        }

        return actualProximity;
      }
    }

    public ProximityReader(String cPort)
    {
      commPort = cPort;
    }

    private String CleanRead(char input)
    {
      //Reads anything that is in the buffer
      String preRead = ReadFromSerial();
      if (preRead.Length > 0)
        logger.Debug(String.Format("PreRead: {0}", preRead));

      port.Write(new char[1] { input }, 0, 1);
      Thread.Sleep(250);

      String readingS = ReadFromSerial();
      logger.Debug(String.Format("Read: {0}", readingS));

      String postRead = ReadFromSerial();
      if (postRead.Length > 0)
        logger.Debug(String.Format("PostRead: {0}", postRead));

      return readingS;
    }

    private IList<ProximityReading> ReadProximity(String readingS)
    {
      List<ProximityReading> readings = new List<ProximityReading>();

      Regex expression = new Regex("([0-9]{1,3})", RegexOptions.Compiled);
      MatchCollection matches = expression.Matches(readingS);
      if (matches.Count > 0)
      {
        Position actual = Position.Left;
        if (matches.Count == 1)
          actual = Position.Undefined;
        foreach (Match match in matches)
        {
          ProximityReading reading = new ProximityReading();
          reading.Position = actual;
          reading.Distance = Int32.Parse(match.Value);

          readings.Add(reading);
          if (actual != Position.Undefined)
            actual++;
        }
      }

      return readings;
    }

    private String ReadFromSerial()
    {
      StringBuilder stringB = new StringBuilder();
      while (port.BytesToRead > 0)
      {
        char c = (char)port.ReadByte();
        stringB.Append(c);
      }
      return stringB.ToString();
    }

    public bool TestConnection()
    {
      try
      {
        port = new SerialPort(commPort, 9600, Parity.None, 8, StopBits.One);
        port.Open();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
      }
      return true;
    }


    public void SetMode(char mode)
    {
      String newMode = CleanRead(mode);

      String expectedOutPut = "0";
      if (mode == ProximityReader.INPUT_MODECOMPLEX)
        expectedOutPut = "1";

      if (!expectedOutPut.Equals(newMode))
        throw new ApplicationException(String.Format("Changing mode to '{0}' failed. Response received: {1}", mode, newMode));
    }

    public string GetVersion()
    {
      return CleanRead(ProximityReader.INPUT_VERSION);
    }

  }

}
