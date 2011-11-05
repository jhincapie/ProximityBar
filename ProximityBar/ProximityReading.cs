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

namespace ProximityBar
{
  public class ProximityReading
  {
    public Position Position { get; set; }
    public Int32 Distance { get; set; }

    public override string ToString()
    {
      return String.Format("Position: {0}, Distance: {1}", Position, Distance);
    }
  }

  public enum Position { Left, Center, Right, Undefined };
}
