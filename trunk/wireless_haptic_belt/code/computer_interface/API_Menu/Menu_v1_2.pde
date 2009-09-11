/*Author: Kristopher Blair (Haptic-Research-Group)
 *Date: March 19nd, 2009
 *
 *Description: Menu display for Arduino controller, for use with
 *haptic belt research project, Arizona State University.
 *
 ***INCOMPLETE*** March 07, 2009
 */
 
#include <avr/pgmspace.h>

int debug_mode = 0; /*Debug mode flag*/
int power_svr = 0; /*Power Saver flag*/
int command_byte = 0; /*Command Byte used for storing*/
int check = 0; /*Breaking ascii value (offset by -48) from int number(int)*/

/*These are blocks of characters that are loaded into flash
 *and not SRAM, due to the 512 byte SRAM limitation*/
const prog_uchar actionMessage[] PROGMEM  = {
  "\r\nAction Mode:\r\n"
  "\t1.) Back to Learning Mode.\r\n"
  "To Activate Motor Enter:\r\n"
  "Rhythm.Magnitude.Duration.Motor#\r\n"
  "(A-E).(1-4).(#ms).(1-16)\r\n"};
const prog_uchar eepromMessage[] PROGMEM  = {
  "\r\nEEPROM Read/Write Mode:\r\n"
  "\t1.) Back to Learning Mode.\r\n"
  "\t2.) Read EEPROM.\r\n"
  "\t3.) Write EEPROM.\r\n"
  "\t4.) Clear EEPROM.\r\n"};
const prog_uchar learningMessage[] PROGMEM  = {
  "\r\nLearning Mode:\r\n"
  "\t1.) Go to Action Mode.\r\n"
  "\t2.) Setup/Test Pulse Width Modulation.\r\n"
  "\t3.) Read/Write EEPROM.\r\n"
  "\t4.) Spatio/Temporal Setup.\r\n"
  "\t5.) Debug Mode Enable/Disable.\r\n"
  "\t6.) Powersaving Mode Enabled/Disabled.\r\n"};
const prog_uchar magnitudeMessage[] PROGMEM  = {
  "\r\nMagnitude Mode.\r\n"
  "\t1.) Back to Pulse Width Modulation Setup/Test Mode.\r\n"
  "\t2.) Back to Learning Mode.\r\n\n"
  "To Configure Magnitude Enter:\r\n"
  "Magnitude#.Percentage\r\n"
  "(1-4).(1-100)\r\n"};
const prog_uchar pwmMessage[] PROGMEM  = {  
  "\r\nPulse Width Modulation Setup/Test Mode.\r\n"
  "\t1.) Back to Learning Mode.\r\n"
  "\t2.) Rythm Setup/Test.\r\n"
  "\t3.) Magnitude Setup/Test.\r\n"};
const prog_uchar rythmMessage[] PROGMEM  = {
  "\r\nRythm Mode.\r\n"
  "\t1.) Back to Learning Mode.\r\n"
  "\t2.) Back to Pulse Width Modulation Setup/Test Mode.\r\n\n" 
  "To Configure Rythm Enter:\r\n"
  "Rhythm.On Duration.Off Duration#\r\n"
  "(A-E).(#ms).(#ms)\r\n\n"
};

void print_flash(const prog_uchar str[])
{
  char c;
  if(!str) return;
  while((c = pgm_read_byte(str++))) Serial.print(c,BYTE);
}

/*Keeps accepting input until a return carriage is found then
 *display default invalid display message*/
void invalid(){
  while(true){
    if(check == 13) break;
    while(!Serial.available()) {} 
    check = Serial.read();
    Serial.print(check, BYTE);
  }
  Serial.println("Invalid Selection!\n"); 
}
/*Calculates number from serial read left to right returns
 *the number. Check will contain the breaking character.*/
int number(int accum){
  while(true){ 
    /*Wait for more input*/
    while(!Serial.available()) {} 
    check = Serial.read();
    Serial.print(check, BYTE); /*Echo Print*/
    if(check < 48 || check > 57) break; /*Not A Digit*/
    else accum = accum*10 + (check - 48); /*Digit*/
  }
  return accum;
}

