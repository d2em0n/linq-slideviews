using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        return visits
            .GroupBy(p => p.UserId)
            .Select(g => g
                .OrderBy(p => p.DateTime)
                .Bigrams()
                .Select(g => Tuple.Create(g.First.SlideType, g.Second.DateTime - g.First.DateTime))
                .Where(x => 
                {
                    var time = x.Item2.TotalMinutes;
                    return x.Item1 == slideType && time >= 1 && time <= 120;                                      
                })
                .Select(x => x.Item2.TotalMinutes))
            .SelectMany(x => x)
            .DefaultIfEmpty()
            .Median();   
    }
}