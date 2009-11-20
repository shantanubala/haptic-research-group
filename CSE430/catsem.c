/*
 * catsem.c
 *
 * 30-1-2003 : GWA : Stub functions created for CS161 Asst1.
 *
 * NB: Please use SEMAPHORES to solve the cat syncronization problem in 
 * this file.

Requirements:
Program output during execution. As a minimum, your kprintf statements should indicate:
•	Start of eating
•	Start and end of an eating cycle
•	End of eating
and for each eating cycle the following or a printout with equivalent information for all cats, mice and bowls:
•	cat CATNUMBER beginning to eat at bowl BOWLNUMBER
•	cat CATNUMBER finishing eating at bowl BOWLNUMBER
•	mouse MOUSENUMBER beginning to eat at bowl BOWLNUMBER
•	mouse MOUSENUMBER finishing eating at bowl BOWLNUMBER
Extra credit (2 points)
•	Numbering cycles and printing out the cycle number at the start and end of every eating cycle 
•	Numbering bowls and printing out the bowl number each time a cat or mouse starts and finishes eating at a bowl (1 point)

 */


/*
 * 
 * Includes
 *
 */

#include <types.h>
#include <lib.h>
#include <test.h>
#include <thread.h>


/*
 * 
 * Constants
 *
 */

/*
 * Number of food bowls.
 */

#define NFOODBOWLS 2

/*
 * Number of cats.
 */

#define NCATS 6

/*
 * Number of mice.
 */

#define NMICE 2

/*
 * Declare cat/mouse semaphores
 */

static struct semaphore *waitsem;
static struct semaphore *bowlsem;

/*
 * Declare static volatile variables
 */

static volatile unsigned long catseating;
static volatile unsigned long miceeating;
static volatile unsigned long catsdoneeating;
static volatile unsigned long micedoneeating;
static volatile unsigned int bowllist[NFOODBOWLS];

/*
 * 
 * Function Definitions
 * 
 */


/*
 * catsem()
 *
 * Arguments:
 *      void * unusedpointer: currently unused.
 *      unsigned long catnumber: holds the cat identifier from 0 to NCATS - 1.
 *
 * Returns:
 *      nothing.
 *
 * Notes:
 *      Write and comment this function using semaphores.
 *
 */

static void catsem(void * unusedpointer, 
				   unsigned long catnumber)
{
	int i; //Declare loop variable
	int bowl; 	//Stores current bowl in use   

	//Avoid unused variable warnings.
	(void) unusedpointer;
	
	//Waits until no mice are in the feeding area
	if(miceeating != 0)
		P(waitsem);
	
	//Cats begin eating	
	catseating++;
	if(catseating == 1)
		kprintf("Cats are starting to eat");
	//Cat starts eating
	P(bowlssem); //Take one bowl
	for(i = 0; i < NFOODBOWLS; i++) //Used to assign bowl #
	{
		if(bowllist[i] == 1)
		{
			bowl = i; //This cat takes a bowl
			bowllist[i] = 0; //Remove bowl from list
			break;
		}
	}
	kprintf("cat %u beginning to eat at bowl %u\n", catnumber, bowl+1);
	//Simulate Eating Time, no mice can be eating
	for(i = 0; i < 5000; i++)
		assert(miceeating == 0);
	//Place bowl back into list
	bowllist[bowl] = 1;
	kprintf("cat %u finishing eating at bowl %u\n", catnumber, bowl+1);
	V(bowlssem); //Replace one bowl
	//Cat finished eating
	catsdoneeating++;
	catseating--;
	
	//True when last catsem thread executes this line of code
	if(catsdoneeating == NCATS)
	{
		//Catsem complete, all cats have finished eating
		kprintf("All %u cats have finished eating\n", catsdoneeating);
		//Allow mice to eat if they havn't done so
		waitsem->count = NMICE;
	}   	             	   
}
        

/*
 * mousesem()
 *
 * Arguments:
 *      void * unusedpointer: currently unused.
 *      unsigned long mousenumber: holds the mouse identifier from 0 to 
 *              NMICE - 1.
 *
 * Returns:
 *      nothing.
 *
 * Notes:
 *      This function will be called NMICE times. Thus upon exit  
 *	of this function that particular mouse has fed. This
 *	simulation also waits indefinately for the condition	
 *
 */

static void mousesem(void * unusedpointer, 
					 unsigned long mousenumber)
{
	int i;	//Declare loop variable
	int bowl; 	//Stores current bowl in use

	//Avoid unused variable warnings.
	(void) unusedpointer;

	//Waits until no cats are in the feeding area
	if(catseating != 0)
		P(waitsem);

	//Mice begin eating
	miceeating++;
	if(miceeating == 1)
		kprintf("Mice are starting to eat");
	//Mouse starts eating
	P(bowlssem); //Take one bowl
	for(i = 0; i < NFOODBOWLS; i++) //Used to assign bowl #
	{
		if(bowllist[i] == 1)
		{
			bowl = i; //This mouse takes a bowl
			bowllist[i] = 0; //Remove bowl from list
			break;
		}
	}
	kprintf("mouse %u beginning to eat at bowl %u\n", mousenumber, bowl+1);
	//Simulate Eating Time, no cats can be eating
	for(i = 0; i < 5000; i++)
		assert(catseating == 0);
	//Place bowl back into list
	bowllist[bowl] = 1;
	kprintf("mouse %u beginning to eat at bowl %u\n", mousenumber, bowl+1);
	V(bowlssem); //Replace one bowl
	//Mouse Finished eating
	micedoneeating++;
	miceeating--;
		
	//True when last mousesem thread executes this line of code
	if(micedoneeating == NMICE) 
	{
		//Mousesem complete, all mice have finished eating
		kprintf("All %u mice have finished eating\n", micedoneeating);
		//Allow cats to eat if they haven't done so
		waitsem->count = NCATS;
	}
}


/*
 * catmousesem()
 *
 * Arguments:
 *      int nargs: unused.
 *      char ** args: unused.
 *
 * Returns:
 *      0 on success.
 *
 * Notes:
 *      Driver code to start up catsem() and mousesem() threads.  Change this 
 *      code as necessary for your solution.
 */

int
catmousesem(int nargs,
            char ** args)
{
	int index, error;

	// Initialize static semaphores
	waitsem = sem_create("waitsem", 0);
	bowlsem = sem_create("bowlsem", NFOODBOWLS);	   

	// Initialize static variables
	catseating = 0;
	miceeating = 0;
	catsdoneeating = 0;
	micedoneeating = 0;
	
	// Initialize static foodbowl array
	for (index = 0; index < NFOODBOWLS; index++)
		bowllist[index] = 1; //Sets all foodbowls as available
	/*
	 * Avoid unused variable warnings.
	 */

	(void) nargs;
	(void) args;

	/*
	 * Start NCATS catsem() threads.
	 */

	for (index = 0; index < NCATS; index++) 
	{
		error = thread_fork("catsem Thread", 
					NULL, index, catsem, NULL);
	        
			/*
			 * panic() on error.
			 */

			if (error) 
				panic("catsem: thread_fork failed: %s\n", 
					strerror(error));
	}

	/*
	 * Start NMICE mousesem() threads.
	 */

	for (index = 0; index < NMICE; index++) 
	{
		error = thread_fork("mousesem Thread", 
					NULL, index, mousesem, NULL);
	        
			/*
			 * panic() on error.
			 */

			if (error)          
				panic("mousesem: thread_fork failed: %s\n", 
					strerror(error));
	}
	return 0;
}


/*
 * End of catsem.c
 */
