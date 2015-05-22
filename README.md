# HD44780-Windows-IoT-Library
A library which allows Windows IoT devices with GPIO pins to control displays which use the HD44780 controller

## Initialisation
You will first have to instantiate the library then call the initialisation method to initialise the library.
The arguments are the pins which the display is connected to and are as follows:
Register select pin, read/write pin, the enable pin then pins d7-d0 (reverse order according to the specifications found [here](http://en.wikipedia.org/wiki/Hitachi_HD44780_LCD_controller).

	HD44780Library display = new HD44780Library();
	display.init(rs, rw, enable, d7, d6, d5, d4, d3, d2, d1, d0);

This will initialise the library with the following settings by default:
- The display will be turned on
- The cursor will be visible and not blinking
- It will run in 8 bit connection mode (4-bit has yet to be implemented)
- It will run with 2 lines of text
- The font size will be 5x8 pixels

## Configuration
The initial settings can be changed either before or after initialisation with these methods. If changed before, they will not take effect until the display has been initialised.

	TurnDisplayOff();
	TurnCursorOn();
	TurnCursorOff();
	StartCursorBlinking();
	StopCursorBlinking();
	ClearDisplay();
	SwitchTo8BitMode();
	SwitchTo4BitMode();
	SwitchToDoubleLines();
	SwitchToSingleLines();
	SwitchTo5x8Font();
	SwitchTo5x10Font();

## Utilisation
Text can be written using the `Write();` command so for instance if you want to write "Hello World" to the screen, you would use `Write("Hello World");`. 
The command `WriteLine();` does the same but moves to the second line afterwards (if the display is capable of displaying two lines).

To move the cursor to a specific position on the screen, you can use the `SetCursorPosition(row, column);` command where `row` is the row you want to and `column` 
is the column you want to move to. If the display is configured for single line display then the `row` parameter will have no effect.