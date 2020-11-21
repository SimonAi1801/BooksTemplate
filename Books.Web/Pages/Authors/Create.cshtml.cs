using Books.Core.Contracts;
using Books.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Books.Web.Pages.Authors
{
  public class CreateModel : PageModel
  {
    private readonly IUnitOfWork _uow;

    [BindProperty]
    public string Author { get; set; }

    public CreateModel(IUnitOfWork uow)
    {
      _uow = uow;
    }

    // POST: Authors/Create
    public Task<IActionResult> OnPost()
    {
      throw new NotImplementedException();
    }
  }
}
