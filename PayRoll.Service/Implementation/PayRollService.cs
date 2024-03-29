﻿using PayRoll.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayRoll.Persistence;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace PayRoll.Service.Implementation
{
    public class PayRollService : IPayRollService
    {
        private decimal contractualEarnings;
        private decimal overtimeHours;
        private readonly ApplicationDbContext _db;

        public PayRollService(ApplicationDbContext context)
        {
            _db = context;
        }

     
        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if (hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;
            }
            else
            {
                contractualEarnings = contractualHours * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _db.PaymentRecords.AddAsync(paymentRecord);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<PaymentRecord> GetAll() => _db.PaymentRecords.OrderBy(p => p.EmployeeId);


        public IEnumerable<SelectListItem> GetAllTaxYear()
        {
            var allTaxYear = _db.TaxYears.Select(taxYears => new SelectListItem
            {
                Text = taxYears.YearOfTax,
                Value = taxYears.Id.ToString()
            });
            return allTaxYear;
        }

        public PaymentRecord GetById(int id) =>
            _db.PaymentRecords.Where(pay => pay.Id == id).FirstOrDefault();


        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
            => totalEarnings - totalDeduction;


        public decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours)
            => overtimeHours * overtimeRate;

        public decimal OvertimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if (hoursWorked <= contractualHours)
            {
                overtimeHours = 0.00m;
            }
            else if (hoursWorked > contractualHours)
            {
                overtimeHours = hoursWorked - contractualHours;
            }
            return overtimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate) => hourlyRate * 1.5m;

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFees)
        => tax + nic + studentLoanRepayment + unionFees;

        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        => overtimeEarnings + contractualEarnings;

        public TaxYear GetTaxYearById(int id)
        => _db.TaxYears.Where(year => year.Id == id).FirstOrDefault();
    }
}
