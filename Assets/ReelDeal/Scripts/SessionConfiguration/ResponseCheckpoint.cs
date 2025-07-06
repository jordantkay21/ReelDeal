using KayosTech.ReelDeal.Prototype.LogSystem;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfig.Response
{
    public static class ResponseDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager);
            EventManager.OnServiceCommandDispatched += HandleIncomingServiceCommand;

            ServiceResponseFactory.OnResponseReady = DispatchServiceResponse;
        }
        private static void DispatchServiceResponse(IServiceResponse response) => EventManager.DispatchServiceResponse(response);

        private static async void HandleIncomingServiceCommand(Command.IServiceCommand command)
        {
            DevLog.Highlight($"5 - Service Command received: {command.GetType().Name}", "Data Flow");

            try
            {
                var response = await ServiceResponseFactory.ExecuteCommandAsync(command);
                if (response != null)
                    EventManager.DispatchServiceResponse(response);
            }
            catch (Exception ex)
            {
                DevLog.Error($"[ExecuteCommandAsync] {ex.GetType().Name} - {ex.Message}", "Data Flow");
            }
        }

    }

    #region Service Response Checkpoint

    #region Service Response TPOs
    public interface IServiceResponse
    {
        ActionType Action { get; }
        string Response { get; }
    }
    public class ServiceResponse: IServiceResponse
    {
        public ActionType Action { get; }

        public string Response { get; }

        public ServiceResponse(ActionType action, string response)
        {
            Action = action;
            Response = response;
        }

        public override string ToString()
        {
            return $"{Action} Service Response:  {Response}";
        }
    }

    public class PollResponse : IServiceResponse
    {
        public ActionType Action { get; }
        public string Response { get; }
        public int Count { get; }
        public float ElapsedTime { get; }

        public PollResponse(ActionType action, string response, int count, float elapsed)
        {
            Action = action;
            Response = response;
            Count = count;
            ElapsedTime = elapsed;
        }

        public override string ToString()
        {
            return $"{Action} Polling Response {Count} " +
                $"\n ElapsedTime (in seconds) - {ElapsedTime} " +
                $"\n {Response}";
        }
    }
    #endregion

    #region Service Response DTOs

    #endregion
    public static class ServiceResponseFactory
    {
        public static Action<IServiceResponse> OnResponseReady;
        public static async Task<IServiceResponse> ExecuteCommandAsync(Command.IServiceCommand command)
        {
            switch (command)
            {
                case Command.RegisterDeviceCommand registerDevice:
                    var rdResponse = await ExecuteRequestUtility.ExecuteAsync(command);
                    return new ServiceResponse(ActionType.RegisterDevice, rdResponse);
                
                case Command.PollAuthCommand:
                    _ = PollingUtility.StartPoll(
                        command,
                        async (raw, count, elapsed) =>
                        {
                            var tpo = new PollResponse(ActionType.PollAuth, raw, count, elapsed);
                            OnResponseReady?.Invoke(tpo);
                            await Task.CompletedTask;
                        });

                    return null;
                default:
                    throw new NotSupportedException($"Unsupported Service Command: {command.GetType().Name}");
            }
        }
    }

    #region UTILITIES

    public static class ExecuteRequestUtility
    {
        public static readonly HttpClient client = new HttpClient();

        public static async Task<string> ExecuteAsync(Command.IServiceCommand command)
        {
            
            var httpResponse = await client.SendAsync(command.BuildRequest());
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }

    public static class PollingUtility
    {
        private static bool _stopPollRequested;
        private static void StopPoll() => _stopPollRequested = true;

        public static async Task StartPoll(Command.IServiceCommand command, Func<string, int, float, Task> onRawResponse, float interval = 2f, float timeout = 600f)
        {
            _stopPollRequested = false;
            float elapsed = 0;
            int responseCount = 1;

            EventManager.OnStopPollingIssued += StopPoll;

            while (!_stopPollRequested && elapsed < timeout)
            {
                try
                {
                    string raw = await ExecuteRequestUtility.ExecuteAsync(command);
                    await onRawResponse.Invoke(raw, responseCount, elapsed);
                }
                catch (Exception ex)
                {
                    DevLog.Error($"Error during poll: {ex.Message}", "Polling");
                }

                await Task.Delay(TimeSpan.FromSeconds(interval));
                responseCount += 1;
                elapsed += interval;
            }

            EventManager.OnStopPollingIssued -= StopPoll;

            if (!_stopPollRequested)
            {
                DevLog.Warning("Polling Timed Out After 600 seconds.", "Polling");
            }
        }

    }

        #endregion
        #endregion
    }