using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Internal;
using PayRoll.Entity;
using PayRoll.Models;
using PayRoll.Service;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;

namespace PayRoll.Controllers
{
    public class EmployeeController:Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment hostingEnvironment) 
        {
            _employeeService = employeeService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(int? pageNumber)
        {
            var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
            {
                Id=employee.Id,
                EmployeeNo=employee.EmployeeNo,
                ImageUrl=employee.ImageUrl,
                FullName=employee.FullName,
                Gender=employee.Gender,
                Designation=employee.Designation,
                City=employee.City,
                DateJoined=employee.DateJoined
            }).ToList();
            int pageSize = 4;
            return View(EmployeeListPagination<EmployeeIndexViewModel>.Create(employees,pageNumber?? 1,pageSize));
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeCreateViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//Prevents cross-site Request Forgery Attacks
        public async Task<IActionResult> Create(EmployeeCreateViewModel EmployeeCreatemodel)
        {
            if (ModelState.IsValid) {
                var employee = new Employee
                {
                    Id = EmployeeCreatemodel.Id,
                    EmployeeNo = EmployeeCreatemodel.EmployeeNo,
                    FirstName = EmployeeCreatemodel.FirstName,
                    LastName = EmployeeCreatemodel.LastName,
                    MiddleName = EmployeeCreatemodel.MiddleName,
                    FullName = EmployeeCreatemodel.FullName,
                    Email = EmployeeCreatemodel.Email,
                    DOB = EmployeeCreatemodel.DOB,
                    DateJoined = EmployeeCreatemodel.DateJoined,
                    NationalInsuranceNo = EmployeeCreatemodel.NationalInsuranceNo,
                    PaymentMethod = EmployeeCreatemodel.PaymentMethod,
                    StudentLoan = EmployeeCreatemodel.StudentLoan,
                    Phone = EmployeeCreatemodel.Phone,
                    Postcode = EmployeeCreatemodel.Postcode,
                    Designation = EmployeeCreatemodel.Designation,
                    ImageUrl = getImageUrl(null,EmployeeCreatemodel),
                    Gender=EmployeeCreatemodel.Gender,
                    UnionMember = EmployeeCreatemodel.UnionMember,
                    Address = EmployeeCreatemodel.Address,
                    City = EmployeeCreatemodel.City,

                };
                await  _employeeService.CreateAsync(employee);

                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        public IActionResult Edit(int id)
        {

            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                Phone = employee.Phone,
                Postcode = employee.Postcode,
                Designation = employee.Designation
            };
            return View(model);
        }

        public string getImageUrl([Optional] EmployeeEditViewModel EmployeeEditmodel, [Optional] EmployeeCreateViewModel EmployeeCreatemodel)
        {
            string ImageUrl = "";
            if (EmployeeEditmodel!=null)
            {
                if (EmployeeEditmodel.ImageUrl != null && EmployeeEditmodel.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(EmployeeEditmodel.ImageUrl.FileName);
                    var extension = Path.GetExtension(EmployeeEditmodel.ImageUrl.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("mmddyyssff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    EmployeeEditmodel.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    ImageUrl = "/" + uploadDir + "/" + fileName;

                }
            }
            else
            {
                if (EmployeeCreatemodel.ImageUrl != null && EmployeeCreatemodel.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(EmployeeCreatemodel.ImageUrl.FileName);
                    var extension = Path.GetExtension(EmployeeCreatemodel.ImageUrl.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("mmddyyssff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    EmployeeCreatemodel.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    ImageUrl = "/" + uploadDir + "/" + fileName;

                }
            }
           
            return (ImageUrl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel EmployeeEditView) 
        {
            if (ModelState.IsValid)
            {

                var employee = _employeeService.GetById(EmployeeEditView.Id);
                if (employee == null)
                {
                    return NotFound();
                }

                employee.Id = EmployeeEditView.Id;
                employee.EmployeeNo = EmployeeEditView.EmployeeNo;
                employee.FirstName = EmployeeEditView.FirstName;
                employee.LastName = EmployeeEditView.LastName;
                employee.MiddleName = EmployeeEditView.MiddleName;
                employee.Email = EmployeeEditView.Email;
                employee.DOB = EmployeeEditView.DOB;
                employee.DateJoined = EmployeeEditView.DateJoined;
                employee.NationalInsuranceNo = EmployeeEditView.NationalInsuranceNo;
                employee.PaymentMethod = EmployeeEditView.PaymentMethod;
                employee.StudentLoan = EmployeeEditView.StudentLoan;
                employee.Phone = EmployeeEditView.Phone;
                employee.Postcode = EmployeeEditView.Postcode;
                employee.Designation = EmployeeEditView.Designation;
                employee.Phone = EmployeeEditView.Phone;
                employee.ImageUrl = getImageUrl(EmployeeEditView);


              await _employeeService.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            EmployeeDetailViewModel model = new EmployeeDetailViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                Gender = employee.Gender,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                Phone = employee.Phone,
                Email = employee.Email,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                Address = employee.Address,
                City = employee.City,
                ImageUrl = employee.ImageUrl,
                Postcode = employee.Postcode

            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeDeleteViewModel()
            {
                Id = employee.Id,
                FullName = employee.FullName
            };

            return View(model);
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
          await _employeeService.Delete(model.Id);


            return RedirectToAction(nameof(Index));
        }
    }

    
}
