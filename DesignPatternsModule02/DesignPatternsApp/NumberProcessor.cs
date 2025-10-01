public class NumberProcessor
{
    public void ProcessNumbers(int[] numbers)
    {
        if (numbers == null || numbers.Length == 0)
            return;

        foreach (var number in numbers)
            if (number > 0)
                Console.WriteLine(number);
    }
}
