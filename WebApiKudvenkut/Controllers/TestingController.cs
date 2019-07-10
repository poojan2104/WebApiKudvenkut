using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApiKudvenkut.Models;
using WebApiKudvenkut.Data;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace WebApiKudvenkut.Controllers
{
    public class TestingController : Controller
    {
        // GET: Testing
        test1Entities1 db = new test1Entities1();
        public ActionResult Index()
        {
            Student student = new Student();
            student.GetStudentList = db.StudentDropDowns.Select(x => new Student { StudentId = x.Id, StudentName = x.Name }).ToList();
            return View(student);
        }

        [HttpPost]
        public ActionResult Index(Student student)
        {
            string[] selectedStudent = student.studentList;
            TempData["test"] = selectedStudent;
            return RedirectToAction("Display", "Testing");
        }

        public ActionResult Display()
        {
            ViewBag.selectedStudent = TempData["test"];
            return View();
        }

        public ActionResult DynamicLoading()
        {
            return View(db.Employees.ToList());
        }

        [HttpPost]
        public ActionResult EmployeeInfo(int id)
        {
            List<Employee> employeeInfo = db.Employees.Where(x => x.emp_id == id).ToList();
            return View(employeeInfo);
        }

        public ActionResult DynamicFieldGenrator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BillGenrator(List<Order> obj)
        {
            List<Order> OrderResult = new List<Order>();
            Order order;
            foreach (var item in obj)
            {
                order = new Order();
                order.ProductName = item.ProductName;
                order.Price = item.Price;
                order.Quantity = item.Quantity;
                order.Amount = order.Price * order.Quantity;
                OrderResult.Add(order);
            }

            double totalAmount = 0;
            int totalItem = 0;

            foreach (var item in OrderResult)
            {
                totalAmount = totalAmount + item.Amount;
                totalItem = totalItem + item.Quantity;
            }

            TempData["totalAmt"] = String.Format("{0:n2}", totalAmount);
            TempData["totalItem"] = totalItem;


            return PartialView("~/Views/Testing/Bill.cshtml", OrderResult);
        }


        #region stored procedure

        //exec pr_customer2_selectAll;

        //exec pr_customer2_selectView @customerId = '3';

        //exec PR_Customer2_Insert @Name = 'a',@Address = 'a',@Mobileno = '9' ,@Birthdate = '1997-10-10',@EmailID = 'a@a.com'; 

        //exec PR_customer2_Update @Name = 'bq',@Address = 'ba',@Mobileno = '20' ,@Birthdate = '1996-11-11',@EmailID = 'b@a.com',@CustomerID = '4';

        //exec pr_customer2_Delete @customerId = '4';

        [HttpGet]
        public ActionResult ViewAllCustomer()
        {
            SqlConnection con = null;
            DataSet ds = null;
            List<Customer> custlist = null;
            try
            {
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["storedProcedureContext"].ConnectionString);
                SqlCommand cmd = new SqlCommand("pr_customer2_selectAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                custlist = new List<Customer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Customer cobj = new Customer();
                    cobj.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerID"].ToString());
                    cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    cobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                    cobj.Mobileno = ds.Tables[0].Rows[i]["Mobileno"].ToString();
                    cobj.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                    cobj.Birthdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Birthdate"].ToString());

                    custlist.Add(cobj);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }

            return View(custlist);
        }

        [HttpGet]
        public ActionResult AddOrEditCustomer(int id=0)
        {
            if (id != 0)
            {
                SqlConnection con = null;
                DataSet ds = null;
                Customer cobj = null;
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["storedProcedureContext"].ConnectionString);
                SqlCommand cmd = new SqlCommand("pr_customer2_selectView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", id);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cobj = new Customer();
                    cobj.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerID"].ToString());
                    cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    cobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                    cobj.Mobileno = ds.Tables[0].Rows[i]["Mobileno"].ToString();
                    cobj.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                    cobj.Birthdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Birthdate"].ToString());
                }
                con.Close();
                return View(cobj);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddOrEditCustomer(Customer objcust)
        {
            SqlConnection con = null;
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["storedProcedureContext"].ConnectionString);
            try
            {
                if (objcust.CustomerID == 0)
                {

                    SqlCommand cmd = new SqlCommand("PR_Customer2_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", objcust.Name);
                    cmd.Parameters.AddWithValue("@Address", objcust.Address);
                    cmd.Parameters.AddWithValue("@Mobileno", objcust.Mobileno);
                    cmd.Parameters.AddWithValue("@Birthdate", Convert.ToDateTime(objcust.Birthdate));
                    cmd.Parameters.AddWithValue("@EmailID", objcust.EmailID);
                    con.Open();
                    cmd.ExecuteScalar();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("PR_customer2_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", objcust.CustomerID);
                    cmd.Parameters.AddWithValue("@Name", objcust.Name);
                    cmd.Parameters.AddWithValue("@Address", objcust.Address);
                    cmd.Parameters.AddWithValue("@Mobileno", objcust.Mobileno);
                    cmd.Parameters.AddWithValue("@Birthdate", Convert.ToDateTime(objcust.Birthdate));
                    cmd.Parameters.AddWithValue("@EmailID", objcust.EmailID);
                    con.Open();
                    cmd.ExecuteScalar();
                }
            }
            catch
            {
                return RedirectToAction("ViewAllCustomer", "Testing");
            }
            finally
            {
                con.Close();
            }
            return RedirectToAction("ViewAllCustomer", "Testing");
        }

        [HttpGet]
        public ActionResult DeleteCustomer(int id = 0)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["storedProcedureContext"].ConnectionString);
                SqlCommand cmd = new SqlCommand("pr_customer2_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
            return RedirectToAction("ViewAllCustomer", "Testing");
        }

        [HttpGet]
        public ActionResult CustomerDetailById(int id = 0)
        {
            SqlConnection con = null;
            DataSet ds = null;
            Customer cobj = null;
            try
            {
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["storedProcedureContext"].ConnectionString);
                SqlCommand cmd = new SqlCommand("pr_customer2_selectView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", id);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cobj = new Customer();
                    cobj.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[i]["CustomerID"].ToString());
                    cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    cobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                    cobj.Mobileno = ds.Tables[0].Rows[i]["Mobileno"].ToString();
                    cobj.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                    cobj.Birthdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Birthdate"].ToString());
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }

            return View(cobj);
        }
        #endregion

        #region notification hub


        //public JsonResult GetNotificationContacts()
        //{
        //    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
        //    NotificationComponents NC = new NotificationComponents();
        //    var list = NC.GetContacts(notificationRegisterTime);
        //    //update session here for get only new added contacts (notification)
        //    Session["LastUpdate"] = DateTime.Now;
        //    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        #endregion


        public ActionResult Chat()
        {
            return View();
        }


        public ActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFiles(HttpPostedFile File, string Name)
        {
            //string path = Server.MapPath("~/Content/Upload/");
            //HttpFileCollectionBase files = Request.Files;
            //for (int i = 0; i < files.Count; i++)
            //{
            //    HttpPostedFileBase file = files[i];
            //    file.SaveAs(path + file.FileName);
            //}
            //return Json(files.Count + " Files Uploaded!");





            return Json("");
        }

        public ActionResult FileUpload2(int id=1)
        {
            if (id != 0)
            {
                var customer = db.CustomerFileUploads.Where(x => x.Id == id).FirstOrDefault();
                return View(customer);
            }
            else
            {
                return View();
            }        
        }

        [HttpPost]
        public ActionResult FileUpload2(CustomerFileUpload customer)
        {
            //test1Entities1 db = new test1Entities1();
            //if (customer.Id == null)
            //{
            //    string FileName = Path.GetFileNameWithoutExtension(customer.ImageUpload.FileName);
            //    string FileExtension = Path.GetExtension(customer.ImageUpload.FileName);
            //    FileName = FileName.Trim() + "-" + DateTime.Now.ToString("yymmssfff") + FileExtension;
            //    customer.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Upload/"), FileName));
                
            //    CustomerFileUpload obj = new CustomerFileUpload();
            //    obj.Name = customer.Name;
            //    obj.FilePath = "~/Content/Upload/" + FileName;
            //    db.CustomerFileUploads.Add(obj);
            //    db.SaveChanges();
            //}
            //else
            //{
            //    //update code

            //}
            return View();
        }

        public ActionResult CustomerListForDelete()
        {
            test1Entities db = new test1Entities();
            List<Customer2> customerList = db.Customer2.ToList<Customer2>();
            return View(customerList);
        }

        [HttpPost]
        public ActionResult DeleteMultipleCustomer(IEnumerable<int> customerIdsToDelete)
        {
            test1Entities dc = new test1Entities();
            var customers = dc.Customer2.Where(x => customerIdsToDelete.Contains(x.CustomerID)).ToList();
            foreach (var i in customers)
            {
                dc.Customer2.Remove(i);
            }
            dc.SaveChanges();
            return RedirectToAction("CustomerListForDelete");
        }
    }
}