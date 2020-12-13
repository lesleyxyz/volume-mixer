class Knob{
    public:
        Knob(unsigned char id, uint8_t knobPin, uint8_t ledPin);
        unsigned char getId();
        unsigned short getLastRawValue();
        unsigned short readValue();
    private:
        unsigned char _id;
        unsigned short _lastRawValue;
        uint8_t _knobPin;
        uint8_t _ledPin;
};
