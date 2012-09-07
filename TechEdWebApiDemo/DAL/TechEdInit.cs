//
// Grant Archibald (c) 2012
//
// See Licence.txt
//
using System;
using System.Collections.Generic;
using System.Data.Entity;
using TechEdWebApiDemo.Models;

namespace TechEdWebApiDemo.DAL
{
    /// <summary>
    /// Create a new database is it does not exists and seed data
    /// </summary>
    public class TechEdInit : CreateDatabaseIfNotExists<TechEdContext>
    {
        /// <summary>
        /// Setup the initial data in the database
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(TechEdContext context)
        {
            var members = new List<Member>
                              {
                                  new Member
                                      {
                                          FirstMidName = "Carson",
                                          LastName = "Alexander",
                                          Joined = DateTime.Parse("2005-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Meredith",
                                          LastName = "Alonso",
                                          Joined = DateTime.Parse("2002-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Arturo",
                                          LastName = "Anand",
                                          Joined = DateTime.Parse("2003-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Gytis",
                                          LastName = "Barzdukas",
                                          Joined = DateTime.Parse("2002-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Yan",
                                          LastName = "Li",
                                          Joined = DateTime.Parse("2002-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Peggy",
                                          LastName = "Justice",
                                          Joined = DateTime.Parse("2001-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Laura",
                                          LastName = "Norman",
                                          Joined = DateTime.Parse("2003-09-01")
                                      },
                                  new Member
                                      {
                                          FirstMidName = "Nino",
                                          LastName = "Olivetto",
                                          Joined = DateTime.Parse("2005-09-01")
                                      }
                              };
            members.ForEach(s => context.Members.Add(s));
            context.SaveChanges();
        }
    }
}