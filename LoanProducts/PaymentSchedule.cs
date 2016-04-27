using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanProducts {
    public class PaymentSchedule {
        public int StartDate;
        public DateTime payDate1;
        public DateTime payDate2;
        public ScheduleType Type;
        public List<Payment> payments;
    }

    public class Payment {
        public int Number;
        public DateTime date;
        public Payment(int number, DateTime date) { this.Number = number; this.date = date.Date; }
    }
}
