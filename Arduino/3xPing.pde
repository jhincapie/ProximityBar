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

char iVersion[] = "01.00:S:C3";

int MODESIMPLE = 0;
int MODECOMPLEX = 1;
int actualMode = MODESIMPLE;

int rightPingPin = 2;
int rightPowerPin = 3;
int rightLedPin = 4;

int centerPingPin = 8;
int centerPowerPin = 9;
int centerLedPin = 10;

int leftPingPin = 11;
int leftPowerPin = 12;
int leftLedPin = 13;

int msgSerial;
long cmRight, cmCenter, cmLeft, cmClosest;

void setup()
{
  Serial.begin(9600);

  pinMode(rightLedPin, OUTPUT);
  pinMode(centerLedPin, OUTPUT);
  pinMode(leftLedPin, OUTPUT);

  pinMode(rightPowerPin, OUTPUT);
  pinMode(centerPowerPin, OUTPUT);  
  pinMode(leftPowerPin, OUTPUT);

  digitalWrite(rightPowerPin, HIGH);
  digitalWrite(centerPowerPin, HIGH);  
  digitalWrite(leftPowerPin, HIGH);
}

void loop()
{
  cmRight = readFromSensor(rightPingPin);
  cmCenter = readFromSensor(centerPingPin);
  cmLeft = readFromSensor(leftPingPin);

  cmClosest = cmRight;
  if(cmCenter < cmClosest)
    cmClosest = cmCenter;
  if(cmLeft < cmClosest)
    cmClosest = cmLeft;

  toggleLed(rightLedPin, cmRight);
  toggleLed(centerLedPin, cmCenter);
  toggleLed(leftLedPin, cmLeft);

  processSerialInput();
  delay(100);
}

void processSerialInput()
{
  if(Serial.available() > 0)
  {
    msgSerial = Serial.read();
    if(msgSerial == 'R')
    {
      if(actualMode == MODESIMPLE)
      {
        Serial.print(cmClosest);
        Serial.flush();
      }
      else 
      {
        Serial.print(cmRight);
        Serial.print(',');
        Serial.print(cmCenter);
        Serial.print(',');
        Serial.print(cmLeft);
        Serial.flush();
      }
    }
    else if(msgSerial == 'V')
    {
      Serial.print(iVersion);
      Serial.flush();
    }
    else if(msgSerial == 'S')
    {
      actualMode = MODESIMPLE;
      Serial.print(actualMode);
      Serial.flush();
    }
    else if(msgSerial == 'C')
    {
      actualMode = MODECOMPLEX;
      Serial.print(actualMode);
      Serial.flush();
    }
  }
}

long readFromSensor(int pingPin)
{
  long duration;

  // The PING))) is triggered by a HIGH pulse of 2 or more microseconds.
  // We give a short LOW pulse beforehand to ensure a clean HIGH pulse.
  pinMode(pingPin, OUTPUT);
  digitalWrite(pingPin, LOW);
  delayMicroseconds(5);
  digitalWrite(pingPin, HIGH);
  delayMicroseconds(5);
  digitalWrite(pingPin, LOW);

  // The same pin is used to read the signal from the PING))): a HIGH
  // pulse whose duration is the time (in microseconds) from the sending
  // of the ping to the reception of its echo off of an object.
  pinMode(pingPin, INPUT);
  duration = pulseIn(pingPin, HIGH);
  return microsecondsToCentimeters(duration);
}

long microsecondsToCentimeters(long microseconds)
{
  // The speed of sound is 340 m/s or 29 microseconds per centimeter.
  // The ping travels out and back, so to find the distance of the
  // object we take half of the distance travelled.
  return microseconds / 29 / 2;
}

void toggleLed(int ledPin, long cm)
{
  if(cm > 20)
    digitalWrite(ledPin, LOW);
  else
    digitalWrite(ledPin, HIGH);  
}
