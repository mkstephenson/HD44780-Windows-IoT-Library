using System;
using System.Collections;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Popups;

namespace LiquidCrystalLibrary
{
	public class HD44780Library
	{
		private GpioPin rsPin;
		private GpioPin rwPin;
		private GpioPin enablePin;
		private GpioPin d0Pin;
		private GpioPin d1Pin;
		private GpioPin d2Pin;
		private GpioPin d3Pin;
		private GpioPin d4Pin;
		private GpioPin d5Pin;
		private GpioPin d6Pin;
		private GpioPin d7Pin;

		private bool isConfigured = false;

		private bool isDisplayOn = true;
		private bool isCursorOn = true;
		private bool isCursorBlinking = false;

		private bool is8BitMode = true;
		private bool isDoubleLines = true;
		private bool isFont5x8 = true;

		public HD44780Library() { }

		/// <summary>
		/// The initialisation method, this needs to be called before the screen is ready for use.
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="rw"></param>
		/// <param name="enable"></param>
		/// <param name="d7"></param>
		/// <param name="d6"></param>
		/// <param name="d5"></param>
		/// <param name="d4"></param>
		/// <param name="d3"></param>
		/// <param name="d2"></param>
		/// <param name="d1"></param>
		/// <param name="d0"></param>
		/// <returns></returns>
		public async Task init(int rs, int rw, int enable, int d7, int d6, int d5, int d4, int d3, int d2, int d1, int d0)
		{
			var gpio = GpioController.GetDefault();

			rsPin = gpio.OpenPin(rs);
			rsPin.SetDriveMode(GpioPinDriveMode.Output);
			rwPin = gpio.OpenPin(rw);
			rwPin.SetDriveMode(GpioPinDriveMode.Output);
			enablePin = gpio.OpenPin(enable);
			enablePin.SetDriveMode(GpioPinDriveMode.Output);
			enablePin.Write(GpioPinValue.Low);
			d0Pin = gpio.OpenPin(d0);
			d0Pin.SetDriveMode(GpioPinDriveMode.Output);
			d1Pin = gpio.OpenPin(d1);
			d1Pin.SetDriveMode(GpioPinDriveMode.Output);
			d2Pin = gpio.OpenPin(d2);
			d2Pin.SetDriveMode(GpioPinDriveMode.Output);
			d3Pin = gpio.OpenPin(d3);
			d3Pin.SetDriveMode(GpioPinDriveMode.Output);
			d4Pin = gpio.OpenPin(d4);
			d4Pin.SetDriveMode(GpioPinDriveMode.Output);
			d5Pin = gpio.OpenPin(d5);
			d5Pin.SetDriveMode(GpioPinDriveMode.Output);
			d6Pin = gpio.OpenPin(d6);
			d6Pin.SetDriveMode(GpioPinDriveMode.Output);
			d7Pin = gpio.OpenPin(d7);
			d7Pin.SetDriveMode(GpioPinDriveMode.Output);

			isConfigured = true;

			await ClearDisplay();
			await ConfigureTextDisplay();
			await ConfigureDisplay();
			await SendInstruction(InstructionDefinitions.ENTRY_MODE_SET(true, false));
			await SetCursorPosition(0, 0);
		}

		private async Task ConfigureTextDisplay()
		{
			if (isConfigured)
			{
				await SendInstruction(InstructionDefinitions.FUNCTION_SET(is8BitMode, isDoubleLines, !isFont5x8));
			}
		}

		private async Task ConfigureDisplay()
		{
			if (isConfigured)
			{
				await SendInstruction(InstructionDefinitions.DISPLAY_ONOFF_CONTROL(isDisplayOn, isCursorOn, isCursorBlinking));

			}
		}

		private async Task SendInstruction(BitArray instructions)
		{
			if (instructions.Length != 10 || !isConfigured)
			{
				return;
			}

			SetPin(rsPin, instructions[0]);
			SetPin(rwPin, instructions[1]);
			SetPin(d7Pin, instructions[2]);
			SetPin(d6Pin, instructions[3]);
			SetPin(d5Pin, instructions[4]);
			SetPin(d4Pin, instructions[5]);
			SetPin(d3Pin, instructions[6]);
			SetPin(d2Pin, instructions[7]);
			SetPin(d1Pin, instructions[8]);
			SetPin(d0Pin, instructions[9]);

			enablePin.Write(GpioPinValue.Low);
			await Task.Delay(1);
			enablePin.Write(GpioPinValue.High);
			await Task.Delay(1);
			enablePin.Write(GpioPinValue.Low);
			await Task.Delay(1);
		}


		private void SetPin(GpioPin pin, bool value)
		{
			pin.Write(value ? GpioPinValue.High : GpioPinValue.Low);
		}

		public async Task SetCursorPosition(int row, int column)
		{
			if (row > 1 || column > 40)
			{
				return;
			}

			if (!isDoubleLines)
			{
				row = 0;
			}

			BitArray pos = new BitArray(new int[] { ((40 * row) + column) });
			await SendInstruction(InstructionDefinitions.SET_DDRAM_ADDRESS(pos[6], pos[5], pos[4], pos[3], pos[2], pos[1], pos[0]));
		}

		public async Task Write(string text)
		{
			foreach (var c in text)
			{
				BitArray character = new BitArray(new byte[] { (byte)c });
				await SendInstruction(InstructionDefinitions.WRITE_DATA(character[7], character[6], character[5], character[4], character[3], character[2], character[1], character[0]));
			}
		}

		public async Task WriteLine(string text)
		{
			await Write(text);
			await SetCursorPosition(1, 0);
		}

		public async Task TurnDisplayOn()
		{
			isDisplayOn = true;
			await ConfigureDisplay();
		}

		public async Task TurnDisplayOff()
		{
			isDisplayOn = false;
			await ConfigureDisplay();
		}

		public async Task TurnCursorOn()
		{
			isCursorOn = true;
			await ConfigureDisplay();
		}

		public async Task TurnCursorOff()
		{
			isCursorOn = false;
			await ConfigureDisplay();
		}

		public async Task StartCursorBlinking()
		{
			isCursorBlinking = true;
			await ConfigureDisplay();
		}

		public async Task StopCursorBlinking()
		{
			isCursorBlinking = false;
			await ConfigureDisplay();
		}

		public async Task ClearDisplay()
		{
			await SendInstruction(InstructionDefinitions.CLEAR_DISPLAY());
		}

		public async Task SwitchTo8BitMode()
		{
			is8BitMode = true;
			await ConfigureTextDisplay();
		}

		public async Task SwitchTo4BitMode()
		{
			//TODO: Implement 4 bit mode
			//is8BitMode = false;
			//await ConfigureTextDisplay();
		}

		public async Task SwitchToDoubleLines()
		{
			isDoubleLines = true;
			await ConfigureTextDisplay();
		}

		public async Task SwitchToSingleLines()
		{
			isDoubleLines = false;
			await ConfigureTextDisplay();
		}

		public async Task SwitchTo5x8Font()
		{
			isFont5x8 = true;
			await ConfigureTextDisplay();
		}

		public async Task SwitchTo5x10Font()
		{
			isFont5x8 = false;
			await ConfigureTextDisplay();
		}
	}
}