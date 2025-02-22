﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace Library_Project.Resources.Classes
{
    public class BorrowedBook
    {
        public string nameBook { get; set; }
        public string gotDate { get; set; }
        public string returnDate { get; set; }
        public string remainDate { get; set; }
        public int Row { get; set; }

        public static List<BorrowedBook> infoBorrowed(string UserName)
        {
            DataTable data = DatabaseControl.Select("SELECT * FROM T_Borrowed WHERE username='" + UserName + "'");
            List<BorrowedBook> Borrowed = new List<BorrowedBook>();
            string NameBook, GotDate, ReturnDate;
            string RemainDate;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                NameBook = data.Rows[i]["bookName"].ToString();
                GotDate = data.Rows[i]["gotDate"].ToString();
                ReturnDate = data.Rows[i]["returnDate"].ToString();
                TimeSpan result = DateTime.Parse(ReturnDate) - DateTime.Parse(DateTime.Now.ToShortDateString());
                RemainDate = int.Parse(result.TotalDays.ToString()).ToString();

                Borrowed.Add(new BorrowedBook { Row = i + 1, nameBook = NameBook, gotDate = GotDate, returnDate = ReturnDate, remainDate = RemainDate });
            }
            return Borrowed;
        }
        public static void RemoveBook(string UserName)
        {
            DataTable data = DatabaseControl.Select("SELECT * FROM T_Borrowed INNER JOIN T_Books ON T_Borrowed.bookName = T_Books.bookName");
            DatabaseControl.Exe("DELETE FROM T_Borrowed WHERE username='" + UserName + "'");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["username"].ToString() == UserName)
                    DatabaseControl.Exe("UPDATE T_Books SET quantity='" + (int.Parse(data.Rows[i]["quantity"].ToString()) + 1) + "' WHERE bookName='" + data.Rows[i]["bookName"] + "'");
            }
        }
    }
    public class Book
    {
        private string _name;
        private string _author;
        private string _category;
        private int _publishNumber;
        private int _quantity;
        private int _row;

        public string Name { get => _name; set => _name = value; }
        public string Author { get => _author; set => _author = value; }
        public string Category { get => _category; set => _category = value; }
        public int Row { get => _row; set => _row = value; }
        public int PublishNumber
        {
            get => _publishNumber;
            set
            {
                // check if the publishnumber is positive or not
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("PublishNumber", "Should be positive number.");
                _publishNumber = value;
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                // check if the quantity is positive or not
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Quantity", "Should be positive number.");
                _quantity = value;
            }
        }
        public Book(int Row, string name, string author, string category, string publishNumber, string quantity)
        {
            this.Name = name;
            this.Author = author;
            this.Category = category;
            this.Quantity = int.Parse(quantity);
            this.PublishNumber = int.Parse(publishNumber);
            this.Row = Row;
        }
        public Book(string name, string author, string category, string publishNumber, string quantity)
        {
            this.Name = name;
            this.Author = author;
            this.Category = category;
            this.Quantity = int.Parse(quantity);
            this.PublishNumber = int.Parse(publishNumber);
        }
        // <summary> a method for taking all the books from database </summary>
        // <returns> an array of Book </returns>

        public static Book[] TakeAllBooks()
        {
            List<Book> books = new List<Book>();
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");

            try
            {
                DataTable table = DatabaseControl.TableFiller("select * from T_Books", connection);

                string name, author, category, publishNumber, quantity;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    name = table.Rows[i]["bookName"].ToString();
                    author = table.Rows[i]["author"].ToString();
                    category = table.Rows[i]["category"].ToString();
                    publishNumber = table.Rows[i]["publishNumber"].ToString();
                    quantity = table.Rows[i]["quantity"].ToString();

                    books.Add(new Book(i + 1, name, author, category, publishNumber, quantity));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return books.ToArray();
        }

        // <summary> a method for taking borrowed books from database. </summary>
        // <returns> an array of Book </returns>

        public static Book[] TakeBorrowedBooks()
        {
            List<Book> books = new List<Book>();
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");

            try
            {
                DataTable table = DatabaseControl.TableFiller("select * from T_Borrowed", connection);

                var allBooks = TakeAllBooks();

                List<string> names = new List<string>();
                for (int i = 0; i < table.Rows.Count; i++)
                    names.Add(table.Rows[i]["bookName"].ToString());

                books = names.Distinct()
                             .Join(allBooks,
                             name => name,
                             book => book.Name,
                             (name, book) => book)
                             .ToList();
                for (int i = 0; i < books.Count; i++)
                {
                    books[i].Row = i + 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return books.ToArray();
        }

        // <summary> a method for taking Available books from database. it takes all the books
        // first and then checks wether thay have been borrowed or not. if borrowed, then
        // they will be deleted from the list </summary>
        // <returns> an array of Book </returns>

        public static Book[] TakeAvailableBooks()
        {
            List<Book> books = new List<Book>();

            if (TakeAllBooks() == null)
                return null;

            List<Book> allBooks = TakeAllBooks().ToList();

            var availableBooks = allBooks.Where(x => x.Quantity != 0).ToList();

            for (int i = 0; i < availableBooks.Count; i++)
            {
                availableBooks[i].Row = i + 1;
            }
            return availableBooks.ToArray();
        }

        // <summary> a method for searching the book by name </summary>
        // <returns> a Book </returns>

        public static Book SearchBookByName(string name)
        {
            var books = TakeAllBooks();
            Book book = Array.Find(books, b => b.Name == name);

            if (book == null)
                throw new ArgumentNullException("Book", "Book can not be found.");

            return book;
        }

        /// <summary> a method for searching an available book by name </summary>
        /// <returns> a Book </returns>
        /// <Exceptions> 
        ///     ArgumentNullException : if there are no books added to database. 
        ///     ArgumentNullException : if no book with this name can be found in the database. 
        /// </Exceptions>

        public static Book[] SearchAvailableBooksByName(string name)
        {
            var books = TakeAvailableBooks();

            if (books == null)
                throw new ArgumentNullException("Books", "No book is available.");

            var book = Array.FindAll(books, b => b.Name.ToLower() == name.ToLower());

            if (book.Length == 0)
                throw new ArgumentNullException("Book", "Book can not be found.");

            return book;
        }

        /// <summary> a method for searching an available book by author </summary>
        /// <returns> a Book </returns>
        /// <Exceptions> 
        ///     ArgumentNullException : if there are no books added to database. 
        ///     ArgumentNullException : if no book with this author can be found in the database. 
        /// </Exceptions>

        public static Book[] SearchAvailableBooksByAuthor(string author)
        {
            var books = TakeAvailableBooks();

            if (books == null)
                throw new ArgumentNullException("Books", "No book is available.");

            var book = Array.FindAll(books, b => b.Author.ToLower() == author.ToLower());

            if (book.Length == 0)
                throw new ArgumentNullException("Book", "Book can not be found.");

            return book;
        }

        /// <summary> a method for checking existance of book by name </summary>
        /// <returns> an integer . 0 if book exists, -1 if no book with this name found. 1 if book with this name found but are not same book.</returns>
        /// <Exceptions> 
        ///      ArgumentNullException : if there are no books added to database. 
        /// </Exceptions>

        public static int BookExists(Book book)
        {
            var books = TakeAllBooks();

            if (books == null)
                throw new ArgumentNullException("Books", "No books have been added.");

            var index = Array.FindIndex(books, b => b.Name.ToLower() == book.Name.ToLower());

            if (index < 0) return -1;
            else
            {
                if (books[index].Author.ToLower() == book.Author.ToLower() && books[index].Category.ToLower() == book.Category.ToLower() && books[index].PublishNumber == book.PublishNumber) return 0;
                return 1;
            }
        }

        // <summary> a method for checking availability of book by name </summary>
        // <returns> a boolean. true if book available, false if not available.</returns>
        // <Exceptions> 
        //      ArgumentNullException : if there are no books available in database. 
        // </Exceptions>

        public static bool IsBookAvailable(string name)
        {
            var books = TakeAvailableBooks();
            if (books == null)
                throw new ArgumentNullException("Books", "No book is available.");

            var index = Array.FindIndex(books, b => b.Name == name);

            if (index < 0) return false;
            else return true;
        }

        public void AddBook()
        {
            string command = "insert into T_Books (bookName,publishNumber,quantity,author,category) VALUES('" + Name + "','" + PublishNumber + "','" + Quantity + "','" + Author + "','" + Category + "')";
            DatabaseControl.Exe(command);
        }

        public static string[] GetAllNames(string username)
        {
            string command = "SELECT * FROM T_Borrowed WHERE username='" + username + "'";
            var data = DatabaseControl.Select(command);

            var nameCollection = new List<string>();
            for (int i = 0; i < data.Rows.Count; i++)
                nameCollection.Add(data.Rows[i]["bookName"].ToString());

            return nameCollection.ToArray();
        }
    }
}
