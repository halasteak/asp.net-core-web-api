﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using my_books.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder) {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any()) {
                    context.Books.AddRange(new Book()
                    {
                        Title = "1st book",
                        Description = "1st book description",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-5),
                        Rate = 4,
                        Genre = "Biography",
                        CoverUrl = "https://...",
                        AddedDate = DateTime.Now
                    }, new Book()
                    {
                        Title = "2st book",
                        Description = "2st book description",
                        IsRead = false,
                        Genre = "Biography",
                        CoverUrl = "https://...",
                        AddedDate = DateTime.Now
                    }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
