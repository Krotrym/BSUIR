
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Laboratornay3;

public partial class NewPage1 : ContentPage
{
	private string firstNumber = "";
	string secondNumber = "";
	string currentOperation = "";
	private bool operationPressed = false;
	private string memoryNumber = "";
	private bool error = false;
	public NewPage1()
	{
		InitializeComponent();
	}
	private void DeleteError()
	{
		error = false;
		displayEntry.Text = "";
        firstNumber = "";
        secondNumber = "";
        currentOperation = "";
        operationPressed = false;
    }

	private void Error()
	{
        error = true;
        displayEntry.Text = "Error";
    }
	private void DeleteNumber(string number)
	{
        int lastIndex = displayEntry.Text.LastIndexOf(number);
        if (lastIndex >= 0)
            displayEntry.Text = displayEntry.Text.Remove(lastIndex, number.Length);
    }
	private void DigitButtonClick(object sender, EventArgs e)
	{
		if(error)
		{
			DeleteError();
		}

		Button button = (Button)sender;

		if (operationPressed == false)	
			firstNumber = firstNumber + button.Text;
		else
			secondNumber = secondNumber + button.Text;

			displayEntry.Text = displayEntry.Text + button.Text;
	}

	private void ButtonOperation(object sender, EventArgs e)
	{
        Button button = (Button)sender;

        if (secondNumber != "" && operationPressed)
			ButtonEqual(sender, e);
		else if (operationPressed)
			displayEntry.Text = displayEntry.Text.Substring(0, displayEntry.Text.Length - 1);

		operationPressed = true;
		currentOperation = button.Text;
		displayEntry.Text = displayEntry.Text + currentOperation;
	}
	private void ButtonEqual(object sender, EventArgs e)
	{
		try
		{
			operationPressed = false;
			double newNumber = 0;
			switch (currentOperation)
			{
				case "+":
					newNumber = double.Parse(firstNumber) + double.Parse(secondNumber);
					break;
				case "-":
					newNumber = double.Parse(firstNumber) - double.Parse(secondNumber);
					break;
				case "×":
					newNumber = double.Parse(firstNumber) * double.Parse(secondNumber);
					break;
				case "÷":
					newNumber = double.Parse(firstNumber) / double.Parse(secondNumber);
					break;
			}
			firstNumber = newNumber.ToString();
			secondNumber = "";
			displayEntry.Text = newNumber.ToString();
		}
		catch 
		{
			Error();
		}
    }
	private void ButtonBackSpace(object sender, EventArgs e)
	{
		try
		{
			if (secondNumber == "" && displayEntry.Text.Substring(displayEntry.Text.Length - 1) != "÷" && displayEntry.Text.Substring(displayEntry.Text.Length - 1) != "×" && displayEntry.Text.Substring(displayEntry.Text.Length - 1) != "+" && displayEntry.Text.Substring(displayEntry.Text.Length - 1) != "-" && firstNumber != "")
			{
				firstNumber = firstNumber.Remove(firstNumber.Length - 1);
				displayEntry.Text = displayEntry.Text.Remove(displayEntry.Text.Length - 1);
			}
			else if (displayEntry.Text.Substring(displayEntry.Text.Length - 1) == "÷" || displayEntry.Text.Substring(displayEntry.Text.Length - 1) == "×" || displayEntry.Text.Substring(displayEntry.Text.Length - 1) == "+" || displayEntry.Text.Substring(displayEntry.Text.Length - 1) == "-")
			{
				operationPressed = false;
				displayEntry.Text = displayEntry.Text.Remove(displayEntry.Text.Length - 1);
			}
			else if (secondNumber != "")
			{
				secondNumber = secondNumber.Remove(secondNumber.Length - 1);
				displayEntry.Text = displayEntry.Text.Remove(displayEntry.Text.Length - 1);
			}
		}
		catch
		{
			Error();
		}
	}

	private void ButtonC(object sender, EventArgs e)
	{
		firstNumber = "";
		secondNumber = "";
		displayEntry.Text = "";
		currentOperation = "";
		operationPressed = false;
	}
	private void ButtonCE(object sender, EventArgs e)
	{
        if (secondNumber != "")
        {
            DeleteNumber(secondNumber);
			secondNumber = "";
        }
        else if (operationPressed == false)
        {
            DeleteNumber(firstNumber); 
			firstNumber = "";
        }
    }

