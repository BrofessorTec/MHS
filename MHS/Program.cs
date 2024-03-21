namespace MHS
{
    //Test to see if this pushes to Master
    //I just want my own comment

    internal class Program
    {
        static void Main()
        {
            MemoryHierarchySimulator mhs = new MemoryHierarchySimulator();

            while (true)
            {
                Console.WriteLine("Memory Hierarchy Simulator");

                // Read configuration or set default values

                Console.WriteLine("Choose algorithm to simulate:");
                Console.WriteLine("1. Optimal Greedy Algorithm");
                Console.WriteLine("2. Optimal FIFO Algorithm");
                Console.WriteLine("3. Optimal LRU Algorithm");
                Console.WriteLine("Q. Quit the program");

                string choice = Console.ReadLine();


                switch (choice.ToLower())
                {
                    case "q":
                        Console.WriteLine("Thanks for using the program, exiting now..");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                        break;
                    case "1":
                        mhs.OptimalGreedyAlgorithm();
                        mhs.DisplaySummaryStatistics();
                        break;
                    case "2":
                        //OptimalFIFOAlgorithm();
                        mhs.DisplaySummaryStatistics();
                        break;
                    case "3":
                        //OptimalLRUAlgorithm(); 
                        mhs.DisplaySummaryStatistics();
                        break;
                    default:
                        Console.WriteLine("Invalid Number Entered...");
                        break;
                }

                Console.WriteLine("Enter any key to continue.");
                Console.ReadLine();
                Console.Clear();
            }


        }
    }
}
