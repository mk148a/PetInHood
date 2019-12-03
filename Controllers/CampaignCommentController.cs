using AnimalProtect.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalProtect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignCommentController : ControllerBase
    {
        // GET all comments
        [HttpGet]
        public ActionResult<IEnumerable<CommentPost>> Get()
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                return _context.CommentPosts.ToList();
            }

        }

        
        //Get User Comments
        [Route("api/[controller]/getUsercomments")]
        [HttpPost("getUsercomments")]
        public ActionResult<List<CommentPost>> Post([FromBody] User user)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                try
                {
                return _context.CommentPosts.Where(a => a.creatorUsername == user.username).ToList();
                }
                catch (Exception e)
                {
                    List<CommentPost> errorList = new List<CommentPost>();
                    CommentPost d = new CommentPost();
                    d.description = e.InnerException.Message;
                    errorList.Add(d);
                    return errorList;
                   
                }
            }
        }

        
        //Create new comment
        [Route("api/[controller]/create")]
        [HttpPost("create")]
        public ActionResult<ResponseMessage> Post([FromBody] CommentPost value)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                ResponseMessage response = new ResponseMessage();
                try
                {
                    int result = 0;                    
                    if (value.description.Length < 2)
                    {
                        response.message = "Descripton lenght is not less than 2";
                    }
                    else
                    {                      

                        value.createdAt = DateTime.Now;     
                        try
                        {
                            int maxid = 0;
                            if (_context.CommentPosts.Count()>0)
                            {
                                 maxid = _context.CommentPosts.OrderByDescending(a => a.id).FirstOrDefault().id;
                            }
                            value.id = maxid + 1;
                        }
                        catch (Exception e)
                        {

                            response.message = e.Message;
                        }
                        _context.CommentPosts.Add(value);
                        try
                        {
                            result = _context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            response.message += e.InnerException.Message;
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
                catch(Exception e)
                {
                    response.message = "Fail"+Environment.NewLine+e.Message;

                }
                return response;
            }
               

            }


        // PUT api/values/5
        [Route("api/[controller]/edit")]
        [HttpPut("edit")]
        public ActionResult<ResponseMessage> Put([FromBody] CommentPost value)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;


                if (_context.CommentPosts.Any(a => a.id == value.id))
                {
                    response.message = "CommentPost is found";
                    _context.CommentPosts.Remove(_context.CommentPosts.Where(a=>a.id==value.id).FirstOrDefault());
                    _context.CommentPosts.Add(value);
                    try
                    {


                        result = _context.SaveChanges();
                        if (result == 1)
                        {
                            response.message = "Success";
                        }
                        else
                        {
                            response.message = "Fail";
                        }
                    }
                    catch(Exception e)
                    {
                        response.message = "Error"+Environment.NewLine+e.InnerException.Message;

                    }



                }
                else
                {
                    response.message = "CommentPost not found";
                }


            }
            return response;
        }


        // DELETE api/values/5
        [Route("api/[controller]/delete")]
        [HttpDelete("delete")]
        public ActionResult<ResponseMessage> Delete([FromBody] CommentPost value)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;


                if (_context.CommentPosts.Any(a => a.id == value.id))
                {
                    response.message = "CommentPost is exist";
                    _context.CommentPosts.Remove(value);
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
                    response.message = "CommentPost not found";
                }
            }
            return response;
        }
    }
}

