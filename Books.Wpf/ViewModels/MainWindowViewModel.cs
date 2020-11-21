using Books.Core.Contracts;
using Books.Core.Entities;
using Books.Persistence;
using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Books.Wpf.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<Book> _books;
        private Book _selectedBook;
        private string _filterText;

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
            LoadCommands();
        }

        private void LoadCommands()
        {
        }



        /// <summary>
        /// Lädt die gefilterten Buchdaten
        /// </summary>
        public async Task LoadBooksAsync()
        {
            await using IUnitOfWork uow = new UnitOfWork();
            var books = await uow.Books.GetBooksByFilter(FilterText);
            Books = new ObservableCollection<Book>(books);
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