/*Displays actionList commands and reads in user selection*/
void actionList(){ 
  print_flash(actionMessage);
  command_byte = number(0); /*Has int number(int) grab the field*/
  if(check == 13 || (check > 64 && check < 70) || (check > 96 && check < 102)) actionSelection(command_byte); /*Return carriage '\r found*/
  else {
    invalid();
    actionList();
  }
}
/*Performs the actionList command specified*/
void actionSelection(int sel){
  boolean valid = false; /*Used to detect valid/invalid Activation format*/
  switch(sel){
    /*Expecting a valid format (A-E).(1-4).(#ms).(1-16)*/    
    case 0: /*Purify each field, detect incorrect formats*/      
      sel = check % 32; /*(converts a-e & A-E uniformly to 1-5)*/
      if(number(0) == 0){ /*Advances input one character, if digit it aborts*/       
        if(check == 46){ /*Check for '.'*/
          /*Use sel as Rhythm HERE, before next line*/
          sel = number(0); /*Gets a number*/
          if(sel > 0 && sel < 5){ /*Check for 1-4*/
            if(check == 46){ /*Check for '.'*/
              /*Use sel as Magnitude HERE, before next line*/             sel = number(0); /*Gets a number*/
              if(check == 46){ /*Check to see if a '.' was present*/
                /*Use sel as Duration HERE, before next line*/
                sel = number(0);
                if(sel > 0 && sel < 17){
                  if(check == 13){ /*Check for '\r'*/
                    /*Use sel as Motor Number*/
                    /*Process all Fields*/
                    valid = true;
                  }
                }    
              }                  
            }
          }    
        }
      }
      if(valid == false) { 
        invalid();
        actionList();
      }
      else{
        command_byte = number(0); /*Has int number(int) grab the field*/
        if(check == 13 || (check > 64 && check < 70) || (check > 96 && check < 102)) actionSelection(command_byte); /*Return carriage '\r found*/
        else {
          invalid();
          actionList();
        }
      }   
      break;
      /*User Selected back to Learning Mode, by breaking we actually go back to learning mode.*/
    case 1:
      break;
    default:
      invalid();
      actionList();
      break;
  }
}

/*Displays EEPROM options list*/
void eepromList(){
  print_flash(eepromMessage);
  /*Wait for command*/
  command_byte = number(0);
  if(check == 13) eepromSelection(command_byte); /*Return carriage '\r found*/
  else {
    invalid();
    eepromList();
  }
}

/*Performs the eepromList command specified*/
void eepromSelection(int sel){
  switch(sel){
    /*User Selected back to Learning Mode, breaking will take us back to learning mode*/
    case 1:
      break;
    /*User Selected Read EEPROM*/
    case 2:
      break;
    /*User Selected Write EEPROM*/
    case 3:
      break;
    /*User Selected Clear EEPROM*/
    case 4:
      break;
    default:
      invalid();
      eepromList();
    break;
  }
}

/*Displays learningList commands and reads in user selection*/
void learningList(){
  print_flash(learningMessage);
  command_byte = number(0); /*Has int number(int) grab the field*/
  if(check == 13) {
    learningSelection(command_byte); /*Return carriage '\r found*/
  }
  else {
    invalid();
    learningList();
  }
}
/*Performs the learningList command specified*/
void learningSelection(int sel){
  switch(sel){
    /*User Selected go to Action Mode*/
    case 1:
      actionList();
      break;
    /*User Selected Test/Setup PWM*/
    case 2:
      pwmList();
      break;
    /* User Selected Read/Write EEPROM Mode*/
    case 3:
      eepromList();
      break;
    /* User Selected Disable/Enable Debug Mode*/
    case 4:
      break;
    /* User Selected Disable/Enable Debug Mode*/
    case 5:
      if(debug_mode == 0) {
        debug_mode = 1;
        Serial.println("Debug Mode Enabled.\n");
      }
      else
      {
        debug_mode = 0;
        Serial.println("Debug Mode Disabled.\n");
      }
      break;
    case 6:
      if(power_svr == 0) {
        power_svr = 1;
        Serial.println("Power Saver Mode Enabled.\n");
      }
      else
      {
        power_svr = 0;
        Serial.println("Power Saver Mode Disabled.\n");
      }
      break; 
    default:
      invalid();
    break;
  }
}

