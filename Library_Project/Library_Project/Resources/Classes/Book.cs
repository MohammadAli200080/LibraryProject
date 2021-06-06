using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Project.Resources.Classes
{
    public class Book
    {
        private string _name;
        private string _author;
        private string _category;
        private int _publishNumber;
        private int _quantity;

        public string Name { get => _name; set => _name = value; }
        public string Author { get => _author; set => _author = value; }
        public string Category { get => _category; set => _category = value; }
        public int PublishNumber
        {
            get => _publishNumber;
            set
            {
                // check if the publishnumber is positive or not
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Quantity", "Should be positive number.");
                _publishNumber = value;
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                // check if the quantity is positive or not
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Quantity", "Should be positive number.");
                _quantity = value;
            }
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
            // take data from sql
            string[] data = null;

            if (data == null)
                return null;

            List<Book> books = new List<Book>();

            for (int i = 0; i < data.Length; i++)
            {
                Book book = new Book(data[i++], data[i++], data[i++], data[i++], data[i]);
                books.Add(book);
            }

            return books.ToArray();
        }

        // <summary> a method for taking borrowed books from database. </summary>
        // <returns> an array of Book </returns>

        public static Book[] TakeBorrowedBooks()
        {
            // take data from tabke Bookname_CustomerName
            string[] data = null;

            if (data == null)
                return null;

            List<Book> books = new List<Book>();
            Book[] allBooks = TakeAllBooks();

            for (int i = 0; i < data.Length; i += 2)
            {
                Book book = SearchBookByName(data[i]);
                books.Add(book);
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

            if (TakeBorrowedBooks() == null)
                return allBooks.ToArray();

            Book[] borrowedBooks = TakeBorrowedBooks();

            for (int i = 0; i < borrowedBooks.Length; i++)
            {
                Book book = SearchBookByName(borrowedBooks[i].Name);
                allBooks.Remove(book);
            }

            return allBooks.ToArray();
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

        // <summary> a method for searching the book by author </summary>
        // <returns> a Book.</returns>
        // <Exceptions> 
        //      ArgumentNullException : if there are no books added to database. 
        //      ArgumentNullException : if no book with this name can be found in the database. 
        // </Exceptions>

        public static Book SearchBookByAuthor(string author)
        {
            var books = TakeAllBooks();

            if (books == null)
                throw new ArgumentNullException("Books", "No books have been added.");

            Book book = Array.Find(books, b => b.Author == author);

            if (book == null)
                throw new ArgumentNullException("Book", "Book can not be found.");

            return book;
        }

        // <summary> a method for checking existance of book by name </summary>
        // <returns> a boolean. true if book exists, false if not exists.</returns>
        // <Exceptions> 
        //      ArgumentNullException : if there are no books added to database. 
        // </Exceptions>

        public static bool BookExists(string name)
        {
            var books = TakeAllBooks();

            if (books == null)
                throw new ArgumentNullException("Books", "No books have been added.");

            var index = Array.FindIndex(books, b => b.Name == name);

            if (index < 0)
                return false;
            else
                return true;
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

            if (index < 0)
                return false;
            else
                return true;
        }
    }
}
