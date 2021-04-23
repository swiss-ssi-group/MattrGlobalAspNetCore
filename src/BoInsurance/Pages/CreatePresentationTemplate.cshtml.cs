using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoInsurance.Pages
{
    public class CreatePresentationTemplateModel : PageModel
    {
        private readonly MattrVerifyService _mattrVerifyService;
        public bool CreatingPresentationTemplate { get; set; } = true;
        public string Callback { get; set; }

        [BindProperty]
        public PresentationTemplate PresentationTemplate { get; set; }
        public CreatePresentationTemplateModel(MattrVerifyService mattrVerifyService)
        {
            _mattrVerifyService = mattrVerifyService;
        }
        public void OnGet()
        {
            PresentationTemplate = new PresentationTemplate();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Callback = await _mattrVerifyService.CreatePresentationTemplateUrl(PresentationTemplate.DidId);
            CreatingPresentationTemplate = false;
            return Page();
        }
    }

    public class PresentationTemplate
    {
        [Required]
        public string DidId { get; set; }
    }
}
