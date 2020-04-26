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
    public class CampaignController : ControllerBase
    {
        // GET api/values     
        [HttpGet]
        public ActionResult<IEnumerable<CampaignPost>> Get()
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                return _context.CampaignPosts.ToList();
            }

        }

        
        //Get User campaign posts
        [Route("api/[controller]/getUserposts")]
        [HttpPost("getUserposts")]
        public ActionResult<IEnumerable<CampaignPost>> Post([FromBody] User user)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                try
                {
                return _context.CampaignPosts.Where(a => a.creatorUsername == user.username).ToList();
                }
                catch (Exception e)
                {
                    List<CampaignPost> errorList = new List<CampaignPost>();
                    CampaignPost d = new CampaignPost();
                    d.description = e.InnerException.Message;
                    errorList.Add(d);
                    return errorList;
                   
                }
            }
        }

        
        //Create new campaign posts
        [Route("api/[controller]/create")]
        [HttpPost("create")]
        public ActionResult<ResponseMessage> Post([FromBody] CampaignPost value)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                ResponseMessage response = new ResponseMessage();
                try
                {
                    int result = 0;
                    //username ,mail address and mobileId exist control
                    if (JsonConvert.DeserializeObject <List<string>>(value.photos).Count < 1)
                    {
                        response.message = "Add at least 1 photo";
                    }
                    else
                    {
                        if (value.description.Length < 100)
                        {
                            response.message = "Descripton lenght is not less than 100";
                        }
                        else
                        {
                            if (value.location.Length < 1)
                            {
                                response.message = "Location must be requirement";
                            }
                            else
                            {

                                value.createdAt = DateTime.Now;
                                try
                                {
                                    int maxid = 0;
                                    if (_context.CommentPosts.Count() > 0)
                                    {
                                        maxid = _context.CampaignPosts.OrderByDescending(a => a.id).FirstOrDefault().id;
                                    }
                                 
                                    value.id = maxid + 1;
                                }
                                catch (Exception e)
                                {

                                    response.message = e.Message;
                                }
                                _context.CampaignPosts.Add(value);
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
                catch(Exception e)
                {
                    response.message = "Fail"+Environment.NewLine+e.Message+Environment.NewLine+ value.photos;

                }
                return response;
            }
               

            }


        // PUT api/values/5
        [Route("api/[controller]/edit")]
        [HttpPut("edit")]
        public ActionResult<ResponseMessage> Put([FromBody] CampaignPost value)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;


                if (_context.CampaignPosts.Any(a => a.id == value.id))
                {
                    response.message = "Edit post is found";
                    _context.CampaignPosts.Remove(_context.CampaignPosts.Where(a=>a.id==value.id).FirstOrDefault());
                    _context.CampaignPosts.Add(value);
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
                    response.message = "Edit post not found";
                }


            }
            return response;
        }


        // DELETE api/values/5
        [Route("api/[controller]/delete")]
        [HttpDelete("delete")]
        public ActionResult<ResponseMessage> Delete([FromBody] CampaignPost value)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;


                if (_context.CampaignPosts.Any(a => a.id == value.id))
                {
                    response.message = "Campaign is exist";
                    _context.CampaignPosts.Remove(value);
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
                    response.message = "Campaign not found";
                }


            }
            return response;
        }
    }
}

