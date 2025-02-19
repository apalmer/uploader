using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Uploader.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BlobServiceClient _blobServiceClient;


        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public IEnumerable<Azure.Storage.Blobs.Models.BlobItem> Blobs { get; set; }

        public IndexModel(ILogger<IndexModel> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            Blobs = Enumerable.Empty<Azure.Storage.Blobs.Models.BlobItem>();
        }

        public void OnGet()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("uploads");
            Blobs = containerClient.GetBlobs();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var containerClient = _blobServiceClient.GetBlobContainerClient("uploads");
            Blobs = containerClient.GetBlobs();

            if (UploadedFile != null)
            {
                if (UploadedFile.Length > 0)
                {
                    var blobClient = containerClient.GetBlobClient(UploadedFile.FileName);
                    await blobClient.UploadAsync(UploadedFile.OpenReadStream(), true);
                }
            }

            return Page();
        }

    }
}
