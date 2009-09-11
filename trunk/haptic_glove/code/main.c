//this header file imports the corresponding IO header file
//	for the atmega 168
#include <avr/io.h>

#include "SerialCom.h"

//we use this for durations, as the headerfile implies
#include <util/delay.h>

//NOTE: serialRead, setup_serial, and serialWrite functions are in SerialCom.c
//NOTE: data_direction_setup and motor_switch functions are in HapticGlove.c

int main () {
	//specify all port directions
	data_direction_setup();
	
	//sets up baud rates, Rx, Tx, etc.
	setup_serial();
	
	//welcomes and prompts user
	menu_display();
	
	//hold the value of serialRead
	char selection[6];
	
	//keep prompting the user once operations are finished
	//	using this infinite while loop
	while (1) {
		//get the first 4 inputs and store them in the
		//	selection character array for use later
		//	so that our motors are activated almost simultaneously
		for (int i = 0; i < 6; i ++) {
			selection[i] = serialRead();
			serialWrite(selection[i]);
		}
		serialWrite('\r');
		serialWrite('\n');
		kill_all_motors();
		
		//we take the sixth character the user input,
		//	convert it to an integer, multiply it by 25
		//	and use the result as the interval of our buzz
		//	for a simulated change in intensity
		
		//0-9 is accepted where 0 is a 25ms interval
		//	and 9 is a 250ms interval
		int intensity = (selection[6] - '0') + 1;
		int delay = intensity * 25;
		//while the user doesn't input another character
		while (serialCheckRxComplete() == 0) {
			//loop through the array of prior user input
			for (int i = 0; i < 5; i ++) {
				switch (selection[i]) {
					case 'a':
						motor_switch(PORTB0, &PORTB, 0);
						break;
					case 'b':
						motor_switch(PORTD7, &PORTD, 0);
						break;
					case 'c':
						motor_switch(PORTD6, &PORTD, 0);
						break;
					case 'd':
						motor_switch(PORTD5, &PORTD, 0);
						break;
					case 'e':
						motor_switch(PORTB7, &PORTB, 0);
						break;
					case 'f':
						motor_switch(PORTB6, &PORTB, 0);
						break;
					case 'g':
						motor_switch(PORTD4, &PORTD, 0);
						break;
					case 'h':
						motor_switch(PORTD3, &PORTD, 0);
						break;
					case 'i':
						motor_switch(PORTD2, &PORTD, 0);
						break;
					case 'j':
						motor_switch(PORTB1, &PORTB, 0);
						break;
					case 'k':
						motor_switch(PORTC0, &PORTC, 0);
						break;
					case 'l':
						motor_switch(PORTC1, &PORTC, 0);
						break;
					case 'm':
						motor_switch(PORTC2, &PORTC, 0);
						break;
					case 'n':
						motor_switch(PORTC3, &PORTC, 0);
						break;
					case '~':
						kill_all_motors();
						break;
					default:
						//if you see an *, you input an invalid  character
						serialWrite('*');
						break;
				}
			}
			//this is the secret sauce
			_delay_ms(delay);
		}

		//welcomes and prompts user until you disconnect
		menu_display();
		
	}

}


