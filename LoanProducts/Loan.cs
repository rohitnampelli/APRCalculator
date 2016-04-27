using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanProducts {
    public class Loan {
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public int gracePeriod { get; set; }
        public int paymentCount { get; set; }
        public PaymentSchedule schedule { get; set; }
        public List<InterestTier> InterestTiers { get; set; }
        public InterestType InterestType;
        public PaymentScheduleGenerator scheduler;
        public decimal APR { get; set; }
        public FeesType FeeType { get; set; }
        public decimal FeeRate { get; set; }
        private decimal principal { get; set; }
        private decimal serviceCharge { get; set; }
        
        public Loan() {
            Initialize(1000m, DateTime.ParseExact("01/01/2015", "d/M/yyyy", null), 10, 10, InterestType.Simple, 
                ScheduleType.BiWeekly, DateTime.ParseExact("01/15/2016", "M/d/yyyy", null), DateTime.ParseExact("01/29/2016", "M/d/yyyy", null),
                FeesType.Discounted, 9.00m);
            AddInterestTier(0.00m, 100000.00m, 9.00m);
        }
        public Loan(decimal Amount, DateTime Date, int GracePeriod, int paymentCount, int InterestType, int ScheduleType, 
                DateTime payDate1, DateTime payDate2, int FeeType, decimal FeeRate) {
            Initialize(Amount, Date, GracePeriod, paymentCount, (InterestType)InterestType, (ScheduleType)ScheduleType, payDate1, payDate2, (FeesType)FeeType, FeeRate);
        }

        public void AddInterestTier(decimal MinAmount, decimal MaxAmount, decimal Value) {

        }

        private void Initialize(decimal Amount, DateTime Date, int GracePeriod, int paymentCount, InterestType interestType,
                ScheduleType ScheduleType, DateTime payDate1, DateTime payDate2, FeesType FeeType, Decimal FeeRate) {
            this.amount = Amount;
            this.date = Date;
            this.gracePeriod = GracePeriod;
            this.paymentCount = paymentCount;
            this.InterestTiers = new List<InterestTier>();
            this.InterestType = interestType;
            this.FeeType = FeeType;
            this.FeeRate = FeeRate;
            scheduler = new PaymentScheduleGenerator(this);
            this.schedule = scheduler.generateSchedule(payDate1, payDate2, (ScheduleType)ScheduleType);
        }
    }
}
