using System.Collections;

namespace LiquidCrystalLibrary
{
	public static class InstructionDefinitions
	{
		private static readonly BitArray clearDisplay = new BitArray(new bool[]
			{
				false,  //RS
				false,  //RW
				false,  //DB7
				false,  //DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				false,  //DB1
				true    //DB0
			});

		private static readonly BitArray returnHome = new BitArray(new bool[]
			{
				false,  //RS
				false,  //RW
				false,  //DB7
				false,  //DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				true,   //DB1
				true    //DB0
			});

		private static readonly BitArray entryModeSet = new BitArray(new bool[]
			{
				false,          //RS
				false,          //RW
				false,          //DB7
				false,          //DB6
				false,          //DB5
				false,          //DB4
				false,          //DB3
				true,           //DB2
				false,			//DB1
				true			//DB0
			});

		private static readonly BitArray displayOnOff = new BitArray(new bool[]
			{
				false,	//RS
				false,	//RW
				false,	//DB7
				false,	//DB6
				false,	//DB5
				false,	//DB4
				true,	//DB3
				true,	//DB2
				false,	//DB1
				false	//DB0
			});

		private static readonly BitArray cursorDisplayShift = new BitArray(new bool[]
			{
				false,	//RS
				false,	//RW
				false,	//DB7
				false,	//DB6
				false,	//DB5
				true,	//DB4
				false,  //DB3
				false,	//DB2
				false,	//DB1
				false	//DB0
			});

		private static readonly BitArray functionSet = new BitArray(new bool[]
			{
				false,	//RS
				false,	//RW
				false,	//DB7
				false,	//DB6
				true,	//DB5
				false,	//DB4
				false,	//DB3
				false,	//DB2
				false,	//DB1
				false	//DB0
			});

		private static readonly BitArray setCGRAMAddress = new BitArray(new bool[]
			{
				false,  //RS
				false,  //RW
				false,  //DB7
				true,   //DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				false,  //DB1
				false   //DB0
			});

		private static readonly BitArray setDDRAMAddress = new BitArray(new bool[]
			{
				false,  //RS
				false,  //RW
				true,	//DB7
				false,	//DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				false,  //DB1
				false   //DB0
			});

		private static readonly BitArray readBusyFlagAddressCounter = new BitArray(new bool[]
			{
				false,  //RS
				true,   //RW
				false,  //DB7
				false,  //DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				false,  //DB1
				false   //DB0
			});

		private static readonly BitArray writeData = new BitArray(new bool[]
			{
				true,	//RS
				false,	//RW
				false,  //DB7
				false,  //DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				false,  //DB1
				false   //DB0
			});

		private static readonly BitArray readData = new BitArray(new bool[]
			{
				true,	//RS
				true,   //RW
				false,  //DB7
				false,  //DB6
				false,  //DB5
				false,  //DB4
				false,  //DB3
				false,  //DB2
				false,  //DB1
				false   //DB0
			});

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 0 / DB5: 0 / DB4: 0 / DB3: 0 / DB2: 0 / DB1: 0 / DB0:1 
		/// </summary>
		/// <returns>The command to clear the display and set the DDRAM address to 0 in the address counter</returns>
		public static BitArray CLEAR_DISPLAY()
			{
				return new BitArray(clearDisplay);
			}

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 0 / DB5: 0 / DB4: 0 / DB3: 0 / DB2: 0 / DB1: 1 / DB0:1 
		/// </summary>
		/// <returns>The command to set the DDRAM address to 0 in the address counter. Also returns display from being shifted to original position. DDRAM content remains unchanged.</returns>
		public static BitArray RETURN_HOME()
		{
			return new BitArray(returnHome);
        }

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 0 / DB5: 0 / DB4: 0 / DB3: 0 / DB2: 1 / DB1: I/D / DB0:S
		/// </summary>
		/// <param name="INCREMENT">True: Incremental shift, False: Decremental shift</param>
		/// <param name="DISPLAY_SHIFT">Not sure what this does if false, usually true</param>
		/// <returns>The command to set the cursor move direction and specifies display shift. These operations are performed during data write and read.</returns>
		public static BitArray ENTRY_MODE_SET(bool INCREMENT, bool DISPLAY_SHIFT = true)
		{
			BitArray temp = new BitArray(entryModeSet);
			temp.Set(8, INCREMENT);
			temp.Set(9, DISPLAY_SHIFT);
			return temp;
		}

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 0 / DB5: 0 / DB4: 0 / DB3: 1 / DB2: D / DB1: C / DB0:B
		/// </summary>
		/// <param name="DISPLAY_ONOFF">Whether to turn the display on or off</param>
		/// <param name="CURSOR_ONOFF">Whether to turn the cursor on or off</param>
		/// <param name="BLINK_ONOFF">Whether to make the cursor blink or not</param>
		/// <returns>The command to set cursor move direction and specify display shift.</returns>
		public static BitArray DISPLAY_ONOFF_CONTROL(bool DISPLAY_ONOFF, bool CURSOR_ONOFF, bool BLINK_ONOFF)
		{
			BitArray temp = new BitArray(displayOnOff);
			temp.Set(7, DISPLAY_ONOFF);
			temp.Set(8, CURSOR_ONOFF);
			temp.Set(9, BLINK_ONOFF);
			return temp;
		}

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 0 / DB5: 0 / DB4: 1 / DB3: S/C / DB2: R/L / DB1: - / DB0:-
		/// </summary>
		/// <param name="DISPLAY_SHIFT">True: Shifts display, False: Shifts cursor</param>
		/// <param name="RIGHT_SHIFT">True: Shifts to the right, False: Shifts to the left</param>
		/// <returns>The command to move the cursor and shifts display without changing DDRAM contents</returns>
		public static BitArray CURSOR_DISPLAY_SHIFT(bool DISPLAY_SHIFT, bool RIGHT_SHIFT)
		{
			BitArray temp = new BitArray(cursorDisplayShift);
			temp.Set(6, DISPLAY_SHIFT);
			temp.Set(7, RIGHT_SHIFT);
			return temp;
		}

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 0 / DB5: 1 / DB4: DL / DB3: N / DB2: F / DB1: - / DB0:-
		/// </summary>
		/// <param name="BIT_8">True: Use 8-bit instructions, False: Use 4-bit instructions</param>
		/// <param name="DOUBLE_LINES">True: Use double display lines, False: Use single display lines</param>
		/// <param name="FIVE_X_TEN">True: Use a font with 5x10 dots, False: Use a font with 5x8 dots</param>
		/// <returns>The command to set the inteface length, number of display lines and character font</returns>
		public static BitArray FUNCTION_SET(bool BIT_8, bool DOUBLE_LINES, bool FIVE_X_TEN)
		{
			BitArray temp = new BitArray(functionSet);
			temp.Set(5, BIT_8);
			temp.Set(6, DOUBLE_LINES);
			temp.Set(7, FIVE_X_TEN);
			return temp;
		}

		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 0 / DB6: 1 / DB5: ACG / DB4: ACG / DB3: ACG / DB2: ACG / DB1: ACG / DB0: ACG
		/// </summary>
		/// <param name="DB5"></param>
		/// <param name="DB4"></param>
		/// <param name="DB3"></param>
		/// <param name="DB2"></param>
		/// <param name="DB1"></param>
		/// <param name="DB0"></param>
		/// <returns>The command to set the CGRAM address</returns>
		public static BitArray SET_CGRAM_ADDRESS(bool DB5, bool DB4, bool DB3, bool DB2, bool DB1, bool DB0)
		{
			BitArray temp = new BitArray(setCGRAMAddress);
			temp.Set(4, DB5);
			temp.Set(5, DB4);
			temp.Set(6, DB3);
			temp.Set(7, DB2);
			temp.Set(8, DB1);
			temp.Set(9, DB0);
			return temp;
		}


