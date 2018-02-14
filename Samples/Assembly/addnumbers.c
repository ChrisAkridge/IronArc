// addnumbers.c
// An approximation of C code for the addnumbers.iasm program
// Adds two user-supplied numbers, digit-by-digit, and prints the result

void main()
{
	// Get the numbers from the user and store them as length-prefixed strings 
	// at 0x1000 and 0x2000
	hwcall("Terminal::WriteLine", "Enter a number:");
	*(0x1000) = hwcall("Terminal::ReadLine");
	lpstring* addend1 = (lpstring*)0x1004;
	hwcall("Terminal::WriteLine", "Enter a second number:");
	*(0x2000) = hwcall("Terminal::ReadLine");
	lpstring* addend2 = (lpstring*)0x2004;
	
	// Take the length in digits of each addend
	int addend1Length = *(int*)(addend1 - 4);
	int addend2Length = *(int*)(addend2 - 4);
	
	// Convert them to sequences of digits to add
	convertStringToDigits(((byte*)addend1), addend1Length);
	convertStringToDigits(((byte*)addend2), addend2Length);
	
	// Find the length of the shorter number
	int min = 0;
	if (addend1Length >= addend2Length) { min = addend2Length; }
	else { min = addend1Length; }
	
	// Find the length of the longer number
	int max = 0;
	if (addend1Length >= addend2Length) { max = addend1Length; }
	else { max = addend2Length; }
	
	// State that the result should be stored at 0x3004 (its length is at 0x3000)
	byte* result = (byte*)0x3004;
	
	// Copy the longer of the two numbers to the result
	if (max == addend1Length)
	{
		memcpy((addend1 - 4), (result - 4), addend1Length + 4);
	}
	else
	{
		memcpy((addend2 - 4), (result - 4), addend2Length + 4);
	}
	
	// Get the last digits of both numbers and the last digit of the result
	byte* current1 = ((byte*)addend1) + (addend1Length - 1);
	byte* current2 = ((byte*)addend2) + (addend2Length - 1);
	byte* currentResult = result + (max - 1);
	byte carryFlag = 0;
	
	int i = 0;
	while (i < min)
	{
		// Sum the two digits, check for an overflow and set the carry if needed
		byte digit1 = *current1;
		byte digit2 = *current2;
		byte sum = digit1 + digit2 + carryFlag;
		if (sum >= 10)
		{
			sum %= 10;
			carryFlag = 1;
		}
		else
		{
			carryFlag = 0;
		}
		
		// Assign the sum to the result's digit and move to the previous digit
		*currentResult = sum;
		current1--;
		current2--;
		currentResult--;
		
		i++;
	}
	
	// Convert the result sequence of digits back into a string and write it
	convertDigitsToString(result, max);
	hwcall("Terminal::WriteLine", *(lpstring*)(result - 4));
}

void convertStringToDigits(byte* stringFirstChar, int strlen)
{
	byte* current = stringFirstChar;
	
	// For each character in the string...
	int i = 0;
	while (i < strlen)
	{
		// ...subtract 0x30 from it, converting an ASCII number into a number between 0x00 and 0x09
		*current -= 0x30;
		current++;
		
		i++;
	}
}

void convertDigitsToString(byte* digitsFirstDigit, int digitLen)
{
	byte* current = digitsFirstDigit;
	
	// For each digit in the sequence...
	int i = 0;
	while (i < digitLen)
	{
		// ...add 0x30 from it, converting the number into an ASCII number
		*current += 0x30;
		current++;
		
		i++;
	}
	
	// Assign the length of the digits to the length prefix
	*(int*)(digitFirstDigit - 4) = digitLen;
}