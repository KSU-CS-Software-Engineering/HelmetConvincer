//#include <SoftwareSerial.h>
 int speedPin = A0;
 int turnPin = A1;
 bool running = true;
 unsigned long timer;
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

  if (millis() - timer > 20)
  {
    lastReading = nowReading;
    nowReading = analogRead(speedPin);
    turnReading = analogRead(turnPin);
    Serial.print(nowReading - lastReading);
    Serial.print(',');
    Serial.println(turnReading);
    Serial.flush();
    timer = millis();
  }
}
