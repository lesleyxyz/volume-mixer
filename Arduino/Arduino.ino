#define POT1 A0
#define POT2 A2 //PERFECT
#define POT3 A4 //PERFECT, soms slecht contact op data
#define POT4 A6 //PERFECT

#define LED1 3 // werkt niet meer
#define LED2 9
#define LED3 10
#define LED4 11

#define LEDDEBUG false

#include "Knob.h"
#include "PacketManager.h"

const int knobCount = 4;
Knob knobs[knobCount] = {
    Knob(1, POT1, LED1),
    Knob(2, POT2, LED2),
    Knob(3, POT3, LED3),
    Knob(4, POT4, LED4)
};
PacketManager pm;

void setup()
{
  Serial.begin(9600);
}

int loopState = -1;

void loop()
{
  if(LEDDEBUG){
    pinMode(LED1, OUTPUT);
    pinMode(LED2, OUTPUT);
    pinMode(LED3, OUTPUT);
    pinMode(LED4, OUTPUT);
    analogWrite(LED1, 128);
    analogWrite(LED2, 255);
    analogWrite(LED3, 128);
    analogWrite(LED4, 255);
    return; 
  }

  loopState++;
  if(loopState > 8){
    loopState = 0;
    pm.ping();  
  }
  
  for (int k = 0; k < knobCount; k++) {
    unsigned short prev = knobs[k].getLastRawValue();
    unsigned short val = knobs[k].readValue();

    if(prev != val){
      pm.sendPacket(knobs[k].getId(), val);
    }    
  }

  delay(25);
}
