using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AzureBlobChecker.Pages
{
    public class IndexModel : PageModel
    {
        private static string connectionString = "UseDevelopmentStorage=true";  // ��� Azurite
        private static string containerName = "test"; // ��� ���������� (�������� �� test)

        // ����� ��� �������� ����������, ���� �� �� ����������
        private async Task CreateContainerIfNotExistsAsync(BlobContainerClient containerClient)
        {
            try
            {
                // ���� ��������� �� ����������, ������� ���
                if (!await containerClient.ExistsAsync())
                {
                    await containerClient.CreateAsync();
                    Console.WriteLine($"��������� '{containerName}' ��� ������.");
                }
                else
                {
                    Console.WriteLine($"��������� '{containerName}' ��� ����������.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� �������� ����������: {ex.Message}");
            }
        }

        public string ImageUrl { get; set; }

        // ���������� ��������
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // ������� ������� ��� Blob Storage
                var blobServiceClient = new BlobServiceClient(connectionString);

                // �������� ������ �� ���������
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // ���� ��������� �� ����������, ������� ���
                await CreateContainerIfNotExistsAsync(blobContainerClient);

                // �������� ������ �� ���������� blob (����)
                var blobClient = blobContainerClient.GetBlobClient(file.FileName);

                // �������� ���������� � ��������
                Console.WriteLine($"��������� ����: {file.FileName}");

                // ��������� ���� � Blob Storage
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                // �������� URL ����� ��������
                Console.WriteLine($"���� ��������, URL: {blobClient.Uri.ToString()}");

                // ���������� URL �� ��������
                ImageUrl = blobClient.Uri.ToString();
            }

            return Page();  // ��������� �������� � ����� ImageUrl
        }
    }
}
