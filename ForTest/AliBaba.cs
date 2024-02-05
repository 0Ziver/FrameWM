using System.Management;

namespace Frame.Helpers;

public class AliBaba : IDisposable
{
    private WqlEventQuery query;
    private ManagementEventWatcher watcher;
    /*public AliBaba()
    {
        // Создаем запрос для отслеживания событий Win32_ProcessStop
        query = new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace");

        // Создаем объект ManagementEventWatcher с использованием запроса
        watcher = new ManagementEventWatcher(query);

        // Устанавливаем обработчик события
        watcher.EventArrived += ProcessStoppedEvent;

        // Начинаем отслеживание
        if (watcher != null)
        {
            watcher.Start();

            Console.WriteLine("Track process termination events. Press Enter to complete.");
            Console.ReadLine();

            // Останавливаем отслеживание приложения
        }
    }*/

    private void ProcessStoppedEvent(object sender, EventArrivedEventArgs e)
    {
        // Получаем информацию о событии
        PropertyDataCollection properties = e.NewEvent.Properties;
        string processName = properties["ProcessName"].Value.ToString();
        int processId = Convert.ToInt32(properties["ProcessID"].Value);

        Console.WriteLine($"Process close. Name: {processName}, ID: {processId}");
    }

    public void Dispose()
    {
        watcher.Stop();
    }
}