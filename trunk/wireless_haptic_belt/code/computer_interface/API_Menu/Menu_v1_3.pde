/*Author: Kristopher Blair (Haptic-Research-Group)
 *Date: April 22nd, 2009
 *
 *Description: Menu display for Arduino controller, for use with
 *haptic belt research project, Arizona State University.
 *
 */
#include <avr/pgmspace.h>
#define buf_size 35

char buffer[buf_size]; /*Command is max length of possible valid string*/
int check = 0; /*Breaking ascii value (offset by -48) from int number(int)*/

/*These are blocks of characters that are loaded into flash
 *and not SRAM, due to the 512 byte SRAM limitation*/
const prog_uchar actionMessage[] PROGMEM  = {
  "\r\nAction Mode:\r\n"
  "To Activate Motor Enter:\r\n"
  "<Motor #> <Rhythm> <Magnitude> <Duration>\r\n\n"
  "Fields\t\tParameters\r\n"
  "<Motor #>\t1-16\r\n"
  "<Rhythm>\tA-H\r\n"
  "<Magnitude>\t1-4\r\n"
  "<Duration>\t1-8\r\n"
  "Type HELP to redisplay this menu.\r\n\n"};
const prog_uchar learningMessage[] PROGMEM  = {
  "\r\nLearning Mode:\r\n"
  "Commands:\r\n"
  "\t1) Display Rhythm Commands.\r\n"
  "\t2) Display Magnitude Commands.\r\n"
  "Type START to enter activation mode.\r\n"
  "Type QUERY to query current settings.\r\n"
  "Type HELP to redisplay this menu.\r\n\n"};
const prog_uchar rhythmMessage[] PROGMEM  = {
  "Rhythm Commands:\r\n"
  "TEST RHYTHM <Pattern> <Bits>\r\n"
  "LEARN RHYTHM <ID> <Pattern> <Bits>\r\n\n"
  "Fields\t\tParameters\r\n"
  "<ID>\t\tA-H\r\n"
  "<Pattern>\t16 Hex Values\r\n"
  "<Bits>\t\t1-64\r\n\n"
};
const prog_uchar magnitudeMessage[] PROGMEM  = {
  "Magnitude Commands:\r\n"
  "TEST MAGNITUDE <Period> <Duty Cycle>\r\n"
  "LEARN MAGNITUDE <ID> <Period> <Duty Cycle>\r\n\n"
  "Fields\t\tParameters\r\n"
  "<ID>\t\t1-4\r\n"
  "<Period>\t0-65535\r\n"
  "<Duty Cycle>\t0-65535\r\n\n"
};


/*Sends command over I2C to atTiny, and recieves
 *a response in the buffer.*/
void sendCommand()
{
  /*FIX ME Parse command and send via I2C*/
}

/*Determines of both character arrays are equal until
 *the null byte character is reached.*/
boolean equals(char *s, char *t)
{
  int i = 0;
  while(s[i] == t[i])
  {
    if(s[i] == '\0') return true;
    i++;
  }  
  return false;
}

void print_flash(const prog_uchar str[])
{ 
  char c;
  if(!str) return;
  while((c = pgm_read_byte(str++))) Serial.print(c,BYTE);
}

/*Gets a command, accepting only valid character types*/
void getCommand()
{
  int buf_ptr = 0;
  while(true){ 
    /*Wait for more input*/
    while(!Serial.available()) {} 
    check = Serial.read();
    if(check == 13) 
    {
      Serial.print(check, BYTE); /*Echo Print*/
      Serial.print('\n', BYTE);  /*Print Newline*/
      buffer[buf_ptr] = '\0';
      /*Note Buf_point = size of array now*/
      break;
    }
    /*Digit, Capital Character, or space*/
    else if((check > 47 && check < 58) || (check > 54 && check < 91) || check == 32)
    {
      /*Stop accepting characters at max length of char array*/
      if(buf_ptr < buf_size)
      {
        Serial.print(check, BYTE); /*Echo Print*/
        buffer[buf_ptr] = (char)check;
        buf_ptr++;
      }
    }   
  }
}

/*Initialization Procedures*/
void setup(){
  Serial.begin(9600);
  Serial.println("Startup...");
  /*Initialize Serial Port*/
  /*Call startup procedures*/
  /*IRQ*/
  /*I2C*/
  /*Wireless*/
  /*EEPROM-(Last Config)*/
}

/*Start of menu*/
void loop(){
  print_flash(learningMessage);
  /*Inside Learning Mode*/
  while(true)
  {
    getCommand();  /*Populates Buffer*/
    if(equals(buffer,"1")) print_flash(rhythmMessage);
    else if(equals(buffer,"2")) print_flash(magnitudeMessage);
    else if(equals(buffer,"HELP")) print_flash(learningMessage);
    else if(equals(buffer,"START")) break; /*Goto Action Mode*/
    else sendCommand();
  }
  sendCommand(); /*Sends START command to belt*/
  print_flash(actionMessage);
  /*Perminately stays in Action Mode*/
  while(true)
  {
    getCommand(); /*grab the field*/
    if(equals(buffer,"HELP"))
      print_flash(actionMessage);
    else
      sendCommand();
  }
}
