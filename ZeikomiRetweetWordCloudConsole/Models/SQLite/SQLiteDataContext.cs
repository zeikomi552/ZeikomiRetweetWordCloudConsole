using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeikomiRetweetWordCloud.Models.SQLite
{
    public class SQLiteDataContext : DbContext
    {
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        public DbSet<target_tweetBase> DbSet_target_tweet { get; internal set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。


        // 最初にココを変更する
        static string db_file_path = @"tmp\tmp.db";

        /// <summary>
        /// SQLiteのファイルパス
        /// </summary>
        public static string SQLitePath
        {
            get { return db_file_path; }
            set { db_file_path = value; }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new SqliteConnectionStringBuilder { DataSource = db_file_path }.ToString();
            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<target_tweetBase>().HasKey(c => new { c.id });

        }
    }
}
