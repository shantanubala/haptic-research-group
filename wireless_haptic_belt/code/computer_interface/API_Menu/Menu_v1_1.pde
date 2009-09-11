/*Author: Kristopher Blair (Haptic-Research-Group)
 *Date: March 7nd, 2009
 *
 *Description: Menu display for Arduino controller, for use with
 *haptic belt research project, Arizona State University.
 *
 ***INCOMPLETE*** March 07, 2009
 */
 
int debug_mode = 0; /*Debug mode flag*/
int power_svr = 0; /*Power Saver flag*/
int command_byte = 0; /*Command Byte used for storing*/
int check = 0; /*Breaking ascii value (offset by -48) from int number(int)*/

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

/*Default invalid display message*/
void invalid(){
  Serial.println("Invalid Selection!");  
}
/*Calculates number from serial read left to right returns
 *the number. Check will contain the breaking character.*/
int number(int accum){
  Serial.flush(); /*Clears input buffer*/
  while(true){ 
    /*Wait for more input*/
    while(!Serial.available()) {} 
    check = Serial.read();
    Serial.print((char)check); /*Echo Print*/
    if(check < 48 || check > 57) break; /*Not A Digit*/
    else accum = accum*10 + (check - 48); /*Digit*/
  }
  return accum;
}

/*Waits and gets returns next byte from serial and 
 *echo prints it. Check also takes this byte.*/
int next(){
  Serial.flush(); /*Clears input buffer*/
  while(!Serial.available()){}
  check = Serial.read();
  Serial.print((char)check); /*Echo Print*/
  return check;
}

