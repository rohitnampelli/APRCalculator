using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanProducts {
    
    public class PaymentScheduleGenerator {
        protected Loan loan { get; set; }
        protected PaymentSchedule schedule { get; set; }
        public PaymentScheduleGenerator(Loan Loan) {
            this.loan = Loan;
        }

        public PaymentSchedule generateSchedule(DateTime PayDate1, DateTime PayDate2, ScheduleType Type) {
            schedule = new PaymentSchedule();
            schedule.payDate1 = PayDate1;
            schedule.payDate2 = PayDate2;
            schedule.Type = Type;
            schedule.payments = new List<Payment>();
            DateTime temp = new DateTime();
            bool odd = false;
            int last = System.DateTime.DaysInMonth(PayDate1.Year, PayDate1.Month);
            DateTime lastDay = new DateTime(PayDate1.Year, PayDate1.Month, last);
            DateTime mid = new DateTime();

            // Says of the number of payments is odd or even.
            odd = (this.loan.paymentCount % 2) == 0 ? false : true;
            switch (Type) {
                // All payments are n days apart starting Paydate 1, Paydate 2.
                case ScheduleType.FixedDays:
                    int daysApart = PayDate2.Subtract(PayDate1).Days;
                    while (PayDate1 <= this.loan.date.AddDays(this.loan.gracePeriod))
                        PayDate1 = PayDate1.AddDays(daysApart);
                    for (int counter = 0; counter < this.loan.paymentCount; counter++) {
                        schedule.payments.Add(new Payment(counter + 1, PayDate1.AddDays(counter * daysApart)));
                    }
                    break;
                // All payments are 7 days apart starting Paydate 1
                case ScheduleType.Weekly:
                    while (PayDate1 <= this.loan.date.AddDays(this.loan.gracePeriod))
                        PayDate1 = PayDate1.AddDays(7);
                    for (int counter = 0; counter < this.loan.paymentCount; counter++) {
                        schedule.payments.Add(new Payment(counter+1, PayDate1.AddDays(counter * 7)));
                    }
                    break;
                // All payments are 14 days apart starting Paydate 1
                case ScheduleType.BiWeekly:
                    while (PayDate1 <= this.loan.date.AddDays(this.loan.gracePeriod))
                        PayDate1 = PayDate1.AddDays(14);
                    for (int counter = 0; counter < this.loan.paymentCount; counter++) {
                        schedule.payments.Add(new Payment(counter + 1, PayDate1.AddDays(counter * 14)));
                    }
                    break;
                // Semi monthly on fixed days of the month
                case ScheduleType.SemiMonthly:
                    
                    while(PayDate1 < this.loan.date.AddDays(this.loan.gracePeriod) || PayDate2 < this.loan.date.AddDays(this.loan.gracePeriod)) {
                        if (PayDate1 < this.loan.date.AddDays(this.loan.gracePeriod) && PayDate2 < this.loan.date.AddDays(this.loan.gracePeriod)) {
                            PayDate1 = PayDate1.AddMonths(1);
                            PayDate2 = PayDate2.AddMonths(1);
                        }
                        else if (PayDate1 < this.loan.date.AddDays(this.loan.gracePeriod) && PayDate2 >= this.loan.date.AddDays(this.loan.gracePeriod)) {
                            temp = PayDate2;
                            PayDate2 = PayDate1;
                            PayDate1 = temp;
                            PayDate2 = PayDate2.AddMonths(1);
                        }
                        else if (PayDate2 < this.loan.date.AddDays(this.loan.gracePeriod) && PayDate1 >= this.loan.date.AddDays(this.loan.gracePeriod)) {
                            PayDate2 = PayDate2.AddMonths(1);
                        }
                    }
                    for (int counter = 0; counter < this.loan.paymentCount / 2; counter++) {
                        schedule.payments.Add(new Payment((counter * 2) + 1, PayDate1.AddMonths(counter)));
                        schedule.payments.Add(new Payment((counter * 2) + 2, PayDate2.AddMonths(counter)));
                        if(odd && (counter == (this.loan.paymentCount / 2) - 1))
                            schedule.payments.Add(new Payment((counter+1 * 2) + 1, PayDate1.AddMonths(counter+1)));
                    }
                    break;
                // one of the two days in the semi monthly schedule month is the last day of the month.
                case ScheduleType.SemiMonthlyLastDay:
                    if (PayDate1 > PayDate2) {
                        temp = PayDate2;
                        PayDate2 = PayDate1;
                        PayDate1 = temp;
                    }
                    while (PayDate1 < this.loan.date.AddDays(this.loan.gracePeriod) || PayDate2 < this.loan.date.AddDays(this.loan.gracePeriod)) {
                        if (PayDate1 < this.loan.date.AddDays(this.loan.gracePeriod) && PayDate2 < this.loan.date.AddDays(this.loan.gracePeriod)) {
                            PayDate1 = PayDate1.AddMonths(1);
                            PayDate2 = PayDate2.AddMonths(1);
                        }
                        else if (PayDate1 < this.loan.date.AddDays(this.loan.gracePeriod) && PayDate2 >= this.loan.date.AddDays(this.loan.gracePeriod)) {
                            temp = PayDate2;
                            PayDate2 = PayDate1;
                            PayDate1 = temp;
                            PayDate2 = PayDate2.AddMonths(1);
                        }
                        else if (PayDate2 < this.loan.date.AddDays(this.loan.gracePeriod) && PayDate1 >= this.loan.date.AddDays(this.loan.gracePeriod)) {
                            PayDate2 = PayDate2.AddMonths(1);
                        }
                    }
                    last = System.DateTime.DaysInMonth(PayDate1.Year, PayDate1.Month);
                    lastDay = new DateTime(PayDate1.Year, PayDate1.Month, last);
                    // If PayDate 1 is the last day in the month
                    // Assume that PayDate 2 is mid month of the next month
                    if (PayDate1 == lastDay) {
                        
                        for (int counter = 0; counter < this.loan.paymentCount / 2; counter++) {
                            lastDay = PayDate2.AddMonths(counter);
                            last = System.DateTime.DaysInMonth(lastDay.Year, lastDay.Month);
                            mid = PayDate2.AddMonths(counter);
                            schedule.payments.Add(new Payment((counter * 2) + 1, new DateTime(lastDay.Year, lastDay.Month, last)));
                            schedule.payments.Add(new Payment((counter * 2) + 2, mid));
                            if (odd && (counter == (this.loan.paymentCount / 2) - 1))
                                schedule.payments.Add(new Payment((counter + 1 * 2) + 1, mid));
                        }
                    }
                    // If PayDate 1 is the Mid day in the month
                    // Assume that PayDate 2 is Last day in the month
                    else {
                        for (int counter = 0; counter < this.loan.paymentCount / 2; counter++) {
                            temp = PayDate1.AddMonths(counter);
                            last = System.DateTime.DaysInMonth(temp.Year, temp.Month);
                            schedule.payments.Add(new Payment((counter * 2) + 1, temp));
                            schedule.payments.Add(new Payment((counter * 2) + 2, new DateTime(temp.Year, temp.Month, last)));
                            if (odd && (counter == (this.loan.paymentCount / 2) - 1))
                                schedule.payments.Add(new Payment((counter + 1 * 2) + 1, PayDate1.AddMonths(counter + 1)));
                        }
                    }
                    
                    break;
                // Monthly payment on set day. Starting on Paydate 1
                case ScheduleType.Monthly:
                    while (PayDate1 <= this.loan.date.AddDays(this.loan.gracePeriod))
                        PayDate1 = PayDate1.AddMonths(1);
                    for (int counter = 0; counter < this.loan.paymentCount; counter++) {
                        schedule.payments.Add(new Payment(counter + 1, PayDate1.AddMonths(counter)));
                    }
                    break;
                // Monthly payment on last day of the month, starting the month of PayDate 1
                case ScheduleType.MonthlyLastDay:
                    while (PayDate1 <= this.loan.date.AddDays(this.loan.gracePeriod))
                        PayDate1 = PayDate1.AddMonths(1);
                    for (int counter = 0; counter < this.loan.paymentCount; counter++) {
                        lastDay = PayDate1.AddMonths(counter);
                        last = System.DateTime.DaysInMonth(lastDay.Year, lastDay.Month);
                        schedule.payments.Add(new Payment(counter + 1, new DateTime(lastDay.Year, lastDay.Month, last)));
                    }
                    break;
            }

            return schedule;
        }
    }
}
