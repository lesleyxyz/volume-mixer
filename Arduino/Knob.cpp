#include "Arduino.h"
#include "Knob.h"
#define KNOBDEBUG false

Knob::Knob(unsigned char id, uint8_t knobPin, uint8_t ledPin){
    _id = id;
    _knobPin = knobPin;
    pinMode(ledPin, OUTPUT);
    _ledPin = ledPin;
}

unsigned char Knob::getId(){
    return _id;
}

unsigned short Knob::getLastRawValue(){
    return _lastRawValue;
}

unsigned short Knob::readValue(){
    _lastRawValue = analogRead(_knobPin) >> 3;
    //double devider = 127 / 127; // max knob value devided by max led value
    analogWrite(_ledPin, _lastRawValue);

    if(KNOBDEBUG){
      Serial.print(_id);
      Serial.print("\n"); 
      Serial.print(_lastRawValue);
      Serial.print("\n");
    }
    
    return _lastRawValue;
}
