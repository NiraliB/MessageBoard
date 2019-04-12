using MessageBoardApp.Data;
using MessageBoardApp.Models;
using MessageBoardApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly MessageBoardDbContext dbContext;

        public MessageController(MessageBoardDbContext db)
        {
            dbContext = db;
        }

        public IActionResult MessageBoard()
        {
            string sessionUserName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(sessionUserName))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        [HttpPost]
        public JsonResult SaveMainPostMes(PostedMessage objPostedMes)
        {
            try
            {
                if (objPostedMes != null)
                {

                    if (objPostedMes.MessageId == 0)
                    {
                        objPostedMes.CreatedDate = DateTime.Now;
                        dbContext.Entry(objPostedMes).State = EntityState.Added;
                    }
                    else
                    {
                        objPostedMes.UpdatedDate = DateTime.Now;
                        dbContext.Entry(objPostedMes).Property(x => x.CreatedDate).IsModified = false;
                        dbContext.Entry(objPostedMes).State = EntityState.Modified;
                    }
                    dbContext.SaveChanges();
                    return Json("DataSaved");
                }
                else
                {
                    return Json("Error");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public List<PostedMesViewModel> GetAllMainPostMes()
        {
            try
            {
                string sessionUserId = HttpContext.Session.GetString("UserId");
                var lstPost = (from mes in dbContext.PostedMessage
                               join log in dbContext.LoginUsers on mes.LoginUserId equals log.UserId
                               select new PostedMesViewModel
                               {
                                   MessageId = mes.MessageId,
                                   LoginUserId = mes.LoginUserId,
                                   LogerFullName = log.FullName,
                                   PostedMessages = mes.PostedMes,
                                   CreatedDate = mes.CreatedDate,
                                   LikeCount = (from cont in dbContext.UsersActivity
                                                where (cont.PostedMessageId == mes.MessageId && cont.IsLike == true)
                                                select cont.IsLike).Count(),

                                   userActivity = (from act in dbContext.UsersActivity
                                                   where (act.PostedMessageId == mes.MessageId && act.LoginUserId == Convert.ToInt32(sessionUserId))
                                                   select new UsersActivity
                                                   {
                                                       ActivityId = act.ActivityId,
                                                       IsLike = act.IsLike,
                                                       PostedMessageId = act.PostedMessageId,
                                                       PostedUserId = act.PostedUserId,
                                                       LoginUserId = act.LoginUserId
                                                   }).FirstOrDefault(),

                                   userCommentsAct = (from cmnt in dbContext.UsersActivity
                                                      join userLog in dbContext.LoginUsers on cmnt.LoginUserId equals userLog.UserId
                                                      where (cmnt.PostedMessageId == mes.MessageId && cmnt.PostedComments != null)
                                                      select new UsersActivity
                                                      {
                                                          ActivityId = cmnt.ActivityId,
                                                          PostedComments = cmnt.PostedComments,
                                                          PostedUserId = cmnt.PostedUserId,
                                                          CommentedUserName = userLog.FullName,
                                                          LoginUserId = cmnt.LoginUserId,
                                                          PostedMessageId = cmnt.PostedMessageId
                                                      }).ToList()
                               }).OrderByDescending(m => m.CreatedDate).ToList();

                return lstPost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public UsersActivity SaveLikeUpdated(UsersActivity objActivity)
        {
            try
            {
                UsersActivity objNewActivity = new UsersActivity();
                string sessionUserId = HttpContext.Session.GetString("UserId");
                if (objActivity != null)
                {
                    var getOldData = dbContext.UsersActivity.Where(x => x.PostedMessageId == objActivity.PostedMessageId && x.LoginUserId == Convert.ToInt32(sessionUserId)).FirstOrDefault();

                    if (getOldData != null)
                    {
                        if (getOldData.IsLike == true)
                        {
                            getOldData.IsLike = false;
                        }
                        else
                        {
                            getOldData.IsLike = true;
                        }
                        getOldData.UpdatedDate = DateTime.Now;
                        dbContext.Entry(getOldData).Property(x => x.CreatedDate).IsModified = false;
                        dbContext.Entry(getOldData).State = EntityState.Modified;
                        objNewActivity = getOldData;
                    }
                    else
                    {
                        objNewActivity.LoginUserId = objActivity.LoginUserId;
                        objNewActivity.PostedUserId = objActivity.PostedUserId;
                        objNewActivity.PostedMessageId = objActivity.PostedMessageId;
                        objNewActivity.IsLike = true;
                        objNewActivity.CreatedDate = DateTime.Now;
                        dbContext.Entry(objNewActivity).State = EntityState.Added;
                    }

                    dbContext.SaveChanges();
                }

                return objNewActivity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public UsersActivity SavePostedComments(UsersActivity objCommnt)
        {
            try
            {
                UsersActivity objNewComment = new UsersActivity();
                string sessionUserId = HttpContext.Session.GetString("UserId");

                if (objCommnt != null)
                {
                    var getOldTblData = dbContext.UsersActivity.Where(x => x.PostedMessageId == objCommnt.PostedMessageId && x.LoginUserId == Convert.ToInt32(sessionUserId)).FirstOrDefault();
                    if (getOldTblData != null)
                    {
                        if (getOldTblData.PostedComments != null)
                        {
                            objNewComment.LoginUserId = objCommnt.LoginUserId;
                            objNewComment.PostedUserId = objCommnt.PostedUserId;
                            objNewComment.PostedMessageId = objCommnt.PostedMessageId;
                            objNewComment.PostedComments = objCommnt.PostedComments;
                            objNewComment.CreatedDate = DateTime.Now;
                            dbContext.Entry(objNewComment).State = EntityState.Added;
                        }
                        else
                        {
                            getOldTblData.PostedComments = objCommnt.PostedComments;
                            getOldTblData.UpdatedDate = DateTime.Now;
                            dbContext.Entry(getOldTblData).Property(x => x.CreatedDate).IsModified = false;
                            dbContext.Entry(getOldTblData).Property(x => x.IsLike).IsModified = false;
                            dbContext.Entry(getOldTblData).State = EntityState.Modified;
                            objNewComment = getOldTblData;
                        }
                    }
                    else
                    {
                        objNewComment.LoginUserId = objCommnt.LoginUserId;
                        objNewComment.PostedUserId = objCommnt.PostedUserId;
                        objNewComment.PostedMessageId = objCommnt.PostedMessageId;
                        objNewComment.PostedComments = objCommnt.PostedComments;
                        objNewComment.CreatedDate = DateTime.Now;
                        dbContext.Entry(objNewComment).State = EntityState.Added;
                    }
                    dbContext.SaveChanges();
                }
                return objNewComment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}