Version 1.3 is different that prevvious version take note:

At startup you can enter any learning command, or ask for the format of commands.
After entering the command START, you can no longer go back to learning mode,
as the firmware doesn't support this yet.

The debug menu only accepts capital characters A-Z, digits 0-9, spaces and return cariages.
All invalid characters will be ignored, and not be echoed. Furthermore, all characters after
a length of 34 will be ignored since this is the maximum possible valid string.

There is no correction testing on learning, testing commands, Jon said not to do any parsing,
or error checking.