using System;
using System.Collections.Generic;
using System.IO;
using Identity.Api.User;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "blogging.db");
    }

    public DbSet<UserEntity> Users { get; set; }

    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
                    .HasData(
                        new List<UserEntity>
                        {
                            new()
                            {
                                Username = "admin",
                                Password = "123"
                            }
                        });
    }
}