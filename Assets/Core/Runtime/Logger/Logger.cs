using System.Collections.Generic;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private LogMessage logTemplate;

    [SerializeField]
    private int maxSize;

    // Stored required properties.
    private Queue<LogMessage> logs = new Queue<LogMessage>();

    public void Log(string text)
    {
        LogMessage log = Instantiate(logTemplate, transform);
        log.Display(text);

        logs.Enqueue(log);
        if (logs.Count > maxSize)
        {
            Destroy(logs.Dequeue().gameObject);
        }
    }
}
