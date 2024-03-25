namespace MHS
{
    internal class Program
    {
        public static void Main()
        {
            MemoryHierarchySimulator mhs = new MemoryHierarchySimulator();

            bool isRunning = false;
            //This should likely be put in as one of the options able to be changed with 0. Edit Configuration alongside the capacityOfCache.
            string pathOfTrace = "trunc_12.dat";


            while (!isRunning)
            {

                mhs.ParseMemoryAddress(mhs.ReadMemoryTrace(pathOfTrace));

                Console.WriteLine($"\nMemory Hierarchy Simulator for {pathOfTrace}");
                Console.WriteLine("Choose algorithm to simulate:");
                Console.WriteLine("0. Edit Configuration");
                Console.WriteLine("1. Optimal FIFO Algorithm");
                Console.WriteLine("2. Optimal Greedy Algorithm");
                Console.WriteLine("3. Optimal LRU Algorithm");
                Console.WriteLine("Q. Quit the program\n");

                string choice = Console.ReadLine();

                switch (choice.ToLower())
                {
                    case "q":
                        Console.WriteLine("\nThanks for using the program, exiting now..");
                        Thread.Sleep(1000);
                        isRunning = true;
                        Environment.Exit(0);
                        break;
                    case "0":
                        // TODO enter config options here
                        Console.WriteLine("\nWhat trace would you like to use?" +
                            "\n1. trunc_12.dat" +
                            "\n2. trace.dat" +
                            "\n3. real_tr.dat");
                        string choice2 = Console.ReadLine();
                        if (choice2 == "1")
                        {
                            pathOfTrace = "trunc_12.dat";
                        }
                        else if (choice2 == "2")
                        {
                            pathOfTrace = "trace.dat";
                        }
                        else if (choice2 == "3")
                        {
                            pathOfTrace = "real_tr.dat";
                        }
                        else
                        {
                            Console.WriteLine("Invalid Entry..");
                        }
                        break;
                    case "1":
                        FIFO.Run(mhs);
                        Console.WriteLine(mhs.DisplaySummaryStatistics());
                        break;
                    case "2":
                        // TODO enter Greedy run() here
                        break;
                    case "3":
                        LRU.Run(mhs);
                        break;
                    // TODO enter Furthest in Future case 4 run() here?
                    default:
                        Console.WriteLine("Invalid Entry...");
                        break;
                }

                Console.WriteLine("\nPress enter to continue.");
                Console.ReadLine();
                Console.Clear();

                mhs.ResetValues();

            }

        }

    }
    
    /*
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
    */
}

