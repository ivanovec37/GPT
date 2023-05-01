using Azure.Core;
using GPT;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

var apiKey = Environment.GetEnvironmentVariable("Key");
using var httpClient = new HttpClient()
{
    DefaultRequestHeaders =
    {
        Authorization =  AuthenticationHeaderValue.Parse($"Bearer {apiKey}")
    }

};

string endpoint = "https://api.pawan.krd/v1/chat/completions";

// Создаю список сообщений диалога с чат-ботом
List<ChatGptResponse> messages = new();

Console.WriteLine("\t\t\t\t\t\'Пустые Сообщения не вводить'");

while (true)
{
    try
    {
        // ввод сообщения пользователя
        Console.Write("Вы :" + "\n");
        var content = Console.ReadLine();

        // если введенное сообщение имеет длину меньше 1 символа,то выходим из цикла и завершаем программу

        if (content is not { Length: > 0 })
        {
            Console.WriteLine("Программа завершена,нельзя вводить пустые сообщения!"); break;
        }

        // формируем отправляемое сообщение
        var message = new ChatGptResponse() { Role = "user", Content = content };
        // добавляем сообщение в  наш список messages
        messages.Add(message);

        // формируем отправляемые данные
        var requestData = new GPT.Request()
        {
            ModelId = "gpt-3.5-turbo",
            Messages = messages
        };
        // отправляем запрос
        using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

        // если произошла ошибка, выводим сообщение об ошибке на консоль
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");
            break;
        }
        // получаем данные ответа
        ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();
        var choices = responseData?.Choices ?? new List<Choice>();
        #region
        //responseData?.Choices - Это нулевое условное выражение, которое проверяет,
        //responseData не является ли оно нулевым. Если оно не равно нулю,
        //выражение возвращает значение свойства Choices объекта responseData.
        //Если responseData значение null, выражение возвращает значение null.
        //?? -Это оператор объединения с нулевым значением.Он возвращает левый операнд,
        //если он не равен нулю; в противном случае возвращается правый операнд.
        #endregion
        if (choices.Count == 0)
        {
            Console.WriteLine("API не возвратил никаких вариантов");
            continue;
        }
        var choice = choices[0];
        var responseMessage = choice.Message;
        // добавляем полученное сообщение в список сообщений
        messages.Add(responseMessage);
        var responseText = responseMessage.Content.Trim();
        Console.WriteLine($"ChatGPT: {responseText}");
    }
    catch (HttpRequestException ex)
    {
        // Обработка ошибки
        Console.WriteLine("Ошибка при отправке запроса: {0}", ex.Message);
        if (ex.InnerException != null)
        {
            Console.WriteLine("Внутреннее исключение: {0}", ex.InnerException.Message);
        }
        if (ex != null)
        {
            Console.WriteLine("Код ошибки: {0}", ex.StatusCode);

        }
    }
}




