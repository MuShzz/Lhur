using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using StatusOptions = Unity.Services.Matchmaker.Models.MultiplayAssignment.StatusOptions;
using System.Collections;

public class MMTicketer : MonoBehaviour
{
    
    public string QueueName = "default-queue";
    private string ticketId = "";
    private bool searching = false;
    private IEnumerator pollingCoroutine = null;
    private TicketStatusResponse ticketStatusResponse = null;
    public MultiplayAssignment paired_assignment = null;
    private string ticket_id;

    // Start is called before the first frame update
    async void Start()
    {
        if (!Application.isEditor)
        {
            var args = GetCommandlineArgs();
            if (args.TryGetValue("-mode", out string mode))
            {
                if (mode == "server")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/SampleScene");
                    return;
                }
            }
        }
        DontDestroyOnLoad(this);
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("User AccessToken: "+AuthenticationService.Instance.AccessToken);
        OnFindMatch();
    }
    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }

    public async void OnFindMatch() 
    {
        Debug.Log("OnFindMatch");

        try
        {
            // Check toggle
            if (!searching)
            {
                if (ticketId.Length > 0)
                {
                    Debug.Log($"A Matchmaking ticket is already active for this client!");
                    return;
                }

                
                searching = true;

                await StartSearch();
            }
            else
            {
                if (ticketId.Length == 0)
                {
                    Debug.Log("Cannot delete ticket as no ticket is currently active for this client!");
                    return;
                }

                await StopSearch();
                
                searching = false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: "+e.Message);
        }
    }

    private async Task StartSearch() 
    {
        Debug.Log("Enter - StartSearch");
        var attributes = new Dictionary<string, object>();
        var players = new List<Player>
        { 
            new Player(AuthenticationService.Instance.PlayerId, new Dictionary<string, object>{ {"skill", 455.6} }), 
        };

        // Set options for matchmaking
        var options = new CreateTicketOptions(QueueName, attributes);

        // Create ticket
        var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(players, options);
        ticketId = ticketResponse.Id;
        //TicketIdText.text = ticketId = ticketResponse.Id;
        Debug.Log($"Ticket '{ticketResponse.Id}' created!");
        
        //Poll ticket status
        pollingCoroutine = PollTicketStatus();
        StartCoroutine(pollingCoroutine);
    }

    private async Task StopSearch()
    {
        Debug.Log("Enter - StopSearch");
        //Stop any active coroutines
        if (pollingCoroutine != null)
        {
            StopCoroutine(pollingCoroutine);
        }

        //Delete ticket
        await MatchmakerService.Instance.DeleteTicketAsync(ticketId);
        Debug.Log("Ticket deleted!");
        //TicketIdText.text = "N/A";
        ticketId = "";
    }

    //This async task call is wrapped in a Coroutine to ensure WebGL compatibility.
    private IEnumerator GetTicket()
    {
        Debug.Log("Enter - GetTicket");
        async Task GetTicketAsync()
        {
            ticketStatusResponse = await MatchmakerService.Instance.GetTicketAsync(ticketId);
        }

        var task = GetTicketAsync();
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            if (task.Exception != null)
            {
                Debug.LogException(task.Exception);

                // Note that exceptions on IEnumerators / coroutines are generally not handled
                throw task.Exception;
            }
        }
    }

    private IEnumerator PollTicketStatus()
    {
        Debug.Log("PollTicketStatus");

        string waitingMessage = "Finding match...";
        //string preMessagePaneText = Environment.NewLine + InfoPaneText.text;
        //InfoPaneText.text = waitingMessage + preMessagePaneText;

        ticketStatusResponse = null;
        MultiplayAssignment assignment = null;
        bool gotAssignment = false;

        while (!gotAssignment)
        {
            waitingMessage += ".";
            //InfoPaneText.text = waitingMessage + preMessagePaneText;

            StartCoroutine(GetTicket());
            yield return new WaitForSeconds(2f);

            if (ticketStatusResponse != null)
            {
                if (ticketStatusResponse.Type == typeof(MultiplayAssignment))
                {
                    assignment = ticketStatusResponse.Value as MultiplayAssignment;
                }

                if (assignment == null)
                {
                    var message = $"GetTicketStatus returned a type that was not a {nameof(MultiplayAssignment)}. This operation is not supported.";
                    throw new InvalidOperationException(message);
                }

                switch (assignment.Status)
                {
                    case StatusOptions.Found:
                        gotAssignment = true;
                        break;
                    case StatusOptions.InProgress:
                        //Do nothing
                        break;
                    case StatusOptions.Failed:
                        Debug.Log("Failed to get ticket status. See logged exception for more details.");
                        throw new MatchmakerServiceException(MatchmakerExceptionReason.Unknown, assignment.Message);
                    case StatusOptions.Timeout:
                        gotAssignment = true;
                        Debug.Log("Failed to get ticket status. Ticket timed out.");
                        break;
                    default:
                        throw new InvalidOperationException("Assignment status was a value other than 'In Progress', 'Found', 'Timeout' or 'Failed'! " +
                            $"Mismatch between Matchmaker SDK expected responses and service API values! Status value: '{assignment.Status}'");
                }
            }
        }
        Debug.Log("assignment.Ip: "+assignment.Ip);
        Debug.Log("assignment.Port: " + assignment.Port);
        paired_assignment = assignment;
        object toSerialize = assignment != null ? (object)assignment : (object)ticketStatusResponse;
        string jsonOutput = JsonConvert.SerializeObject(toSerialize, Formatting.Indented);
        Debug.Log(jsonOutput);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/SampleScene");
    }
}
