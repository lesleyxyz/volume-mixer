#include "Arduino.h"
#include "PacketManager.h"

/*
    How are packets formatted?
    Each packet is 8 bytes
    First 4 bytes it the header
========================================
HEADER:
 First 3 bytes is my name Lesley, in hex "1e51e4"
 4th, last byte of header, is 0x00 for ping and 0xaa for volume change

PING:
 Byte 5-8 is always 0x00 00 00 00a (last byte 0x0a = \n)

VOLUME CHANGE:
 Byte 5 is unique ID of knob
 Byte 6 & 7 is the value of the knob
 Byte 8 is 0x0a = \n for ending the packet
*/
unsigned char _ping[] = {0x1e, 0x51, 0xe4, 0x00, 0xFF, 0x88, 0xE2, 0x0a};

PacketManager::PacketManager(){
    //Serial.begin(9600);
}

void PacketManager::sendPacket(unsigned char id, unsigned short value){
    unsigned char _packet[] = {0x1e, 0x51, 0xe4, 0xaa, id, id ^ value, value, 0x0a};
    Serial.write(_packet, sizeof(_packet));
}

void PacketManager::ping(){
    Serial.write(_ping, sizeof(_ping));
}