	private void ButtonPrecent(object sender, EventArgs e)
	{
		try
		{
			if (operationPressed == false)
				firstNumber = (double.Parse(firstNumber) / 100).ToString();
			else if (currentOperation == "÷")
				firstNumber = (double.Parse(firstNumber) / (double.Parse(secondNumber) / 100)).ToString();
			else if (currentOperation == "×")
				firstNumber = (double.Parse(firstNumber) / 100 * double.Parse(secondNumber)).ToString();
			else if (currentOperation == "+")
				firstNumber = (double.Parse(firstNumber) + (double.Parse(firstNumber) / 100 * double.Parse(secondNumber))).ToString();
			else if (currentOperation == "-")
				firstNumber = (double.Parse(firstNumber) - (double.Parse(firstNumber) / 100 * double.Parse(secondNumber))).ToString();
			else if (operationPressed && secondNumber == "")
				return;

			displayEntry.Text = firstNumber;
			operationPressed = false;
			secondNumber = "";
			currentOperation = "";
		}
		catch
		{
			Error();
		}
	}
	private void ButtonReverseSign(object sender, EventArgs e)
	{
		try
		{
			if (secondNumber != "")
			{
				DeleteNumber(secondNumber);
				secondNumber = (double.Parse(secondNumber) * (-1)).ToString();
				displayEntry.Text = displayEntry.Text + secondNumber;
			}
			else if (operationPressed == false)
			{
				DeleteNumber(firstNumber);
				firstNumber = (double.Parse(firstNumber) * (-1)).ToString();
				displayEntry.Text = displayEntry.Text + firstNumber;
			}
		}
		catch
		{
			Error(); 
		}
	}
	private void ButtonOneDivideX(object sender, EventArgs e)
	{
		try
		{
			if (secondNumber != "")
			{
				DeleteNumber(secondNumber);
				secondNumber = (1 / double.Parse(secondNumber)).ToString();
				displayEntry.Text = displayEntry.Text + secondNumber;
			}
			else if (operationPressed == false)
			{
				DeleteNumber(firstNumber);
				firstNumber = (1 / double.Parse(firstNumber)).ToString();
				displayEntry.Text = displayEntry.Text + firstNumber;
			}
		}
		catch
		{
			Error(); 
		}
	}
    private void ButtonSqrt(object sender, EventArgs e)
    {
		try
		{
			if (secondNumber != "")
			{
				DeleteNumber(secondNumber);
				secondNumber = (Math.Sqrt(double.Parse(secondNumber))).ToString();
				displayEntry.Text = displayEntry.Text + secondNumber;
			}
			else if (operationPressed == false)
			{
				DeleteNumber(firstNumber);
				firstNumber = (Math.Sqrt(double.Parse(firstNumber))).ToString();
				displayEntry.Text = displayEntry.Text + firstNumber;
			}
		}
		catch
		{
			Error();
		}
    }
	private void ButtonSquare(object sender, EventArgs e)
	{
		try
		{
			if (secondNumber != "")
			{
				DeleteNumber(secondNumber);
				secondNumber = (double.Parse(secondNumber) * double.Parse(secondNumber)).ToString();
				displayEntry.Text = displayEntry.Text + secondNumber;
			}
			else if (operationPressed == false)
			{
				DeleteNumber(firstNumber);
				firstNumber = (double.Parse(firstNumber) * double.Parse(firstNumber)).ToString();
				displayEntry.Text = displayEntry.Text + firstNumber;
			}
		}
		catch
		{
			Error(); 
		}
    }
	private void ButtonMC(object sender, EventArgs e) =>
		memoryNumber = "";
	
    private void ButtonMR(object sender, EventArgs e)
    {
        if (operationPressed)
        {
            DeleteNumber(secondNumber);
            secondNumber = memoryNumber;
            displayEntry.Text = displayEntry.Text + secondNumber;
        }
        else if (operationPressed == false)
        {
            DeleteNumber(firstNumber);
            firstNumber = memoryNumber;
            displayEntry.Text = displayEntry.Text + firstNumber;
        }
    }
	private void ButtonMPlus(object sender, EventArgs e)
	{
		try
		{
			if (secondNumber != "")
				memoryNumber = (double.Parse(memoryNumber) + double.Parse(secondNumber)).ToString();
			else if (operationPressed == false)
				memoryNumber = (double.Parse(memoryNumber) + double.Parse(firstNumber)).ToString();
		}
		catch
		{
			Error(); 
		}
	}
	private void ButtonMMinus(object sender, EventArgs e)
	{
		try {
			if (secondNumber != "")
				memoryNumber = (double.Parse(memoryNumber) - double.Parse(secondNumber)).ToString();
			else if (operationPressed == false)
				memoryNumber = (double.Parse(memoryNumber) - double.Parse(firstNumber)).ToString();
		}
		catch
		{
			Error(); 
		}
	}
    private void ButtonMS(object sender, EventArgs e)
    {
		if (currentOperation == "" && operationPressed == false)
			memoryNumber = firstNumber;
    }
	private void ButtonCircle(object sender, EventArgs e)
	{
		try
		{
			if (secondNumber == "" && operationPressed == false)
			{
				firstNumber = (3.1415 * double.Parse(firstNumber) * double.Parse(firstNumber)).ToString();
				displayEntry.Text = firstNumber;
			}
		}
		catch
		{
			Error();
		}
	}
}
	