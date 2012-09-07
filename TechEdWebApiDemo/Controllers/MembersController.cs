using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Query;
using AutoMapper;
using OData.Framework;
//using TechEdWebApiDemo.Models;
using TechEd.Integration.ServiceModels;
using TechEdWebApiDemo.DAL;

namespace TechEdWebApiDemo.Controllers
{
    public class MembersController : ApiController
    {
        private TechEdContext db = new TechEdContext();

        //[Queryable]
        public IEnumerable<Member> GetMembers(
            ODataQueryOptions query)
        {
            var mappedQuery = query.Parse<Models.Member>(Request);
            var results = new List<Member>();

            foreach (var result in mappedQuery.ApplyTo(db.Members))
            {
                results.Add(Mapper.Map<Member>(result));
            }
            return results;
        }

        // GET api/Members/5
        public Member GetMember(int id)
        {
            Models.Member member = db.Members.Find(id);
            if (member == null)
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Mapper.Map<Member>(member);
        }

        // PUT api/Members/5
        public HttpResponseMessage PutMember(int id, Member member)
        {
            if (ModelState.IsValid && id == member.MemberID)
            {
                db.Entry(Mapper.Map<Models.Member>(member)).
                    State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // POST api/Members
        public HttpResponseMessage PostMember(Member member)
        {
            if (ModelState.IsValid)
            {
                var newMember = Mapper.Map<Models.Member>(member);
                newMember.Joined = DateTime.Now;
                newMember.CreatedBy = User.Identity.Name;
                db.Members.Add(newMember);
                db.SaveChanges();

                var responseMember = Mapper.Map<Member>(newMember);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created,
                                                                      responseMember);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = responseMember.MemberID}));
                return response;
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Authorize]
        // DELETE api/Members/5
        public HttpResponseMessage DeleteMember(int id)
        {
            var member = db.Members.Find(id);
            if (member == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Members.Remove(member);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK,
                                          Mapper.Map<Member>(member));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}