//using FPro;
//using System;

//namespace FMSCore.Events
//{
//    public class RouteHistoryEvent : EventArgs
//    {
//        public GVAR HistoricalPointData { get; }

//        public RouteHistoryEvent(GVAR historicalPointData)
//        {
//            HistoricalPointData = historicalPointData;
//        }
//    }

//    public static class HistoricalPointEvent
//    {
//        public static event EventHandler<RouteHistoryEvent> HistoricalPointAdded;

//        public static void OnHistoricalPointAdded(GVAR historicalPointData)
//        {
//            HistoricalPointAdded?.Invoke(null, new RouteHistoryEvent(historicalPointData));
//        }
//    }
//}
