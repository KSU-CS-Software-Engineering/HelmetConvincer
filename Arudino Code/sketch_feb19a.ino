//#include <SoftwareSerial.h>
 int speedPin = A0;
 int turnPin = A1;
 int incomingByte = 0; 
 String final = "";
void setup() {
 Serial.begin(9600);
 while (!Serial);
}

void loop() {
  // put your main code here, to run repeatedly:
  
  
  if (Serial.available() > 0){
    incomingByte = Serial.read();
    final = "";
    final = final + analogRead(speedPin);
    final = final + " " + analogRead(turnPin);
    Serial.println(final);
    Serial.flush();
  }
  delay(20);
  
}
