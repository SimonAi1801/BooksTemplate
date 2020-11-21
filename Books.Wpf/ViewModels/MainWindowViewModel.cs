using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Books.Wpf.ViewModels
{
  public class MainWindowViewModel : BaseViewModel
  {
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
    public Task LoadBooks()
    {
      throw new NotImplementedException();
    }

    public static async Task<BaseViewModel> Create(IWindowController controller)
    {
      var model = new MainWindowViewModel(controller);
      await model.LoadBooks();
      return model;
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      throw new NotImplementedException();
    }
  }
}