		/// <summary>
		/// RS: 0 / RW: 0 / DB7: 1 / DB6: ADD / DB5: ADD / DB4: ADD / DB3: ADD / DB2: ADD / DB1: ADD / DB0: ADD
		/// </summary>
		/// <param name="DB6"></param>
		/// <param name="DB5"></param>
		/// <param name="DB4"></param>
		/// <param name="DB3"></param>
		/// <param name="DB2"></param>
		/// <param name="DB1"></param>
		/// <param name="DB0"></param>
		/// <returns>The command to set the DDRAM address</returns>
		public static BitArray SET_DDRAM_ADDRESS(bool DB6, bool DB5, bool DB4, bool DB3, bool DB2, bool DB1, bool DB0)
		{
			BitArray temp = new BitArray(setDDRAMAddress);
			temp.Set(3, DB6);
			temp.Set(4, DB5);
			temp.Set(5, DB4);
			temp.Set(6, DB3);
			temp.Set(7, DB2);
			temp.Set(8, DB1);
			temp.Set(9, DB0);
			return temp;
		}

		/// <summary>
		/// RS: 0 / RW: 1 / DB7: BF / DB6: AC / DB5: AC / DB4: AC / DB3: AC / DB2: AC / DB1: AC / DB0: AC
		/// </summary>
		/// <returns>The command to read the busy flag, BF and AC are irrelevant for the command to read, currently set to false</returns>
		public static BitArray READ_BUSY_FLAG_AND_ADDRESS()
		{
			return new BitArray(readBusyFlagAddressCounter);
		}

		/// <summary>
		/// RS: 1 / RW: 0 / DB7: DB7 / DB6: DB6 / DB5: DB5 / DB4: DB4 / DB3: DB3 / DB2: DB2 / DB1: DB1 / DB0: DB0
		/// </summary>
		/// <param name="DB7"></param>
		/// <param name="DB6"></param>
		/// <param name="DB5"></param>
		/// <param name="DB4"></param>
		/// <param name="DB3"></param>
		/// <param name="DB2"></param>
		/// <param name="DB1"></param>
		/// <param name="DB0"></param>
		/// <returns>The command to write data to either the CGRAM or the DDRAM</returns>
		public static BitArray WRITE_DATA(bool DB7, bool DB6, bool DB5, bool DB4, bool DB3, bool DB2, bool DB1, bool DB0)
		{
			BitArray temp = new BitArray(writeData);
			temp.Set(2, DB7);
			temp.Set(3, DB6);
			temp.Set(4, DB5);
			temp.Set(5, DB4);
			temp.Set(6, DB3);
			temp.Set(7, DB2);
			temp.Set(8, DB1);
			temp.Set(9, DB0);
			return temp;
		}

		/// <summary>
		/// RS: 1 / RW: 1 / DB7-0: Data to be read
		/// </summary>
		/// <returns>The command to read the data from either the CGRAM or the DDRAM, pin values are irrelevant for the command to read, currently set to false</returns>
		public static BitArray READ_DATA()
		{
			return new BitArray(readData);
		}
	}
}
