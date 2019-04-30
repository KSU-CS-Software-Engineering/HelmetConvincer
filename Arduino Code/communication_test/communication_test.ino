unsigned int timer;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  timer = millis();
}

void loop() {
  // put your main code here, to run repeatedly:
  if(millis() - timer >= 2500) {
    Serial.println("Hello World!");
    Serial.flush();
    timer = millis();
  }
}
