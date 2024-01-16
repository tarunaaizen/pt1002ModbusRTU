#include <Maxim_MAX31865.h>
#include <SimpleModbusSlave.h>

enum 
{     
  RTD0_VAL,                 //reg0
  RTD1_VAL,                 //reg1               
  RTD2_VAL,                 //reg2   
  RTD3_VAL,                 //reg3
  RTD4_VAL,                 //reg4
  FINGER_VAL,               //reg5
  RADAR_VAL,                //reg6
  HOLDING_REGS_SIZE // leave this one
};

unsigned int holdingRegs[HOLDING_REGS_SIZE]; // function 3 and 16 register array


Maxim_MAX31865 thermo = Maxim_MAX31865(5, 23, 19, 18); //SPI: CS, DI, DO, CLK
#define RREF      430.0
#define RNOMINAL  100.0

void setup() {
  Serial.begin(9600);
  Serial2.begin(38400);
  modbus_configure(&Serial2, 38400, SERIAL_8N2, 1, 2, HOLDING_REGS_SIZE, holdingRegs);
  modbus_update_comms(38400, SERIAL_8N2, 10);

  thermo.begin(MAX31865_2WIRE);

}


void loop() {
  modbus_update();
  uint16_t rtd = thermo.readRTD();
  float ratio = rtd;
  ratio /= 32768;
  holdingRegs[RTD1_VAL] = thermo.temperature(RNOMINAL, RREF);
  Serial.print("Temperature = "); Serial.println(thermo.temperature(RNOMINAL, RREF));
  delay(10);
}