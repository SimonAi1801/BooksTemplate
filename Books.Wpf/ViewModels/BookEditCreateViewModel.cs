using Books.Core.Contracts;
using Books.Core.Entities;
using Books.Core.Validations;
using Books.Persistence;
using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Books.Wpf.ViewModels
{
    public class BookEditCreateViewModel : BaseViewModel
    {
        public string WindowTitle { get; set; }

        private Book _book;
        private ObservableCollection<Author> _authors;
        private ObservableCollection<BookAuthor> _bookAuthors;
        private ObservableCollection<string> _publishers;
        private Author _selectedAuthor;
        private BookAuthor _selectdBookAuthor;
        private string _selectedPublisher;
        private string _isbn;
        private string _title;
        private bool _isNewBook = false;

        [Required(ErrorMessage = "Titel muss angegeben werden!")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                ValidateViewModelProperties();
            }
        }



        public string Isbn
        {
            get => _isbn;
            set
            {
                _isbn = value;
                OnPropertyChanged(nameof(Isbn));
                ValidateViewModelProperties();
            }
        }

        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Publishers
        {
            get => _publishers;
            set
            {
                _publishers = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPublisher
        {
            get => _selectedPublisher;
            set
            {
                _selectedPublisher = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookAuthor> BookAuthors
        {
            get => _bookAuthors;
            set
            {
                _bookAuthors = value;
                OnPropertyChanged();
            }
        }



        public BookAuthor SelectedBookAuthor
        {
            get => _selectdBookAuthor;
            set
            {
                _selectdBookAuthor = value;
                OnPropertyChanged(nameof(SelectedBookAuthor));
            }
        }


        public BookEditCreateViewModel() : base(null)
        {
        }

        public BookEditCreateViewModel(IWindowController windowController, Book book) : base(windowController)
        {
            _book = book;
        }

        public static async Task<BaseViewModel> Create(IWindowController controller, Book book)
        {
            var model = new BookEditCreateViewModel(controller, book);
            await model.LoadData();
            return model;
        }

        private async Task LoadData()
        {
            await using IUnitOfWork uow = new UnitOfWork();

            var authors = await uow.Authors.GetAllAuthorsAsync();
            Authors = new ObservableCollection<Author>(authors);

            var publishers = await uow.Books.GetAllPublishersAsync();
            Publishers = new ObservableCollection<string>(publishers);

            BookAuthors = new ObservableCollection<BookAuthor>();
            if (_book == null)
            {
                _book = new Book();
                _isNewBook = true;
                return;
            }

            _title = _book.Title;
            OnPropertyChanged(Title);

            _isbn = _book.Isbn;
            OnPropertyChanged(Isbn);
            ValidateViewModelProperties();
            SelectedPublisher = _book.Publishers;
            BookAuthors = new ObservableCollection<BookAuthor>(_book.BookAuthors);
        }

        private ICommand _cmdAddAuthor;

        public ICommand CmdAddAuthor
        {
            get
            {
                if (_cmdAddAuthor == null)
                {
                    _cmdAddAuthor = new RelayCommand(
                        execute: _ => AddAuthor(),
                        canExecute: _ => true);
                }
                return _cmdAddAuthor;
            }
        }

        private ICommand _cmdDeleteAuthor;

        public ICommand CmdDeleteAuthor
        {
            get
            {
                if (_cmdDeleteAuthor == null)
                {
                    _cmdDeleteAuthor = new RelayCommand(
                        execute: _ => DeleteAuthor(),
                        canExecute: _ => SelectedBookAuthor != null);
                }
                return _cmdDeleteAuthor;
            }
        }

        private ICommand _cmdSaveBook;

        public ICommand CmdSaveBook
        {
            get
            {
                if (_cmdSaveBook == null)
                {
                    _cmdSaveBook = new RelayCommand(
                        execute: async _ => await SaveBookAsync(),
                        canExecute: _ => IsValid);
                }
                return _cmdSaveBook;
            }
        }

        private async Task SaveBookAsync()
        {
            await using IUnitOfWork uow = new UnitOfWork();
            Book bookInDb;
            if (_isNewBook)
            {
                bookInDb = _book;
            }
            else
            {
                bookInDb = await uow.Books.GetBookByIdAsync(_book.Id);
            }

            bookInDb.Title = _title;
            bookInDb.Isbn = _isbn;
            bookInDb.Publishers = _selectedPublisher;

            foreach (var bookAuthor in _book.BookAuthors)
            {
                bookInDb.BookAuthors.Add(bookAuthor);
            }

            try
            {
                await uow.SaveChangesAsync();
                Controller.CloseWindow(this);
            }
            catch (ValidationException vE)
            {
                if (vE is IEnumerable<string> properties)
                {
                    foreach (var property in properties)
                    {
                        Errors.Add(property, new List<string> { vE.ValidationResult.ErrorMessage });
                    }
                }
                else
                {
                    DbError = vE.ValidationResult.ToString();
                }
            }
        }

        private void DeleteAuthor()
        {
            BookAuthors.Remove(_selectdBookAuthor);
            OnPropertyChanged(nameof(BookAuthors));
        }

        private void AddAuthor()
        {
            BookAuthor bookAuthor = new BookAuthor
            {
                Author = _selectedAuthor,
                Book = _book
            };
            _book.BookAuthors.Add(bookAuthor);
            OnPropertyChanged(nameof(BookAuthors));
            
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Book.CheckIsbn(Isbn)))
            {
                yield return new ValidationResult($"Isbn {Isbn} ist ungültig", new string[] { nameof(Isbn) });
            }
        }
    }
}
