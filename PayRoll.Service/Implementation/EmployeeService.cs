using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayRoll.Entity;
using PayRoll.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRoll.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext db;
        private decimal studentLoanAmount;

        public EmployeeService(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task CreateAsync(Employee newEmployee)
        {
           await db.Employees.AddAsync( newEmployee);
            await db.SaveChangesAsync();
      
        }

        public async Task Delete(int employeeId)
        {
            var employee = GetById(employeeId);
            db.Remove(employee);
            await db.SaveChangesAsync();

        }

        public IEnumerable<Employee> GetAll() => db.Employees.AsNoTracking().OrderBy(emp=>emp.FullName);
       

        public Employee GetById(int employeeId) =>
            db.Employees.Where(e => e.Id == employeeId).FirstOrDefault();
        


        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);
            if (employee.StudentLoan == StudentLoan.Yes && totalAmount > 1750 && totalAmount < 2000)
            {
                studentLoanAmount = 15m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2000 && totalAmount < 2250)
            {
                studentLoanAmount = 38m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2250 && totalAmount < 2500)
            {
                studentLoanAmount = 60m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2500)
            {
                studentLoanAmount = 83m;
            }
            else
            {
                studentLoanAmount = 0m;
            }
            return studentLoanAmount;
        }
        public decimal UnionFees(int id) 
        {
            var employee = GetById(id);
            var fee = employee.UnionMember == UnionMember.Yes ? 10m : 0m;
            return fee;

        }
        public async Task UpdateAsync(Employee employee)
        {
            db.Update(employee);
            await db.SaveChangesAsync();

        }
        /** update by employee Id**/
        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            db.Update(employee);
            await db.SaveChangesAsync();
        }
        public IEnumerable<SelectListItem> GetAllEmployeesForPayroll()
        {
            return GetAll().Select(emp => new SelectListItem()
            {
                Text = emp.FullName,
                Value = emp.Id.ToString()
            });
        }
    }
}
