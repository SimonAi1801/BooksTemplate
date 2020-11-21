using Books.Core.Entities;
using Books.Core.Validations;
using Books.Persistence;
using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Books.Wpf.ViewModels
{
  public class BookEditCreateViewModel : BaseViewModel
  {
    public string WindowTitle { get; set; }

    public BookEditCreateViewModel() : base(null)
    {
    }

    public BookEditCreateViewModel(IWindowController windowController, Book book) : base(windowController)
    {
      LoadCommands();
    }

    public static async Task<BaseViewModel> Create(WindowController controller, Book book)
    {
      var model = new BookEditCreateViewModel(controller, book);
      await model.LoadData();
      return model;
    }

    private Task LoadData()
    {
      throw new NotImplementedException();
    }

    private void LoadCommands()
    {
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return ValidationResult.Success;
    }

  }
}