/*Needs Work*/
void magnitudeList(){
  print_flash(magnitudeMessage);
  /*Wait for command*/
  command_byte = number(0);
  if(check == 13 || check == 46) magnitudeSelection(command_byte);
  else {
    invalid();
    magnitudeList();
  }
}
void magnitudeSelection(int sel){
  boolean valid = false;
  /*User selected menu option*/
  if(check == 13){
    switch(sel){
      /*User Selected back to Learning Mode, breaking will return us to learning mode.*/
      case 1:
        break;
      /*User Selected Rythm Setup/Test*/
      case 2:
        pwmList();
        break;
      /*User Selected Magnitude Setup/Test*/
      default:
        invalid();
        rythmList();
      break;
    }
  }
  /*Magnitude Entry*/
  else{
    if(sel > 0 && sel < 5){
      /*Use sel as Magnitude #*/
      sel = number(0);
      if(sel > 0 && sel < 101){
        if(check == 13){
          /*Use sel as Percentage*/
          valid = true;
        }
      }
    }
    if(valid == false){
      invalid();
      magnitudeList();  
    }
    else{
      command_byte = number(0);
      if(check == 13 || check == 46) magnitudeSelection(command_byte);
      else {
        invalid();
        magnitudeList();
      }   
    }
  }  
}

/*Displays pwmList options list*/
void pwmList(){
  print_flash(pwmMessage);
  /*Wait for command*/
  command_byte = number(0);
  if(check == 13) pwmSelection(command_byte);
  else{
    invalid();
    pwmList();
  }
}
/*Performs the pwmList command specified*/
void pwmSelection(int sel){
  switch(sel){
    /*User Selected back to Learning Mode, breaking will return us to learning mode.*/
    case 1:
      break;
    /*User Selected Rythm Setup/Test*/
    case 2:
      rythmList();
      break;
    /*User Selected Magnitude Setup/Test*/
    case 3:
      magnitudeList();
      break;
    default:
      invalid();
      pwmList();
    break;
  }   
}

/*Displays rythmList options list*/
void rythmList(){
  print_flash(rythmMessage);
  /*Wait for command*/
  command_byte = number(0);
  /*Check for '\r' or a-e or A-E*/
  if(check == 13 || (check > 64 && check < 70) || (check > 96 && check < 102)) rythmSelection(command_byte);
  else {
    invalid();
    rythmList();
  }
}
/*Performs the rythmList command specified*/
void rythmSelection(int sel){
  boolean valid = false;
  switch(sel){
    case 0:
      sel = check % 32; /*(converts a-e & A-E uniformly to 1-5)*/
      if(number(0) == 0){ /*Advances input one character, if digit it aborts*/        
        if(check == 46){ /*Check for '.'*/
          /*Use sel as Rhythm, HERE before next line*/
          sel = number(0); /*Gets a number*/
          if(check == 46){
            /*Use sel as On Duration, HERE before next line*/
            sel = number(0);
            if(check == 13){
              /*Use sel as Off Duration*/
              valid = true;
              /*Process Command*/
            }
          }
        }
      }
      if(valid == false){
        invalid(); 
        rythmList();  
      }
      else{
        command_byte = number(0);
        /*Check for '\r' or a-e or A-E*/
        if(check == 13 || (check > 64 && check < 70) || (check > 96 && check < 102)) rythmSelection(command_byte);
        else {
          invalid();
          rythmList();
        }  
      }
      break;
    /*User Selected back to Learning Mode*/
    case 1:
      break;
    /*User Selected Rythm Setup/Test*/
    case 2:
      pwmList(); 
      break;
    /*User Selected Magnitude Setup/Test*/
    default:
      invalid();
      rythmList();
    break;
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

/*Start of menu, prompts user for Manual setup option*/
void loop(){
  learningList(); /*Call Learning Mode*/ 
}
