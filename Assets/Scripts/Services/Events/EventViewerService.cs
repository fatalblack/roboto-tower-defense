using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventViewerService : MonoBehaviour
{
    // public variables
    public TextMeshProUGUI eventText;
    public ScrollRect viewScroller;

    public static EventViewerService Instance { get; private set; }

    // private variables
    private List<string> events;

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            events = new List<string>();

            Instance = this;
        }
    }

    public void AddEventInfo(string eventMessage)
    {
        AddEvent(eventMessage, EventTypes.INFO);
    }

    public void AddEventSuccess(string eventMessage)
    {
        AddEvent(eventMessage, EventTypes.SUCCESS);
    }

    public void AddEventDanger(string eventMessage)
    {
        AddEvent(eventMessage, EventTypes.DANGER);
    }

    public void AddEventError(string eventMessage)
    {
        AddEvent(eventMessage, EventTypes.ERROR);
    }

    private void AddEvent(string eventMessage, EventTypes type)
    {
        // gets color for the selected type
        string color = GetColor(type);

        // build the message for event
        string formatedEventMessage = $"<color=#{color}>{eventMessage}</color>";

        // adds the event to the list
        events.Add(formatedEventMessage);

        // shows the event
        RefreshEventsText(formatedEventMessage);
    }

    private string GetColor(EventTypes type)
    {
        string color;

        switch (type)
        {
            case EventTypes.SUCCESS:
                color = "50C956";
                break;
            case EventTypes.DANGER:
                color = "F7A82B";
                break;
            case EventTypes.ERROR:
                color = "A42A2A";
                break;
            case EventTypes.INFO:
            default:
                color = "D0D0D0";
                break;
        }

        return color;
    }

    private void RefreshEventsText(string eventMessage)
    {
        // shows the event
        eventText.text += "\r\n" + eventMessage;

        // sets scroll position at bottom
        StartCoroutine(SetScrollAtBottom());
    }

    private IEnumerator SetScrollAtBottom()
    {
        yield return new WaitForEndOfFrame();

        // sets scroll position at bottom
        viewScroller.verticalNormalizedPosition = 0;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)viewScroller.transform);
    }
}