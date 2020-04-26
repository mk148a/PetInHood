using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalProtect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalProtect.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                return _context.Users.ToList();
            }
               
        }

        // GET api/values/5
        //Login
        [Route("api/[controller]/login")]
        [HttpPost("login")]
        public ActionResult<User> Get([FromBody] User username)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                return _context.Users.FirstOrDefault(a=>a.username == username.username && a.password== username.password);
            }
        }

        // POST api/values
        //Register
        [Route("api/[controller]/register")]
        [HttpPost("register")]
        public ActionResult<ResponseMessage> Post([FromBody] User value)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
               
                ResponseMessage response = new ResponseMessage();
                int result = 0;
                //username ,mail address and mobileId exist control
                if (value.username==null)
                {
                    response.message = "Username is not able to null";
                }
                else
                {
                    if (value.eMailAddress == null)
                    {
                        response.message = "E-mail Address is not able to null";
                    }
                    else
                    {
                        if (value.password.Length<4)
                        {
                            response.message = "Password Lenght is not less than 4";
                        }
                        else
                        {
                            if (_context.Users.Any(a => a.username == value.username))
                            {
                                response.message = "Username is exist";
                            }
                            else
                            {
                                if (_context.Users.Any(a => a.eMailAddress == value.eMailAddress))
                                {
                                    response.message = "Email address is exist";
                                }
                                else
                                {
                                    if (_context.Users.Any(a => a.mobileId == value.mobileId))
                                    {
                                        response.message = "MobileId is exist";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int maxid = _context.Users.OrderByDescending(a => a.id).FirstOrDefault().id;
                                            value.id = maxid + 1;
                                        }
                                        catch (Exception e)
                                        {

                                            response.message = e.Message;
                                        }
                                        _context.Users.Add(value);
                                        try
                                        {
                                            result = _context.SaveChanges();
                                        }
                                        catch (Exception e)
                                        {
                                            response.message = e.Message;
                                        }

                                        if (result == 1)
                                        {
                                            response.message = "Success";
                                        }
                                        else
                                        {
                                            response.message = "Fail";
                                        }
                                    }
                                }
                            }


                        }
                    }
                }
               
                
               
                return response;
            }
           
        }

        // PUT api/values/5
        [Route("api/[controller]/edit")]
        [HttpPut("edit")]
        public ActionResult<ResponseMessage> Put( [FromBody] User value)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;             
                
               
                if (_context.Users.Any(a => a.username == value.username))
                {
                    response.message = "Username is exist";
                    var oldUser= _context.Users.SingleOrDefault(x=>x.username== value.username);
                    oldUser.username = value.username;
                    oldUser.badge = value.badge;
                    oldUser.eMailAddress = value.eMailAddress;
                    oldUser.firebaseToken = value.firebaseToken;
                    oldUser.followers = value.followers;
                    oldUser.following = value.following;
                    oldUser.id = value.id;
                    oldUser.mobileId = value.mobileId;
                    oldUser.password = value.password;
                    oldUser.petCoin = value.petCoin;
                    oldUser.postIds = value.postIds;
                    oldUser.profilePhoto = value.profilePhoto;

                    try
                    {

                        result = _context.SaveChanges();
                        if (result==1)
                        {
                            response.message = "Success";
                        }
                    }
                    catch (Exception e)
                    {
                        response.message = "Error" +e.Message;

                    }



                }
                else
                {
                    response.message = "User not found";
                }
               

            }
                return response;
        }


        // DELETE api/values/5
        [Route("api/[controller]/delete")]
        [HttpDelete("delete")]
        public ActionResult<ResponseMessage> Delete([FromBody] User value)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;


                if (_context.Users.Any(a => a.username == value.username))
                {
                    response.message = "Username is exist";
                    _context.Users.Remove(value);
                    try
                    {  
                        result = _context.SaveChanges();
                        if (result == 1)
                        {
                            response.message = "Success";
                        }
                    }
                    catch
                    {
                        response.message = "Error";
                    }



                }
                else
                {
                    response.message = "User not found";
                }
               

            }
            return response;
        }
    }
}