/*Performs the rythmList command specified*/
void rythmSelection(int sel){
  boolean valid = false;
  switch(sel){
    case 0:
      sel = check % 32; /*(converts a-e & A-E uniformly to 1-5)*/         
      if(next() == 46){ /*Check for '.'*/
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
      Serial.println("");
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
      learningList();
      Serial.println("");
      break;
    /*User Selected Rythm Setup/Test*/
    case 2:
      pwmList();
      Serial.println("");
      break;
    /*User Selected Magnitude Setup/Test*/
    default:
      invalid();
      rythmList();
    break;
  }  
}

/*Displays rythmList options list*/
void rythmList(){
  Serial.println("");
  Serial.println("Rythm Mode.");
  Serial.println("\t1.) Back to Learning Mode.");
  Serial.println("\t2.) Back to Pulse Width Modulation Setup/Test Mode.");  
  Serial.println("");
  Serial.println("To Configure Rythm Enter:");
  Serial.println("Rhythm.On Duration.Off Duration#");
  Serial.println("(A-E).(#ms).(#ms)"); 
  Serial.println("");
  /*Wait for command*/
  command_byte = number(0);
  /*Check for '\r' or a-e or A-E*/
  if(check == 13 || (check > 64 && check < 70) || (check > 96 && check < 102)) rythmSelection(command_byte);
  else {
    invalid();
    rythmList();
  }
}

void magnitudeSelection(int sel){
  boolean valid = false;
  /*User selected menu option*/
  if(check == 13){
    switch(sel){
      /*User Selected back to Learning Mode*/
      case 1:
        learningList();
        Serial.println("");
        break;
      /*User Selected Rythm Setup/Test*/
      case 2:
        pwmList();
        Serial.println("");
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
    Serial.println("");
    if(valid == false){
      magnitudeList();
      invalid();  
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

/*Needs Work*/
void magnitudeList(){
  Serial.println("");
  Serial.println("Magnitude Mode.");
  Serial.println("\t1.) Back to Pulse Width Modulation Setup/Test Mode.");
  Serial.println("\t2.) Back to Learning Mode.");
  Serial.println("");
  Serial.println("To Configure Magnitude Enter:");
  Serial.println("Magnitude#.Percentage");
  Serial.println("(1-4).(1-100)"); 
  Serial.println(""); 
  /*Wait for command*/
  command_byte = number(0);
  if(check == 13 || check == 46) magnitudeSelection(command_byte);
  else {
    invalid();
    magnitudeList();
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

/*Displays pwmList options list*/
void pwmList(){
  Serial.println("");
  Serial.println("Pulse Width Modulation Setup/Test Mode.");
  Serial.println("\t1.) Back to Learning Mode.");
  Serial.println("\t2.) Rythm Setup/Test."); /*Use 100% maginutde or 50% or some standard*/
  Serial.println("\t3.) Magnitude Setup/Test."); /*Use continuous vibration*/
  Serial.println("");
  Serial.flush();
  /*Wait for command*/
  command_byte = number(0);
  Serial.println("");
  if(check == 13) pwmSelection(command_byte);
  else{
    invalid();
    pwmList();
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

/*Displays EEPROM options list*/
void eepromList(){
  Serial.println("");
  Serial.println("EEPROM Read/Write Mode:");
  Serial.println("\t1.) Back to Learning Mode.");
  /*We need to specify by what quantities we want to read/write etc*/
  Serial.println("\t2.) Read EEPROM.");
  Serial.println("\t3.) Write EEPROM.");
  Serial.println("\t4.) Clear EEPROM.");
  Serial.println("");
  /*Wait for command*/
  command_byte = number(0);
  Serial.println("");
  if(check == 13) eepromSelection(command_byte); /*Return carriage '\r found*/
  else {
    invalid();
    eepromList();
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
    case 5:
      if(debug_mode == 0) {
        debug_mode = 1;
        Serial.println("Debug Mode Enabled.");
      }
      else
      {
        debug_mode = 0;
        Serial.println("Debug Mode Disabled.");
      }
      learningList();
      break;
    case 6:
      if(power_svr == 0) {
        power_svr = 1;
        Serial.println("Power Saver Mode Enabled.");
      }
      else
      {
        power_svr = 0;
        Serial.println("Power Saver Mode Disabled.");
      }
      learningList();
      break; 
    default:
      invalid();
    break;
  }
}
/*Displays learningList commands and reads in user selection*/
void learningList(){
  Serial.println("");
  Serial.println("Learning Mode:");
  Serial.println("\t1.) Go to Action Mode.");
  Serial.println("\t2.) Setup/Test Pulse Width Modulation.");
  Serial.println("\t3.) Read/Write EEPROM.");
  Serial.println("\t4.) Spatio/Temporal Setup.");
  Serial.println("\t5.) Debug Mode Enable/Disable.");
  Serial.println("\t6.) Powersaving Mode Enabled/Disabled");
  Serial.println("");
  command_byte = number(0); /*Has int number(int) grab the field*/
  Serial.println("");
  if(check == 13) learningSelection(command_byte); /*Return carriage '\r found*/
  else {
    invalid();
    learningList();
  }
}
/*Performs the actionList command specified*/
void actionSelection(int sel){
  boolean valid = false; /*Used to detect valid/invalid Activation format*/
  switch(sel){
    /*Expecting a valid format (A-E).(1-4).(#ms).(1-16)*/    
    case 0: /*Purify each field, detect incorrect formats*/      
      sel = check % 32; /*(converts a-e & A-E uniformly to 1-5)*/         
      if(next() == 46){ /*Check for '.'*/
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
      Serial.println("");
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
      Serial.println("");
      break;
    default:
      invalid();
      actionList();
      break;
  }
}
/*Displays actionList commands and reads in user selection*/
void actionList(){ 
  Serial.println("");
  Serial.println("Action Mode:");
  Serial.println("\t1.) Back to Learning Mode.");
  Serial.println("");
  Serial.println("To Activate Motor Enter:");
  Serial.println("Rhythm.Magnitude.Duration.Motor#");
  Serial.println("(A-E).(1-4).(#ms).(1-16)");
  command_byte = number(0); /*Has int number(int) grab the field*/
  if(check == 13 || (check > 64 && check < 70) || (check > 96 && check < 102)) actionSelection(command_byte); /*Return carriage '\r found*/
  else {
    invalid();
    actionList();
  }
}
