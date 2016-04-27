using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanProducts;

namespace APRClient {

    public enum ScheduleType {
        Weekly,
        BiWeekly,
        SemiMonthly,
        SemiMonthlyLastDay,
        Monthly,
        MonthlyLastDay,
        FixedDays
    }

    public enum FeesType {
        Regular,
        Discounted
    }

    public enum InterestType {
        Simple,
        Compound
    }

    class Program {
        static void Main(string[] args) {
            Loan L1 = new Loan();
            Console.WriteLine("Loan Date: \t {0}", L1.date);
            Console.WriteLine("Loan Amount: \t {0}", L1.amount);
            Console.WriteLine("Payment Count: \t {0}", L1.paymentCount);
            Console.WriteLine("Grace Period: \t {0}", L1.gracePeriod);
            Console.WriteLine("Schedule Type: \t {0}", L1.schedule.Type);
            Console.WriteLine("Payment Schedule:");
            foreach (Payment P in L1.schedule.payments){
                Console.WriteLine("{0}: \t{1}", P.Number, P.date);
            }
            Console.WriteLine("");
            Loan L2 = new Loan(1000m, new DateTime(2016, 01, 01), 16, 10, (int)InterestType.Simple, (int)ScheduleType.SemiMonthly, 
                new DateTime(2016, 01, 15), new DateTime(2016, 01, 28), (int)FeesType.Discounted, 9.00m);
            L2.AddInterestTier(0.00m, 100000.00m, 9.00m);
            Console.WriteLine("Loan Date: \t {0}", L2.date);
            Console.WriteLine("Loan Amount: \t {0}", L2.amount);
            Console.WriteLine("Payment Count: \t {0}", L2.paymentCount);
            Console.WriteLine("Grace Period: \t {0}", L2.gracePeriod);
            Console.WriteLine("Schedule Type: \t {0}", L2.schedule.Type);
            Console.WriteLine("Payment Schedule:");
            foreach (Payment P in L2.schedule.payments) {
                Console.WriteLine("{0}: \t{1}", P.Number, P.date);
            }
            Console.WriteLine("");
            Loan L3 = new Loan(1000m, new DateTime(2016, 01, 01), 30, 10, (int)InterestType.Simple, (int)ScheduleType.SemiMonthlyLastDay, 
                new DateTime(2016, 01, 28), new DateTime(2016, 02, 15), (int)FeesType.Discounted, 9.00m);
            L3.AddInterestTier(0.00m, 500.00m, 36.00m);
            L3.AddInterestTier(500.00m, 1000.00m, 24.00m);
            L3.AddInterestTier(1000.00m, 5000.00m, 18.00m);
            Console.WriteLine("Loan Date: \t {0}", L3.date);
            Console.WriteLine("Loan Amount: \t {0}", L3.amount);
            Console.WriteLine("Payment Count: \t {0}", L3.paymentCount);
            Console.WriteLine("Grace Period: \t {0}", L3.gracePeriod);
            Console.WriteLine("Schedule Type: \t {0}", L3.schedule.Type);
            Console.WriteLine("Payment Schedule:");
            foreach (Payment P in L3.schedule.payments) {
                Console.WriteLine("{0}: \t{1}", P.Number, P.date);
            }
            Console.ReadLine();
        }
    }
}
