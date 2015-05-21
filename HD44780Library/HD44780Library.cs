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

		public HD44780Library()
		{

		}

		public async Task init(int rs, int rw, int enable, int d0, int d1, int d2, int d3, int d4, int d5, int d6, int d7)
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

			await SendInstruction(InstructionDefinitions.CLEAR_DISPLAY());
			await SendInstruction(InstructionDefinitions.FUNCTION_SET(true, true, false));
			await SendInstruction(InstructionDefinitions.DISPLAY_ONOFF_CONTROL(true, true, false));
			await SendInstruction(InstructionDefinitions.ENTRY_MODE_SET(true, false));
			await SetCursorPosition(0, 0);
		}

		private async Task SendInstruction(BitArray instructions)
		{
			if (instructions.Length != 10)
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
	}
}
