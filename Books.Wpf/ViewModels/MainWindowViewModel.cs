using Books.Core.Contracts;
using Books.Core.Entities;
using Books.Persistence;
using Books.Wpf.Common;
using Books.Wpf.Views;
using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Books.Wpf.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<Book> _books;
        private Book _selectedBook;
        private string _filterText;
        private ICommand _cmdNewBook;
        private ICommand _cmdEditBook;
        private ICommand _cmdDeleteBook;

        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                _ = LoadBooksAsync();
            }
        }

        public MainWindowViewModel() : base(null)
        {
        }

        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
        }

        public ICommand CmdNewBook
        {
            get 
            {
                if (_cmdNewBook == null)
                {
                    _cmdNewBook = new RelayCommand(
                        execute: async _ => await NewBookAsync(),
                        canExecute: _ => true);
                }
                return _cmdNewBook;
            }
        }

   

        public ICommand CmdEditBook
        {
            get 
            {
                if (_cmdEditBook == null)
                {
                    _cmdEditBook = new RelayCommand(
                        execute: async _ => await EditbookAsync(),
                        canExecute: _ => SelectedBook != null);
                }
                return _cmdEditBook; 
            }
        }

        public ICommand CmdDeleteBook
        {
            get
            {
                if (_cmdDeleteBook == null)
                {
                    _cmdDeleteBook = new RelayCommand(
                        execute: async _ => await DeleteAsync(SelectedBook),
                        canExecute: _ => SelectedBook != null);
                }
                return _cmdDeleteBook;
            }
        }
        private async Task NewBookAsync()
        {
            var window = await BookEditCreateViewModel.Create(Controller, null);
            Controller.ShowWindow(window, true);
            await LoadBooksAsync();
        }

        private async Task EditbookAsync()
        {
            var window = await BookEditCreateViewModel.Create(Controller, SelectedBook);
            Controller.ShowWindow(window, true);
            await LoadBooksAsync();
        }


        private async Task DeleteAsync(Book selectedBook)
        {
            await using IUnitOfWork uow = new UnitOfWork();
            uow.Books.DeleteBook(selectedBook);
            await uow.SaveChangesAsync();
            await LoadBooksAsync();
        }

        /// <summary>
        /// Lädt die gefilterten Buchdaten
        /// </summary>
        public async Task LoadBooksAsync()
        {
            await using IUnitOfWork uow = new UnitOfWork();
            Books = new ObservableCollection<Book>(await uow.Books.GetBooksByFilterAsync(FilterText));
        }

        public static async Task<BaseViewModel> Create(IWindowController controller)
        {
            var model = new MainWindowViewModel(controller);
            await model.LoadBooksAsync();
            return model;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
