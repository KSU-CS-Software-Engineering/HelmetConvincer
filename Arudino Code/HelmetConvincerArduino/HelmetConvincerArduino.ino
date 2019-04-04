//#include <SoftwareSerial.h>
 int speedPin = A0;
 int turnPin = A1;
 bool running = true;
 unsigned int timer;
 unsigned int lastReading;
 unsigned int nowReading;
 unsigned int turnReading;
 
void setup() {
 Serial.begin(9600);
 timer = millis();
 lastReading = 0;
 nowReading = 0;
 turnReading = 0;
}

void loop() {
  // put your main code here, to run repeatedly:

  if (timer - millis() >= 10 && running)
  {
    lastReading = nowReading;
    nowReading = analogRead(speedPin);
    turnReading = analogRead(turnPin);
    Serial.print(nowReading - lastReading);
    Serial.print(',');
    Serial.println(turnReading);
    Serial.flush();
  }

  if (Serial.available())
  {
    String data = Serial.readString();
    
    if (data.indexOf("Pause") != -1)
    {
      running = false;
    }

    if (data.indexOf("Resume") != -1)
    {
      running = true;
    }
    
  }
  
}
