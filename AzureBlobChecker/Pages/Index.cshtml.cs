using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AzureBlobChecker.Pages
{
    public class IndexModel : PageModel
    {
        private static string connectionString = "UseDevelopmentStorage=true";  // для Azurite
        private static string containerName = "test"; // Имя контейнера (заменили на test)

        // Метод для создания контейнера, если он не существует
        private async Task CreateContainerIfNotExistsAsync(BlobContainerClient containerClient)
        {
            try
            {
                // Если контейнер не существует, создаем его
                if (!await containerClient.ExistsAsync())
                {
                    await containerClient.CreateAsync();
                    Console.WriteLine($"Контейнер '{containerName}' был создан.");
                }
                else
                {
                    Console.WriteLine($"Контейнер '{containerName}' уже существует.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании контейнера: {ex.Message}");
            }
        }

        public string ImageUrl { get; set; }

        // Обработчик загрузки
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Создаем клиента для Blob Storage
                var blobServiceClient = new BlobServiceClient(connectionString);

                // Получаем ссылку на контейнер
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Если контейнер не существует, создаем его
                await CreateContainerIfNotExistsAsync(blobContainerClient);

                // Получаем ссылку на конкретный blob (файл)
                var blobClient = blobContainerClient.GetBlobClient(file.FileName);

                // Логируем информацию о загрузке
                Console.WriteLine($"Загружаем файл: {file.FileName}");

                // Загружаем файл в Blob Storage
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                // Логируем URL после загрузки
                Console.WriteLine($"Файл загружен, URL: {blobClient.Uri.ToString()}");

                // Отправляем URL на страницу
                ImageUrl = blobClient.Uri.ToString();
            }

            return Page();  // Обновляем страницу с новым ImageUrl
        }
    }
}
